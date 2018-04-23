using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Plan.Request
{
    /// <summary>
    /// 最近竞猜请求类
    /// </summary>
    public class LastPlayReqDto
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long uid { get; set; }
        /// <summary>
        /// 玩法名称
        /// </summary>
        public string playName { get; set; }
    }
}
