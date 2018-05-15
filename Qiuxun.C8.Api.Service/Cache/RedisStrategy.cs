using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Cache
{
    public class RedisStrategy : ICacheStrategy
    {
        DoRedisString doRedisString;

        public RedisStrategy()
        {
            doRedisString = new DoRedisString();
        } 

        #region
        /// <summary>
        /// 添加缓存 永久
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public void AddObject<T>(string cacheKey, T obj)
        {
            doRedisString.Set(cacheKey, obj);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="expire">分钟</param>
        public void AddObject<T>(string cacheKey, T obj, int expire = 10)
        {
           // string json = obj.ToJsonString();

            doRedisString.Set(cacheKey, obj, DateTime.Now.AddMinutes(expire));            
        }

        /// <summary>
        /// 添加缓存 永久
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public void SetObject<T>(string cacheKey, T obj)
        {
            doRedisString.Set(cacheKey, obj);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="data">日期</param>
        public void SetObject<T>(string cacheKey, T obj, DateTime data)
        {
            // string json = obj.ToJsonString();
            doRedisString.Set(cacheKey, obj, data);
        }
        #endregion

        #region 不实现缓存依赖
        /// <summary>
        /// 不实现
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string cacheKey, object obj, string[] dependKey)
        {

        }
        /// <summary>
        /// 不实现
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string cacheKey, object obj, string dependKey)
        {

        }
        /// <summary> 
        #endregion

        #region 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public void RemoveObject(string cacheKey)
        {
            doRedisString.Remove(cacheKey);
        }
        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public T GetObject<T>(string cacheKey)
        {
           return doRedisString.Get<T>(cacheKey);
        }
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void FlushAll()
        {
            doRedisString.RemoveAll();
        }


        #endregion


        public bool Exists(string cacheKey)
        {
           return doRedisString.KeyExists(cacheKey);
        }
    }
}
