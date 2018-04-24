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
            string showInfo = Util.GetShowInfo(5, "01,02,03,04,05,06,07");
            Assert.IsNotNull(showInfo);

            Console.WriteLine(showInfo);
        }
    }
}