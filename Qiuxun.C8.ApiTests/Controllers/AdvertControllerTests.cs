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
    public class AdvertControllerTests : BaseControllerTest
    {
        AdvertController _controller = new AdvertController();
        [TestMethod()]
        public void GetAdvertisementListTest()
        {
            base.InitController(_controller);
            var result = _controller.GetAdvertisementList(1, 2, "");


            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Data == null);
            Console.WriteLine(result.ToJsonString());
        }
    }
}