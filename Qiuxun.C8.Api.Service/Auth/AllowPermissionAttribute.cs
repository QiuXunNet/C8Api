using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Auth
{
    public class AllowPermissionAttribute : AuthorizeAttribute
    {
        public AllowPermissionAttribute()
        {
        }

        public AllowPermissionAttribute(string functionCode)
        {
            this.FunctionCode = string.IsNullOrWhiteSpace(functionCode) ? null : functionCode;
        }

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
            return base.IsAuthorized(httpContext);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization != null && authorization.Parameter != null)
            {
                var encryptTicket = authorization.Parameter;
                if (ValidateTicket(encryptTicket))
                {
                    IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            base.OnAuthorization(actionContext);
        }

        public string FunctionCode { get; set; }

        private bool ValidateTicket(string encryptTicket)
        {
            if (string.IsNullOrWhiteSpace(encryptTicket)) return false;

            var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;

            return true;
        }
    }
}
