using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class CheckerResDto
    {
        /// <summary>
        /// 状态 1：客户端无需升级 2：用户可选择是否升级 3：强制用户升级
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 更新说明
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Downurl { get; set; }
    }
}
