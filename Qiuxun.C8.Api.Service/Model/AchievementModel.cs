using System.Collections.Generic;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Service.Model
{

    public class AchievementModel
    {
        public LotteryNum LotteryNum { get; set; }//开奖号

        public List<BettingRecord> BettingRecord { get; set; }//成绩

    }


    public class LotteryNum
    {
        public string SubTime { get; set; }

        public int lType { get; set; }
        public string Issue { get; set; }
        public string Num { get; set; }
        /// <summary>
        /// 开奖号码别名
        /// </summary>
        public string NumAlias {
            get { return Util.GetShowInfo(lType, Num); }
        }
    }
}
