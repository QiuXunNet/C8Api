using System.Collections.Generic;

namespace Qiuxun.C8.Api.Service.Common.Paging
{


    public class PagedListDto<T>
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public IList<T> PageData;
        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtraData { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页码
        /// </summary>
        public int TotalPages { get; set; }
    }
}

