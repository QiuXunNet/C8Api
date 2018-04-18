using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 聊天黑名单
    /// 卢晨
    /// 2018-03-30
    /// </summary>
    public class TalkBlackList
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 房间Id
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 禁言时间
        /// </summary>
        public DateTime BanTime { get; set; }

        /// <summary>
        /// 是否永久禁言
        /// </summary>
        public bool IsEverlasting { get; set; }

        /// <summary>
        /// 禁言时长
        /// </summary>
        public int TimeLong { get; set; }

        /// <summary>
        /// 禁言截止时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
