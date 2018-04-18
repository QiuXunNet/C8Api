using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 专家
    /// </summary>
    public class Expert
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 玩法名称
        /// </summary>
        public string PlayName { get; set; }
        /// <summary>
        /// 玩法总积分
        /// </summary>
        public int PlayTotalScore { get; set; }
        /// <summary>
        /// 彩种总积分
        /// </summary>
        public int LTypeTotalScore { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }
        /// <summary>
        /// 命中率
        /// </summary>
        public decimal HitRate { get; set; }
        /// <summary>
        /// 命中率描述（10中6）
        /// </summary>
        public string HitRateDesc { get; set; }
        /// <summary>
        /// 上期是否中
        /// </summary>
        public bool LastWin { get; set; }
        /// <summary>
        /// 最大连中
        /// </summary>
        public int MaxWin { get; set; }

        /// <summary>
        /// 排名编号
        /// </summary>
        public int RowNumber { get; set; }
    }
}
