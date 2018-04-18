using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Qiuxun.C8.Api.Service.Api.InterfaceFilters;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Api
{

    public class QiuxunHttpMessageHandler : DelegatingHandler
    {
        private IList<IApiRequestFilter> filterList;

        public QiuxunHttpMessageHandler()
        {
            this.filterList = new List<IApiRequestFilter>();
        }

        public QiuxunHttpMessageHandler(IEnumerable<IApiRequestFilter> filters)
        {
            this.filterList = new List<IApiRequestFilter>();
            this.filterList.AddRange<IApiRequestFilter>(filters);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Func<Task<HttpResponseMessage>, HttpResponseMessage> continuationFunction = null;
            if ((request.Headers.TransferEncodingChunked == true) && (request.Content.Headers.ContentLength == 0L))
            {
                request.Content.Headers.ContentLength = null;
            }
            RequestInfo property = request.GetProperty<RequestInfo>("");
            if (property == null)
            {
                property = RequestHelper.BuildRequestInfo(request);
                request.AddProperty<RequestInfo>(property, "");
            }
            if (property.HttpMethod != HttpMethod.Options)
            {
                foreach (IApiRequestFilter filter in this.filterList)
                {
                    HttpResponseMessage result = null;
                    try
                    {
                        result = filter.DoFilter(request, property);
                    }
                    catch (Exception exception)
                    {
                        result = request.CreateErrorResponse(HttpStatusCode.InternalServerError, "请求筛选错误", exception);
                    }
                    if (result != null)
                    {
                        TaskCompletionSource<HttpResponseMessage> source = new TaskCompletionSource<HttpResponseMessage>();
                        source.SetResult(result);
                        if (continuationFunction == null)
                        {
                            continuationFunction = task => this.SendCompletion(request, task.Result);
                        }
                        return source.Task.ContinueWith<HttpResponseMessage>(continuationFunction);
                    }
                }
            }
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(task => this.SendCompletion(request, task.Result));
        }

        private HttpResponseMessage SendCompletion(HttpRequestMessage request, HttpResponseMessage response)
        {
            object obj2;
            if (ServerRoles.Instance.IsDevServer || ServerRoles.Instance.IsTestServer)
            {
                RequestInfo property = request.GetProperty<RequestInfo>("");
                if ((property.ClientType == DevicePlatform.Android) || (property.ClientType == DevicePlatform.Ios))
                {
                    response.Headers.Add("t-imei", property.ImeiInfo.RealImei);
                    response.Headers.Add("t_request_dt", property.ImeiInfo.GenerateTime.HasValue ? property.ImeiInfo.GenerateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
                }
            }
            if (request.Properties.TryGetValue("eprepare-set-cookie", out obj2))
            {
                IEnumerable<CookieHeaderValue> cookies = (IEnumerable<CookieHeaderValue>)obj2;
                response.Headers.AddCookies(cookies);
            }
            ObjectContent content = response.Content as ObjectContent;
            if (content == null)
            {
                return response;
            }
            HttpError error = content.Value as HttpError;
            if (error == null)
            {
                return response;
            }
            int logCode = new Random().Next(900000, 9999999);
            string exceptionMessage = error.ExceptionMessage;
            string stackTrace = error.StackTrace;
            if (string.IsNullOrEmpty(exceptionMessage) || string.IsNullOrEmpty(stackTrace))
            {
                exceptionMessage = error.Message;
                stackTrace = error.MessageDetail;
            }
            QiuxunLogResult result = new QiuxunLogResult(-100, "未知错误，请联系管理员，日志码：" + logCode, logCode, exceptionMessage, stackTrace);
            LogHelper.WriteLog(string.Format("请求错误，日志码{0}，地址：{1}\r\n{2}\r\n{3}", logCode, request.RequestUri, exceptionMessage, stackTrace));
            //LogManager.GetLogger(string.IsNullOrEmpty(error.ExceptionType) ? "Default" : error.ExceptionType).ErrorFormat("请求错误，日志码{0}，地址：{1}\r\n{2}\r\n{3}", logCode, request.RequestUri, exceptionMessage, stackTrace);
            return request.CreateResponse<QiuxunLogResult>(response.StatusCode, result);
        }
    }
}
