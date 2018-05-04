using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.ApiTests.Public
{
    [TestClass]
    public class ApiVersionTest
    {
        /// <summary>
        /// 获取完整版本号测试
        /// </summary>
        [TestMethod]
        public void GetFullVersionTest()
        {
            ApiVersion version = new ApiVersion("1.1");

            long fullVersion = version.FullVersion;

            Console.WriteLine(fullVersion);
        }

    }
}
