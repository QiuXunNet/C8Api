using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Public;
using System.Data.SqlClient;
using Qiuxun.C8.Caching;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Data
{
    public class PlanService
    {
        /// <summary>
        /// 获取官方计划推荐数据(分页)
        /// </summary>
        public PagedListP<Plan> GetPlanData(int lType, int pageIndex, int pageSize)
        {
            var pager = new PagedListP<Plan>();
            pager.PageSize = pageSize;
            pager.PageIndex = pageIndex;
            int count = Util.GetGFTJCount(lType);
            int totalSize = (pageSize + 1) * count;

            string totalsql = "select COUNT(1) from ( select row_number() over(order by Id desc) as rownumber,* from [Plan] where lType = " + lType + ") as temp";
            string sql = "select top " + totalSize + " temp.* from ( select row_number() over(order by Id desc) as rownumber,* from [Plan] where lType = " + lType + ")as temp where rownumber>" + ((pageIndex - 1) * totalSize);
            pager.PageData = Util.ReaderToList<Plan>(sql);        //计划列表
            pager.TotalCount = ToolsHelper.ObjectToInt(SqlHelper.ExecuteScalar(totalsql));
            return pager;
        }

        /// <summary>
        /// 发布计划
        /// </summary>
        public ApiResult Bet(int lType, string currentIssue, string betInfo, long userId)
        {
            //数据清理
            string sql = "delete from BettingRecord where UserId=" + userId + " and lType =" + lType + " and Issue=@Issue";
            SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@Issue", currentIssue));



            string[] betInfoArr = betInfo.Split('$');

            string playName = "";
            string playName2 = "";            //单双  大小 五码
            string betNum = "";
            string s1 = "";


            foreach (string s in betInfoArr)
            {
                string[] arr = s.Split('*');

                playName = arr[0];
                betNum = arr[1];

                string[] betNumArr = betNum.Split('|');

                for (int i = 0; i < betNumArr.Length; i++)
                {
                    s1 = betNumArr[i];

                    if (!string.IsNullOrEmpty(s1))
                    {
                        playName2 = Util.GetPlayName(lType, playName, s1, i);

                        sql = "insert into BettingRecord(UserId,lType,Issue,BigPlayName,PlayName,BetNum,SubTime) values(" + userId + "," + lType + ",@Issue,@BigPlayName,@PlayName,@BetNum,GETDATE())";

                        SqlParameter[] pms =
                        {
                            new SqlParameter("@Issue", currentIssue),
                            new SqlParameter("@BigPlayName", playName),
                            new SqlParameter("@PlayName", playName + playName2),
                            new SqlParameter("@BetNum", s1),
                        };

                        SqlHelper.ExecuteNonQuery(sql, pms);
                    }
                }

            }
            return new ApiResult();
        }

        /// <summary>
        /// 检查是否看过该帖子，没看过，金币是否足够查看
        /// </summary>
        public ApiResult CanViewPlan(int id, int ltype, int uid, int coin)
        {
            //step1.查询用户是否点阅过该帖子
            string readRecordSql = @"select count(1) from ComeOutRecord where [Type]=@Type and UserId=@UserId and OrderId=@Id";

            var readRecordParameter = new[]
            {
                    new SqlParameter("@Type",(int)TransactionTypeEnum.点阅),
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Id",id),
                };

            object objReadRecord = SqlHelper.ExecuteScalar(readRecordSql, readRecordParameter);

            if (objReadRecord != null && Convert.ToInt32(objReadRecord) > 0)
            {
                //已经点阅过，直接跳转
                return new ApiResult();
            }
            UserInfo user = PersonalService.GetUser(uid);
            //step2.判断当前用户积分是否小于查看帖子所需金币
            if (coin > user.Coin)
            {
                return new ApiResult(60015, "金币数不足");
            }
            return new ApiResult();
        }

        /// <summary>
        /// 获取该用户最新计划(同时会插入点阅记录，收费专家扣除用户金币数、获得佣金)
        /// </summary>
        public ApiResult<BettingRecord> GetLastPlay(int lType, long uid, string playName, long userId)
        {
            if (string.IsNullOrWhiteSpace(playName))
            {
                return new ApiResult<BettingRecord>() { Code = 6001, Desc = "彩种名称不能为空", Data = null };
            }
            if (uid == userId)
            {
                return new ApiResult<BettingRecord>() { Code = 6002, Desc = "相同用户，不能获取", Data = null };
            }

            #region 校验,添加点阅记录，扣费，分佣
            UserInfo user = PersonalService.GetUser(userId);
            //step1.查询最新发帖
            string lastBettingSql = @" select top 1 * from BettingRecord where UserId=@UserId 
                 and lType=@lType and WinState=1 and PlayName=@PlayName order by SubTime desc";
            var lastBettingParameter = new[]
            {
                    new SqlParameter("@UserId", uid),
                    new SqlParameter("@lType", lType),
                    new SqlParameter("@PlayName", playName),
                };
            var records = Util.ReaderToList<BettingRecord>(lastBettingSql, lastBettingParameter);
            var lastBettingRecord = records.FirstOrDefault();

            if (lastBettingRecord == null)
            {
                return new ApiResult<BettingRecord>() { Code = 6003, Desc = "未获取到数据", Data = null };
            }

            //step2:查询用户是否点阅过该帖子。若未点阅过，则校验金币是否充足
            string readRecordSql = @"select count(1) from ComeOutRecord 
                    where [Type]=@Type and UserId=@UserId and OrderId=@Id";

            var readRecordParameter = new[]
            {
                    new SqlParameter("@Type", (int) TransactionTypeEnum.点阅),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Id", lastBettingRecord.Id),
                };

            object objReadRecord = SqlHelper.ExecuteScalar(readRecordSql, readRecordParameter);

            //用户未点阅过该帖子
            if (objReadRecord == null || Convert.ToInt32(objReadRecord) <= 0)
            {
                //step3.1:查询点阅所需金币
                int totalIntegral = LuoUtil.GetUserIntegral(uid, lType);
                var setting = GetLotteryCharge().FirstOrDefault(
                    x => x.MinIntegral <= totalIntegral
                         && x.MaxIntegral > totalIntegral
                         && x.LType == lType
                    );

                int readCoin = 0; //点阅所需金币

                if (setting != null) readCoin = setting.Coin;

                StringBuilder executeSql = new StringBuilder();
                if (readCoin > 0)
                {
                    //step3.2:校验用户金币是否充足
                    if (user.Coin < readCoin)
                    {
                        //金币不足
                        return new ApiResult<BettingRecord>() { Code = 6004, Desc = "收费帖子，金币不足", Data = null };
                    }
                    else
                    {
                        //1.扣除用户金币
                        executeSql.AppendFormat("update UserInfo set Coin-={0} where Id={1};", readCoin, user.Id);
                        //2.添加点阅记录
                        executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
                            VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅, readCoin);


                        //3:查询用户分佣比例
                        var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == GetlType(lType) && x.Type == (int)CommissionTypeEnum.点阅佣金);

                        if (userRateSetting != null && userRateSetting.Percentage > 0)
                        {
                            int commission = (int)(userRateSetting.Percentage * readCoin);
                            executeSql.AppendFormat("update UserInfo set Money+={0} where Id={1}", commission, uid);

                            executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
                                    VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅佣金, commission);
                        }
                    }
                }
                else
                {
                    //免费专家，仅记录点阅记录
                    executeSql.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
                                            VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, lastBettingRecord.Id, (int)TransactionTypeEnum.点阅, 0);
                }

                try
                {

                    SqlHelper.ExecuteTransaction(executeSql.ToString());

                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(string.Format("查看最新帖子异常。帖子Id:{0}，查看人：{1}，异常消息:{2}，异常堆栈：{3}",
                        lastBettingRecord.Id, user.Id, ex.Message, ex.StackTrace));

                    return new ApiResult<BettingRecord>() { Code = -999, Desc = "数据库错误", Data = null };
                }


            }
            return new ApiResult<BettingRecord>() { Code = 100, Desc = "", Data = lastBettingRecord };
            #endregion
        }

        /// <summary>
        /// 获取当前用户某个彩种当前期号计划集合
        /// </summary>
        public ApiResult<List<BettingRecord>> AlreadyPostData(int lType, long userId)
        {
            string sql = "select * from BettingRecord where UserId  =" + userId + " and lType=" + lType + " and Issue = '" + Util.GetCurrentIssue(lType) + "'";
            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql);

            return new ApiResult<List<BettingRecord>>() { Code = 100, Data = list };
        }

        /// <summary>
        /// 获取专家搜索数据集合
        /// </summary>
        public ApiResult<List<ExpertSearchModel>> GetExpertSearchList(int lType, string nickName, long userId)
        {
            string strsql = @" select UserId,lType,
	                            isnull(u.Name,'') as Name,isnull(r.RPath,'') as Avater,(select count(1)  from [dbo].[Follow] where UserId=@MyUserId and [Followed_UserId]=b.UserId and Status=1) isFollow 
	                            from [BettingRecord] b 
	                            left join UserInfo u	on b.UserId=u.Id
	                            left join ResourceMapping r on r.FkId =b.UserId and r.[Type]=@ResourceType
                                where WinState>1 and lType=@lType and u.Name like @Name+'%'  and b.UserId<>@MyUserId
                                group by UserId, lType,u.Name,r.RPath";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@MyUserId",userId),
                 new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType),
                new SqlParameter("@Name",nickName)

            };

            List<ExpertSearchModel> list = Util.ReaderToList<ExpertSearchModel>(strsql, sp);

            return new ApiResult<List<ExpertSearchModel>>() { Code = 100, Desc = "", Data = list };


        }

        /// <summary>
        /// 删除搜索历史记录
        /// </summary>
        public ApiResult DeleteHistory(int uid, int lType, long userId)
        {
            string memberKey = "history_" + userId + "_" + lType;
            List<ExpertSearchModel> list = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey);
            if (uid > 0)
            {
                ExpertSearchModel e1 = list.Where(x => x.UserId == uid && x.lType == lType).FirstOrDefault();
                list.Remove(e1);
                CacheHelper.WriteCache(memberKey, list, 144000);
            }
            else
            {
                CacheHelper.DeleteCache(memberKey);
            }
            return new ApiResult();
        }

        /// <summary>
        /// 插入搜索数据
        /// </summary>
        public ApiResult InsertHotSearch(long uid, int lType, long userId)
        {
            //ReturnMessageJson msg = new ReturnMessageJson();
            string countsql = string.Format("select count(1) from ExpertHotSearch where UserId ={0} and lType = {1}", uid, lType);
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));
            string strsql = "";
            if (count > 0)
            {
                strsql = @"update ExpertHotSearch  set Count=Count+1 where UserId=@UserId and lType=@lType ";
            }
            else
            {
                strsql = @"insert into ExpertHotSearch(UserId, Count, lType)
                           values(@UserId,1,@lType)";
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",uid),
                new SqlParameter("@lType",lType)
            };

            int data = SqlHelper.ExecuteNonQuery(strsql, sp);

            if (data > 0)
            {
                List<ExpertSearchModel> list = new List<ExpertSearchModel>();

                long MyUserId = userId;
                string memberKey = "history_" + MyUserId + "_" + lType;
                list = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();

                UserInfo u = PersonalService.GetUser(uid);

                ExpertSearchModel e = new ExpertSearchModel();
                e.Avater = u.Headpath;
                e.UserId = (int)uid;
                e.Name = u.Name;
                e.lType = lType;
                e.isFollow = 0;
                if (list.Count > 0)
                {
                    ExpertSearchModel e1 = list.Where(x => x.UserId == uid && x.lType == lType).FirstOrDefault();
                    if (e1 == null)
                    {
                        list.Add(e);
                    }
                }
                else
                {
                    list.Add(e);
                }

                CacheHelper.WriteCache(memberKey, list, 144000);
                return new ApiResult();
            }
            else
            {
                return new ApiResult(-999,"数据库错误");
            }

        }
    

        /// <summary>
        /// 获取专家热搜前N条数据
        /// </summary>
        public ApiResult<List<ExpertSearchModel>> GetTopExpertSearch(int lType, int top,long userId)
        {
            string strsql = string.Format(@"select top {0} UserId,lType,isnull(u.Name, '') as Name,isnull(r.RPath, '') as Avater,(select count(1)  from [dbo].[Follow] where UserId=@MyUserId and [Followed_UserId]=e.UserId and Status=1) isFollow 
                            from ExpertHotSearch e
                            left join UserInfo u    on e.UserId = u.Id
                            left join ResourceMapping r on r.FkId = e.UserId and r.[Type] = @ResourceType
                             where lType=@lType and e.UserId<>@MyUserId
                            order by e.Count desc", top);
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@MyUserId",userId),
                new SqlParameter("@lType",lType),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像)
            };
            List<ExpertSearchModel> list = Util.ReaderToList<ExpertSearchModel>(strsql, sp);
            return new ApiResult<List<ExpertSearchModel>>() { Code = 100, Desc = "", Data = list };
        }

        /// <summary>
        /// 打赏
        /// </summary>
        public ApiResult GiftCoin(int id, int coin, long userId)
        {
            var user = PersonalService.GetUser(userId);
            #region 校验
            //step1.验证金币输入是否正确
            if (coin < 10)
            {
                return new ApiResult(60016, "最低打赏10金币");
            }
            //step2.验证帖子是否存在
            var model = Util.GetEntityById<BettingRecord>(id);
            if (model == null)
                return new ApiResult(60017, "该计划不存在");

            //step3.验证用户金币是否充足
            if (user.Coin < coin)
            {
                return new ApiResult(60018, "金币余额不足");
            }
            #endregion


            StringBuilder sqlBuilder = new StringBuilder();
            //step4.扣除打赏人账户金币
            sqlBuilder.AppendFormat("UPDATE dbo.UserInfo SET Coin-={1} WHERE Id={0};", user.Id, coin);
            //step5.添加打赏记录
            sqlBuilder.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
                                        VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, id, (int)TransactionTypeEnum.打赏, coin);

            var userRateSetting = GetCommissionSetting().FirstOrDefault(x => x.LType == GetlType(model.lType) && x.Type == (int)CommissionTypeEnum.打赏佣金);
            if (userRateSetting != null && userRateSetting.Percentage > 0)
            {
                int commission = (int)(userRateSetting.Percentage * coin);
                //step6.发放发帖人金币账户
                sqlBuilder.AppendFormat("UPDATE dbo.UserInfo SET Money+={1} WHERE Id={0};", model.UserId, commission);
                //step7.添加打赏佣金记录
                sqlBuilder.AppendFormat(@"INSERT INTO [dbo].[ComeOutRecord]([UserId],[OrderId],[Type] ,[Money],[State],[SubTime])
                                            VALUES({0},{1},{2},{3}, 1, GETDATE());", user.Id, id, (int)TransactionTypeEnum.打赏佣金, commission);
            }

            //LogHelper.WriteLog(sqlBuilder.ToString());
            SqlHelper.ExecuteTransaction(sqlBuilder.ToString());

            return new ApiResult(100, "打赏成功");

        }

        /// <summary>
        /// 获取用户彩种下的打赏总金额
        /// </summary>
        public ApiResult<int> GetTotalTipMoeny(int lType, int uid)
        {
            string sumSql = @"select isnull(sum(a.[Money]),0) from ComeOutRecord a
                            left join BettingRecord b on b.Id=a.OrderId
                            where b.UserId=" + uid + " and a.[Type]=" + (int)TransactionTypeEnum.打赏 + " and b.lType=" + lType;
            object sumObj = SqlHelper.ExecuteScalar(sumSql);

            return new ApiResult<int>() { Code = 100, Desc = "", Data = sumObj.ToInt32() };
        }

        /// <summary>
        /// 获取用户彩种下前固定条数的打赏记录
        /// </summary>
        public ApiResult<List<TipRecordModel>> GetTipRecord(int lType, int uid, int top)
        {
            string sql = string.Format(@"select Top {0} a.Id,a.UserId,a.OrderId,a.[Type],a.[Money],a.[State],a.SubTime,c.Name,d.RPath as Avater 
                        from ComeOutRecord a
                        left join BettingRecord b on b.Id=a.OrderId 
                        left join UserInfo c on c.Id=a.UserId
                        left join ResourceMapping d on d.FkId=a.UserId and d.[Type]=@ResourceType
                        where b.UserId=@UserId and a.[Type]=@RecordType and b.lType=@lType
                        order by SubTime DESC", top);

            var sqlParameter = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@UserId",uid),
                new SqlParameter("@RecordType",(int)TransactionTypeEnum.打赏),
                new SqlParameter("@lType",lType),
            };

            var list = Util.ReaderToList<TipRecordModel>(sql, sqlParameter);

            return new ApiResult<List<TipRecordModel>>() { Code = 100, Desc = "", Data = list };
        }

        /// <summary>
        /// 根据彩种获取最新官方玩法推荐
        /// </summary>
        public ApiResult<List<Plan>> GetNewPlan(int lType)
        {
            string sql = "select top(" + Util.GetGFTJCount(lType) + ")* from [Plan] where lType = " + lType + " order by Issue desc";
            List<Plan> list = Util.ReaderToList<Plan>(sql);
            return new ApiResult<List<Plan>>() { Code = 100, Desc = "", Data = list };
        }

        /// <summary>
        /// 获取用户近期竞猜记录(返回当前用户是否点阅过该记录)
        /// </summary>
        public PagedListP<AchievementModel> GetUserLastPlay(long uid, int lType, string playName, long userId)
        {
            string strsql = string.Empty;
            string numsql = string.Empty;
            string playSql = string.Empty;

            var pager = new PagedListP<AchievementModel>();
            pager.PageIndex = 1;
            pager.PageSize = 10;
            SqlParameter[] sp = new SqlParameter[] { };
            if (playName == "全部")//全部
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}", uid, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber, Num,l.SubTime,l.Issue from LotteryRecord l
	                                      ,BettingRecord b
	                                      where b.Issue=l.Issue and b.lType=l.lType
	                                      and b.UserId={0} and b.lType={1}  and b.WinState in(3,4)
	                                      group by l.Issue,Num,l.SubTime
	                                      )t
	                                      where   rowNumber BETWEEN {2} AND {3}  ", uid, lType, pager.StartIndex, pager.EndIndex);
                playSql = string.Format(@" select top 1 * from BettingRecord where UserId={0} 
                                                     and lType={1} and WinState=1 order by SubTime desc", uid, lType);
            }
            else
            {
                strsql = string.Format(@"
                select * from BettingRecord   where UserId ={0} and lType = {1}  and PlayName = @PlayName", uid, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue from LotteryRecord l
	                                      ,BettingRecord b
	                                      where b.Issue=l.Issue and b.lType=l.lType
	                                      and b.UserId={0} and b.lType={1} and b.PlayName=@PlayName  and b.WinState in(3,4)
	                                      group by l.Issue,Num,l.SubTime
	                                      )t
	                                      where   rowNumber BETWEEN {2} AND {3} ", uid, lType, pager.StartIndex, pager.EndIndex);
                playSql = string.Format(@" select top 1 * from BettingRecord where UserId={0} 
                                                     and lType={1} and WinState=1 and PlayName=@PlayName order by SubTime desc", uid, lType);

                sp = new SqlParameter[] { new SqlParameter("@PlayName", playName) };

            }

            List<LotteryNum> listnum = Util.ReaderToList<LotteryNum>(numsql, sp);//我对应的开奖数据
            List<BettingRecord> listbet = Util.ReaderToList<BettingRecord>(strsql, sp);
            List<AchievementModel> list = new List<AchievementModel>();
            if (listnum.Count > 0)
            {
                foreach (var item in listnum)
                {
                    AchievementModel model = new AchievementModel();
                    LotteryNum l = new LotteryNum();
                    l.Issue = item.Issue;
                    l.Num = item.Num;
                    l.SubTime = Convert.ToDateTime(item.SubTime).ToString("yyyy-MM-dd");
                    model.LotteryNum = l;
                    if (listbet.Count() > 0)
                        model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                    list.Add(model);

                }
            }
            pager.PageData = list;

            //查询最新一期玩法
            var lastPlay = Util.ReaderToList<BettingRecordViewModel>(playSql, sp);
            var extraData = lastPlay.FirstOrDefault();

            if (extraData != null)
            {
                //查询当前用户是否点阅过该记录
                string isSubSql = "select count(1) from dbo.ComeOutRecord where Type="
                    + (int)TransactionTypeEnum.点阅 + " and UserId=" + userId + " and OrderId=" + extraData.Id;
                object objIsSub = SqlHelper.ExecuteScalar(isSubSql);
                extraData.IsRead = objIsSub != null && Convert.ToInt32(objIsSub) > 0;
            }

            pager.ExtraData = extraData;
            return pager;
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        public PagedListP<Expert> GetExpertList(int lType, string playName, int type, int pageIndex, int pageSize)
        {
            var pager = new PagedListP<Expert>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;

            string sqlWhere = type == 1 ? ">=" : "<";

            #region 分页查询专家排行数据行
            string sql = string.Format(@"select * from (
                                         select top 100 row_number() over(order by a.playTotalScore DESC ) as rowNumber,
                                            a.*,b.ltypeTotalScore,c.MinIntegral,isnull(d.Name,'') as Name,isnull(e.RPath,'') as avater 
                                        from (
                                          select UserId,lType,PlayName, isnull( sum(score),0) AS playTotalScore from [C8].[dbo].[BettingRecord]
                                          where WinState>1 and lType=@lType and PlayName=@PlayName
                                          group by UserId, lType, PlayName
                                         ) a
                                          left join (
                                           select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [C8].[dbo].[BettingRecord]
                                           where WinState>1 and lType=@lType
                                           group by UserId, lType
                                          ) b on b.lType=a.lType and b.UserId=a.UserId
                                          left join ( 
	                                        select lType, isnull( min(MinIntegral),0) as MinIntegral 
	                                        from [dbo].[LotteryCharge] 
                                            where lType=@lType
                                            group by lType
                                          ) c on c.lType=a.lType
                                          left join UserInfo d on d.Id=a.UserId
                                          left join ResourceMapping e on e.FkId =a.UserId and e.[Type]=@ResourceType

                                          where b.ltypeTotalScore {0} c.MinIntegral and a.PlayName=@PlayName and a.lType=@lType 
                                          ) tt
                                          where tt.rowNumber between @Start and  @End", sqlWhere);

            var sqlParameter = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@PlayName",playName),
                new SqlParameter("@lType",lType),
                new SqlParameter("@Start",pager.StartIndex),
                new SqlParameter("@End",pager.EndIndex),

            };
            pager.PageData = Util.ReaderToList<Expert>(sql, sqlParameter);

            pager.PageData.ForEach(x =>
            {
                GetLastBettingRecord(x);
            });

            #endregion

            #region 数据总行数
            //查询分页总数量
            string countSql = string.Format(@"select count(1) from (
                                              select UserId,lType,PlayName, isnull( sum(score),0) AS playTotalScore from [dbo].[BettingRecord]
                                              where WinState>1 and lType=@lType and PlayName=@PlayName
                                              group by UserId, lType, PlayName
                                             ) a
                                              left join (
                                               select UserId,lType, isnull( sum(score),0) AS ltypeTotalScore from [dbo].[BettingRecord]
                                               where WinState>1 and lType=@lType
                                               group by UserId, lType
                                              ) b on b.lType=a.lType and b.UserId=a.UserId
                                              left join ( 
	                                            select lType, isnull( min(MinIntegral),0) as MinIntegral 
	                                            from [dbo].[LotteryCharge]
                                                where lType=@lType
                                                group by lType
                                              ) c on c.lType=a.lType

                                              where b.ltypeTotalScore {0} c.MinIntegral and a.PlayName=@PlayName and a.lType=@lType", sqlWhere);

            var countSqlParameter = new[]
            {
                new SqlParameter("@PlayName",playName),
                new SqlParameter("@lType",lType),
            };
            object obj = SqlHelper.ExecuteScalar(countSql, countSqlParameter);
            int totalCount = Convert.ToInt32(obj ?? 0);
            pager.TotalCount = totalCount > 100 ? 100 : totalCount;
            #endregion
            return pager;
        }

        /// <summary>
        /// 获取用户在某一玩法中奖率，最大连中，上期是否中奖
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filterRow"></param>
        private void GetLastBettingRecord(Expert model, int filterRow = 1000)
        {
            string sql = string.Format(@"select top {0} Issue,WinState from dbo.BettingRecord
                              where lType=@lType and PlayName=@PlayName and UserId=@UserId and WinState>1
                              order by Issue desc", filterRow);

            var sqlParameter = new[]
            {
                new SqlParameter("@lType",model.lType),
                new SqlParameter("@PlayName",model.PlayName),
                new SqlParameter("@UserId",model.UserId),

            };

            var list = Util.ReaderToList<BettingRecord>(sql, sqlParameter) ?? new List<BettingRecord>();

            //step1.上期是否中奖
            var lastBettingRecord = list.FirstOrDefault();
            model.LastWin = lastBettingRecord != null && lastBettingRecord.WinState == 3;

            //step2.查询10中几
            int winCount = 0;//10中几

            //查询最后10期，并按顺序排序
            var last10Record = list.Take(10).OrderBy(x => x.Issue);
            int last10RecordLength = last10Record.Count();
            if (last10RecordLength > 0)
            {
                foreach (var record in last10Record)
                {
                    if (record.WinState == 3)
                        winCount++;
                }
                model.HitRate = winCount / last10RecordLength;
                model.HitRateDesc = string.Format("{0}中{1}", last10RecordLength, winCount);
            }

            //step3.查询最大连中
            int continuousWinCount = 0;//最大连中
            int tempContinuousWinCount = 0;
            //按期号顺序排序，并遍历
            foreach (var record in list.OrderBy(x => x.Issue))
            {
                if (record.WinState == 3)
                {
                    tempContinuousWinCount++;
                }
                else
                {
                    //当期未中时，将当前最大连中赋值给continuousWinCount，并重置临时计算最大连中
                    continuousWinCount = tempContinuousWinCount;
                    tempContinuousWinCount = 0;
                }
            }
            if (tempContinuousWinCount > continuousWinCount)
                continuousWinCount = tempContinuousWinCount;

            model.MaxWin = continuousWinCount;

        }

        /// <summary>
        /// 获取分佣配置
        /// </summary>
        /// <returns></returns>
        private IList<CommissionSetting> GetCommissionSetting()
        {
            string memKey = "base_commission_settings";
            var list = CacheHelper.GetCache<IList<CommissionSetting>>(memKey);
            if (list == null || list.Count < 1)
            {
                string sql = "SELECT [Id],[lType],[Percentage],[Type] FROM [dbo].[SharedRevenue] WHERE IsDeleted=0";
                list = Util.ReaderToList<CommissionSetting>(sql);
                if (list != null)
                {
                    CacheHelper.WriteCache(memKey, list, 60);
                }
            }

            return list ?? new List<CommissionSetting>();
        }

        /// <summary>
        /// 获取大彩种lType
        /// </summary>
        /// <returns></returns>
        private int GetlType(int LotteryCode)
        {
            string strsql = string.Format("select lType from [dbo].[Lottery] where LotteryCode={0}", LotteryCode);
            return Convert.ToInt32(SqlHelper.ExecuteScalar(strsql));
        }

        /// <summary>
        /// 获取用户点阅计划所需金币数
        /// </summary>
        public ApiResult<int> GetReadCoin(int lType, long userId)
        {
            //step6.查询用户彩种积分，
            int totalIntegral = LuoUtil.GetUserIntegral(userId, lType);
            //step7.根据用户该彩种积分，查询点阅所需金币
            var setting = GetLotteryCharge().FirstOrDefault(
                    x => x.MinIntegral <= totalIntegral
                    && x.MaxIntegral > totalIntegral
                    && x.LType == lType
                );

            int ReadCoin = setting != null ? setting.Coin : 0;
            return new ApiResult<int>() { Code = 100, Desc = "", Data = ReadCoin };
        }

        /// <summary>
        /// 获取贴子点阅扣费配置表
        /// </summary>
        /// <returns></returns>
        private IList<LotteryCharge> GetLotteryCharge()
        {
            string memKey = "base_lottery_charge_settings";
            var list = CacheHelper.GetCache<IList<LotteryCharge>>(memKey);
            if (list != null && list.Any()) return list;

            string sql = "SELECT Id,lType,MinIntegral,MaxIntegral,Coin FROM dbo.LotteryCharge";
            list = Util.ReaderToList<LotteryCharge>(sql);

            if (list != null)
            {
                CacheHelper.WriteCache(memKey, list);
                return list;
            }

            return new List<LotteryCharge>();
        }

        /// <summary>
        /// 查询当前用户是否发表过计划
        /// </summary>
        public ApiResult<bool> HasSubBet(int lType, long userId, string playName, int type)
        {
            string isSubSql = "select count(1) from dbo.BettingRecord where lType=" + lType + " and UserId=" + userId;
            if (type == 1)
            {
                isSubSql += " and WinState=1";
            }
            if (!string.IsNullOrWhiteSpace(playName))
            {
                isSubSql += " and PlayName='" + playName + "'";
            }
            object objIsSub = SqlHelper.ExecuteScalar(isSubSql);
            bool flag = objIsSub != null && Convert.ToInt32(objIsSub) > 0;
            return new ApiResult<bool>() { Code = 100, Desc = "", Data = flag };
        }
    }
}
