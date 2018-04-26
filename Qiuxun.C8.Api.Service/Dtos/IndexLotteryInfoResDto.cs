using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 首页彩种信息
    /// </summary>
    public class IndexLotteryInfoResDto
    {
        /// <summary>
        /// 彩种Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LTypeName { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string OpenTime { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNum { get; set; }
        /// <summary>
        /// 开奖号码别名
        /// </summary>
        public string OpenNumAlias { get; set; }

        /// <summary>
        /// 当前期号
        /// </summary>
        public string CurrentIssue { get; set; }
    }
}
