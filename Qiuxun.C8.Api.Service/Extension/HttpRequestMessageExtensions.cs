using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Qiuxun.C8.Api.Service.Common
{


    public static class HttpRequestMessageExtensions
    {
        private const string _httpContext_key = "MS_HttpContext";
        private const string _remoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private static readonly string host = ConfigurationManager.AppSettings["api_Host"];
        public const string Key_EprepareSetCookie = "eprepare-set-cookie";

        public static bool AddProperty<T>(this HttpRequestMessage request, T item, string key = "")
        {
            string str = key;
            if (string.IsNullOrEmpty(str))
            {
                str = typeof(T).GUID.ToString("N");
            }
            if (request.Properties.ContainsKey(str))
            {
                return false;
            }
            request.Properties.Add(str, item);
            return true;
        }

        public static ClientIpSource GetClientIpAddress(this HttpRequestMessage request)
        {
            ClientIpSource clientIpSource = new ClientIpSource();
            IEnumerable<string> source;
            if (request.Headers.TryGetValues("X-Real-IP", out source))
            {
                clientIpSource.ClientIpFromHttp = source.FirstOrDefault<string>();
            }
            if (request.Headers.TryGetValues("request_port", out source))
            {
                clientIpSource.IsHttps = (source.FirstOrDefault<string>() == "443");
            }
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                object obj = request.Properties["MS_HttpContext"];
                if (obj is HttpContextBase)
                {
                    HttpContextBase httpContextBase = (HttpContextBase)obj;
                    clientIpSource.ClientIpFromTcpIp = httpContextBase.Request.UserHostAddress;
                }
            }
            else if (request.Properties.ContainsKey("System.ServiceModel.Channels.RemoteEndpointMessageProperty"))
            {
                object arg = request.Properties["System.ServiceModel.Channels.RemoteEndpointMessageProperty"];
                
            }
            return clientIpSource;
        }

        public static CookieState GetCookie(this HttpRequestMessage request, string key)
        {
            Collection<CookieHeaderValue> cookies = request.Headers.GetCookies();
            List<CookieState> source = new List<CookieState>();
            foreach (CookieHeaderValue value2 in cookies)
            {
                foreach (CookieState state in value2.Cookies)
                {
                    if (state.Name == key)
                    {
                        source.Add(state);
                    }
                }
            }
            return source.LastOrDefault<CookieState>();
        }

        public static string GetHeaderValue(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> values = null;
            if (request.Headers.TryGetValues(key, out values))
            {
                return values.FirstOrDefault<string>();
            }
            return null;
        }

        public static T GetHeaderValue<T>(this HttpRequestMessage request, string key) where T : struct
        {
            IEnumerable<string> values = null;
            if (!request.Headers.TryGetValues(key, out values))
            {
                return default(T);
            }
            string str = values.FirstOrDefault<string>();
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }
            return str.ParseTo<T>();
        }

        public static T GetProperty<T>(this HttpRequestMessage request, string key = "")
        {
            object obj2;
            string str = key;
            if (string.IsNullOrEmpty(str))
            {
                str = typeof(T).GUID.ToString("N");
            }
            if (request.Properties.TryGetValue(str, out obj2))
            {
                return (T)obj2;
            }
            return default(T);
        }

        public static void RemoveProperty<T>(this HttpRequestMessage request, string key = "")
        {
            string str = key;
            if (string.IsNullOrEmpty(str))
            {
                str = typeof(T).GUID.ToString("N");
            }
            if (request.Properties.ContainsKey(str))
            {
                request.Properties.Remove(str);
            }
        }

        public static void SetCookie(this HttpRequestMessage request, string key, string value, DateTime expired)
        {
            object obj2;
            List<CookieHeaderValue> cookies;
            if (!request.Properties.TryGetValue("eprepare-set-cookie", out obj2))
            {
                cookies = new List<CookieHeaderValue>();
                request.Properties.Add("eprepare-set-cookie", cookies);
            }
            else
            {
                cookies = (List<CookieHeaderValue>)obj2;
            }
            cookies.Where<CookieHeaderValue>(delegate (CookieHeaderValue ck)
            {
                foreach (CookieState state in ck.Cookies)
                {
                    if (state.Name == key)
                    {
                        return true;
                    }
                }
                return false;
            }).ToList<CookieHeaderValue>().ForEach(ck => cookies.Remove(ck));
            CookieHeaderValue item = new CookieHeaderValue(key, value)
            {
                Expires = new DateTimeOffset(expired),
                Path = "/"
            };
            if (!string.IsNullOrEmpty(host))
            {
                item.Domain = host;
            }
            cookies.Add(item);
        }
    }
}

