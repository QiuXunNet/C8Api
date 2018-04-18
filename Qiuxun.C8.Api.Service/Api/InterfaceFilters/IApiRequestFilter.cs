using System.Net.Http;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{
    public interface IApiRequestFilter
    {
        HttpResponseMessage DoFilter(HttpRequestMessage request, RequestInfo requestInfo);
    }
}
