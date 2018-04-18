using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Public
{
    public class LuoUtil
    {
        /// <summary>
        /// 获取用户积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public static int GetUserIntegral(int userId, int lType)
        {
            string totalIntegralByLtypeSql = string.Format(@"select isnull(sum(score),0) 
        from dbo.BettingRecord where lType={0} and UserId={1} and WinState in(3,4)", lType, userId);
            object objTotalIntegral = SqlHelper.ExecuteScalar(totalIntegralByLtypeSql);
            int totalIntegral = objTotalIntegral != null ? Convert.ToInt32(objTotalIntegral) : 0;
            return totalIntegral;
        }
        
    }
}
