using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 文件上传响应类
    /// </summary>
    public class UploadFileResDto
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResouceId { get; set; }
        /// <summary>
        /// 资源地址
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 文件上传参数类
    /// </summary>
    public class UploadFileReqDto
    {
        /// <summary>
        /// Base64图片信息
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// 资源类型，1=文章资源 2=用户头像 3=评论图片，可空
        /// </summary>
        public int? Type { get; set; }
    }
}
