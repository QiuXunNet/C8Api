using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Web;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// 本地缓存
    /// </summary>
    public class LocalCacheStrategy : ICacheStrategy
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
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        public void AddObject<T>(string cacheKey, T obj)
        {
            string json = obj.ToJsonString();

            if (string.IsNullOrEmpty(cacheKey) || obj == null)
            {
                return;
            }
            cache.Insert(cacheKey, json, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        }
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="expire">到期时间,单位:分</param>
        public void AddObject<T>(string cacheKey, T obj, int expire)
        {
            string json = obj.ToJsonString();

            if (string.IsNullOrEmpty(cacheKey) || obj == null)
            {
                return;
            }
            if (expire == 0)
            {
                cache.Insert(cacheKey, json, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                cache.Insert(cacheKey, json, null, DateTime.Now.AddMinutes(expire),TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="date">日期</param>
        public void SetObject<T>(string cacheKey, T obj)
        {
            string json = obj.ToJsonString();

            if (string.IsNullOrEmpty(cacheKey) || obj == null)
            {
                return;
            }
           
            cache.Insert(cacheKey, json, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="date">日期</param>
        public void SetObject<T>(string cacheKey, T obj, DateTime date)
        {
            string json = obj.ToJsonString();

            if (string.IsNullOrEmpty(cacheKey) || obj == null)
            {
                return;
            }
          
            cache.Insert(cacheKey, json, null, date, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            
        }

        ///// <summary>
        ///// 添加指定ID的对象(关联指定键值组)
        ///// </summary>
        ///// <param name="cacheKey">缓存键</param>
        ///// <param name="obj">缓存对象</param>
        ///// <param name="dependcacheKey">依赖键</param>
        //public void AddObjectWithDepend(string cacheKey, object obj, string[] dependKey)
        //{
        //    if (string.IsNullOrEmpty(cacheKey) || obj == null)
        //    {
        //        return;
        //    }

        //    CacheDependency dep = new CacheDependency(null, dependKey, DateTime.UtcNow);

        //    cache.Insert(cacheKey, obj, dep, DateTime.Now.AddMinutes(TimeOut), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        //}

        ///// <summary>
        ///// 添加指定ID的cache 有依赖项
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <param name="obj"></param>
        ///// <param name="dep">ICacheDependency</param>
        //public void AddObjectWithDepend(string cacheKey, object obj, string dependKey)
        //{
        //    if (string.IsNullOrEmpty(cacheKey) || obj == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        AddObjectWithDepend(cacheKey, obj, new string[] { dependKey });
        //    }
        //}

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public void RemoveObject(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                return;
            }
            cache.Remove(cacheKey);
        }
        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        public T GetObject<T>(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                return default(T);
            }
            var json =cache.Get(cacheKey).ToString();
            return json.FromJsonString<T>();
        }

        //public T Get<T>(string cacheKey)
        //{
        //    if (string.IsNullOrEmpty(cacheKey))
        //    {
        //        return default(T);
        //    }
        //    return (T)cache.Get(cacheKey);
        //}

        //public void Set(string cacheKey,object obj)
        //{
        //    if (string.IsNullOrEmpty(cacheKey) || obj == null)
        //    {
        //        return;
        //    }
        //    cache.Insert(cacheKey, obj, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        //}
        //public void Set(string cacheKey, object obj, int expire)
        //{
        //    if (string.IsNullOrEmpty(cacheKey) || obj == null)
        //    {
        //        return;
        //    }
        //    if (expire == 0)
        //    {
        //        cache.Insert(cacheKey, obj, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        //    }
        //    else
        //    {
        //        cache.Insert(cacheKey, obj, null, DateTime.Now.AddMinutes(expire),TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
        //    }
        //}
        /// <summary>
        /// 清空的有缓存数据
        /// </summary>
        public void FlushAll()
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                cache.Remove(CacheEnum.Key.ToString());
            }
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
    }
}
