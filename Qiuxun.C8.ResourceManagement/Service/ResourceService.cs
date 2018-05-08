using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
