using System;

namespace Qiuxun.C8.Api.Service.Model.News
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描述：新闻类型实体类
    /// </summary>
    public class NewsType
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
        /// <summary>
        /// SEO标题
        /// </summary>
        public string SeoSubject { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public string SeoKeyword { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        public string SeoDescription { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>		
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>		
        public string UpdateUser { get; set; }
        /// <summary>
        /// 更改时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
    }
}
