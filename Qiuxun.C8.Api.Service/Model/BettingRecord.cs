using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Newtonsoft.Json;

namespace Qiuxun.C8.Api.Model
{
    public class BettingRecord
    {
        public long Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public string BigPlayName { get; set; }
        public string PlayName { get; set; }
        public string BetNum { get; set; }
        public int WinState { get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public DateTime SubTime { get; set; }

        public string LogoIndex
        {
            get { return Util.GetLotteryIcon(lType); }
        }

        public string LotteryTypeName
        {
            get { return Util.GetLotteryTypeName(lType); }
        }

        public string TimeStr
        {
            get { return SubTime.ToString("MM-dd HH:mm"); }
        }
    }
}
