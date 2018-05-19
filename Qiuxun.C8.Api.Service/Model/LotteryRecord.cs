using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Public;

namespace Qiuxun.C8.Api.Model
{
    public partial class LotteryRecord
    {
        public int Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public string Num { get; set; }
        public System.DateTime SubTime { get; set; }

        public string ShowOpenTime
        {
            get { return LotteryTime.GetTime(lType.ToString()); }
        }
    }
}
