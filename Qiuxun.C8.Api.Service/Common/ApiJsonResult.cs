using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Qiuxun.C8.Api.Service.Common
{
    public class ApiJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(base.ContentType))
            {
                response.ContentType = base.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (base.ContentEncoding != null)
            {
                response.ContentEncoding = base.ContentEncoding;
            }
            if (base.Data != null)
            {
                response.Write(base.Data.ToJsonString());
            }
        }
    }
}
