using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// PV EV  IP 保存
    /// </summary>
    public class FriendshipLinksService
    {
        public int GetLinkVisitRecordNum(int id)
        {
            string visitRecordSql = "select count(*) from dbo.LinkVisitRecord where RefId=@RefId and SubTime=@Date and [Type]=1";
            //添加
            var sqlParameter = new[]
            {
                        new SqlParameter("@RefId",id),
                        new SqlParameter("@Date",DateTime.Today),
                };
            var count = Convert.ToInt32(SqlHelper.ExecuteScalar(visitRecordSql, sqlParameter));
            return count;
        }

        /// <summary>
        /// 添加Ip数量
        /// </summary>
        /// <param name="id"></param>
        public void AddIp(int id)
        {
            var count = GetLinkVisitRecordNum(id);

            if (count == 0)
            {
                //新增
                string insertRecordSql = string.Format(
                        @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},0,0,1,0,0,1,'{1}')", id, DateTime.Today);
                SqlHelper.ExecuteScalar(insertRecordSql);

            }
            else
            {
                //修改
                string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set IP+=1 where RefId={0} and SubTime = '{1}' and [Type]=1 ", id, DateTime.Today);
                SqlHelper.ExecuteScalar(updateRecordSql);
            }
        }

        /// <summary>
        /// 添加PV数量
        /// </summary>
        /// <param name="id"></param>
        public void AddPv(int id,int pvCount)
        {
            var count = GetLinkVisitRecordNum(id);

            if (count == 0)
            {
                //新增
                string insertRecordSql = string.Format(
                        @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},0,0,0,{1},0,1,'{2}')", id, pvCount, DateTime.Today);
                SqlHelper.ExecuteScalar(insertRecordSql);

            }
            else
            {
                //修改
                string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set PV+={2} where RefId={0} and SubTime = '{1}' and [Type]=1 ", id, DateTime.Today, pvCount);
                SqlHelper.ExecuteScalar(updateRecordSql);
            }
        }

        /// <summary>
        /// 添加UV数量
        /// </summary>
        /// <param name="id"></param>
        public void AddUv(int id,int uvCount)
        {
            var count = GetLinkVisitRecordNum(id);

            if (count == 0)
            {
                //新增
                string insertRecordSql = string.Format(
                        @"insert into dbo.LinkVisitRecord (RefId,ClickCount,UV,IP,PV,RegCount,Type,SubTime)
                        values({0},0,{1},0,0,0,1,'{2}')", id, uvCount, DateTime.Today);
                SqlHelper.ExecuteScalar(insertRecordSql);

            }
            else
            {
                //修改
                string updateRecordSql = string.Format(@"update dbo.LinkVisitRecord set UV+={2} where RefId={0} and SubTime = '{1}' and [Type]=1 ", id, DateTime.Today, uvCount);
                SqlHelper.ExecuteScalar(updateRecordSql);
            }
        }

        /// <summary>
        /// 查询所有友联
        /// </summary>
        /// <returns></returns>
        public List<FriendLink> GetFriendLink()
        {
            var friendLinkList = Util.ReaderToList<FriendLink>("select * from dbo.FriendLink where [Type]=1 and state = 0 ");

            return friendLinkList;
        }
    }
}
