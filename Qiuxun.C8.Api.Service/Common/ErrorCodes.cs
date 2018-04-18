using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    public static class ErrorCodes
    {
        public const int CommonBusinessFailure = 99999;
        public const int CommonFailure = -100;
        public const string CommonFailureDesc = "未知错误，请联系管理员";
        public const int CommonSuccess = 100;
        public const string CommonSuccessDesc = "成功";
    }
}
