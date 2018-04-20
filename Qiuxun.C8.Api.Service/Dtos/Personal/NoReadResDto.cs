using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    public class NoReadResDto
    {
        /// <summary>
        /// 动态消息未读数量
        /// </summary>
        public int DynamicCount { get; set; }

        /// <summary>
        /// 系统消息未读数量
        /// </summary>
        public int SysMessageCount { get; set; }
    }
}
