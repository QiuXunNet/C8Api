using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Memcached.Client;
using QX.Common.Cache;

namespace Qiuxun.C8.Api.Service.Cache
{

    /// <summary>
    /// MemCached缓存
    /// </summary>
    public class MemCachedStrategy : ICacheManager
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
                    pool.MaxConnections = 5000;
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


        public T Get<T>(string key)
        {
            string json = MemClient.Get(key).ToString();
            return json.FromJsonString<T>();
        }

        public T Get<T>(string key, Func<T> fun, int cacheTime = 20)
        {
            if (IsSet(key))
            {
                return Get<T>(key);
            }
            else
            {
                T obj = fun();
                Set(key, obj, cacheTime);
                return obj;
            }
        }

        public void Set(string key, object data)
        {
            Set(key, data, 30);
        }

        public void Set(string key, object data, int expire)
        {
            string json = data.ToJsonString();

            if (MemClient.KeyExists(key))
            {
                MemClient.Replace(key, json, DateTime.Now.AddMinutes(expire));
            }
            else
            {
                MemClient.Set(key, json, DateTime.Now.AddMinutes(expire));
            }
        }

        public bool IsSet(string key)
        {
            return MemClient.KeyExists(key);
        }

        public void Remove(string key)
        {
            MemClient.Delete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            MemClient.FlushAll();
        }

        public void RemoveList(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {

                MemClient.Delete(key);
            }
        }
    }
}
