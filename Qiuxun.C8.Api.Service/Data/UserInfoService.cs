using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptSharp;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Extension;

namespace Qiuxun.C8.Api.Service.Data
{
    public class UserInfoService
    {


        public ApiResult<LoginResDto> Login(LoginReqDto reqDto, HttpRequestMessage request)
        {
            UserInfoService service = new UserInfoService();
            UserInfo accountInfo = service.GetFullUserInfoByMobile(reqDto.Account);
            if (accountInfo == null)
            {
                throw new ApiException(15023, "用户名不存在或密码错误");
            }

            if (accountInfo.Password.StartsWith("$2y"))
            {
                if (!Crypter.CheckPassword(reqDto.Password, accountInfo.Password))
                {
                    if (Tool.GetMD5(reqDto.Password) != accountInfo.Password)
                    {
                        throw new ApiException(15023, "用户名不存在或密码错误");
                    }
                    //throw new ApiException(15023, "用户名不存在或密码错误");
                }


                //if (HashHelper.Encrypt(HashCryptoType.MD5, loginPassword, "") != accountInfo.Password)
                //{
                //    throw new ApiException(15023, "用户名不存在或密码错误");
                //}
            }

            LoginResDto resDto = new LoginResDto()
            {
                Account = accountInfo.Mobile,
                Avater = accountInfo.Headpath,
                UserId = accountInfo.Id,
                Mobile = accountInfo.Mobile,
                NickName = accountInfo.Name
            };

            #region 下发登录token
            IdentityInfo authInfo = new IdentityInfo()
            {
                UserId = accountInfo.Id,
                UserAccount = accountInfo.Mobile,
                UserStatus = (int)accountInfo.State,
                UserName = accountInfo.Name,
                IsTemp = false,
                Avater = accountInfo.Headpath
            };

            var tokenAuth = new QiuxunTokenAuthorizer(new ApiAuthContainer(request));
            tokenAuth.Authorize(authInfo);
            #endregion

            return new ApiResult<LoginResDto>()
            {
                Data = resDto
            };
        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="reqDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApiResult SetPassword(SetPasswordReqDto reqDto, long userId)
        {
            string password = Tool.GetMD5(reqDto.Password);

            string sql = "update dbo.userInfo set [Password]=@Password where Id=@UserId";
            var sqlParameter = new[]
            {
                new SqlParameter("@Password",password),
                new SqlParameter("@UserId",userId),
            };
            int count = SqlHelper.ExecuteNonQuery(sql, sqlParameter);

            if (count < 1)
            {
                return new ApiResult(11001, "设置失败");
            }

            return new ApiResult();
        }

        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByMobile(string account)
        {
            string sql = "select top(1) * from dbo.UserInfo where Mobile = @mobile ";

            var list = Util.ReaderToList<UserInfo>(sql, new SqlParameter("@Mobile", account));

            if (list != null)
                return list.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 获取用户完整信息，
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserInfo GetFullUserInfoByMobile(string account)
        {
            string sql = @"select top(1) * from dbo.UserInfo a 
        left join dbo.ResourceMapping b on b.FkId=a.Id and b.[Type]=@ResourceType
        where a.Mobile = @mobile ";

            var sqlParameter = new[]
            {
                new SqlParameter("@Mobile", account),
                new SqlParameter("@ResourceType", (int)ResourceTypeEnum.用户头像),
            };
            var list = Util.ReaderToList<UserInfo>(sql, sqlParameter);

            if (list != null)
                return list.FirstOrDefault();
            return null;
        }



    }
}
