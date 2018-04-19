using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Qiuxun.C8.Api.Controllers
{
    public class PersonalController : QiuxunApiController
    {
        /// <summary>
        /// 获取个人中心首页展示数据
        /// </summary>
        /// <returns>IndexResDto：个人中心首页数据对象</returns>
        [HttpGet]
        public ApiResult<IndexResDto> PersonalIndexData()
        {
            PersonalService service = new PersonalService();
            IndexResDto resDto = service.GetPersonalIndexData(this.UserInfo.UserId);

            if (resDto == null)
            {
                return new ApiResult(60000, "登录超时，需要重新登录");
            }

            return new ApiResult<IndexResDto>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpwd">旧密码</param>
        /// <param name="newpwd">新密码</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ApiResult ModifyPWD(string oldpwd, string newpwd)
        {
            if (string.IsNullOrWhiteSpace(oldpwd))
            {
                return new ApiResult(60001, "旧密码不能为空");
            }
            if (string.IsNullOrWhiteSpace(newpwd))
            {
                return new ApiResult(60002, "新密码不能为空");
            }
            if (newpwd.Length < 6 || newpwd.Length > 12)
            {
                return new ApiResult(60003, "新密码长度要在6-12位之间");
            }

            PersonalService service = new PersonalService();

            return service.ModifyPWD(oldpwd, newpwd, this.UserInfo.UserId);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="value">新的值</param>
        /// <param name="type">1、昵称 2、签名 3、性别</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult EditUserInfo(string value, int type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new ApiResult(60001, "旧密码不能为空");
            }
            return new ApiResult(60001, "旧密码不能为空");
        }
    }
}
