using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Auth;
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
        public ApiResult<LoginResDto> Login(LoginReqDto reqDto)
        {
            if (string.IsNullOrWhiteSpace(reqDto.Account))
                throw new ApiException(16000, "参数account验证失败");

            if (string.IsNullOrWhiteSpace(reqDto.Password))
                throw new ApiException(16000, "参数password验证失败");

            return userInfoService.Login(reqDto, this.Request);

        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public ApiResult<LoginResDto> Register(RegisterReqDto reqDto)
        {
            #region 验证请求

            var smsService = new SmsService();
            smsService.ValidateSmsCode(new SmsCodeValidateDto()
            {
                Phone = reqDto.Phone,
                Code = reqDto.Code,
                Type = 1
            });

            if (reqDto == null)
                throw new ApiException(50000, "验证参数失败");
            if (!ValidateUtil.IsValidMobile(reqDto.Phone))
                throw new ApiException(50000, "手机号不正确");
            if (!ValidateUtil.IsValidUserName(reqDto.NickName))
                throw new ApiException(50000, "昵称格式不正确");
            if (!ValidateUtil.IsValidPassword(reqDto.Password))
                throw new ApiException(50000, "密码不符合");

            if (Tool.CheckSensitiveWords(reqDto.NickName))
                throw new ApiException(50000, "昵称包含敏感字符");
            if (userInfoService.ExistsMobile(reqDto.Phone))
                throw new ApiException(50000, "手机号已注册");
            if (userInfoService.ExistsNickName(reqDto.NickName))
                throw new ApiException(50000, "该昵称已被占用");

            #endregion

            return userInfoService.UserRegister(reqDto, this.Request);
        }

        /// <summary>
        /// 验证手机号存在
        /// </summary>
        /// <param name="phone">手机号，不可空</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult ValidateMobile(string phone)
        {
            if (!ValidateUtil.IsValidMobile(phone))
                throw new ApiException(50000, "手机号不正确");

            if (userInfoService.ExistsMobile(phone))
                throw new ApiException(50000, "手机号已存在");

            return new ApiResult();
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

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public ApiResult ForgotPassword(ForgotPasswordReqDto reqDto)
        {
            #region 验证参数
            if (reqDto == null)
                throw new ApiException(11000, "参数验证失败");
            //验证码验证
            var smsService = new SmsService();
            smsService.ValidateSmsCode(new SmsCodeValidateDto()
            {
                Phone = reqDto.Phone,
                Code = reqDto.Code,
                Type = 1
            });

            if (string.IsNullOrWhiteSpace(reqDto.Phone))
                throw new ApiException(11000, "参数Phone验证失败");
            if (string.IsNullOrWhiteSpace(reqDto.Password))
                throw new ApiException(11000, "参数Password验证失败");
            if (ValidateUtil.IsValidPassword(reqDto.Password))
                throw new ApiException(11000, "密码包含非法字符");
            #endregion

            return userInfoService.ForgotPassword(reqDto);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult Logout()
        {
            var authorizer = new QiuxunTokenAuthorizer(new ApiAuthContainer(this.Request));
            authorizer.Expire();

            return new ApiResult();
        }

        /// <summary>
        /// 删除用户账户
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult DeleteAccount(string phone)
        {
            #region 验证参数
            if (string.IsNullOrWhiteSpace(phone))
                throw new ApiException(11000, "参数Phone验证失败");

            if (!ValidateUtil.IsValidMobile(phone))
                throw new ApiException(50000, "手机号不正确");
            #endregion

            return userInfoService.DeleteAccount(phone);
        }

        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<UserInfo> GetUserInfo()
        {

            var data = userInfoService.GetFullUserInfoByMobile(UserInfo.UserAccount);

            return new ApiResult<UserInfo>()
            {
                Data = data
            };
        }
    }
}