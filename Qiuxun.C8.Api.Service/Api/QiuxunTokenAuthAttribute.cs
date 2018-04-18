using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using log4net;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api
{
    public class QiuxunTokenAuthAttribute : AuthorizeAttribute
    {
        private const string _token_key = "token";
        private static readonly ILog webRequestLogger = LogManager.GetLogger(typeof(QiuxunTokenAuthAttribute));

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            ApiResult result = new ApiResult
            {
                Desc = "无权限，需要登录后操作",
                Code = 401
            };
            actionContext.Response = actionContext.Request.CreateResponse<ApiResult>(HttpStatusCode.Unauthorized, result);
        }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            return (httpContext.Request.GetProperty<IdentityInfo>("") != null);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            actionContext.Request.GetProperty<RequestInfo>("");
            IdentityInfo authInfo = new QiuxunTokenAuthorizer(new ApiAuthContainer(actionContext.Request)).GetAuthInfo();
            if (authInfo != null)
            {
                actionContext.Request.AddProperty<IdentityInfo>(authInfo, "");
            }
            base.OnAuthorization(actionContext);
        }
    }
}
