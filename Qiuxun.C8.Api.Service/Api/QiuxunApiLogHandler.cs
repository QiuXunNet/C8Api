using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api
{

    public class QiuxunApiLogHandler : DelegatingHandler
    {
        private bool allWebRequest;

        public QiuxunApiLogHandler()
        {
            this.allWebRequest = true;
        }

        public QiuxunApiLogHandler(bool allWebRequest)
        {
            this.allWebRequest = true;
            this.allWebRequest = allWebRequest;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestInfo requestInfo = request.GetProperty<RequestInfo>("");
            if (requestInfo == null)
            {
                requestInfo = RequestHelper.BuildRequestInfo(request);
                request.AddProperty<RequestInfo>(requestInfo, "");
            }
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(delegate (Task<HttpResponseMessage> task)
            {
                object obj2;
                InterfaceSetting property = request.GetProperty<InterfaceSetting>("");
                if ((requestInfo.HttpMethod == HttpMethod.Options) || ((property != null) && property.DisableLog))
                {
                    return task.Result;
                }
                RequestLog log = RequestHelper.BuildRequestLog(request, requestInfo);
                HttpResponseMessage message = task.Result;
                string str = string.Empty;
                ObjectContent content = message.Content as ObjectContent;
                if (content != null)
                {
                    QiuxunLogResult result = content.Value as QiuxunLogResult;
                    if (result != null)
                    {
                        log.ApiStatus = new int?(result.LogCode);
                        log.ApiDesc = result.LogDesc;
                        log.ApiDescDetail = result.LogDescDetail;
                    }
                    else
                    {
                        ApiResult result2 = content.Value as ApiResult;
                        if (result2 != null)
                        {
                            log.ApiStatus = new int?(result2.Code);
                            log.ApiDesc = result2.Desc;
                            if (((property != null) && property.IsRecordResponseData) && (result2.Code == 100))
                            {
                                str = result2.ToJsonString();
                            }
                        }
                    }
                }
                if (request.Properties.TryGetValue("eprepare-set-cookie", out obj2))
                {
                    IEnumerable<CookieHeaderValue> enumerable = (IEnumerable<CookieHeaderValue>)obj2;
                    foreach (CookieHeaderValue value2 in enumerable)
                    {
                        log.ResponseCookie = log.ResponseCookie + string.Format("{0}~", value2.ToString());
                    }
                    if (log.ResponseCookie.Length > 0)
                    {
                        log.ResponseCookie = log.ResponseCookie.TrimEnd(new char[] { '~' });
                    }
                }
                log.HttpStatus = (int)message.StatusCode;
                if (this.allWebRequest)
                {
                    RequestHelper.WriteAllWebRequestLog(log);
                    return message;
                }
                if (((property != null) && property.IsRecordResponseData) && !string.IsNullOrEmpty(str))
                {
                    RequestHelper.WriteWebRequestLog(log, str);
                    return message;
                }
                RequestHelper.WriteAllWebRequestLog(log);
                return message;
            });
        }
    }
}
