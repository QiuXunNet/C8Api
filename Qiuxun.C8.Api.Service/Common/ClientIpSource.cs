using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    public class ClientIpSource
    {
        public string ClientIpFromHttp { get; set; }

        public string ClientIpFromTcpIp { get; set; }

        public bool IsHttps { get; set; }
    }
}
