using System;
using System.Collections;
using System.Collections.Generic;

namespace Qiuxun.C8.Api.Service.Common.Paging
{


    public interface IPagedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IPagedList
    {
        PagedListDto<T> GetPagedListDto();
        PagedListDto<T2> GetPagedListDto<T2>(Func<T, T2> set);
        IPagedList<T2> ToPagedList<T2>(Func<T, T2> set);
    }
}

