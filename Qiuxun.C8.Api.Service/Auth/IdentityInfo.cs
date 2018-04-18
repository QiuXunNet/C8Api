using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Auth
{
    /// <summary>
    /// 身份认证信息
    /// </summary>
    public class IdentityInfo
    {
        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime AuthTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 设备识别号
        /// </summary>
        public string Imei { get; set; }

        /// <summary>
        /// 票据
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int UserStatus { get; set; }

        /// <summary>
        /// 是否临时（第三方登录未绑定手机时为true)
        /// </summary>
        public bool IsTemp { get; set; }
        /// <summary>
        /// 授权登录类型
        /// </summary>
        public OauthType OauthType { get; set; }
        /// <summary>
        /// 授权登录账号
        /// </summary>
        public string OauthAccount { get; set; }
    }
}
