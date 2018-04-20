using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class GalleryTypeResDto
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 检索字母
        /// </summary>
        public string QuickQuery { get; set; }
        
        /// <summary>
        /// 最新期号
        /// </summary>
        public string LastIssue { get; set; }
    }
}
