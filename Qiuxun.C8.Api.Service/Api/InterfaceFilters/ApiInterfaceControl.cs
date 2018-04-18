using System.Configuration;
using System.Net.Http;
using log4net;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{


    public class ApiInterfaceControl : IApiRequestFilter
    {
        private static readonly ClientIpType _clientIpType;
        private static readonly object lockObj = new object();
        private static ILog logger = LogManager.GetLogger("接口控制");

        static ApiInterfaceControl()
        {
            if (!System.Enum.TryParse<ClientIpType>(ConfigurationManager.AppSettings["client_ip_switch"], out _clientIpType))
            {
                _clientIpType = ClientIpType.TcpIp;
            }
        }

        public HttpResponseMessage DoFilter(HttpRequestMessage request, RequestInfo requestInfo)
        {
            //TODO:IP限制过滤

            return null;
        }
    }
}

