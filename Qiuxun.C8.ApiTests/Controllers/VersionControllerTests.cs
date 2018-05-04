using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiuxun.C8.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ApiTests;

namespace Qiuxun.C8.Api.Controllers.Tests
{
    [TestClass()]
    public class VersionControllerTests : BaseControllerTest
    {
        VersionController _controller = new VersionController();
        [TestMethod()]
        public void CheckverTest()
        {
            base.InitController(_controller);

            var result = _controller.Checkver();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Console.WriteLine(result.ToJsonString());
        }
    }
}