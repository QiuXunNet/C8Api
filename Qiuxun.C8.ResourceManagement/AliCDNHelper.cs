using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Cdn.Model.V20141111;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Qiuxun.C8.ResourceManagement.Data;

namespace Qiuxun.C8.ResourceManagement
{
    /// <summary>
    /// Aliyun CDN帮助类
    /// </summary>
    public class AliCDNHelper
    {
        private static string serverUrl = "http://cdn.aliyuncs.com";

        private static string accessKeyId = ConfigurationManager.AppSettings["oss_key_id"] ?? "";

        private static string accessKeySecret = ConfigurationManager.AppSettings["oss_key_secret"] ?? "";

        private static string regionId = ConfigurationManager.AppSettings["oss_region_id"] ?? "cn-shenzhen";

        private static IAcsClient client = new DefaultAcsClient(DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret));
        /// <summary>
        /// 刷新CDN缓存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        public static void Refresh(string path, string type = "File")
        {
            //RefreshObjectCachesRequest
            RefreshObjectCachesRequest refreshObjectCachesRequest = new RefreshObjectCachesRequest();
            refreshObjectCachesRequest.ObjectType = type;
            refreshObjectCachesRequest.ObjectPath = path;
            refreshObjectCachesRequest.AcceptFormat = FormatType.JSON;

            try
            {
                var httpResponse = client.DoAction(refreshObjectCachesRequest);
                if (httpResponse.Status != 200)
                    throw new HttpRequestException(string.Format("刷新CDN缓存失败,Path:{0}", path));

            }
            catch (WebException ex)
            {
                //记录日志：刷新CDN缓存失败
                LogHelper.ErrorFormat($"刷新CDN缓存失败。Path:{path}；异常消息：{ex.Message},异常堆栈：{ex.StackTrace}");
            }

        }
    }
}
