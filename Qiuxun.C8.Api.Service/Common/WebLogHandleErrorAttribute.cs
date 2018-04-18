using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using log4net;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// 错误日志处理
    /// </summary>
    public class WebLogHandleErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            ILog logger = LogManager.GetLogger(filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType);

            ApiUnauthorizedException exception = filterContext.Exception as ApiUnauthorizedException;
            if (exception != null)
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                ApiResult result = new ApiResult(exception.Code, exception.Desc);
                StringContent content = new StringContent(result.ToJsonString())
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };
                message.Content = content;
                filterContext.Response = message;
                logger.ErrorFormat("请求错误，地址：{0}。\r\n{1}", filterContext.Request.RequestUri, filterContext.Exception);
            }
            else
            {
                ApiException exception2 = filterContext.Exception as ApiException;
                if (exception2 != null)
                {
                    filterContext.Response = filterContext.Request.CreateResponse<ApiResult>(
                        HttpStatusCode.Forbidden, new ApiResult(exception2.Code, exception2.Desc));
                    logger.ErrorFormat("请求错误，地址：{0}。\r\n{1}", filterContext.Request.RequestUri, filterContext.Exception);
                }
                else
                {
                    ApiResult result4 = new ApiResult(-100, "未知错误，请联系管理员。");
                    filterContext.Response = filterContext.Request.CreateResponse<ApiResult>(HttpStatusCode.InternalServerError, result4);
                    logger.ErrorFormat("请求错误，地址：{0}。\r\n{1}", filterContext.Request.RequestUri, filterContext.Exception);

                }

            }
        }
    }
}
