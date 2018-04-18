using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.Securities
{
    public class RsaLoResult : ApiResult
    {
        public static RsaLoResult CopyFrom(ApiResult result, string sign)
        {
            return new RsaLoResult { Code = result.Code, Desc = result.Desc, Sign = sign };
        }

        public string Sign { get; set; }
    }

    public class RsaLoResult<T> : ApiResult<T>
    {
        public static RsaLoResult<T> CopyFrom(ApiResult<T> result, string sign)
        {
            return new RsaLoResult<T> { Code = result.Code, Desc = result.Desc, Data = result.Data, Sign = sign };
        }

        public string Sign { get; set; }
    }
}
