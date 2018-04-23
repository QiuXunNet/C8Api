using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Plan.Request
{
    /// <summary>
    /// 发布计划请求类
    /// </summary>
    public class BetReqDto
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 当前期号
        /// </summary>
        public string currentIssue { get; set; }
        /// <summary>
        /// 计划投注内容
        /// </summary>
        public string betInfo { get; set; }
    }
}
