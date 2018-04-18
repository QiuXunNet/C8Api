using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 点阅配置表
    /// </summary>
    [Serializable]
    public class LotteryCharge
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 起始积分
        /// </summary>
        public int MinIntegral { get; set; }
        /// <summary>
        /// 截至积分
        /// </summary>
        public int MaxIntegral { get; set; }
        /// <summary>
        /// 金币数量
        /// </summary>
        public int Coin { get; set; }

    }
}
