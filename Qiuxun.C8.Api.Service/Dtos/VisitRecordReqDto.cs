using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 新增访问记录请求参数类
    /// </summary>
    public class AddVisitRecordReqDto
    {
        /// <summary>
        /// 受访人用户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 访问模块 1=主页
        /// </summary>
        public int Module { get; set; } = 1;
    }
}
