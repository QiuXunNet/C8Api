using System;
using System.Runtime.Remoting.Messaging;
using Memcached.Client;

namespace Qiuxun.C8.Caching
{
    public class MemClientFactory
    {
        private static MemcachedClient client = null;

        public static MemcachedClient GetCurrentMemClient()
        {
            MemcachedClient client = CallContext.GetData("client") as MemcachedClient;
            if (client == null)
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
                client = new MemcachedClient();
                client.EnableCompression = false;
                CallContext.SetData("client", client);
            }
            return client;
        }



        public static MemcachedClient GetCurrentMemClient2()
        {
            if (client == null)
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
                client = new MemcachedClient();
                client.EnableCompression = false;
            }

            return client;
        }



    }
}
