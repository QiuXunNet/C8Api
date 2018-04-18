using System.Runtime.Remoting.Messaging;
using log4net;

namespace Qiuxun.C8.Api.Service.Common
{
    public class LogerFactory
    {
        public static ILog GetCurrentLoger()
        {
            ILog loger = CallContext.GetData("loger") as ILog;
            if (loger == null)
            {
                log4net.Config.XmlConfigurator.Configure();
                loger = LogManager.GetLogger("RollingLogFileAppender");
                CallContext.SetData("loger", loger);
            }
            return loger;
        }
    }
}
