using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 校验验证码请求参数类
    /// </summary>
    public class ValidateCaptchaReqDto
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public int Type { get; set; } = 1;
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
