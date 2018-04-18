using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ApiResult
    {
        public ApiResult()
        {
            this.Code = 100;
            this.Desc = "成功";
        }

        public ApiResult(int code) : this(code, null)
        {
        }

        public ApiResult(int code, string desc)
        {
            this.Code = code;
            this.Desc = desc;
        }

        public ApiResult CopyStatus(ApiResult result)
        {
            this.Code = result.Code;
            this.Desc = result.Desc;
            return this;
        }

        public int Code { get; set; }

        public string Desc { get; set; }

        public bool IsSuccess
        {
            get { return (this.Code == 100); }
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public ApiResult()
        {
        }

        public ApiResult(int code) : base(code, null)
        {
        }

        public ApiResult(int code, string desc) : base(code, desc)
        {
        }

        public ApiResult<T> CopyStatus(ApiResult result)
        {
            base.Code = result.Code;
            base.Desc = result.Desc;
            return (ApiResult<T>)this;
        }

        public T Data { get; set; }
    }
}
