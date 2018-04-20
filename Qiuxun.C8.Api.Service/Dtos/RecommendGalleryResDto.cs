using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 图库详情推荐响应类
    /// </summary>
    public class RecommendGalleryResDto
    {
        /// <summary>
        /// 图库Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { get; set; }
    }
}
