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

namespace Qiuxun.C8.Api.Controllers
{
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

            return userInfoService.Login(reqDto,this.Request);

        }

        [HttpGet]
        public ApiResult GetUserInfo()
        {
            return new ApiResult(100, "获取成功");
        }
    }
}