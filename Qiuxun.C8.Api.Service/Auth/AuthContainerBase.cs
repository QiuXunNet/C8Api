using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Auth
{
    public abstract class AuthContainerBase
    {
        private bool? _isRefresh;
        private string _token;

        protected AuthContainerBase()
        {
        }

        private string FormatRefreshKey(Guid userId)
        {
            return string.Format("refr_{0}", userId.ToString("N"));
        }

        protected abstract KeyValuePair<string, string> GetCookie(string key);
        internal bool GetRefreshFlag(Guid userId)
        {
            if (!this._isRefresh.HasValue)
            {
                string key = this.FormatRefreshKey(userId);
                KeyValuePair<string, string> cookie = this.GetCookie(key);
                this._isRefresh = new bool?(!string.IsNullOrEmpty(cookie.Key));
            }
            return this._isRefresh.Value;
        }

        internal string GetToken()
        {
            if (this._token == null)
            {
                KeyValuePair<string, string> cookie = this.GetCookie("token");
                this._token = string.IsNullOrEmpty(cookie.Key) ? "" : cookie.Value;
            }
            return this._token;
        }

        protected abstract void SetCookie(string key, string value, DateTime expired);
        internal void SetRefreshFlag(Guid userId)
        {
            string key = this.FormatRefreshKey(userId);
            this.SetCookie(key, Guid.NewGuid().ToString("N"), DateTime.Now.AddMinutes(43200.0));
        }

        internal void SetToken(string value, DateTime expired)
        {
            this.SetCookie("token", value, expired);
        }
    }
}
