using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Api.InterfaceFilters;
using Qiuxun.C8.Api.Service.Api.Securities;
using Qiuxun.C8.Api.Service.Common;

namespace System.Web.Http
{
    public static class HttpConfigExtensions
    {
        public static void EnableJsonNegotiator(this HttpConfiguration config)
        {
            config.Formatters.Add(new RsaJsonMediaTypeFormatter());
            config.Formatters.Add(new RsaTextJsonMediaTypeFormatter());
            config.Services.Replace(typeof(IContentNegotiator), new QiuxunApiContentNegotiator(new JsonMediaTypeFormatter()));
        }

        public static void EnableLogHandler(this HttpConfiguration config, bool allWebRequest = true)
        {
            QiuxunApiLogHandler qiuxunApiLogHandler = null;
            foreach (DelegatingHandler current in config.MessageHandlers)
            {
                if (current is QiuxunApiLogHandler)
                {
                    qiuxunApiLogHandler = (QiuxunApiLogHandler)current;
                    break;
                }
            }
            if (qiuxunApiLogHandler != null)
            {
                config.MessageHandlers.Remove(qiuxunApiLogHandler);
            }
            config.MessageHandlers.Add(new QiuxunApiLogHandler(allWebRequest));
        }

        public static void EnableCommonHandler(this HttpConfiguration config, bool headerCheck = false, bool interfaceCheck = false)
        {
            List<IApiRequestFilter> list = new List<IApiRequestFilter>();
            if (headerCheck)
            {
                list.Add(new HttpHeaderCheckFilter());
            }
            if (interfaceCheck)
            {
                list.Add(new ApiInterfaceControl());
            }
            QiuxunHttpMessageHandler qiuxunHttpMessageHandler = null;
            foreach (DelegatingHandler current in config.MessageHandlers)
            {
                if (current is QiuxunHttpMessageHandler)
                {
                    qiuxunHttpMessageHandler = (QiuxunHttpMessageHandler)current;
                    break;
                }
            }
            if (qiuxunHttpMessageHandler != null)
            {
                config.MessageHandlers.Remove(qiuxunHttpMessageHandler);
            }
            config.MessageHandlers.Add(new QiuxunHttpMessageHandler(list));
        }

        public static void EnableErrorHandler(this HttpConfiguration config)
        {
            IFilter filter = null;
            foreach (FilterInfo current in config.Filters)
            {
                if (current.Instance is WebLogHandleErrorAttribute)
                {
                    filter = (WebLogHandleErrorAttribute)current.Instance;
                    break;
                }
            }
            if (filter != null)
            {
                config.Filters.Remove(filter);
            }
            config.Filters.Add(new WebLogHandleErrorAttribute());
        }
    }
}
