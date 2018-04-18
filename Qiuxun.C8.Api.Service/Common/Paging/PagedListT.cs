using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qiuxun.C8.Api.Service.Common.Paging
{


    public class PagedList<T> : List<T>, IPagedList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IPagedList
    {
        private int totalCount;

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            this.totalCount = source.Count<T>();
            this.TotalPages = this.TotalCount / pageSize;
            if ((this.TotalCount % pageSize) > 0)
            {
                this.TotalPages++;
            }
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            base.AddRange(source.Skip<T>((pageIndex * pageSize)).Take<T>(pageSize).ToList<T>());
        }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            this.totalCount = totalCount;
            this.TotalPages = this.TotalCount / pageSize;
            if ((this.TotalCount % pageSize) > 0)
            {
                this.TotalPages++;
            }
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            base.AddRange(source);
        }

        public PagedListDto<T> GetPagedListDto()
        {
            return new PagedListDto<T> { PageIndex = this.PageIndex, PageSize = this.PageSize, TotalCount = this.TotalCount, TotalPages = this.TotalPages, HasPreviousPage = this.HasPreviousPage, HasNextPage = this.HasNextPage, ExtraData = this.ExtraData, PageData = this };
        }

        public PagedListDto<T2> GetPagedListDto<T2>(Func<T, T2> set)
        {
            PagedListDto<T2> dto = new PagedListDto<T2>
            {
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                TotalCount = this.TotalCount,
                TotalPages = this.TotalPages,
                HasPreviousPage = this.HasPreviousPage,
                HasNextPage = this.HasNextPage,
                ExtraData = this.ExtraData
            };
            List<T2> list = new List<T2>();
            foreach (T local in this)
            {
                list.Add(set(local));
            }
            dto.PageData = list;
            return dto;
        }

        public IPagedList<T2> ToPagedList<T2>(Func<T, T2> set)
        {
            List<T2> source = new List<T2>();
            foreach (T local in this)
            {
                source.Add(set(local));
            }
            return new PagedList<T2>(source, this.PageIndex, this.PageSize, this.TotalCount);
        }

        public object ExtraData { get; set; }

        public bool HasNextPage
        {
            get
            {
                return ((this.PageIndex + 1) < this.TotalPages);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 1);
            }
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount
        {
            get
            {
                return this.totalCount;
            }
            set
            {
                this.totalCount = value;
                this.TotalPages = this.TotalCount / this.PageSize;
                if ((this.TotalCount % this.PageSize) > 0)
                {
                    this.TotalPages++;
                }
            }
        }

        public int TotalPages { get; private set; }
    }
}

