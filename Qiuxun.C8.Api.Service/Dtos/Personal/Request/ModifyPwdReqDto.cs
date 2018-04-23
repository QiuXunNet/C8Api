using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal.Request
{
    /// <summary>
    /// 修改密码请求类
    /// </summary>
    public class ModifyPwdReqDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string oldpwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string newpwd { get; set; }
    }
}
