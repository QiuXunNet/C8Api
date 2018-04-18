using System.Configuration;

namespace Qiuxun.C8.Api.Service.Common.Paging
{
    public class ApiPageParameter : IPageParameter
    {
        private static int limitMaxTotalCount = (int.TryParse(ConfigurationManager.AppSettings["Common.Paging.MaxTotalCount"], out value) ? value : 0x4e20);
        private int pageIndex;
        private static int pageIndexMax = (int.TryParse(ConfigurationManager.AppSettings["Common.Paging.MaxPageIndex"], out value) ? value : 100);
        private int pageSize;
        private static int pageSizeMax = (int.TryParse(ConfigurationManager.AppSettings["Common.Paging.MaxPageSize"], out value) ? value : 0x7d0);
        private bool requireTotalCount;
        private int totalCount;
        private static int value = 0;

        public ApiPageParameter()
        {
            this.requireTotalCount = true;
        }

        public ApiPageParameter(int maxPageIndex, int maxPageSize, int maxTotalCount)
        {
            this.requireTotalCount = true;
            pageIndexMax = maxPageIndex;
            pageSizeMax = maxPageSize;
            limitMaxTotalCount = maxTotalCount;
        }

        public int EndIndex
        {
            get
            {
                return ((this.PageIndex + 1) * this.PageSize);
            }
        }

        public int MaxRecordCount
        {
            get
            {
                return limitMaxTotalCount;
            }
        }

        public int PageIndex
        {
            get
            {
                if ((this.pageIndex >= 0) && (this.pageIndex <= pageIndexMax))
                {
                    return this.pageIndex;
                }
                return pageIndexMax;
            }
            set
            {
                this.pageIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                if ((this.pageSize >= 1) && (this.pageSize <= pageSizeMax))
                {
                    return this.pageSize;
                }
                return pageSizeMax;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public bool RequireTotalCount
        {
            get
            {
                return this.requireTotalCount;
            }
            set
            {
                this.requireTotalCount = value;
            }
        }

        public int SkipIndex
        {
            get
            {
                return (this.PageIndex * this.PageSize);
            }
        }

        public int StartIndex
        {
            get
            {
                return ((this.PageIndex * this.PageSize) + 1);
            }
        }

        public int TotalCount
        {
            get
            {
                return this.totalCount;
            }
            set
            {
                this.totalCount = value;
            }
        }
    }
}

