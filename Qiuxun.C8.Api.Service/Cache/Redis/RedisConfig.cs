using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Cache
{
    public static class RedisConfig
    {
        //public static RedisConfig GetConfig()
        //{
        //    RedisConfig section = GetConfig("RedisConfig");
        //    return section;
        //}

        //public static RedisConfig GetConfig(string sectionName)
        //{
        //    RedisConfig section = (RedisConfig)ConfigurationManager.GetSection(sectionName);
        //    if (section == null)
        //        throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
        //    return section;
        //}
        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        [ConfigurationProperty("WriteServerConStr", IsRequired = false)]
        public static string WriteServerConStr
        {
            //get
            //{
            //    return (string)base["WriteServerConStr"];
            //}
            //set
            //{
            //    base["WriteServerConStr"] = value;
            //}
            get { return ConfigurationManager.AppSettings["WriteServerConStr"]; }
        }


        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        [ConfigurationProperty("ReadServerConStr", IsRequired = false)]
        public static string ReadServerConStr
        {
            //get
            //{
            //    return (string)base["ReadServerConStr"];
            //}
            //set
            //{
            //    base["ReadServerConStr"] = value;
            //}
            get { return ConfigurationManager.AppSettings["ReadServerConStr"]; }
        }
        /// <summary>
        /// 最大写链接数
        /// </summary>
        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false, DefaultValue = 5)]
        public static int MaxWritePoolSize
        {
            //get
            //{
            //    int _maxWritePoolSize = (int)base["MaxWritePoolSize"];
            //    return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            //}
            //set
            //{
            //    base["MaxWritePoolSize"] = value;
            //}
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxWritePoolSize"]); }
        }


        /// <summary>
        /// 最大读链接数
        /// </summary>
        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public static int MaxReadPoolSize
        {
            //get
            //{
            //    int _maxReadPoolSize = (int)base["MaxReadPoolSize"];
            //    return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            //}
            //set
            //{
            //    base["MaxReadPoolSize"] = value;
            //}
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxReadPoolSize"]); }
        }


        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public static bool AutoStart
        {
            //get
            //{
            //    return (bool)base["AutoStart"];
            //}
            //set
            //{
            //    base["AutoStart"] = value;
            //}
            get { return Convert.ToBoolean (ConfigurationManager.AppSettings["AutoStart"]); }
        }



        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        [ConfigurationProperty("LocalCacheTime", IsRequired = false, DefaultValue = 36000)]
        public static int LocalCacheTime
        {
            //get
            //{
            //    return (int)base["LocalCacheTime"];
            //}
            //set
            //{
            //    base["LocalCacheTime"] = value;
            //}
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["LocalCacheTime"]); }
        }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        [ConfigurationProperty("RecordeLog", IsRequired = false, DefaultValue = false)]
        public static bool RecordeLog
        {
            //get
            //{
            //    return (bool)base["RecordeLog"];
            //}
            //set
            //{
            //    base["RecordeLog"] = value;
            //}
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["RecordeLog"]); }
        }
    }
}
