using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Enum
{
    public enum ClientUpdateStatus
    {
        /// <summary>
        /// 无需升级
        /// </summary>
        None=1,
        /// <summary>
        /// 用户可选择是否升级
        /// </summary>
        Optional = 2,
        /// <summary>
        /// 强制用户升级
        /// </summary>
        Force = 3
    }
}
