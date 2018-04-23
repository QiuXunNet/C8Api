using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Plan.Request
{
    public class ExpertSearchListReqDto
    {
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
