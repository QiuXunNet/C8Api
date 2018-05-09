using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.ApiTests
{
    [TestClass]
    //[DeploymentItem("..\\..\\..\\..\\Qiuxun.C8.Api\\Qiuxun.C8.Api\\App_Data", "App_Data")]
    //[DeploymentItem("..\\..\\..\\..\\Qiuxun.C8.Api\\Qiuxun.C8.Api\\bin")]
    public class BaseControllerTest
    {
        protected void InitController(QiuxunApiController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Request.AddProperty(new IdentityInfo()
            {
                UserId = 2,
                UserName = "15880269263",
                UserAccount = "15880269263",
                UserStatus = (int)UserState.Normal
            });

            controller.Request.AddProperty(new RequestInfo()
            {
                InterfaceVersion = "1.0",
                ClientVersion = "1.0.0",
                ClientType = DevicePlatform.Ios,
                ClientSourceId = 2
            });
        }
    }
}
