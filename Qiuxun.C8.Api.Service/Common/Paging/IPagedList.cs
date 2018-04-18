
namespace Qiuxun.C8.Api.Service.Common.Paging
{
    public interface IPagedList
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        object ExtraData { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 总数量
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 总页码
        /// </summary>
        int TotalPages { get; }
    }
}

