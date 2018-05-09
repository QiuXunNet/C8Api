using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// 缓存提供者枚举
    /// </summary>
    public static class CacheProvider
    {
        /// <summary>
        /// 本地缓存
        /// </summary>
        public const string LocalCache = "localcache";
        /// <summary>
        /// Memcached分布式缓存
        /// </summary>
        public const string Memcached = "memcached";
        /// <summary>
        /// Redis分布式缓存
        /// </summary>
        public const string Redis = "redis";
        /// <summary>
        /// 本地加Memcached二级缓存
        /// </summary>
        public const string Localmemcached = "localmemcached";
        /// <summary>
        /// 本地加Redis二级缓存
        /// </summary>
        public const string LocalRedis = "localredis";
    
    }
}
