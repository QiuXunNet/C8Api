using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptSharp;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Cache;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Extension;
using Qiuxun.C8.Caching;
using UserState = Qiuxun.C8.Api.Service.Enum.UserState;

namespace Qiuxun.C8.Api.Service.Auth
{


    public class QiuxunTokenAuthorizer
    {
        private AuthContainerBase _container;
        private IdentityInfo _identity;
        private string _token;
        private TokenInfo _tokenInfo;

        public QiuxunTokenAuthorizer(AuthContainerBase container)
        {
            this._container = container;
            string token = this._container.GetToken();
            this._token = token ?? "";
        }

        public QiuxunTokenAuthorizer(AuthContainerBase container, string token)
        {
            this._container = container;
            this._token = token ?? "";
        }

        private static IdentityInfo AuthCheckingOfDefault(string userAccount, string loginPassword)
        {
            UserInfoService service = new UserInfoService();
            UserInfo accountInfo = service.GetUserInfoByMobile(userAccount);
            if (accountInfo == null)
            {
                throw new ApiException(15023, "用户名不存在或密码错误");
            }

            if (accountInfo.Password.StartsWith("$2y"))
            {
                if (!Crypter.CheckPassword(loginPassword, accountInfo.Password))
                {
                    throw new ApiException(15023, "用户名不存在或密码错误");
                }

                if (Tool.GetMD5(loginPassword) != accountInfo.Password)
                {
                    throw new ApiException(15023, "用户名不存在或密码错误");
                }

                //if (HashHelper.Encrypt(HashCryptoType.MD5, loginPassword, "") != accountInfo.Password)
                //{
                //    throw new ApiException(15023, "用户名不存在或密码错误");
                //}
            }

            IdentityInfo info = accountInfo.ToIdentityInfo();
            if (info != null)
            {
                //TODO:查询第三方绑定信息
            }
            return info;
        }

        public void Authorize(IdentityInfo identity)
        {
            DateTime now = DateTime.Now;
            DateTime expireTime = now.AddMinutes(43200.0);
            identity.AuthTime = now;
            identity.ExpireTime = expireTime;
            this._token = PermissionHelper.EncryptToken(identity.UserId, now);
            identity.Token = this._token;

            try
            {
                //CacheHelper.WriteCache("identity_" + _token, identity, 43200);
                CacheHelper.AddCache("identity_" + _token, identity, 43200);
            }
            catch (Exception ex)
            {

                throw new ApiException(-100, "缓存设置失败，身份标识操作无效");
            }
            this._container.SetToken(this._token, expireTime);
        }

        public static ApiResult<IdentityInfo> CheckAuth(string userAccount, string loginPassword, int role = 0)
        {
            IdentityInfo info;
            ApiResult<IdentityInfo> result = new ApiResult<IdentityInfo>();
            if (string.IsNullOrEmpty(userAccount) || string.IsNullOrEmpty(loginPassword))
            {
                result.Code = 15021;
                result.Desc = "用户名或密码不能为空";
                return result;
            }
            if (userAccount.StartsWith("_"))
            {
                result.Code = 15000;
                result.Desc = "临时账号不允许登录";
                return result;
            }


            info = AuthCheckingOfDefault(userAccount, loginPassword);

            if ((info != null) && (info.UserStatus == (int)UserState.Disable))
            {
                result.Code = 15024;
                result.Desc = "用户已禁用";
                return result;
            }
            result.Data = info;
            return result;
        }

        public void Expire()
        {
            CacheHelper.DeleteCache("identity_" + _token);
            this._container.SetToken(this._token, DateTime.Now.AddMinutes(-60.0));
        }

        public IdentityInfo GetAuthInfo()
        {
            if (string.IsNullOrEmpty(this._token))
            {
                return null;
            }
            if (this._identity == null)
            {
                this._identity = CacheHelper.GetCache<IdentityInfo>("identity_" + _token);

                if (this._identity == null)
                    return null;
            }


            this._identity.Token = this._token;
            DateTime expireTime = DateTime.Now.AddMinutes(43200.0);
            TimeSpan span = (TimeSpan)(expireTime - this._identity.ExpireTime);
            if (span.TotalMinutes > 21600.0)
            {
                this._identity.ExpireTime = expireTime;

                CacheHelper.SetCache("identity_" + _token, _identity, expireTime);
                this._container.SetToken(this._token, expireTime);
            }
            return this._identity;
        }

        public static IdentityInfo GetAuthInfo(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            IdentityInfo info = CacheHelper.GetCache<IdentityInfo>("identity_" + token);
            if (info == null)
            {
                return null;
            }
            return info;
        }


        public void Update(IdentityInfo identity)
        {
            IdentityInfo info = CacheHelper.GetCache<IdentityInfo>("identity_" + _token);
            identity.ExpireTime = info.ExpireTime;
            try
            {
                //CacheHelper.SetCache("identity_" + _token, identity, identity.ExpireTime);
                CacheHelper.SetCache("identity_" + _token, identity, identity.ExpireTime);
            }
            catch (Exception ex)
            {
                throw new ApiException(-100, "缓存设置失败，身份标识操作无效");
            }

        }

        public void Update(IdentityInfo identity, DateTime expireTime)
        {
            identity.ExpireTime = expireTime;
            try
            {
                //CacheHelper.SetCache("identity_" + _token, identity, identity.ExpireTime);
                CacheHelper.SetCache("identity_" + _token, identity, identity.ExpireTime);
            }
            catch (Exception ex)
            {
                throw new ApiException(-100, "缓存设置失败，身份标识操作无效");
            }
        }

        public string Token
        {
            get
            {
                return this._token;
            }
        }

        public TokenInfo TokenInfo
        {
            get
            {
                if (this._tokenInfo == null)
                {
                    if (string.IsNullOrEmpty(this._token))
                    {
                        return null;
                    }
                    this._tokenInfo = PermissionHelper.DecryptToken(this._token);
                }
                return this._tokenInfo;
            }
        }
    }
}
