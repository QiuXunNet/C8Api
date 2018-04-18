using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Auth;

namespace Qiuxun.C8.Api.Service.Common
{
    public class ApiAuthContainer : AuthContainerBase
    {
        private HttpRequestMessage _request;

        public ApiAuthContainer(HttpRequestMessage request)
        {
            this._request = request;
        }

        protected override KeyValuePair<string, string> GetCookie(string key)
        {
            CookieState cookie = this._request.GetCookie(key);
            if (cookie == null)
            {
                return new KeyValuePair<string, string>();
            }
            return new KeyValuePair<string, string>(cookie.Name, cookie.Value);
        }

        protected override void SetCookie(string key, string value, DateTime expired)
        {
            this._request.SetCookie(key, value, expired);
        }
    }
}
