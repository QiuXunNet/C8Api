using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Auth
{
    public class AuthSettings
    {
        internal static readonly byte[] _aes_iv_token = Convert.FromBase64String("LUDtqeTw36EbzBIho6RWNg==");
        private const string _aes_iv_token_str = "LUDtqeTw36EbzBIho6RWNg==";
        internal static readonly byte[] _aes_key_token = Convert.FromBase64String("PdVXDIZdBSPCImYo6iuNbxnN/rdKay65x5OoFrpHYq4=");
        private const string _aes_key_token_str = "PdVXDIZdBSPCImYo6iuNbxnN/rdKay65x5OoFrpHYq4=";
        public const string _cache_distribute_apps_key = "app_info_cache";
        public const string _cache_distribute_function_key = "function_{appId}";
        public const string _cache_distribute_permission_key = "permission_{roleId}_{appId}";
        public const string _cache_distribute_token_key = "identity_{token}";
        public const string _cache_distribute_user_in_role_key = "user_in_role_{userId}";
        public const string _cache_memory_apps = "app_info_cache";
        public const string _cache_memory_roles_permissions = "roles_and_permissions_{0}_{1}";
        public const string _cookie_refresh_key = "refr_{0}";
        public const string _cookie_token_key = "token";
        public const int _identity_distribute_expire_minutes = 43200;
        public const int _permission_distribute_expire_minutes = 1440;
        public const int _permission_memory_expire_minutes = 5;
        public const string _temp_account_prefix = "_";
        public const string _user_permission_request_get_key = "_user_permission_request_get_key";
        public const string _weixin_account_prefix = "_wx_";
        public const string C8AppName = "c8.api";
    }
}
