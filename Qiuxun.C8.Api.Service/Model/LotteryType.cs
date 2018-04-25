using System;

namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描述：彩种分类实体
    /// </summary>
    public class LotteryType
    {
        /// <summary>
		/// 类型Id
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>		
        public string TypeName { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// Logo图地址
        /// </summary>
        public string Icon { get; set; }
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
        /// 描述
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
        /// 最后更改人
        /// </summary>		
        public string UpdateUser { get; set; }
        /// <summary>
        /// 更改时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// logo图片地址
        /// </summary>
        public string LogoPath { get; set; }
    }
}
