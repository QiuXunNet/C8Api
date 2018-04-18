using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// 未授权异常
    /// </summary>
    [Serializable]
    public class ApiUnauthorizedException : ApiException
    {
        public ApiUnauthorizedException()
        {
            base.Code = 401;
            base.Desc = "无权限，需要登录后操作";
        }

        public ApiUnauthorizedException(int code, string desc) : base(code, desc)
        {
            base.Code = code;
            base.Desc = desc;
        }

        public ApiUnauthorizedException(int code, string desc, Exception ex) : base(code, desc, ex)
        {
            base.Code = code;
            base.Desc = desc;
        }
    }
}
