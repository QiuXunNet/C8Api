using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Enum
{
    /// <summary>
    /// SMS状态
    /// </summary>
    public enum SmsStatusEnum
    {
        /// <summary>
        /// 已发送
        /// </summary>
        Sent = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Failue = 3,
        /// <summary>
        /// 已使用
        /// </summary>
        Used = 2
    }
}
