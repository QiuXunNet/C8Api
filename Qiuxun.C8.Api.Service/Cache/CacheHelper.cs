using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Qiuxun.C8.Api.Service.Cache;
using QX.Common.Cache;
using QX.Common.Cache.Redis;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// 缓存进行全局控制管理
    /// </summary>
    public static class CacheHelper
    {
        private static ICacheManager cs;
        private static object lockHelper = new object();

        /// <summary>
        /// 缓存键的前缀，以系统名为前缀
        /// </summary>
        private static string prefix = string.Empty;

        /// <summary>
        /// 缓存的有效期，读取配置信息CacheTimeOut，单位分钟
        /// 如果没有配置但用户又没有传入过期时间则认为永久不失效
        /// 如果配置为0也认为是永久不失效
        /// </summary>
        private static int timeOut = 0;

        static CacheHelper()
        {
            string provider = ConfigurationManager.AppSettings["CacheProvider"].ToLower();
            timeOut = Convert.ToInt32((ConfigurationManager.AppSettings["CacheTimeOut"]));

            switch (provider)
            {
                case CacheProvider.LocalCache:
                    cs = new LocalCacheStrategy();
                    break;
                case CacheProvider.Memcached:
                    cs = new MemCachedStrategy();
                    break;
                case CacheProvider.Redis:
                    cs = new RedisCache();
                    break;
                default:
                    cs = new LocalCacheStrategy();
                    break;
            }
        }

        /// <summary>
        /// 添加缓存，永久有效
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public static void AddCache<T>(string cacheKey, T obj)
        {
            lock (lockHelper)
            {
                // 如果没有配置但用户又没有传入过期时间则认为永久不失效
                // 如果配置为0也认为是永久不失效
                cs.Set(GetCacheKey(cacheKey), obj);
            }
        }

        /// <summary>
        /// 添加缓存，带失效时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="expire">失效时间,单位分钟</param>
        public static void AddCache<T>(string cacheKey, T obj, int expire)
        {
            lock (lockHelper)
            {
                cs.Set(GetCacheKey(cacheKey), obj, expire);
            }
        }

        /// <summary>
        /// 添加缓存，带失效时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public static void SetCache<T>(string cacheKey, T obj)
        {
            lock (lockHelper)
            {
                cs.Set(GetCacheKey(cacheKey), obj);
            }
        }

        /// <summary>
        /// 添加缓存，带失效时间
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="date">日期</param>
        public static void SetCache<T>(string cacheKey, T obj, DateTime date)
        {
            lock (lockHelper)
            {
                var timeSpan = date - DateTime.Now;

                cs.Set(GetCacheKey(cacheKey), obj, (int)timeSpan.TotalMinutes);
            }
        }

        /// <summary>
        /// 获取单个缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            try
            {
                T cacheObject = cs.Get<T>(GetCacheKey(cacheKey));
                if (cacheObject != null)
                {
                    return cacheObject;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        /// <summary>
        /// 删除单个缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void DeleteCache(string cacheKey)
        {
            lock (lockHelper)
            {
                cs.Remove(GetCacheKey(cacheKey));
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void FlushAll()
        {
            lock (lockHelper)
            {
                cs.Clear();
            }
        }

        /// <summary>
        /// 获取缓存键
        /// 建议键的组合：系统名_类名(模块名)_key
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        private static string GetCacheKey(string cacheKey)
        {
            return prefix + cacheKey;
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static bool Exists(string cacheKey)
        {
            return cs.IsSet(cacheKey);
        }
    }
}
