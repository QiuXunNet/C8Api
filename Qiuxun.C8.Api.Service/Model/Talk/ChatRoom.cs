using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 聊天房间实体
    /// </summary>
    public class ChatRoom
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        public int Code { get; set; }

        /// <summary>
        /// 房间名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 房间图片地址
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubTime { get; set; }
    }
}
