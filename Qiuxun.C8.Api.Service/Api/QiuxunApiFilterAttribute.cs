using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api
{
    public class QiuxunApiFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            RequestInfo property = actionContext.Request.GetProperty<RequestInfo>("");
            property.ActionArguments = actionContext.ActionArguments;
            if (!property.HasRegionInfo)
            {
                QiuxunApiController controller = actionContext.ControllerContext.Controller as QiuxunApiController;
                if (controller != null)
                {
                    LocationSearchCriteria baiduLocation = controller.BaiduLocation;
                    if (baiduLocation != null)
                    {
                        property.Province = baiduLocation.Province;
                        property.City = baiduLocation.City;
                        property.District = baiduLocation.District;
                    }
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
