using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {
        public const string COMMON_DATE = "yyyy-MM-dd";
        public const string COMMON_DATETIME = "yyyy-MM-dd HH:mm:ss";

        public static long GetJsTimespan(this DateTime dt)
        {
            TimeSpan span = (TimeSpan)(dt - new DateTime(0x7b2, 1, 1));
            return (long)span.TotalMilliseconds;
        }

        public static DateTime GetMonthStart(this DateTime dt)
        {
            return dt.Date.AddDays((double)(-dt.Day + 1));
        }

        public static DateTime GetWeekStart(this DateTime dt, bool startFromMonday = true)
        {
            int num = 0;

            if (startFromMonday)
            {
                int weeknow = (int)dt.DayOfWeek;
                num = -(weeknow == 0 ? (7 - 1) : (weeknow - 1));
            }
            else
            {
                num = -(int)dt.DayOfWeek;
            }
            return dt.Date.AddDays(num);
        }

        public static bool IsMinValue1970(this DateTime time)
        {
            return (((time.Year == 0x7b2) && (time.Month == 1)) && (time.Day == 1));
        }

        public static bool IsToday(this DateTime time)
        {
            DateTime now = DateTime.Now;
            return (((time.Year == now.Year) && (time.Month == now.Month)) && (time.Day == now.Day));
        }

        public static int MonthDiff(this DateTime dt1, DateTime dt2)
        {
            return ((((dt1.Year - dt2.Year) * 12) + dt1.Month) - dt2.Month);
        }

        public static string ToCommonDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToCommonDateString(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return "";
            }
            return dt.Value.ToString("yyyy-MM-dd");
        }

        public static string ToCommonDateTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToCommonDateTimeString(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return "";
            }
            return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToTimePeriodDescription(this DateTime dt)
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - dt);
            double totalDays = span.TotalDays;
            double totalHours = span.TotalHours;
            double totalMinutes = span.TotalMinutes;
            if (totalDays > 1.0)
            {
                return (((int)totalDays) + "天前");
            }
            if (totalHours > 1.0)
            {
                return (((int)totalHours) + "小时前");
            }
            return (((int)totalMinutes) + "分钟前");
        }

        /// <summary>
        /// 查询时间为当年的第几周
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(this DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }
    }
}

