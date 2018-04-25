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
        public DateTime SubTime { get; set; }
        /// <summary>
        /// Logo Url
        /// </summary>
        public string Logo
        {
            get { return Util.GetLotteryIconUrl(lType); }
        }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LTypeName
        {
            get { return Util.GetLotteryTypeName(lType); }
        }
        
    }
}
