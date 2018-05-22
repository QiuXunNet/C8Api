using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Model;
using System.Configuration;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 充值服务
    /// </summary>
    public class RechargeService
    {
        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="no">订单号</param>
        /// <param name="money">订单金额(单位：元)</param>
        /// <param name="payType">支付类型</param>
        /// <param name="userId">充值用户Id</param>
        public bool AddComeOutRecord(string no, int money, int payType,int userId)
        {
            string sql = @"insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime,PayType) 
                            values(@UserId,@OrderId,@Money,1,GETDATE(),@PayType);select @@identity;";

            var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@OrderId",no),
                    new SqlParameter("@Money",money*moneyToCoin),
                    new SqlParameter("@PayType",payType)
                 };

            var i = Convert.ToInt32(SqlHelper.ExecuteScalar(sql, regsp));

            return i > 0;
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="no">订单号</param>
        /// <param name="type"></param>
        public bool AlertComeOutRecord(string no, int payType)
        {
            try
            {
                //判断支付中的订单是否存在,如果不存在.则说明已经改变状态了
                string sql = "select * from ComeOutRecord where OrderId=@OrderId and PayType=@PayType and State=1";
                var list = Util.ReaderToList<ComeOutRecord>(sql, new SqlParameter[] { new SqlParameter("@OrderId", no), new SqlParameter("@PayType", payType) });

                if (!list.Any())
                {
                    return true;
                }
                sql = "";
                var money = list.FirstOrDefault().Money;
                var userId = list.FirstOrDefault().UserId;
                var addCoin = 0;  //需要增加的金币数
                var moneyToCoin = Convert.ToInt32(ConfigurationManager.AppSettings["MoneyToCoin"]);

                //每日任务完成充值100元任务
                if (money/moneyToCoin >= 100)
                {
                    var makeMoneyTaskList = Util.ReaderToList<MakeMoneyTask>("select top(1) * from MakeMoneyTask where Code=100");
                    if (makeMoneyTaskList != null && makeMoneyTaskList.Any())
                    {
                        try
                        {
                            var obj = SqlHelper.ExecuteScalar("select top(1) completedCount from usertask where UserId = @UserId and taskId = 100",
                                new SqlParameter[] { new SqlParameter("@UserId", userId) });

                            if (obj == null || (Convert.ToInt32(obj) + 1) == makeMoneyTaskList.FirstOrDefault().Count)//判断今日是否完任务
                            {
                                addCoin = makeMoneyTaskList.FirstOrDefault().Coin;
                                sql += "insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime) values(@UserId,100,@Money,8,GETDATE());";//插入领取任务记录
                            }
                            if (obj == null) //如果usertask表没有数据，则插入
                            {
                                sql += "insert into usertask (UserId,TaskId,CompletedCount) values (@UserId,100,1);";
                            }
                            else
                            {
                                sql += "update usertask set completedCount = completedCount +1 where UserId = @UserId and taskId = 100;";
                            }
                        }
                        catch (Exception) { }
                    }

                }

                sql += @"update ComeOutRecord set State = 3 where OrderId=@OrderId and PayType=@PayType;
                        update UserInfo set Coin = Coin + " + (money + addCoin) + " where Id =@UserId;";

                SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@OrderId",no),
                    new SqlParameter("@PayType",payType),
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@Money",addCoin)
                 };

                SqlHelper.ExecuteTransaction(sql, regsp);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new ApiException(ex);
            }
        }
    }
}
