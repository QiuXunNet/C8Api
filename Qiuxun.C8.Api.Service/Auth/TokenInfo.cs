using System;

namespace Qiuxun.C8.Api.Service.Auth
{
    public class TokenInfo
    {
        public DateTime AuthTime { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserCode { get; set; }
    }
}
