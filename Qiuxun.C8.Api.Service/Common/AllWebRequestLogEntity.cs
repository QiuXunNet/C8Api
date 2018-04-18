using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    public class AllWebRequestLogEntity : RequestLog
    {
        public string Content { get; set; }

        public string AppName { get; set; }

        public string ModelName { get; set; }

        public int Level { get; set; }
    }

    public class WebApiRequestLogEntity : AllWebRequestLogEntity
    {
        public string ResponseData { get; set; }
    }
}
