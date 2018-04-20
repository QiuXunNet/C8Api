using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Plan.Request
{
    /// <summary>
    /// 专家列表请求类
    /// </summary>
    public class ExpertListReqDto
    {
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 玩法名称
        /// </summary>
        public string playName { get; set; }
        /// <summary>
        /// 类型 1=高手推荐 2=免费专家
        /// </summary>
        public int type { get; set; } = 1;
        /// <summary>
        /// 页码
        /// </summary>
        public int pageIndex { get; set; } = 1;
        /// <summary>
        /// 页数据量
        /// </summary>
        public int pageSize { get; set; } = 20;
    }
}
