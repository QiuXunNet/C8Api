using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 拉黑列表返回参数类
    /// </summary>
    public class BlackListResDto
    {
        /// <summary>
        /// 拉黑记录Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 被拉黑人Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 被拉黑人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 被拉黑人头像
        /// </summary>
        public string PhotoImg { get; set; }

        /// <summary>
        /// 被拉黑人所属房间
        /// </summary>
        public string RoomId { get; set; }
    }
}
