using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal.Request
{
    /// <summary>
    /// 修改用户数据请求类
    /// </summary>
    public class EditUserInfoReqDto
    {
        /// <summary>
        /// 新的值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 1、昵称 2、签名 3、性别
        /// </summary>
        public int type { get; set; }
    }
}
