namespace Qiuxun.C8.Api.Service.Common.Paging
{
    public interface IPageParameter
    {
        int EndIndex { get; }

        int MaxRecordCount { get; }

        int PageIndex { get; set; }

        int PageSize { get; set; }

        bool RequireTotalCount { get; set; }

        int StartIndex { get; }

        int TotalCount { get; set; }
    }
}

