using System.Collections.Generic;

namespace Qiuxun.C8.Api.Service.Common.Paging
{


    public static class PagedListExtension
    {
        public static PagedListDto<T> GetPageListDto<T>(this IEnumerable<T> source, int pageIndex, int pageSize) where T : class, new()
        {
            return new PagedListDto<T> { PageData = new List<T>(source), PageIndex = pageIndex, PageSize = pageSize };
        }

        public static IPagedList<T> ToPageList<T>(this IEnumerable<T> source, int pageIndex, int pageSize) where T : class, new()
        {
            return new PagedList<T>(source, pageIndex, pageSize);
        }

        public static IPagedList<T> ToPageList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount) where T : class, new()
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        public static PagedListDto<T> ToPageListDto<T>(this IEnumerable<T> source, int pageIndex, int pageSize) where T : class, new()
        {
            return new PagedList<T>(source, pageIndex, pageSize).GetPagedListDto();
        }

        public static PagedListDto<T> ToPageListDto<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount) where T : class, new()
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount).GetPagedListDto();
        }
    }
}

