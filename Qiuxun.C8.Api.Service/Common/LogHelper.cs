using System;
using log4net;

namespace Qiuxun.C8.Api.Service.Common
{
    public class LogHelper
    {
        public static ILog Loger
        {
            get
            {
                return LogerFactory.GetCurrentLoger();
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
