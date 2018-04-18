using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// API异常
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        private Exception exception;

        public ApiException()
        {
        }

        public ApiException(Exception exception)
        {
            this.exception = exception;
        }

        public ApiException(int code, string desc) : base(desc)
        {
            this.Code = code;
            this.Desc = desc;
        }

        public ApiException(int code, string desc, Exception ex) : base(desc, ex)
        {
            this.Code = code;
            this.Desc = desc;
        }

        public int Code { get; set; }

        public string Desc { get; set; }
    }
}
