using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Memcached.Client;

namespace Qiuxun.C8.Api.Service.Cache
{
    /// <summary>
    /// MemCached缓存
    /// </summary>
    public class MemCachedStrategy : ICacheStrategy
    {
        #region 构造函数 初始化
        private static MemcachedClient MemClient;
        static readonly object padlock = new object();

        //线程安全的单例模式
        public static MemcachedClient getInstance()
        {
            if (MemClient == null)
            {
                lock (padlock)
                {
                    if (MemClient == null)
                    {
                        MemClientInit();
                    }
                }
            }
            return MemClient;
        }
        //初始化缓存
        static void MemClientInit()
        {
            try
            {
                MemClient = CallContext.GetData("client") as MemcachedClient;
                if (MemClient == null)
                {
                    string strAppMemcachedServer = System.Configuration.ConfigurationManager.AppSettings["MemcachedServerList"];
                    string[] servers = strAppMemcachedServer.Split(',');
                    //初始化池
                    SockIOPool pool = SockIOPool.GetInstance();
                    pool.SetServers(servers);
                    pool.InitConnections = 3;
                    pool.MinConnections = 3;
                    pool.MaxConnections = 500;
                    pool.SocketConnectTimeout = 1000;
                    pool.SocketTimeout = 3000;
                    pool.MaintenanceSleep = 30;
                    pool.Failover = true;
                    pool.Nagle = false;
                    pool.Initialize();
                    //客户端实例
                    MemClient = new MemcachedClient();
                    MemClient.EnableCompression = false;
                    CallContext.SetData("client", MemClient);
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //构造函数
        static MemCachedStrategy()
        {
            getInstance();
        }
        #endregion

        #region  添加缓存
        /// <summary>
        /// 添加缓存 永久
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public void AddObject<T>(string cacheKey, T obj)
        {
            AddObject(cacheKey, obj, 0);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        /// <param name="expire">分钟</param>
        public void AddObject<T>(string cacheKey, T obj, int expire = 10)
        {
            string json = obj.ToJsonString();

            if (MemClient.KeyExists(cacheKey))
            {
                MemClient.Replace(cacheKey, json, DateTime.Now.AddMinutes(expire));
            }
            else
            {
                MemClient.Set(cacheKey, json, DateTime.Now.AddMinutes(expire));
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="obj"></param>
        public void SetObject<T>(string cacheKey, T obj)
        {
            string json = obj.ToJsonString();

            if (MemClient.KeyExists(cacheKey))
            {
                MemClient.Replace(cacheKey, json);
            }
            else
            {
                MemClient.Set(cacheKey, json);
            }
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

            if (MemClient.KeyExists(cacheKey))
            {
                MemClient.Replace(cacheKey, json, date);
            }
            else
            {
                MemClient.Set(cacheKey, json, date);
            }
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
            MemClient.Delete(cacheKey);
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
            string json = MemClient.Get(cacheKey).ToString();
            return json.FromJsonString<T>();            
        }
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void FlushAll()
        {
            MemClient.FlushAll();
        }
        #endregion
    }
}
