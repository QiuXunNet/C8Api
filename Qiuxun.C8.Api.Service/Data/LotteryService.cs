using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Service.Data
{
    public class LotteryService
    {
        /// <summary>
        /// 获取所有彩种分类
        /// </summary>
        /// <returns></returns>
        public List<LotteryType2> GetAllLotteryTypeList()
        {
            string sql = "select * from LotteryType2 order by Position";
            return Util.ReaderToList<LotteryType2>(sql);
        }

        /// <summary>
        /// 根据上级Id获取彩种分类
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<LotteryType2> GetLotteryTypeList(int pid = 0)
        {
            string sql = "select * from LotteryType2 where PId = " + pid + " order by Position";
            return Util.ReaderToList<LotteryType2>(sql);
        }
    }
}
