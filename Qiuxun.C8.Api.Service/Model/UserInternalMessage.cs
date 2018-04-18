using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 用户通知消息
    /// </summary>
    public class UserInternalMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 通知消息类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 客户端编号
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// 查看时间
        /// </summary>
        public DateTime ReadTime { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime SubTime { get; set; }

        public string SubTimeStr {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm"); }
        }
        
    }
}
