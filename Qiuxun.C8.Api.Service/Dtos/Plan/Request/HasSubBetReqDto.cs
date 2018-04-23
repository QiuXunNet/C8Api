using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Plan.Request
{
    /// <summary>
    /// 查询用户是否发表过计划 请求类
    /// </summary>
    public class HasSubBetReqDto
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long userId { get; set; }
        /// <summary>
        /// 玩法名称（没有传空）
        /// </summary>
        public string playName { get; set; }
        /// <summary>
        /// 1=未开奖计划 2=全部计划
        /// </summary>
        public int type { get; set; }
    }
}
