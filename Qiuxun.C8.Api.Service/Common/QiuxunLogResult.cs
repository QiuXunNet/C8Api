using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qiuxun.C8.Api.Service.Common
{
    public class QiuxunLogResult : ApiResult
    {
        public QiuxunLogResult(int code, string desc, string logDesc) : base(code, desc)
        {
            this.LogCode = code;
            this.LogDesc = logDesc;
        }

        public QiuxunLogResult(int code, string desc, int logCode, string logDesc) : base(code, desc)
        {
            this.LogCode = logCode;
            this.LogDesc = logDesc;
        }

        public QiuxunLogResult(int code, string desc, int logCode, string logDesc, string logDescDetail) : base(code, desc)
        {
            this.LogCode = logCode;
            this.LogDesc = logDesc;
            this.LogDescDetail = logDescDetail;
        }

        public QiuxunLogResult(int code, string desc, int logCode, string logDesc, string logDescDetail, int serverCode) : base(code, desc)
        {
            this.LogCode = logCode;
            this.LogDesc = logDesc;
            this.LogDescDetail = logDescDetail;
            this.ServerCode = serverCode;
        }

        [JsonIgnore]
        public int LogCode { get; set; }

        [JsonIgnore]
        public string LogDesc { get; set; }

        [JsonIgnore]
        public string LogDescDetail { get; set; }

        [JsonIgnore]
        public int ServerCode { get; set; }
    }
}
