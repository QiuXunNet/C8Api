using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Dtos.Personal;
using Qiuxun.C8.Api.Public;
using System.Data.SqlClient;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Common;
using System.Data;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Common.Paging;
using System.Collections;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Model;
using System.Configuration;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 个人中心业务类
    /// </summary>
    public class PersonalService
    {
        /// <summary>
        /// 获取个人中心用户数据
        /// </summary>
        public IndexResDto GetPersonalIndexData(long userId)
        {

            string usersql = @"select (select count(1)from Follow where UserId=u.Id and Status=1)as Follow,
(select count(1)from Follow where Followed_UserId=u.Id and Status=1)as Fans,
(select count(1) from UserCoupon where UserId=u.Id and State=1 and getdate()<EndTime)as UserCoupon,
r.RPath as Avater,u.Name as NickName,u.Id as UserId,u.* from UserInfo  u 
                              left  JOIN (select RPath,FkId from ResourceMapping where Type = @Type)  r 
                              on u.Id=r.FkId  where u.Id=@userId ";
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@userId", userId), new SqlParameter("@Type", (int)ResourceTypeEnum.用户头像) };

            IndexResDto resDto = Util.ReaderToModel<IndexResDto>(usersql, sp);

            string noticeCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and UserId=" + userId;
            object noticeCount = SqlHelper.ExecuteScalar(noticeCountSql);

            resDto.NoticeCount = Convert.ToInt32(noticeCount);
            return resDto;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUser(long userId)
        {

            string usersql = @"select (select count(1)from Follow where UserId=u.Id and Status=1)as follow,(select count(1)from Follow where Followed_UserId=u.Id and Status=1)as fans, r.RPath as Avater,u.Name as NickName,u.* from UserInfo  u 
                              left  JOIN (select RPath,FkId from ResourceMapping where Type = @Type)  r 
                              on u.Id=r.FkId  where u.Id=@userId ";

            ReturnMessageJson jsonmsg = new ReturnMessageJson();
            UserInfo user = new UserInfo();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@userId", userId), new SqlParameter("@Type", (int)ResourceTypeEnum.用户头像) };
                List<UserInfo> list = Util.ReaderToList<UserInfo>(usersql, sp);
                if (list != null)
                {
                    user = list.FirstOrDefault(x => x.Id == userId);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return user;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public ApiResult ModifyPWD(string oldpwd, string newpwd, long userId)
        {
            string sql = "SELECT Password FROM dbo.UserInfo WHERE Id=" + userId;
            object currentPwd = SqlHelper.ExecuteScalar(sql);
            if (currentPwd == null)
            {
                return new ApiResult(60000, "登录超时，需要重新登录");
            }
            if (Tool.GetMD5(oldpwd) != currentPwd.ToString())
            {
                return new ApiResult(60004, "旧密码错误，请重新输入");
            }
            sql = string.Format("UPDATE dbo.UserInfo SET Password='{0}' WHERE Id={1}", Tool.GetMD5(newpwd), userId);
            int result = SqlHelper.ExecuteNonQuery(sql);
            if (result > 0)
            {
                return new ApiResult(100, "success");
            }
            else
            {
                return new ApiResult(-999, "数据库出现错误。");
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        public ApiResult EditUserInfo(string value, int type, long userId)
        {
            string strsql = string.Empty;

            bool iscz = Tool.CheckSensitiveWords(value);
            if (iscz)
            {
                return new ApiResult(60008, "修改失败，包含敏感字符");
            }

            if (type == 1)
            {
                //昵称
                if (value.Length > 7)
                {
                    return new ApiResult(60012, "昵称不能超过7个字符");
                }

                string namesql = "select count(1) from UserInfo where Name=@value";
                SqlParameter[] sp1 = {
                    new SqlParameter("@value",value),
                    new SqlParameter("@UserId",userId)
                };
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(namesql, sp1));
                if (count > 0)
                {
                    return new ApiResult(60007, "该昵称已经存在");
                }
                strsql = "  Name=@value ";

            }
            else if (type == 2)
            {
                //签名
                if (value.Length > 20)
                {
                    return new ApiResult(60012, "签名不能超过20个字符");
                }

                strsql = "  Autograph=@value ";
            }
            else if (type == 3)
            {
                strsql = " Sex=@value ";
            }

            string usersql = "update  UserInfo set  " + strsql + " where  Id=@UserId";

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@value",value),
                new SqlParameter("@UserId",userId)

                };

            int result = SqlHelper.ExecuteNonQuery(usersql, sp);
            if (result > 0)
            {
                return new ApiResult(100, "success");
            }

            return new ApiResult(-999, "数据库出现错误。");
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        public ApiResult UnFollow(long followed_userId, long userId)
        {
            string strsql = "update Follow set Status=0,FollowTime=getdate() where UserId=@UserId  and Followed_UserId=@Followed_UserId";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Followed_UserId",followed_userId)
              };
            int data = SqlHelper.ExecuteNonQuery(strsql, sp);
            if (data > 0)
            {
                return new ApiResult(100, "success");
            }
            else
            {
                return new ApiResult(-999, "数据库出现错误。");
            }
        }

        /// <summary>
        /// 关注
        /// </summary>
        public ApiResult IFollow(long followed_userId, long userId)
        {
            string yzsql = string.Format("select  count(1) from Follow where UserId={0}  and Followed_UserId={1}", userId, followed_userId);
            string strsql = "";
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(yzsql));
            if (count > 0)
            {
                strsql = "update Follow set Status=1,FollowTime=getdate() where UserId=@UserId  and Followed_UserId=@Followed_UserId";
            }
            else
            {
                strsql = "insert into Follow(UserId, Followed_UserId, Status,FollowTime)values(@UserId, @Followed_UserId, 1,getdate())";
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Followed_UserId",followed_userId)
                };
            if (userId == followed_userId)
            {
                return new ApiResult(60010, "自己不能关注自己");
            }
            else
            {
                int data = SqlHelper.ExecuteNonQuery(strsql, sp);
                if (data > 0)
                {
                    return new ApiResult(100, "success");
                }
                else
                {
                    return new ApiResult(-999, "数据库出现错误。");
                }
            }

        }

        /// <summary>
        /// 粉丝榜数据 只取前100条
        /// </summary>
        public ApiResult<List<FansResDto>> GetFansBangList(string type, int pageIndex, int pageSize)
        {
            List<FansResDto> list = Cache.CacheHelper.GetCache<List<FansResDto>>("GetFansBangListWebSite" + type + "_" + pageIndex);
            if (list == null)
            {
                string strsql = string.Format(@"select  * from ( select top 100 row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId as FollowedUserId,Name as NickName,isnull(RPath,'/images/default_avater.png') as Avater
                                             from Follow f 
                                             left join UserInfo u on f.Followed_UserId=u.id
                                             left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=@ResourceType)
                                            where  f.Status=1  {0}
                                             group by Followed_UserId,Name,RPath
                                            )t
                                            WHERE Rank BETWEEN @Start AND @End", Tool.GetTimeWhere("FollowTime", type));


                SqlParameter[] sp = new SqlParameter[]
                {
                        new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                        new SqlParameter("@Start", (pageIndex - 1) * pageSize + 1),
                        new SqlParameter("@End", pageSize * pageIndex)
                        };
                 list = Util.ReaderToList<FansResDto>(strsql, sp) ?? new List<FansResDto>();
                 Cache.CacheHelper.SetCache<List<FansResDto>>("GetFansBangListWebSite" + type + "_" + pageIndex, list, DateTime.Parse("23:59:59"));
            }
          

            return new ApiResult<List<FansResDto>>()
            {
                Data = list
            };
        }

        /// <summary>
        /// 进入他人主页是否关注（调用同时插入一条访问记录）
        /// </summary>
        public ApiResult<HasFollowResDto> GetHasFollow(long id, long userId)
        {

            HasFollowResDto resdto = new HasFollowResDto();
            if (id == userId)
            {
                resdto.Followed = false;
                resdto.ShowFollow = false;
                return new ApiResult<HasFollowResDto>() { Data = resdto };
            }



            //查询是否存在当前用户对受访人的已关注记录 Status=1:已关注
            string sql = "select count(1) from [dbo].[Follow] where [Status]=1 and [UserId]=" + userId +
                         " and [Followed_UserId]=" + id;

            object obj = SqlHelper.ExecuteScalar(sql);

            resdto.Followed = obj != null && Convert.ToInt32(obj) > 0;
            resdto.ShowFollow = true;

            return new ApiResult<HasFollowResDto>() { Data = resdto };
        }

        /// <summary>
        /// 添加他人的个人中心访问记录
        /// </summary>
        /// <param name="id">受访人UserId</param>
        /// <param name="userId">访问人用户Id</param>
        /// <param name="module">访问模块编码</param>
        /// <returns></returns>
        public ApiResult AddVisitRecord(long id, long userId, int module = 1)
        {
            if (id != userId)
            {
                string visitSql = @"
                          if exists
                          (
                            select 1 from [dbo].[AccessRecord] where UserId = @UserId and RespondentsUserId = @RespondentsUserId and Module=@Module and Datediff(day,AccessDate,GETDATE())=0
                          )
                          begin
                          update [dbo].[AccessRecord] set AccessTime=GETDATE() WHERE  UserId = @UserId and RespondentsUserId = @RespondentsUserId and Module=@Module and Datediff(day,AccessDate,GETDATE())=0
                          end
                          else
                          begin
                          insert into  [dbo].[AccessRecord] (UserId,RespondentsUserId,Module,AccessTime,AccessDate) values(@UserId,@RespondentsUserId,@Module,GETDATE(),GETDATE())
                          end";

                var sqlParameter = new[]
                {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@RespondentsUserId", id),
                    new SqlParameter("@Module", module) //默认访问主页
                };

                int result = SqlHelper.ExecuteNonQuery(visitSql, sqlParameter);

                if (result < 1) return new ApiResult(50000, "添加访问记录失败");
            }

            return new ApiResult();
        }

        /// <summary>
        /// 获取我的计划
        /// </summary>
        public ApiResult<List<LotteryTypeResDto>> GetMyPlan(long userId)
        {
            string sql = "SELECT lType FROM [dbo].[BettingRecord] WHERE UserId=" + userId +
                          " GROUP BY lType";
            var list = Util.ReaderToList<LotteryTypeResDto>(sql);

            list.ForEach(x =>
            {
                x.LTypeName = Util.GetLotteryTypeName(x.LType);
            });

            return new ApiResult<List<LotteryTypeResDto>>() { Data = list };
        }

        /// <summary>
        /// 获取未读通知消息数量
        /// </summary>
        /// <returns></returns>
        public ApiResult<NoReadResDto> GetNoticeNoRead(long userId)
        {
            NoReadResDto noread = new NoReadResDto();
            //查询动态消息未读数量
            string unreadDynamicCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and Type=2 and UserId=" + userId;
            object dynamicCount = SqlHelper.ExecuteScalar(unreadDynamicCountSql);
            noread.DynamicCount = ToolsHelper.ObjectToInt(dynamicCount);

            //查询系统消息未读数量
            string unreadSysMessageCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and Type=1 and UserId=" + userId;
            object sysMessageCount = SqlHelper.ExecuteScalar(unreadSysMessageCountSql);
            noread.SysMessageCount = ToolsHelper.ObjectToInt(sysMessageCount);

            return new ApiResult<NoReadResDto>() { Data = noread };
        }

        /// <summary>
        /// 获取系统消息
        /// </summary>
        public PagedListP<SystemMessage> GetSysMessage(long uid, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<SystemMessage>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string sql = @"SELECT * FROM (
	                select row_number() over(order by a.SubTime DESC ) as rowNumber,
	                a.*,b.Title,b.Content,b.Link from UserInternalMessage a
	                left join InternalMessage b on b.Id=a.RefId and a.[Type]=1
	                where a.[Type]=1 and a.IsDeleted=0  and a.UserId=@UserId
                ) tt
                WHERE rowNumber BETWEEN @Start AND @End";

            if (uid == 0)
                uid = userId;

            var sqlParameters = new[]
            {
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)
                };

            pager.PageData = Util.ReaderToList<SystemMessage>(sql, sqlParameters);

            #region 未读消息处理
            if (pager.PageData.Any(x => x.IsRead == false))
            {
                //若存在未读消息，更新未读消息状态为已读
                var idList = pager.PageData.Where(x => x.IsRead == false).Select(x => x.Id);
                string executeSql = string.Format(@"
                                update dbo.UserInternalMessage 
                                set IsRead=1,ReadTime=GETDATE()
                                where UserId={0} and Id in({1});
                                select count(1) from UserInternalMessage 
                                where [Type]=1 and IsDeleted=0 and IsRead=0 and UserId={0}"
                                , uid, string.Join(",", idList));

                object objUnreadCount = SqlHelper.ExecuteScalar(executeSql);
                pager.ExtraData = new { Unread = objUnreadCount.ToInt32() };

            }
            else
            {
                pager.ExtraData = new { Unread = 0 };
            }
            #endregion

            string countSql = "select count(1) from UserInternalMessage where [Type]=1 and IsDeleted=0 and UserId=" + uid;
            object obj = SqlHelper.ExecuteScalar(countSql);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);

            return pager;
        }

        /// <summary>
        /// 获取我的佣金
        /// </summary>
        public ApiResult<DrawMoneyResDto> GetMyCommission(long userId)
        {
            string strsql = @"	select Txing,Txleiji,KeTx from 
                             (select isnull(sum([Money]),0)as Txing from ComeOutRecord where  [UserId]=@UserId and Type=2 and State=1)t2,
                             (select isnull(sum([Money]),0)as Txleiji  from ComeOutRecord where  [UserId]=@UserId and Type=2 and State=3 )t3,
                             (select isnull([Money],0)as KeTx  from UserInfo  where  [Id]=@UserId )t5";
            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId)
                };
            DrawMoneyModel dr = Util.ReaderToModel<DrawMoneyModel>(strsql, sp);
            var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

            DrawMoneyResDto dto = new DrawMoneyResDto();
            dto.MyYj = Tool.Rmoney((dr.KeTx + dr.Txing) / moneyToCoin);
            dto.Txing = Tool.Rmoney((dr.Txing) / moneyToCoin);
            dto.Txleiji = Tool.Rmoney((dr.Txleiji) / moneyToCoin);
            dto.KeTx = Tool.Rmoney((dr.KeTx) / moneyToCoin);
            return new ApiResult<DrawMoneyResDto>() { Data = dto };
        }

        /// <summary>
        /// 获取提现、收入明细
        /// </summary>
        public PagedListP<ComeOutRecordModel> GetMyCommissionList(int type, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<ComeOutRecordModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string strsql = "";
            string strstate = "";
            if (type == 1)//收入明细
            {
                strsql = @"select * from (  select row_number() over (order by c.Id) as rowNumber,
                             c.UserId as BUserId,b.Issue,b.lType,u.Name as UserName, OrderId, Type, c.Money, c.State, c.SubTime, PayType 
                             from ComeOutRecord c
                            inner join BettingRecord b on c.OrderId=b.Id
                            inner join UserInfo u on  c.UserId=u.Id
                             where b.UserId=@UserId and c.Type in(4,9)

                             )t
                             where   rowNumber BETWEEN @Start AND @End
                                   order by SubTime desc ";
                strstate = "4,9";

            }
            else if (type == 2)//提现明细
            {
                strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
                         where UserId =@UserId and Type=2 

                         )t
                         where   rowNumber BETWEEN  @Start AND @End order by SubTime desc";
                strstate = "2";
            }
            string countsql = @" select count(1) from ComeOutRecord where UserId=@UserId and Type in(" + strstate + @")";
            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)


                };

            List<ComeOutRecordModel> list = Util.ReaderToList<ComeOutRecordModel>(strsql, sp);
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql, sp));
            var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

            if (list != null)
            {

                list.ForEach(x =>
                {
                    if (type == 2)
                    {
                        x.LotteryIcon = "/images/41.png";
                    }
                    if (type == 1)
                    {
                        x.LotteryIcon = Util.GetLotteryIconUrl(x.lType);
                    }

                    x.Money = Convert.ToDecimal((x.Money / moneyToCoin).ToString("f2"));
                });
            }
            pager.PageData = list;
            pager.TotalCount = count;

            return pager;
        }

        /// <summary>
        /// 获取我的卡券列表
        /// </summary>
        public PagedListP<UserCoupon> GetMyUserCouponList(int type, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<UserCoupon>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string strsql = "";
            string countsql = "";

            string strwhere = string.Format(" UserId={0}", userId);
            switch (type)
            {
                case 0:
                    strwhere += " and State=1 and getdate()<EndTime";
                    break;

                case 1:
                    strwhere += " and State=2";
                    break;

                case 2:
                    strwhere += " and getdate()>EndTime";
                    break;
            }
            strsql = string.Format("select * from UserCoupon where {0} ", strwhere);
            countsql = string.Format("select count(*) from UserCoupon where {0} ", strwhere);

            List<UserCoupon> list = Util.ReaderToList<UserCoupon>(strsql);
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));

            pager.PageData = list;
            pager.TotalCount = count;

            return pager;
        }

        /// <summary>
        /// 获取交易记录数据
        /// </summary>
        public PagedListP<ComeOutRecordModel> GetRecordList(int type, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<ComeOutRecordModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string strstate = "";
            string strsql = "";

            if (type == 1)//充值
            {
                strstate = "1";

                strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
                             where UserId =@UserId and Type in(" + strstate + @")  and State=3
                             )t
                             where   rowNumber BETWEEN  @Start AND @End  
                               order by SubTime desc";
            }
            else if (type == 2)//消费
            {
                strstate = "3,5";

                strsql = @"select * from ( select row_number() over (order by c.Id) as rowNumber,b.UserId as BUserId,b.Issue,b.lType,u.Name as UserName,c.* from ComeOutRecord c
                                inner join BettingRecord b
                                inner join UserInfo u
                                on b.UserId=u.Id
                                on c.OrderId=b.Id
                                 where c.UserId=@UserId and c.Type in(" + strstate + @")
                                 )t
                                 where   rowNumber BETWEEN @Start AND @End
                                    order by SubTime desc";

            }
            else if (type == 3)//赚钱 只看任务奖励
            {

                strstate = "8";
                strsql = @"select * from ( select row_number() over (order by Id) as rowNumber, * from ComeOutRecord
                             where UserId = @UserId and Type in(" + strstate + @")  
                             )t
                             where   rowNumber BETWEEN  @Start AND @End
                               order by SubTime desc";

            }

            string countsql = @" select count(1) from ComeOutRecord where UserId=@UserId and Type in(" + strstate + @")";

            if (type == 1)
            {
                countsql += "  and State=3";
            }

            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)


                };
            List<ComeOutRecordModel> list = Util.ReaderToList<ComeOutRecordModel>(strsql, sp);
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql, sp));
            if (list != null)
            {
                if (type == 1)
                {
                    list.ForEach(x =>
                    {

                        x.LotteryIcon = Tool.GetPayImg(Convert.ToInt32(x.PayType));
                    });

                }
                else if (type == 2)
                {
                    list.ForEach(x =>
                    {
                        x.LotteryIcon = Util.GetLotteryIconUrl(x.lType);
                    });
                }
                else if (type == 3)
                {
                    list.ForEach(x =>
                    {

                        x.LotteryIcon = Tool.GetZqImg(Convert.ToInt32(x.OrderId));
                    });
                }

            }
            pager.PageData = list;
            pager.TotalCount = count;

            return pager;
        }

        /// <summary>
        /// 获取我的成绩
        /// </summary>
        public PagedListP<AchievementModel> GetMyBet(int lType, string playName, int pageIndex, int pageSize, long userId)
        {
            string strsql = string.Empty;
            string numsql = string.Empty;
            string countsql = string.Empty;

            var pager = new PagedListP<AchievementModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            SqlParameter[] sp = new SqlParameter[] { };
            if (playName == "全部")//全部
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}", userId, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber, Num,l.SubTime,l.Issue,b.lType from LotteryRecord l
	                                  join BettingRecord b
	                                  on b.Issue=l.Issue and b.lType=l.lType
	                                  where b.UserId={0} and b.lType={1}  and b.WinState in(3,4)
	                                  group by l.Issue,Num,l.SubTime,b.lType
	                                  )t
	                                  where   rowNumber BETWEEN {2} AND {3}  ", userId, lType, pager.StartIndex, pager.EndIndex);
                countsql = string.Format(@"	  select count(distinct Issue)from BettingRecord  
	                                     where UserId={0} and lType={1} and WinState in(3,4)", userId, lType);
            }
            else
            {
                strsql = string.Format(@"select * from BettingRecord   where UserId ={0} and lType = {1}  and PlayName = @PlayName", userId, lType);
                numsql = string.Format(@"SELECT * FROM (  select row_number() over(order by l.SubTime desc  ) as rowNumber,  Num,l.SubTime,l.Issue,b.lType from LotteryRecord l
	                                      join BettingRecord b
	                                      on b.Issue=l.Issue and b.lType=l.lType
	                                      where b.UserId={0} and b.lType={1} and b.PlayName=@PlayName  and b.WinState in(3,4)
	                                      group by l.Issue,Num,l.SubTime,b.lType
	                                      )t
	                                      where   rowNumber BETWEEN {2} AND {3} ", userId, lType, pager.StartIndex, pager.EndIndex);
                countsql = string.Format(@"select count(distinct Issue)from BettingRecord  
	                                        where UserId={0} and lType={1}   and PlayName=@PlayName and WinState in(3,4)", userId, lType);

                sp = new SqlParameter[]{
                    new SqlParameter("@PlayName",playName)
                };

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
                    l.lType = item.lType;
                    model.LotteryNum = l;
                    if (listbet.Count() > 0)
                        model.BettingRecord = listbet.Where(x => x.Issue == item.Issue).ToList();
                    list.Add(model);

                }
            }
            pager.PageData = list;

            object obj = SqlHelper.ExecuteScalar(countsql, sp);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);

            return pager;
        }

        /// <summary>
        /// 获取我的参与竞猜数据
        /// </summary>
        public PagedListP<BetModel> GetBet(int pId, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<BetModel>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string strsql = @"SELECT * FROM(
                      select row_number() over(order by Position) as rowNumber,
                     (select isnull(sum(Score), '0')  from BettingRecord where[UserId] =@UserId
                and lType = l.lType) as Score,* from LotteryType2 l
   
                      where PId = @PId 
                )t
                WHERE rowNumber BETWEEN @Start AND @End";
            string countsql = @"select count(1) from LotteryType2 where PId=@PId ";
            SqlParameter[] sp = new SqlParameter[]
            {
                        new SqlParameter("@PId",pId),
                        new SqlParameter("@UserId",userId),
                        new SqlParameter("@Start",  pager.StartIndex ),
                        new SqlParameter("@End", pager.EndIndex)

             };

            pager.PageData = Util.ReaderToList<BetModel>(strsql, sp);

            object obj = SqlHelper.ExecuteScalar(countsql, sp);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);

            pager.PageData.ForEach(x =>
            {
                x.LotteryIcon = Util.GetLotteryIconUrl(x.lType);
            });
            return pager;
        }

        /// <summary>
        /// 获取评论提醒
        /// </summary>
        public PagedListP<DynamicMessage> GetCommentNotice(long uid, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<DynamicMessage>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;

            #region 分页查询动态消息
            string sql = @"SELECT * FROM (
                                select row_number() over(order by m.SubTime DESC ) as rowNumber,m.*,a.PId,a.Content,
	                            a.[Type] as CommentType,a.ArticleId,a.ArticleUserId,a.RefCommentId,
	                            a.UserId as FromUserId,b.Name as FromNickName,c.RPath as FromAvater 
	                            from UserInternalMessage m
	                            left join Comment a on a.Id=m.RefId
	                            left join UserInfo b on b.Id = a.UserId
	                            left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
	                            where m.IsDeleted=0 and m.UserId=@UserId and m.Type=2
                            ) tt
                            WHERE rowNumber BETWEEN @Start AND @End";

            if (uid == 0)
                uid = userId;

            var sqlParameters = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@UserId",uid),
                new SqlParameter("@Start",pager.StartIndex),
                new SqlParameter("@End",pager.EndIndex)
            };

            pager.PageData = Util.ReaderToList<DynamicMessage>(sql, sqlParameters);
            #endregion

            #region 其他处理
            List<long> unreadList = new List<long>();
            pager.PageData.ForEach(x =>
            {
                if (x.PId > 0)
                {
                    var comment = GetComment(x.PId);
                    x.MyContent = comment.Content;
                }
                var info = GetLotteryTypeName(x.Type, x.ArticleId, x.ArticleUserId, x.RefCommentId);
                x.LotteryTypeName = info.TypeName ?? "";
                x.RefNickName = info.NickName;

                if (string.IsNullOrEmpty(x.FromAvater))
                {
                    x.FromAvater = "/images/default_avater.png";
                }

                if (x.IsRead == false)
                {
                    unreadList.Add(x.Id);
                }
            });
            #endregion

            #region 处理未读消息

            if (unreadList.Count > 0)
            {
                string executeSql = string.Format(@"
                                    update dbo.UserInternalMessage 
                                    set IsRead=1,ReadTime=GETDATE()
                                    where UserId={0} and Id in({1});
                                    select count(1) from UserInternalMessage 
                                    where [Type]=1 and IsDeleted=0 and IsRead=0 and UserId={0}"
                                    , uid, string.Join(",", unreadList));

                object objUnreadCount = SqlHelper.ExecuteScalar(executeSql);
                pager.ExtraData = new { Unread = objUnreadCount.ToInt32() };
            }
            else
            {
                pager.ExtraData = new { Unread = 0 };
            }

            #endregion

            #region 查询动态总条数
            string countSql = "SELECT count(1) FROM UserInternalMessage WHERE IsDeleted=0 AND [Type]=2 AND UserId=" + uid;
            object obj = SqlHelper.ExecuteScalar(countSql);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);
            #endregion

            return pager;
        }

        /// <summary>
        /// 获取访问记录
        /// </summary>
        public PagedListP<AccessRecord> GetVisitRecord(long uid, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<AccessRecord>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string sql = @"SELECT * FROM (
	                      select row_number() over(order by AccessTime DESC ) as rowNumber,a.Id,a.UserId,a.Module,a.AccessTime,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater from  [dbo].[AccessRecord] a
	                      left join UserInfo b on b.Id=a.UserId
	                      left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=@ResourceType
	                      where a.RespondentsUserId=@RespondentsUserId
                    ) tt
                    WHERE rowNumber BETWEEN @Start AND @End";

            if (uid == 0)
                uid = userId;

            var sqlParameters = new[]
            {
                    new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                    new SqlParameter("@RespondentsUserId",uid),
                    new SqlParameter("@Start",pager.StartIndex),
                    new SqlParameter("@End",pager.EndIndex)
                };

            pager.PageData = Util.ReaderToList<AccessRecord>(sql, sqlParameters);

            pager.PageData.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = "/images/default_avater.png";
                }
            });

            string countSql = "SELECT count(1) FROM [AccessRecord] WHERE RespondentsUserId=" + uid;
            object obj = SqlHelper.ExecuteScalar(countSql);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);

            return pager;
        }

        /// <summary>
        /// 获取动态
        /// </summary>
        public PagedListP<Comment> GetDenamic(long uid, int pageIndex, int pageSize, long userId)
        {
            var pager = new PagedListP<Comment>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            string sql = @"SELECT * FROM (
	                select row_number() over(order by a.SubTime DESC ) as rowNumber,
	                a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
	                from Comment a
	                left join UserInfo b on b.Id = a.UserId
	                left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
	                where a.IsDeleted=0 and a.UserId=@UserId
                ) tt
                WHERE rowNumber BETWEEN @Start AND @End";

            if (uid == 0)
                uid = userId;

            var sqlParameters = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@UserId",uid),
                new SqlParameter("@Start",pager.StartIndex),
                new SqlParameter("@End",pager.EndIndex)
            };

            pager.PageData = Util.ReaderToList<Comment>(sql, sqlParameters);

            pager.PageData.ForEach(x =>
            {
                if (x.PId > 0)
                {
                    x.ParentComment = GetComment(x.PId);
                }
                if (string.IsNullOrEmpty(x.Avater))
                {
                    x.Avater = "/images/default_avater.png";
                }


                var info = GetLotteryTypeName(x.Type, x.ArticleId, x.ArticleUserId, x.RefCommentId);

                x.LotteryTypeName = info == null ? "" : info.TypeName;
            });

            string countSql = "SELECT count(1) FROM Comment WHERE IsDeleted=0 AND UserId=" + uid;
            object obj = SqlHelper.ExecuteScalar(countSql);
            pager.TotalCount = Convert.ToInt32(obj ?? 0);

            return pager;
        }

        private Comment GetComment(long id)
        {
            string sql = @"select a.*,isnull(b.Name,'') as NickName
                 from Comment a
                left join UserInfo b on b.Id=a.UserId
                where  a.Id=@Id and a.IsDeleted = 0";
            var parameters = new[]
           {
                new SqlParameter("@Id",id),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);


            if (list.Any())
            {
                return list.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// 查询彩种类型名称
        /// </summary>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="id">彩种Id（上级评论Id）/文章 Id</param>
        /// <param name="articleUserId">发表计划用户Id type=1时</param>
        /// <param name="refCommentId">相关一级评论Id</param>
        /// <returns></returns>
        private DynamicRelatedInfoDto GetLotteryTypeName(int type, int id, int articleUserId, int refCommentId)
        {
            DynamicRelatedInfoDto info = null;
            if (type == 1)
            {
                info = new DynamicRelatedInfoDto();
                UserInfoService userService = new UserInfoService();
                if (refCommentId <= 0)
                {
                    //id为彩种Id
                    info.LType = id;
                    info.TypeName = Util.GetLotteryTypeName(id);

                    var user = userService.GetUserInfo(articleUserId);
                    if (user != null)
                    {
                        info.NickName = user.NickName;
                    }
                }
                else
                {
                    var comment = Util.GetEntityById<Comment>(refCommentId);
                    if (comment != null)
                    {
                        info.LType = comment.ArticleUserId;
                        info.TypeName = Util.GetLotteryTypeName(comment.ArticleUserId);

                        var user = userService.GetUserInfo(articleUserId);
                        if (user != null)
                        {
                            info.NickName = user.NickName;
                        }
                    }
                }

            }
            else
            {
                //查询文章的彩种类型
                string sql = @"select b.Id,d.TypeName, 2 as RelatedType from  news b 
                            left join NewsType c on c.Id=b.TypeId
                            left join LotteryType d on d.Id= c.lType
                            where b.Id=" + id;
                var list = Util.ReaderToList<DynamicRelatedInfoDto>(sql);
                if (list.Any())
                    info = list.First();
            }

            return info ?? new DynamicRelatedInfoDto();
        }

        /// <summary>
        /// 获取计划列表
        /// </summary>
        public PagedListP<BettingRecord> GetPlan(long uid, int ltype, int winState, int pageIndex, int pageSize, long userId)
        {
            string winStateWhere = "";
            int start = pageSize * (pageIndex - 1) + 1;
            int end = pageSize * pageIndex;
            if (winState > 0)
            {
                if (winState == 1)
                {
                    //未开奖
                    winStateWhere = " AND WinState=1";
                }
                else if (winState == 2)
                {
                    //已开奖
                    winStateWhere = " AND WinState in(3,4)";
                }
            }

            string ltypeWhere = "";
            if (ltype > 0) ltypeWhere = " AND lType=" + ltype;

            string sql = string.Format(@"SELECT * FROM ( 
	SELECT row_number() over(order by WinState,SubTime DESC,lType) as rowNumber,* FROM (
		SELECT distinct lType,Issue, (case WinState when 1 then 1 else 2 end) as WinState,SubTime FROM [dbo].[BettingRecord] a
		WHERE SubTime=(select max(SubTime) from [BettingRecord] b where a.lType=b.lType and a.Issue=b.Issue  and a.UserId=b.UserId) and UserId=@UserId{0}{1}
		) t
	) tt
WHERE rowNumber BETWEEN @Start AND @End", ltypeWhere, winStateWhere);

            if (uid == 0)
                uid = userId;

            var sqlParameters = new[]
            {
                    new SqlParameter("@UserId",uid),
                    new SqlParameter("@Start",start),
                    new SqlParameter("@End",end)
                };

            List<BettingRecord> list = Util.ReaderToList<BettingRecord>(sql, sqlParameters);


            string countSql = string.Format(@"SELECT count(1) FROM ( SELECT  distinct lType,Issue  FROM [dbo].[BettingRecord] 
                                WHERE UserId = {0}{1}{2} ) tt", uid, ltypeWhere, winStateWhere);
            object obj = SqlHelper.ExecuteScalar(countSql);
            int total = Convert.ToInt32(obj ?? 0);

            return new PagedListP<BettingRecord>(pageIndex, pageSize, total, list);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        public ApiResult ReplaceHeadImg(long resourceId, long userId)
        {
            string sql = "SELECT COUNT(1) FROM dbo.ResourceMapping WHERE Id=@ResourceId";
            SqlParameter[] paras = { new SqlParameter("@ResourceId", resourceId) };
            int count = ToolsHelper.ObjectToInt(SqlHelper.ExecuteScalar(sql, paras));
            if (count <= 0)
            {
                return new ApiResult(60011, "该资源ID不存在");
            }
            sql = string.Format("UPDATE dbo.ResourceMapping SET FkId={0},Type={1} WHERE Id=@ResourceId", userId, (int)ResourceTypeEnum.用户头像);
            int data = SqlHelper.ExecuteNonQuery(sql, paras);
            if (data > 0)
            {
                return new ApiResult(100, "success");
            }
            else
            {
                return new ApiResult(-999, "数据库出现错误。");
            }
        }

        /// <summary>
        /// 获取当前用户在粉丝榜的排名数据
        /// </summary>
        public ApiResult<FansResDto> GetMyFanBangRank(string type, long userId)
        {

          
            string strsql = string.Format(@" select  * from ( select top 100 row_number() over(order by count(1) desc  ) as Rank, count(1)as Number,Followed_UserId,Name as NickName,isnull(RPath,'/images/default_avater.png') as Avater
                             from Follow f 
                             left join UserInfo u on f.Followed_UserId=u.id
                             left join ResourceMapping r on (r.FkId=f.Followed_UserId and r.Type=@ResourceType)
                            where  f.Status=1  {0}
                             group by Followed_UserId,Name,RPath
                            )t
                            where t.Followed_UserId=@Followed_UserId", Tool.GetTimeWhere("FollowTime", type));

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@Followed_UserId",userId)
            };

            FansResDto fansbang = Util.ReaderToModel<FansResDto>(strsql, sp);

            if (fansbang == null)
            {
                FansResDto fansbangs = new FansResDto();
                string countsql = string.Format("select count(1) from Follow where Followed_UserId={0}  {1} and Status=1 ", userId, Tool.GetTimeWhere("FollowTime", type));
                int number = Convert.ToInt32(SqlHelper.ExecuteScalar(countsql));

                IndexResDto user = GetPersonalIndexData(userId);
                fansbangs.FollowedUserId = Convert.ToInt32(user.Id);
                fansbangs.NickName = user.NickName;
                fansbangs.Rank = 0;
                fansbangs.Number = number;
                fansbangs.Avater = user.Avater;
                fansbang = fansbangs;
            }

            return new ApiResult<FansResDto>() { Data = fansbang };
        }

        /// <summary>
        /// 获取邀请注册信息（已邀请人数、奖励金币、奖励查看券）
        /// </summary>
        public ApiResult<InvitationRegResDto> GetInvitationReg(long userId)
        {
            string strsql = @" select Number,Coin,Voucher from
                              (select count(1) as Number from UserInfo where Pid = @Pid) t1,
                              (select sum([Money]) as Coin from ComeOutRecord  where [UserId]=@UserId and [Type]=7) t2,
							  (select count(1) as Voucher from UserCoupon  where [UserId]=@UserId and FromType=2) t3";

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Pid",userId),
                new SqlParameter("@UserId",userId)
            };

            InvitationRegResDto irmodel = Util.ReaderToModel<InvitationRegResDto>(strsql, sp);

            return new ApiResult<InvitationRegResDto>()
            {
                Data = irmodel
            };
        }

        /// <summary>
        /// 获取的粉丝列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="lastId">上次拉取的Id最小值</param>
        /// <returns></returns>
        public List<MyFanResDto> GetMyFans(int pageSize, long userId, int lastId)
        {
            //当前登录用户Id，客户端自己获取
            //UserId和FollowedUserId为同一个值，为了兼容安卓和IOS已发布的问题
            string strsql = @"SELECT  a.Id,a.UserId,a.UserId as FollowedUserId,a.FollowTime,
            ISNULL(b.Name, '') AS NickName ,
            ISNULL(b.Autograph, '') AS Autograph ,
            ISNULL(c.RPath, '') AS Avater ,
            ( SELECT    COUNT(1)
                FROM      Follow
                WHERE     UserId = @Followed_UserId
                        AND Followed_UserId = a.UserId
                        AND Status = 1
            ) AS [Status]
    FROM    Follow AS a
            LEFT JOIN UserInfo b ON b.Id = a.UserId
            LEFT JOIN ResourceMapping c ON c.FkId = a.UserId
                                            AND c.Type = @Type
    WHERE   a.Followed_UserId = @Followed_UserId
            AND a.Status = 1";
            if (lastId > 0)
            {
                strsql += " AND a.Id < " + lastId;
            }

            strsql += " ORDER BY Id DESC";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Followed_UserId",userId),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像),
            };
            var list = Util.ReaderToList<MyFanResDto>(strsql, sp);

            list.ForEach(x =>
            {
                x.Isfollowed = x.Status;
            });
            return list;
        }

        /// <summary>
        /// 获取我的关注列表
        /// </summary>
        public List<MyFollowResDto> GetMyFollow(int pageSize, long userId, int lastId)
        {
            string strsql = string.Format(@"SELECT Top {0} a.* ,
                                    ISNULL(b.Name, '') AS NickName ,
                                    ISNULL(b.Autograph, '') AS Autograph ,
                                    ISNULL(c.RPath, '') AS Avater,
                                    a.Followed_UserId as FollowedUserId
                            FROM    Follow AS a
                                    LEFT JOIN UserInfo b ON b.Id = a.Followed_UserId
                                    LEFT JOIN ResourceMapping c ON c.FkId = a.Followed_UserId AND c.Type = @Type
                            WHERE   a.UserId = @UserId AND a.Status = 1", pageSize);

            if (lastId > 0)
            {
                strsql += " AND a.Id < " + lastId;
            }

            strsql += " ORDER BY Id DESC";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@Type",(int)ResourceTypeEnum.用户头像)
            };
            var result = Util.ReaderToList<MyFollowResDto>(strsql, sp);

            return result;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        public List<TaskResDto> GetTaskList(long userId)
        {
            List<TaskResDto> list = new List<TaskResDto>();
            string strsql = "select * from MakeMoneyTask where State=1";
            List<MakeMoneyTask> tasklist = Util.ReaderToList<MakeMoneyTask>(strsql);
            foreach (var item in tasklist)
            {
                TaskResDto tm = new TaskResDto();
                tm.Code = item.Code;
                tm.Id = item.Id;
                tm.TaskItem = item.TaskItem;
                tm.Coin = item.Coin;
                tm.Count = item.Count;
                tm.SubTime = item.SubTime;
                tm.CompletedCount = GetCompletedCount(item.Code, userId);
                list.Add(tm);
            }
            return list;
        }

        /// <summary>
        /// 获取任务完成数量
        /// </summary>
        private int GetCompletedCount(int taskid, long userId)
        {
            int result = 0;

            string strsql = "select CompletedCount from  UserTask where UserId =@UserId and TaskId=@TaskId ";
            SqlParameter[] sp = new SqlParameter[] {
                   new SqlParameter("@UserId",userId),
                   new SqlParameter("@TaskId",taskid)
            };
            result = Convert.ToInt32(SqlHelper.ExecuteScalar(strsql, sp));

            return result;
        }

        /// <summary>
        /// 邀请注册
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <returns></returns>
        public ApiResult<ShareDto> InvitationReg(long uid)
        {

            string webHost = BaseService.WebHost;
            var share = new ShareDto()
            {
                Title = "邀请注册",
                Describe = "万彩吧，助你壕梦成真！！新用户注册有惊喜",
                Link = string.Format("{0}/Home/Register/{1}", webHost, uid),
                Icon = string.Format("{0}/images/c8.png", webHost)
            };

            return new ApiResult<ShareDto>()
            {
                Data = share
            };

        }
    }
}
