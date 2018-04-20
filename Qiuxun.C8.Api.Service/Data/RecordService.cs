using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 开奖历史数据服务
    /// </summary>
    public class RecordService
    {
        public List<LotteryRecordRseDto> GetRecordList(ref int totalCount, int lType,int pageIndex,int pageSize, string beginTime,string endTime ="")
        {
            string sql = "";
            string sqlCount = "";
            if (lType >= 9)
            {
                sql = @"select lType,Issue,Num from (
                        select row_number()over(order by Issue desc) as rownumber,lType,Issue,Num 
                        from LotteryRecord 
                        where lType=@lType and SubTime > @BeginTime and SubTime < @EndTime 
                        order by Issue desc
                        ) tab where tab.rownumber between (@PageIndex - 1)*@PageSize and @PageIndex*@PageSize";
                    
                sqlCount = "select count(*) from LotteryRecord where lType=@lType and SubTime > @BeginTime and SubTime < @EndTime ";
            }
            else
            {
                sql = @"select lType,Issue,Num from (
                        select row_number()over(order by Issue desc) as rownumber,lType,Issue,Num 
                        from LotteryRecord 
                        where lType=@lType and SubTime > @BeginTime
                        order by Issue desc
                        ) tab where tab.rownumber between (@PageIndex - 1)*@PageSize and @PageIndex*@PageSize";

                sqlCount = "select count(*) from LotteryRecord where lType=@lType and SubTime > @BeginTime ";
            }

            var pms = new SqlParameter[] {
                new SqlParameter("@lType",lType),
                new SqlParameter("@BeginTime",beginTime),
                new SqlParameter("@EndTime",endTime),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize)
            };

            totalCount = Convert.ToInt32(SqlHelper.ExecuteScalar(sqlCount, pms));

            return Util.ReaderToList<LotteryRecordRseDto>(sql, pms);
        }
    }
}
