using System;

namespace Qiuxun.C8.Api.Model.News
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月12日
    /// 描 述：图库分类
    /// </summary>
    public class GalleryType
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
        /// 彩种分类
        /// </summary>
        public long LType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 检索字母
        /// </summary>
        public string QuickQuery { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 最新期号
        /// </summary>
        public string LastIssue { get; set; }
    }
}
