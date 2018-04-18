using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Public;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 权限控制
    /// </summary>
    public class AuthController : QiuxunApiController
    {
        UserInfoService userInfoService = new UserInfoService();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="reqDto">Account:账号，Password:密码</param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public ApiResult Login(LoginReqDto reqDto)
        {
            if (string.IsNullOrWhiteSpace(reqDto.Account))
                throw new ApiException(16000, "参数account验证失败");

            if (string.IsNullOrWhiteSpace(reqDto.Password))
                throw new ApiException(16000, "参数password验证失败");

            return userInfoService.Login(reqDto, this.Request);

        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="reqDto">设置密码</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult SetPassword(SetPasswordReqDto reqDto)
        {
            if (string.IsNullOrWhiteSpace(reqDto.Password))
                throw new ApiException(11000, "参数Password验证失败");

            if (ValidateUtil.IsValidPassword(reqDto.Password))
                throw new ApiException(11000, "密码包含非法字符");

            return userInfoService.SetPassword(reqDto, this.UserInfo.UserId);
        }


        [HttpGet]
        public ApiResult GetUserInfo()
        {
            return new ApiResult(100, "获取成功");
        }
    }
}