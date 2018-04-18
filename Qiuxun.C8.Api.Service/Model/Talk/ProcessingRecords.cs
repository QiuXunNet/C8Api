using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    public class ProcessingRecords
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 处理人Id
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 处理类型  1:删除消息   2:禁言   3:解除拉黑
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 被处理人Id
        /// </summary>
        public int ProcessToId { get; set; }

        /// <summary>
        /// 被处理人姓名
        /// </summary>
        public string ProcessToName { get; set; }

        /// <summary>
        /// 处理日期
        /// </summary>
        public DateTime ProcessDate { get; set; }

        /// <summary>
        /// ProcessTime
        /// </summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>
        /// 所属房间
        /// </summary>
        public int RoomId { get; set; }
    }
}
