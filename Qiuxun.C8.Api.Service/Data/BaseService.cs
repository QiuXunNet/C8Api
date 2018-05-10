using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Model;
using Qiuxun.C8.Caching;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 通用业务类
    /// </summary>
    public class BaseService
    {
        public static string WebHost = ConfigurationManager.AppSettings["webHost"];
        public static string ApiHost = ConfigurationManager.AppSettings["apiHost"];
        /// <summary>
        /// 获取贴子点阅扣费配置表
        /// </summary>
        /// <returns></returns>
        public List<LotteryCharge> GetLotteryCharge()
        {
            string memKey = "base_lottery_charge_settings";
            var list = CacheHelper.GetCache<List<LotteryCharge>>(memKey);
            if (list != null && list.Any()) return list;

            string sql = "SELECT Id,lType,MinIntegral,MaxIntegral,Coin FROM dbo.LotteryCharge";
            list = Util.ReaderToList<LotteryCharge>(sql);

            if (list != null)
            {
                //CacheHelper.WriteCache(memKey, list);
                CacheHelper.AddCache(memKey, list);
                return list;
            }

            return new List<LotteryCharge>();
        }

        /// <summary>
        /// 获取分佣配置
        /// </summary>
        /// <returns></returns>
        public List<CommissionSetting> GetCommissionSetting()
        {
            string memKey = "base_commission_settings";
            var list = CacheHelper.GetCache<List<CommissionSetting>>(memKey);
            if (list == null || list.Count < 1)
            {
                string sql = "SELECT [Id],[lType],[Percentage],[Type] FROM [dbo].[SharedRevenue] WHERE IsDeleted=0";
                list = Util.ReaderToList<CommissionSetting>(sql);
                if (list != null)
                {
                   // CacheHelper.WriteCache(memKey, list, 60);
                    CacheHelper.AddCache(memKey, list, 60);
                }
            }

            return list ?? new List<CommissionSetting>();
        }

    }
}
