using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 注册请求参数类
    /// </summary>
    public class RegisterReqDto
    {
        /// <summary>
        /// 注册手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邀请注册码
        /// </summary>
        public long? InviteCode { get; set; }
    }
}
