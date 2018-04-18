using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 聊天记录实体
    /// 卢晨
    /// 2018-03-30
    /// </summary>
    public class TalkNotes
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string PhotoImg { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 所属房间
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 记录类型  21:文字    23:表情    22:图片
        /// </summary>
        public int MsgTypeChild { get; set; }

        /// <summary>
        /// 状态  0:删除  1:正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Guid 用于页面聊天记录命名和管理员删除
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 格式化后的发送时间
        /// </summary>
        public string SendTimeStr { get; set; }
    }
}
