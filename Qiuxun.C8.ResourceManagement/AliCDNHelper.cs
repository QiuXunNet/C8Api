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

namespace Qiuxun.C8.ResourceManagement
{
    public class AliCDNHelper
    {
        private static string serverUrl = "http://cdn.aliyuncs.com";

        private static string accessKeyId = ConfigurationManager.AppSettings["oss_key_id"];

        private static string accessKeySecret = ConfigurationManager.AppSettings["oss_key_secret"];

        private static string regionId = ConfigurationManager.AppSettings["oss_region_id"];
        
        private static IAcsClient client = new DefaultAcsClient(DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret));

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
                throw ex;
                //记录日志：刷新CDN缓存失败
            }

        }
    }
}
