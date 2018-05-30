using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 排行榜数据服务类
    /// </summary>
    public class LeaderboardService
    {
        /// <summary>
        /// 获取积分榜数据
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        public List<RankIntegralResDto> GetIntegralList(string queryType)
        {
            string strsql = string.Format(@"
                select top 100 row_number() over(order by Sum(Score) DESC) as [Rank],Sum(Score)Score,UserId,NickName,Avater from
                (
                  SELECT  UserId, Date, Score,b.Name as NickName,isnull(c.RPath,'') as Avater 
                  FROM dbo.SuperiorRecord a
                  join UserInfo b on b.Id=a.UserId
                  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=@ResourceType
                 )t
                 where 1=1   {0}
                 group by UserId,NickName,Avater", Tool.GetTimeWhere("Date", queryType));

            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像)
            };

            return Util.ReaderToList<RankIntegralResDto>(strsql, sp);
        }

        /// <summary>
        /// 获取自己的积分排名
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public RankIntegralResDto GetMyRank(string queryType, int userId)
        {
            string strsql = string.Format(@"
                              SELECT isnull(sum(Score),0)
                                  FROM dbo.SuperiorRecord a                                
                                  where 1=1   and userId = @UserId  {0} ",
                 Tool.GetTimeWhere("Date", queryType));

            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter ("@UserId",userId)
            };

            var score = SqlHelper.ExecuteScalar(strsql, sp);

            return new RankIntegralResDto()
            {
                UserId = userId,
                Score = score.ToInt32()
            };
        }

        /// <summary>
        /// 获取一级彩种
        /// </summary>
        /// <returns></returns>
        public List<LotteryType2> GetLotteryTypeList()
        {
            string strsql = @"select * from LotteryType2 where PId=0  order by Position ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            return list;
        }

        /// <summary>
        /// 获取非一级彩种(小彩种)
        /// </summary>
        /// <returns></returns>
        public List<LotteryType2> GetChildLotteryTypeList()
        {
            string strsql = @"select * from LotteryType2 where PId<>0  order by Position ";
            var list = Util.ReaderToList<LotteryType2>(strsql);
            return list;
        }

        /// <summary>
        /// 根据彩种类型和查询类型高手榜单
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="lType">小彩种类型</param>
        /// <returns></returns>
        public List<RankIntegralResDto> GetHighMasterList(string queryType, int lType)
        {

            string strsql = string.Format(@"
                  select Top 100 row_number() over(order by Score DESC ) as [Rank], * from (
                  SELECT isnull(sum(a.Score),0) as Score,a.UserId, a.lType,b.Name as NickName,c.RPath as Avater 
                  FROM dbo.SuperiorRecord a
                  join UserInfo b on b.Id=a.UserId
                  left join ResourceMapping c on c.FkId=a.UserId and c.[Type]=2
                  WHERE a.lType=@lType {0}
                  GROUP BY a.lType,a.UserId,b.Name,c.RPath
              ) tt WHERE Score > 0", Tool.GetTimeWhere("Date", queryType));

            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType)
            };

            return Util.ReaderToList<RankIntegralResDto>(strsql, sp);
        }

        /// <summary>
        /// 查询当前登录人在高手榜的分数
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="lType">小彩种类型</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public RankIntegralResDto GetMyHighMaster(string queryType, int lType, int userId)
        {

            string sql = string.Format(@"SELECT isnull(sum(Score),0)
                  FROM dbo.SuperiorRecord
                  WHERE lType={0} and UserId={1}{2}", lType, userId, Tool.GetTimeWhere("Date", queryType));

            object score = SqlHelper.ExecuteScalar(sql);

            return new RankIntegralResDto()
            {
                UserId = userId,
                Score = score.ToInt32()
            };
        }

        /// <summary>
        /// 获取盈利打赏榜数据
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="RType">4:盈利榜   9:打赏榜</param>
        /// <param name="lType">小彩种类型</param>
        /// <returns></returns>
        public List<RankIntegralResDto> GetProfitRewardList(string queryType, int RType, int lType)
        {
            string strsql = string.Format(@"select top 100  row_number() over(order by sum(Money) desc  )as Rank,sum(Money)as Score,UserId,NickName,Avater from
                    (select 
                    b.UserId,a.Money,b.lType,c.Name as NickName,d.RPath as Avater  from [dbo].[ComeOutRecord] a
                    left join BettingRecord b on b.Id=a.OrderId 
                    join UserInfo c on c.Id=b.UserId
                    left join ResourceMapping d on d.FkId=b.UserId and d.[Type]=@ResourceType
                    where a.Type=@RType and b.lType=@lType
                    {0}

                    )t
                    group by t.UserId ,t.NickName,t.Avater,t.lType
                    order by Score desc,NickName asc
                    ", Tool.GetTimeWhere("a.SubTime", queryType));
            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@RType",RType),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@lType",lType)
            };
            List<RankIntegralResDto> list = Util.ReaderToList<RankIntegralResDto>(strsql, sp);

            return list;
        }

        /// <summary>
        /// 获取盈利打赏榜数据
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="RType">4:盈利榜   9:打赏榜</param>
        /// <param name="lType">小彩种类型</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public RankIntegralResDto GetMyProfitReward(string queryType, int RType, int lType, int userId)
        {
            string strsql = string.Format(@"select isnull(sum(a.Money),0) 
                            from [dbo].[ComeOutRecord] a
                            left join BettingRecord b on cast( b.Id as nvarchar(50))=a.OrderId                          
                            where a.Type=@RType and b.lType=@lType and b.UserId=@UserId  {0} ", Tool.GetTimeWhere("a.SubTime", queryType));
            SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@RType",RType),
                new SqlParameter("@lType",lType),
                new SqlParameter("@UserId",userId)
            };            

            var score = SqlHelper.ExecuteScalar(strsql, sp);

            return new RankIntegralResDto()
            {
                UserId = userId,
                Score = score.ToInt32()
            };
        }
    }
}
