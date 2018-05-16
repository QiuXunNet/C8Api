using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiuxun.C8.Api.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Public.Tests
{
    [TestClass()]
    public class UtilTests
    {
        [TestMethod()]
        public void GetShowInfoTest()
        {
            string showInfo = Util.GetShowInfo(5, "01,02,03,04,05,06,07", new DateTime());
            Assert.IsNotNull(showInfo);

            Console.WriteLine(showInfo);
        }

        [TestMethod()]
        public void GetCurrentIssueTest()
        {
            var result = Util.GetCurrentIssue(51);
            Assert.IsNotNull(result);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void CompareTimeTest()
        {
            //DateTime dt1 = Convert.ToDateTime("2018-05-11" + "24:00");
            //Assert.IsNull(dt1);
            //Console.WriteLine(dt1);

            //DateTime now = DateTime.Now;

            //int minuteOne = 0;
            //int minute = -1;

            //if (now.Minute < 10)
            //{
            //    minute = now.Minute;
            //}
            //else if(now.Minute>10)
            //{
            //    if (now.Minute%10 == 0)
            //    {
            //        minute = 0;
            //    }
            //    else
            //    {
            //        minute = int.Parse(now.Minute.ToString().Substring(1, 1));
            //    }
            //    minuteOne = int.Parse(now.Minute.ToString().Substring(0, 1));
            //}

            DateTime t1 = new DateTime(2018, 5, 10, 10, 24, 33);
            DateTime t2 = new DateTime(2018, 5, 14, 14, 34, 22);

            var diff = t2 - t1;

            Console.WriteLine($"时间差：{(int)diff.TotalHours}H,{diff.Minutes}M,{diff.Seconds}S");

            DateTime t3 = new DateTime(2018, 5, 14, 10, 24, 33);
            DateTime t4 = new DateTime(2018, 5, 14, 14, 34, 22);

            var diff1 = t4 - t3;

            Console.WriteLine($"时间差：{(int)diff1.TotalHours}H,{diff1.Minutes}M,{diff1.Seconds}S");

        }

        [TestMethod()]
        public void ConvertTimeStringTest1()
        {

            // 14:03:23   
            // 14:10:00

            DateTime t1 = new DateTime(2018, 5, 14, 09, 53, 00);
            DateTime t2 = new DateTime(2018, 5, 14, 19, 33, 22);

            var diff = t2 - t1;

            int totalMilliseconds = (int)diff.TotalMilliseconds;

            int divisorMilliseconds = 10 * 60 * 1000;
            int diffCount = totalMilliseconds / divisorMilliseconds;
            int remainderMilliseconds = totalMilliseconds % divisorMilliseconds;
            int disableMilliseconds = divisorMilliseconds - remainderMilliseconds - 30000;



            Console.WriteLine($"期号：{diffCount}");

            int seconds = disableMilliseconds / 60000;
            int minute = disableMilliseconds / 3600000;
            int hour = disableMilliseconds / (3600000 * 24);

            Console.WriteLine($"封盘倒计时：{hour}H,{minute}M,{seconds}S");

            DateTime t3 = t1.AddMilliseconds((diffCount + 1) * divisorMilliseconds);

            var diff2 = t3 - t2;

            Console.WriteLine($"{t2}");
            Console.WriteLine($"{t3}");

            Console.WriteLine($"倒计时：{(int)diff2.TotalHours}H,{diff2.Minutes}M,{diff2.Seconds}S");

            //Console.WriteLine(totalMilliseconds);
            //Console.WriteLine($"时间差（{totalMilliseconds}）：{(int)diff.TotalHours}H,{diff.Minutes}M,{diff.Seconds}S");

            //int totalSeconds = totalMilliseconds/1000;
            //int seconds = totalMilliseconds % 60000;
            //int minute = totalMilliseconds % 3600000;
            //int hour = totalMilliseconds % (3600000 * 24);


            //Console.WriteLine($"时间差：{hour}H,{minute}M,{seconds}S");
        }

        [TestMethod()]
        public void GetCurrentIssueTest1()
        {
            var result = LuoUtil.GetCurrentIssue(51, DateTime.Now);
            Console.WriteLine(result);
        }
    }
}