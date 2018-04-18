using System.Configuration;
using System.Net;
using System.Net.Http;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{
    

    public class HttpHeaderCheckFilter : IApiRequestFilter
    {
        public static int checkWeixinHttps = (int.TryParse(ConfigurationManager.AppSettings["RequestLimit_Weixin_Https"], out checkWeixinHttps) ? checkWeixinHttps : 1);

        public HttpResponseMessage DoFilter(HttpRequestMessage request, RequestInfo requestInfo)
        {
            if (((checkWeixinHttps > 0) && (requestInfo.ClientSourceId == 100)) && !requestInfo.IsHttps)
            {
                return request.CreateResponse<ApiResult>(HttpStatusCode.BadRequest, new ApiResult(0x15f90, "请求方法错误，接口不可用，请联系管理员。"));
            }
            return null;
        }
    }
}

