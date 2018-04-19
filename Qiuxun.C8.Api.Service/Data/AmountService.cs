﻿using Qiuxun.C8.Api.Public;
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
    /// 银行卡绑定提现数据服务
    /// </summary>
    public class AmountService
    {
        /// <summary>
        /// 根据当前登录人获取银行卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BankInfoResDto GetUserBankCard(int userId)
        {
            string sql = "select Id,TrueName,BankAccount,BankName from bankinfo where userId = @userId";
            var list = Util.ReaderToList<BankInfoResDto>(sql, new SqlParameter[] { new SqlParameter("@userId", userId) });
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        public bool AddBankCard(BankInfoResDto model)
        {
            var rows = Convert.ToInt32(SqlHelper.ExecuteScalar("select count(*) from bankinfo where UserId=@UserId",
                new SqlParameter[] { new SqlParameter("@UserId", model.UserId) }));

            string regsql = "";
            if (rows > 0)
            {
                regsql = @"update bankinfo set BankAccount=@BankAccount,BankName=@BankName,SubTime=@SubTime where UserId=@UserId";
            }
            else
            {
                regsql = @"insert into bankinfo (UserId,TrueName,BankAccount,BankName,SubTime)
                                    values (@UserId,@TrueName,@BankAccount,@BankName,@SubTime)";
            }

            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@TrueName",model.TrueName),
                    new SqlParameter("@BankAccount",model.BankAccount),
                    new SqlParameter("@BankName",model.BankName),
                    new SqlParameter("@SubTime",DateTime.Now)
                 };

            var i = SqlHelper.ExecuteNonQuery(regsql, regsp);

            return i > 0;

        }

        /// <summary>
        /// 根据用户Id获取我的可用佣金数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetMyCommission(int userId)
        {
            var sql1 = @"select isnull(sum([Money]),0)as MyYj from ComeOutRecord c 
                        inner join BettingRecord b on c.OrderId = b.Id
                        where b.[UserId]=@UserId and Type in(4,9)";

            var sql2 = @"select isnull(sum([Money]),0) from ComeOutRecord 
                        where UserId = @UserId and ((Type = 2 and State in(1,3)) or Type in(3,5))";

            SqlParameter[] regsp = new SqlParameter[]
            {
                new SqlParameter("@UserId",userId)
            };

            //我获取到的佣金
            var obtainCommission = Convert.ToInt32(SqlHelper.ExecuteScalar(sql1, regsp));

            //我已提现的佣金
            var useCommission = Convert.ToInt32(SqlHelper.ExecuteScalar(sql2, regsp));

            return obtainCommission - useCommission;
        }

        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="backId">银行卡Id</param>
        /// <param name="money">提现金额</param>
        /// <param name="userId">当前登录人Id</param>
        /// <returns></returns>
        public void AddExtractCash(int backId, int money,int userId)
        {
            //插入提现记录语句
            string sql = @"insert into ComeOutRecord (UserId,OrderId,Money,Type,SubTime,PayType,State) 
                            values(@UserId,@OrderId,@Money,2,GETDATE(),3,1);";

            //修改用户表中Coin字段
            sql += "update userinfo set Coin = Coin-@Money where Id=@UserId;";

            SqlParameter[] regsp = new SqlParameter[] {
                            new SqlParameter("@UserId",userId),
                            new SqlParameter("@OrderId",backId),
                            new SqlParameter("@Money",money)
                        };

            SqlHelper.ExecuteTransaction(sql, regsp);
        }
    }
}
