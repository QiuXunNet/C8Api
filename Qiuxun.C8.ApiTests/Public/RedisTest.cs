using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiuxun.C8.Api.Service.Cache;

namespace Qiuxun.C8.ApiTests.Public
{
    [TestClass()]
    public class RedisTest
    {

        [TestMethod()]
        public void RedisReadWriteTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                string key = "redis_test_key_" + i;
                CacheHelper.AddCache(key, "qwertyuiopasdfghjklzxcvbnm_" + i);

                var data = CacheHelper.GetCache<string>(key);
                Console.WriteLine("第{0}项->key:{1},value:{2}", i, key, data);

            }
        }
    }
}
