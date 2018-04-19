using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// 他人主页
    /// </summary>
    public class HasFollowResDto
    {
        /// <summary>
        /// 是否显示关注按钮
        /// </summary>
        public bool ShowFollow { get; set; }

        /// <summary>
        /// 是否已关注
        /// </summary>
        public bool Followed { get; set; }
    }
}
