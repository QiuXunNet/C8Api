using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 加入黑名单请求参数类
    /// </summary>
    public class AddBlackReqDto
    {
        /// <summary>
        /// 被加入黑名单的人的用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 被加入黑名单的人的用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 房间Id
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 操作人Id(当前登录人Id)
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string ProcessName { get; set; }
    }
}
