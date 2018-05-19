using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Web;
using QX.Common.Cache;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// 本地缓存
    /// </summary>
    public class LocalCacheStrategy : ICacheManager
    {
        protected static volatile System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;

        /// <summary>
        /// 缓存更新或删除的回调代理
        /// </summary>
        private CacheItemRemovedCallback callBack;

        /// <summary>
        /// 默认缓存存活期为3600秒(1小时)
        /// </summary>
        private int _timeOut = 60;

        /// <summary>
        /// 设置到期相对时间[单位: 分钟] 
        /// </summary>
        public virtual int TimeOut
        {
            set { _timeOut = value; }
            get { return _timeOut; }
        }

        public LocalCacheStrategy()
        {
            callBack = new CacheItemRemovedCallback(onRemove);
        }


        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="reason"></param>
        private void onRemove(string cacheKey, object obj, CacheItemRemovedReason reason)
        {
            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Underused:
                    {
                        break;
                    }
                //缓存更新和删除都会走这个分支，可以在这里做缓存更新后的处理工作，比如同步到其他服务器
                case CacheItemRemovedReason.Removed:
                    {
                        UpdateOtherServer(cacheKey, obj);
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// 同步到其他缓存服务器
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        private void UpdateOtherServer(string cacheKey, object obj)
        {

        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            var json = cache.Get(key).ToString();
            return json.FromJsonString<T>();
        }

        public T Get<T>(string key, Func<T> fun, int cacheTime = 20)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = fun();
            Set(key, obj, cacheTime);
            return obj;

        }

        public void Set(string key, object data)
        {
            string json = data.ToJsonString();

            if (string.IsNullOrEmpty(key) || data == null)
            {
                return;
            }

            cache.Insert(key, json, null, DateTime.Now.AddMinutes(TimeOut), TimeSpan.Zero, CacheItemPriority.High, callBack);
        }

        public void Set(string key, object data, int expire)
        {
            string json = data.ToJsonString();

            if (string.IsNullOrEmpty(key) || data == null)
            {
                return;
            }
            if (expire == 0)
            {
                cache.Insert(key, json, null, DateTime.Now.AddMinutes(TimeOut), TimeSpan.Zero, CacheItemPriority.High, callBack);
            }
            else
            {
                cache.Insert(key, json, null, DateTime.Now.AddMinutes(expire), TimeSpan.Zero, CacheItemPriority.High, callBack);
            }
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public void RemoveList(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }
    }
}
