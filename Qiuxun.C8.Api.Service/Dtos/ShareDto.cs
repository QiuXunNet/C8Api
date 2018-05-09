using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 分享
    /// </summary>
    public class ShareDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 成功回调地址
        /// </summary>
        public string SuccessResLink { get; set; }
        /// <summary>
        /// 失败回调地址
        /// </summary>
        public string FailResLink { get; set; }
    }
}
