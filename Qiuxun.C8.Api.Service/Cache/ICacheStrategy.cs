using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheStrategy
    {        
        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        void AddObject<T>(string cacheKey, T obj);

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="expire">到期时间,单位:分钟</param>
        void AddObject<T>(string cacheKey, T obj, int expire);

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="date">日期</param>
        void SetObject<T>(string cacheKey, T obj, DateTime date);

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="obj">缓存对象</param>
        void SetObject<T>(string cacheKey, T obj);

        ///// <summary>
        ///// 添加指定ID的对象(关联指定键值组)
        ///// </summary>
        ///// <param name="cacheKey">缓存键</param>
        ///// <param name="obj">缓存对象</param>
        ///// <param name="dependKey">依赖键</param>
        //void AddObjectWithDepend(string cacheKey, object obj, string[] dependKey);

        ///// <summary>
        ///// 添加指定ID的对象(关联ICacheDependency)
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <param name="obj"></param>
        ///// <param name="dep"></param>
        //void AddObjectWithDepend(string cacheKey, object obj, string dependKey);

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        void RemoveObject(string cacheKey);
        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        T GetObject<T>(string cacheKey);
        
        /// <summary>
        /// 清空的有缓存数据
        /// </summary>
        void FlushAll();

        bool Exists(string cacheKey);
    }
}
