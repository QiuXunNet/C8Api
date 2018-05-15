using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Common;

namespace Qiuxun.C8.ResourceManagement.Service
{
    public class ResourceService
    {
        private const int _expire_mins = 15;        //过期时长（分）
        private const long _max_len = 0x200000L;    //最大单次上传大小
        protected IResourcePathBuilder _pathBuilder;
        protected string _privateBucket;
        private HashSet<int> _privateBusinessTypes = new HashSet<int> { 0x3e9, 0x3ea, 0x44d, 0x44e, 0x44f, 0x450, 0x452, 0x453 };
        protected string _privateHost;
        protected IResourceStorage _privateStorage;
        protected string _privateWatermarkHost;
        protected string _publicBucket;
        protected string _publicHost;
        protected IResourceStorage _publicStorage;


        public ResourceService()
        {
            string accessKeyId = ConfigurationManager.AppSettings["oss_key_id"];
            string accessKeySecret = ConfigurationManager.AppSettings["oss_key_secret"];
            string format = ConfigurationManager.AppSettings["oss_host"];
            string root = ConfigurationManager.AppSettings["oss_root"];
            _publicBucket = ConfigurationManager.AppSettings["oss_public_bucket"];
            string host = string.Format(format, _publicBucket);
            _privateBucket = ConfigurationManager.AppSettings["oss_private_bucket"];
            string privateHost = string.Format(format, _privateBucket);
            _publicStorage = new AliOssStorage(accessKeyId, accessKeySecret, _publicBucket, host, root);
            _privateStorage = new AliOssStorage(accessKeyId, accessKeySecret, _privateBucket, privateHost, root);
            _pathBuilder = new AccurateToHourPathBuilder();
            _publicHost = ConfigurationManager.AppSettings["resource_public_host"];
            if (string.IsNullOrWhiteSpace(_publicHost))
            {
                _publicHost = _publicStorage.Host;
            }
            _privateHost = ConfigurationManager.AppSettings["resource_private_host"];
            if (string.IsNullOrWhiteSpace(_privateHost))
            {
                _privateHost = _privateStorage.Host;
            }
            _privateWatermarkHost = ConfigurationManager.AppSettings["watermark_host"];
        }

        protected string BuildPathWithTimestamp(string path, DateTime updateTime)
        {
            return string.Format("{0}?_t={1}", path, updateTime.ToString("yyyyMMddHHmmssfff"));
        }


        public virtual string BuildUrl(long resourceId, bool isPrivate, bool isOrigin = false)
        {
            //查询资源Id

            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成文件Url
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="isOrigin">是否为源文件</param>
        /// <returns></returns>
        public string BuildUrl(string fileName, bool isPrivate, bool isOrigin = false)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            return BuildUrlInner(fileName, isPrivate, isOrigin);

        }

        /// <summary>
        /// 生成URL
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="isOrigin">是否为源文件</param>
        /// <returns></returns>
        protected string BuildUrlInner(string fileName, bool isPrivate, bool isOrigin)
        {
            if (!isPrivate) return UrlHelper.BuildUrl(_publicHost, fileName);

            if (!isOrigin) return UrlHelper.BuildUrl(_privateWatermarkHost, fileName);

            int index = fileName.IndexOf('@');
            if (index > 0)
            {
                fileName = fileName.Substring(0, index);
            }
            else
            {
                int length = fileName.IndexOf('?');
                if (length > 0)
                {
                    fileName = fileName.Substring(0, length);
                }
            }
            Uri url = _privateStorage.GetUrl(fileName, 15);
            return url.ToString().Replace(url.Scheme + "://" + url.Host, _privateHost);
        }


        public string InsertSingleReference(long resourceId, string businessId, int resourceType)
        {
            throw new NotImplementedException();
        }

        public List<string> InsertMultiReference(List<long> resouceId, string businessId, int resourceType)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="bytes">资源字节</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void Upload(string path, byte[] bytes, bool isPrivate, long maxLength = 0x200000)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Upload(path, stream, isPrivate, maxLength);
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stream">资源文件流</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void Upload(string path, Stream stream, bool isPrivate, long maxLength = 0x200000)
        {
            if (stream.Length > maxLength)
            {
                throw new ArgumentException($"上传的文件大小不能超过{maxLength >> 20}MB");
            }
            if (stream.CanSeek)
                stream.Position = 0;
            string contentType = UrlHelper.GetContentType(path);
            if (isPrivate)
            {
                _privateStorage.Upload(path, contentType, stream);
            }
            else
            {
                _publicStorage.Upload(path, contentType, stream);
            }
        }

        /// <summary>
        /// 上传并刷新CDN缓存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stream">资源文件流</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void UploadAndRefreshCDN(string path, Stream stream, bool isPrivate, long maxLength = 0x200000)
        {
            Upload(path, stream, isPrivate, maxLength);
            AliCDNHelper.Refresh(path);
        }
    }
}
