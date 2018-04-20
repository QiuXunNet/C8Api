using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Auth;

namespace Qiuxun.C8.Api.Service.Extension
{
    public static class IdentityInfoExtensions
    {
        public static IdentityInfo ToIdentityInfo(this UserInfo accountInfo)
        {
            IdentityInfo info = null;
            if (accountInfo != null)
            {
                IdentityInfo info2 = new IdentityInfo
                {
                    UserId = accountInfo.Id,
                    UserAccount = accountInfo.Mobile,
                    UserName = accountInfo.Name,
                    UserStatus = accountInfo.State,
                    Avater = accountInfo.Headpath,

                };
                info = info2;
            }
            return info;
        }
    }
}
