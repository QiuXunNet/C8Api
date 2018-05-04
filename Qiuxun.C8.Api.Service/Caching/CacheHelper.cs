using System;
using Memcached.Client;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Caching;

namespace Qiuxun.C8.Api.Service.Caching
{
    public class CacheHelper
    {
        public static MemcachedClient memcachedClient
        {
            get { return MemClientFactory.GetCurrentMemClient(); }
        }

        public static void AddCache(string key, object value)
        {
            memcachedClient.Add(key, value);
        }

        public static void AddCache(string key, object value, DateTime expDate)
        {
            memcachedClient.Add(key, value, expDate);
        }



        public static object GetCache(string key)
        {
            return memcachedClient.Get(key);
        }


        public static void SetCache(string key, object value)
        {
            memcachedClient.Set(key, value);
        }

        public static void SetCache(string key, object value, DateTime expDate)
        {
            memcachedClient.Set(key, value, expDate);
        }

        public static void DeleteCache(string key)
        {
            memcachedClient.Delete(key);
        }


        public static bool IsExistCache(string key)
        {
            return memcachedClient.KeyExists(key);
        }



        public static void WriteCache<T>(string key, T value, int minutes = 10) where T : class
        {
            if (memcachedClient.KeyExists(key))
            {

                memcachedClient.Replace(key, value.ToJsonString(), DateTime.Now.AddMinutes(minutes));
            }
            else
            {
                memcachedClient.Set(key, value.ToJsonString(), DateTime.Now.AddMinutes(minutes));
            }
        }

        public static T GetCache<T>(string key) where T : class
        {
            try
            {
                string json = memcachedClient.Get(key).ToString();
                LogHelper.InfoFormat("key:{0}，value:{1}", key, json);

                return json.FromJsonString<T>();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }
}
