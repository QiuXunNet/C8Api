using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using log4net;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Api.ClintSecurities;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service
{

    public class RequestHelper
    {
        public const string _client_height_key = "m-ch";
        public const string _client_nettype_key = "m-nw";
        public const string _client_type_key = "m-ct";
        public const string _client_version_key = "m-cv";
        public const string _client_width_key = "m-cw";
        public const string _coodinate_type_key = "m-lt";
        public const string _encrypted_imei_key = "m-ii";
        public const string _header_cookie_key = "_cookies";
        public const string _install_source_key = "m-sr";
        public const string _interface_version_key = "m-iv";
        public const string _lat_key = "m-lat";
        public const string _lng_key = "m-lng";
        public const string _location_code = "m-lc";
        public const string _query_string_header_key = "__h";
        public const string _token_key = "token";
        private static readonly ILog log4Logger = LogManager.GetLogger(typeof(RequestHelper));


        public static RequestInfo BuildRequestInfo(HttpRequestMessage request)
        {
            RequestImeiDto dto;
            IEnumerable<string> enumerable;
            RequestInfo info = new RequestInfo
            {
                RequestStartTime = DateTime.Now
            };
            KeyValuePair<string, string> pair = request.GetQueryNameValuePairs().FirstOrDefault<KeyValuePair<string, string>>(d => d.Key == "__h");
            string encryptedImei = "";
            string regionCode = "";
            if (string.IsNullOrEmpty(pair.Key))
            {
                info.Lng = request.GetHeaderValue<double>("m-lng");
                info.Lat = request.GetHeaderValue<double>("m-lat");
                int headerValue = request.GetHeaderValue<int>("m-lt");
                info.LocationType = (headerValue != 1) ? LocationType.MapLocation : LocationType.GpsLocation;
                info.ClientNetType = request.GetHeaderValue("m-nw");
                info.InterfaceVersion = request.GetHeaderValue("m-iv");
                info.ClientVersion = request.GetHeaderValue("m-cv");
                info.ClientType = request.GetHeaderValue<DevicePlatform>("m-ct");
                info.ClientWidth = request.GetHeaderValue<int>("m-cw");
                info.ClientHeight = request.GetHeaderValue<int>("m-ch");
                encryptedImei = request.GetHeaderValue("m-ii");
                info.ClientSourceId = request.GetHeaderValue<byte>("m-sr");
                regionCode = request.GetHeaderValue("m-lc");
            }
            else
            {
                NameValueCollection collection = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(pair.Value));
                info.Lng = collection.GetValue<double>("m-lng");
                info.Lat = collection.GetValue<double>("m-lat");
                info.LocationType = collection.GetValue<LocationType>("m-lt");
                info.ClientNetType = collection.GetValue("m-nw");
                info.InterfaceVersion = collection.GetValue("m-iv");
                info.ClientVersion = collection.GetValue("m-cv");
                info.ClientType = collection.GetValue<DevicePlatform>("m-ct");
                info.ClientWidth = collection.GetValue<int>("m-cw");
                info.ClientHeight = collection.GetValue<int>("m-ch");
                encryptedImei = collection.GetValue("m-ii");
                string cookies = collection.GetValue("_cookies");
                if (!string.IsNullOrEmpty(cookies))
                {
                    cookies = HttpUtility.UrlDecode(cookies);
                    request.Headers.Add("Cookie", cookies);
                }
                info.ClientSourceId = collection.GetValue<byte>("m-sr");
                info.Token = collection.GetValue("token");
                if (!string.IsNullOrEmpty(info.Token))
                {
                    request.Headers.Add("Cookie", string.Format("{0}={1}", "token", info.Token));
                }
                regionCode = collection.GetValue("m-lc");
            }
            if (!string.IsNullOrEmpty(regionCode))
            {
                try
                {
                    //TODO:解析区域位置

                }
                catch (Exception exception)
                {
                    log4Logger.Error("区域位置解析错误", exception);
                }
            }
            string userAgent = request.Headers.UserAgent.ToString();
            info.UserAgent = userAgent;
            if (userAgent.IndexOf("MicroMessenger") > 0)
            {
                info.ClientSourceId = 100;
                info.ClientType = DevicePlatform.Weixin;
                info.InterfaceVersion = ConfigurationManager.AppSettings["interface_version"];
            }
            if (info.ClientSourceId == 0)
            {
                if (info.ClientType == DevicePlatform.Android)
                {
                    info.ClientSourceId = 1;
                }
                else if (info.ClientType == DevicePlatform.Ios)
                {
                    info.ClientSourceId = 2;
                }
            }
            info.OtherHeader = JsonConvert.SerializeObject(new { ClientSourceId = info.ClientSourceId });
            if (info.ClientSourceId != 100)
            {
                dto = ImeiSecurity.Decrypt(encryptedImei, request.RequestUri.PathAndQuery, info.UserAgent, info.Lng, info.Lat);
            }
            else
            {
                dto = new RequestImeiDto(encryptedImei);
            }
            info.ImeiInfo = dto;
            if (request.Headers.TryGetValues("Cookie", out enumerable))
            {
                foreach (string str10 in enumerable)
                {
                    info.Cookie = info.Cookie + str10 + "~";
                }
                if (info.Cookie.Length > 0)
                {
                    info.Cookie = info.Cookie.TrimEnd(new char[] { '~' });
                }
            }
            ClientIpSource clientIpAddress = request.GetClientIpAddress();
            info.ClientIP = clientIpAddress.ClientIpFromTcpIp;
            info.ClientIpHttp = clientIpAddress.ClientIpFromHttp;
            info.IsHttps = clientIpAddress.IsHttps;
            info.HttpMethod = request.Method;
            return info;
        }

        public static RequestInfo BuildRequestInfo(HttpRequestBase request)
        {
            RequestInfo info = new RequestInfo
            {
                RequestStartTime = DateTime.Now
            };
            string userAgent = request.UserAgent;
            info.UserAgent = userAgent;
            if (request.Cookies.Count > 0)
            {
                foreach (string str2 in request.Cookies.Keys)
                {
                    HttpCookie cookie = request.Cookies[str2];
                    info.Cookie = info.Cookie + string.Format("{0}={1}~", cookie.Name, cookie.Value);
                }
                if (info.Cookie.Length > 0)
                {
                    info.Cookie = info.Cookie.TrimEnd(new char[] { '~' });
                }
            }
            info.ClientIP = request.UserHostAddress;
            info.ClientIpHttp = request.Headers["X-Real-IP"];
            info.IsHttps = request.Headers["request_port"] == "443";
            return info;
        }

        public static RequestLog BuildRequestLog(HttpRequestMessage request, RequestInfo requestInfo)
        {
            RequestLog log = new RequestLog
            {
                Id = requestInfo.Id,
                HttpMethod = request.Method.Method
            };
            IdentityInfo authInfo = new QiuxunTokenAuthorizer(new ApiAuthContainer(request)).GetAuthInfo();
            if (authInfo != null)
            {
                log.CustomerId = new int?((int)authInfo.UserId);
                log.UserName = authInfo.UserAccount;
                log.CustomerGuid = new Guid();
            }
            log.ClientIp = requestInfo.ClientIP;
            log.ClientIpHttp = requestInfo.ClientIpHttp;
            log.Lng = requestInfo.Lng;
            log.Lat = requestInfo.Lat;
            log.LocationType = (int)requestInfo.LocationType;
            log.ClientNetType = requestInfo.ClientNetType;
            log.InterfaceVersion = requestInfo.InterfaceVersion;
            log.ClientVersion = requestInfo.ClientVersion;
            log.ClientType = (int)requestInfo.ClientType;
            log.ClientWidth = requestInfo.ClientWidth;
            log.ClientHeight = requestInfo.ClientHeight;
            log.UserAgent = requestInfo.UserAgent;
            log.RequestCookie = requestInfo.Cookie;
            log.ServerName = Environment.MachineName;
            log.RequestTime = requestInfo.RequestStartTime;
            log.Route = request.RequestUri.LocalPath;
            log.OtherHeader = requestInfo.OtherHeader;
            RequestImeiDto imeiInfo = requestInfo.ImeiInfo;
            log.Imei = imeiInfo.RealImei ?? "";
            log.GenerateTime = imeiInfo.GenerateTime;
            log.IsFake = imeiInfo.IsFake;
            log.RequestData = GetApiRequestData(request, requestInfo);
            if (requestInfo.HasRegionCodeInfo)
            {
                log.ProvinceCode = requestInfo.ProvinceCode;
                log.CityCode = requestInfo.CityCode;
                log.DistrictCode = requestInfo.DistrictCode;
            }
            else
            {
                log.Province = requestInfo.Province;
                log.City = requestInfo.City;
                log.District = requestInfo.District;
            }
            TimeSpan span = (TimeSpan)(DateTime.Now - log.RequestTime);
            log.ElapsedMilliseconds = (int)span.TotalMilliseconds;
            return log;
        }

        private static string GetApiRequestData(HttpRequestMessage request, RequestInfo requestInfo)
        {
            string str = "";
            Dictionary<string, object> actionArguments = requestInfo.ActionArguments;
            if (actionArguments != null)
            {
                if ((actionArguments.Count == 1) && request.Method.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    return actionArguments.First<KeyValuePair<string, object>>().Value.ToJsonString();
                }
                if (actionArguments.Count > 0)
                {
                    str = actionArguments.ToJsonString();
                }
                return str;
            }
            MediaTypeHeaderValue contentType = request.Content.Headers.ContentType;
            if (contentType != null)
            {
                string str2 = "";
                string mediaType = contentType.MediaType;
                if (string.Equals("application/jsone", mediaType, StringComparison.OrdinalIgnoreCase))
                {
                    Stream result = request.Content.ReadAsStreamAsync().Result;
                    int length = (int)result.Length;
                    byte[] buffer = new byte[length];
                    result.Position = 0L;
                    result.Read(buffer, 0, length);
                    str2 = Convert.ToBase64String(buffer);
                }
                else if (string.Equals("application/jsonet", mediaType, StringComparison.OrdinalIgnoreCase))
                {
                    Stream stream2 = request.Content.ReadAsStreamAsync().Result;
                    int count = (int)stream2.Length;
                    byte[] buffer2 = new byte[count];
                    stream2.Position = 0L;
                    stream2.Read(buffer2, 0, count);
                    str2 = Encoding.UTF8.GetString(buffer2);
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    str = string.Format("{{\"RsaData\":\"{0}\"}}", str2);
                }
            }
            return str;
        }

        private static string GetMvcRequestData(HttpRequestBase request, RequestInfo requestInfo)
        {
            if ((request.Form.Count > 0) && (request.QueryString.Count > 0))
            {
                return (request.Form.ToString() + "&" + request.QueryString.ToString());
            }
            if (request.Form.Count > 0)
            {
                return request.Form.ToString();
            }
            if (request.QueryString.Count > 0)
            {
                return request.QueryString.ToString();
            }
            return string.Empty;
        }

        public static void WriteAllWebRequestLog(RequestLog log)
        {
            AllWebRequestLogEntity entity = new AllWebRequestLogEntity
            {
                ApiDesc = log.ApiDesc,
                ApiStatus = log.ApiStatus.HasValue ? log.ApiStatus.Value : 0,
                ClientHeight = log.ClientHeight,
                ClientIp = log.ClientIp,
                ClientIpHttp = log.ClientIpHttp,
                ClientNetType = log.ClientNetType,
                ClientType = log.ClientType,
                ClientVersion = log.ClientVersion,
                ClientWidth = log.ClientWidth,
                Content = log.ApiDescDetail,
                CustomerGuid = log.CustomerGuid.HasValue ? log.CustomerGuid.Value : Guid.Empty,
                CustomerId = log.CustomerId.HasValue ? log.CustomerId.Value : 0,
                ElapsedMilliseconds = log.ElapsedMilliseconds,
                HttpMethod = log.HttpMethod,
                HttpStatus = log.HttpStatus,
                Imei = log.Imei,
                InterfaceVersion = log.InterfaceVersion,
                Lat = log.Lat,
                Lng = log.Lng,
                LocationType = log.LocationType,
                OtherHeader = log.OtherHeader,
                RequestCookie = log.RequestCookie,
                RequestData = log.RequestData,
                RequestTime = log.RequestTime,
                ResponseCookie = log.ResponseCookie,
                Route = log.Route,
                ServerName = log.ServerName,
                UserAgent = log.UserAgent,
                UserName = log.UserName,
                AppName = "C8",
                ModelName = "所有Web请求",
                Level = 1,
                IsFake = log.IsFake,
                GenerateTime = log.GenerateTime,
                Province = log.Province,
                City = log.City,
                District = log.District,
                ProvinceCode = log.ProvinceCode,
                DistrictCode = log.DistrictCode,
                CityCode = log.CityCode,
                Id = log.Id
            };

            try
            {
                //TODO:请求日志持久化
                LogHelper.InfoFormat("请求日志：\r\n{0}", entity.ToJsonString());
            }
            catch (Exception exception)
            {
                LogHelper.Error("", exception);
            }
        }


        public static void WriteWebRequestLog(RequestLog log, string ResponseData)
        {
            WebApiRequestLogEntity entity = new WebApiRequestLogEntity
            {

                ApiDesc = log.ApiDesc,
                ApiStatus = log.ApiStatus.HasValue ? log.ApiStatus.Value : 0,
                ClientHeight = log.ClientHeight,
                ClientIp = log.ClientIp,
                ClientIpHttp = log.ClientIpHttp,
                ClientNetType = log.ClientNetType,
                ClientType = log.ClientType,
                ClientVersion = log.ClientVersion,
                ClientWidth = log.ClientWidth,
                Content = log.ApiDescDetail,
                CustomerGuid = log.CustomerGuid.HasValue ? log.CustomerGuid.Value : Guid.Empty,
                CustomerId = log.CustomerId.HasValue ? log.CustomerId.Value : 0,
                ElapsedMilliseconds = log.ElapsedMilliseconds,
                HttpMethod = log.HttpMethod,
                HttpStatus = log.HttpStatus,
                Imei = log.Imei,
                InterfaceVersion = log.InterfaceVersion,
                Lat = log.Lat,
                Lng = log.Lng,
                LocationType = log.LocationType,
                OtherHeader = log.OtherHeader,
                RequestCookie = log.RequestCookie,
                RequestData = log.RequestData,
                RequestTime = log.RequestTime,
                ResponseCookie = log.ResponseCookie,
                Route = log.Route,
                ServerName = log.ServerName,
                UserAgent = log.UserAgent,
                UserName = log.UserName,
                AppName = "C8",
                ModelName = "所有Web请求",
                Level = 1,
                IsFake = log.IsFake,
                GenerateTime = log.GenerateTime,
                ResponseData = ResponseData,
                Province = log.Province,
                City = log.City,
                District = log.District,
                ProvinceCode = log.ProvinceCode,
                DistrictCode = log.DistrictCode,
                CityCode = log.CityCode,
                Id = log.Id

            };

            try
            {
                //TODO:api请求日志持久化

                LogHelper.InfoFormat("请求日志：\r\n{0}", entity.ToJsonString());
            }
            catch (Exception exception)
            {
                LogHelper.Error("", exception);
            }
        }

    }
}
