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
        

    }
}
