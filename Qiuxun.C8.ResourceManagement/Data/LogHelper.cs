using System;
using System.Runtime.Remoting.Messaging;
using log4net;

namespace Qiuxun.C8.ResourceManagement.Data
{
    public class LogHelper
    {
        public static ILog Loger
        {
            get
            {
                ILog loger = CallContext.GetData("logger") as ILog;
                if (loger == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    loger = LogManager.GetLogger("RollingLogFileAppender");
                    CallContext.SetData("logger", loger);
                }
                return loger;
            }
        }


        public static void WriteLog(string message)
        {
            Loger.Error(message);
        }

        public static void Info(string message)
        {
            Loger.Info(message);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Loger.InfoFormat(format, args);
        }

        public static void Debug(string message)
        {
            Loger.Debug(message);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Loger.DebugFormat(format, args);
        }

        public static void Error(string message)
        {
            Loger.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            Loger.Error(message, ex);
        }


        public static void Error(string message, int code, Exception ex)
        {
            Loger.Error(string.Format("{0}，错误码：{1}", message, code), ex);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Loger.ErrorFormat(format, args);
        }


    }
}
