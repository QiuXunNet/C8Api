using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // Web API CORS support
            var hosts = ConfigurationManager.AppSettings["cors_hosts"];
            if (!string.IsNullOrEmpty(hosts))
            {
                var cors = new EnableCorsAttribute(hosts, "*", "*");
                //设置预检查时间
                cors.PreflightMaxAge = 20 * 60;
                cors.SupportsCredentials = true;
                config.EnableCors(cors);
            }
            //全局错误处理
            //config.MessageHandlers.Add(new CustomErrorMessageDelegatingHandler());
            config.EnableErrorHandler();
            config.EnableLogHandler(false);
            //config.EnableCommonHandler(true, true);
            //////config.EnableVersionRoute();
            //config.EnableJsonNegotiator();

            var route = config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}",
                new { controller = "Auth", action = "Login" }
            );

        }
    }
}
