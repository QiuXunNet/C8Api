using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;

namespace Qiuxun.C8.Api.Service.Common
{
    public class CustomErrorMessageDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;
                HttpError error = null;
                if (response.TryGetContentValue<HttpError>(out error))
                {
                    LogHelper.WriteLog(string.Format("错误信息：{0}，错误堆栈：{1}", error.Message, error.StackTrace));

                    //添加自定义错误处理
                    error.Message = "未知错误，请联系管理员。";
                }

                if (error != null)
                {
                    //LogHelper.WriteLog(string.Format("错误信息：{0}，错误堆栈：{1}", error.Message, error.StackTrace));
                    //获取抛出自定义异常，有拦截器统一解析
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        //封装处理异常信息，返回指定JSON对象
                        Content = new StringContent(new ApiResult(404, error.Message).ToJsonString(), Encoding.UTF8, "application/json"),
                        ReasonPhrase = "Exception"
                    });
                }
                else
                {
                    return response;
                }
            });
        }
    }
}
