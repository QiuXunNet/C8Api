using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class NewsTypeResDto
    {
        /// <summary>
		/// 分类Id
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>		
        public string TypeName { get; set; }
        /// <summary>
        /// 上级分类Id
        /// </summary>		
        public long ParentId { get; set; }
        /// <summary>
        /// 排序编码
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// 彩种分类ID
        /// </summary>		
        public long LType { get; set; }

        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LTypeName { get; set; }
        /// <summary>
        /// 分类层级
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 文章显示方式 1=普通文章 2=图库
        /// </summary>
        public int ShowType { get; set; }
    }
}
