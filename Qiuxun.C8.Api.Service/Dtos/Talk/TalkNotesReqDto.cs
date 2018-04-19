using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class TalkNotesReqDto
    {
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
        /// 所属房间
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// 记录类型  21:文字    23:表情    22:图片
        /// </summary>
        public int MsgTypeChild { get; set; }

        /// <summary>
        /// Guid 用于页面聊天记录命名和管理员删除
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
