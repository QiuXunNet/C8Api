using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// Api版本帮助类
    /// </summary>
    public class VersionHelper
    {
        /// <summary>
        /// 比较版本
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <returns></returns>
        public static int Compare(string ver1, string ver2)
        {
            ApiVersion version = new ApiVersion(ver1);
            ApiVersion other = new ApiVersion(ver2);
            return version.CompareTo(other);
        }
    }
}
