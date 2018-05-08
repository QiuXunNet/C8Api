using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Qiuxun.C8.ResourceManagement.Enum;

namespace Qiuxun.C8.ResourceManagement
{
    public class AliOssStorage : IResourceStorage, IPictureProcessor
    {
        private readonly string _accessKeyId;
        private readonly string _accessKeySecret;
        private readonly string _bucketName;
        private readonly OssClient _ossClient;
        private const int _partSize = 0x200000;
        //protected ILog Log = LogManager.GetLogger(typeof(AliOssStorage));

        /// <summary>
        /// 初始化阿里云Oss存储
        /// </summary>
        /// <param name="accessKeyId">Key</param>
        /// <param name="accessKeySecret">密钥</param>
        /// <param name="bucketName">节点名称</param>
        /// <param name="host">主机头</param>
        /// <param name="root">根节点</param>
        public AliOssStorage(string accessKeyId, string accessKeySecret, string bucketName, string host, string root)
        {
            this._accessKeyId = accessKeyId;
            this._accessKeySecret = accessKeySecret;
            this._bucketName = bucketName;
            this.Host = host;
            this.Root = root;
            ClientConfiguration configuration = new ClientConfiguration()
            {
                ConnectionTimeout = 300000
            };

            var uri = new Uri(Host.Replace(_bucketName + ".", ""));
            this._ossClient = new OssClient(uri, _accessKeyId, _accessKeySecret, configuration);
        }
        /// <summary>
        /// 资源拷贝
        /// </summary>
        /// <param name="originBucket">源Bucket</param>
        /// <param name="destinationBucket">目标Bucket</param>
        /// <param name="path">资源目录</param>
        public void CopyTo(string originBucket, string destinationBucket, string path)
        {
            CopyObjectRequest copyObjectRequest = new CopyObjectRequest(originBucket, path, destinationBucket, path);
            _ossClient.CopyObject(copyObjectRequest);
        }
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="fullFileName"></param>
        public void Delete(string fullFileName)
        {
            _ossClient.DeleteObject(_bucketName, fullFileName);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="fullFileName">文件名称</param>
        /// <returns></returns>
        public Stream Download(string fullFileName)
        {
            OssObject obj = _ossClient.GetObject(_bucketName, fullFileName);
            if (obj == null || obj.Content == null)
                throw new FileNotFoundException();
            return obj.Content;
        }
        /// <summary>
        /// 获取资源Url地址
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public Uri GetUrl(string key, int expireMinutes)
        {
            return _ossClient.GeneratePresignedUri(_bucketName, key, DateTime.Now.AddMinutes(expireMinutes));
        }

        /// <summary>
        /// 上传资源
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="mimeType">文件类型</param>
        /// <param name="stream"></param>
        public void Upload(string fileName, string mimeType, Stream stream)
        {
            //根据资源大小计算Multipart 2MB为一个Part
            int num = (int)(stream.Length / 0x200000L);
            if (num > 0 && (stream.Length % 0x200000L) != 0)
            {
                num += 1;
            }

            ObjectMetadata metadata = new ObjectMetadata()
            {
                ContentType = mimeType
            };

            try
            {
                if (num > 1)
                {
                    InitiateMultipartUploadRequest initiateMultipartUploadRequest =
                        new InitiateMultipartUploadRequest(_bucketName, fileName)
                        {
                            ObjectMetadata = metadata
                        };
                    InitiateMultipartUploadResult result =
                        _ossClient.InitiateMultipartUpload(initiateMultipartUploadRequest);
                    Console.WriteLine("UploadId:" + result.UploadId);
                    List<PartETag> collection = new List<PartETag>();

                    for (int i = 0; i < num; i++)
                    {
                        int begin = 0x200000 * i;
                        stream.Seek((long)begin, SeekOrigin.Begin);
                        long end = 0x200000 < (stream.Length - begin) ? 0x200000L : stream.Length - begin;

                        UploadPartRequest uploadPartRequest = new UploadPartRequest(_bucketName, fileName,
                            result.UploadId)
                        {
                            InputStream = stream,
                            PartSize = end,
                            PartNumber = i + 1
                        };
                        var partResult = _ossClient.UploadPart(uploadPartRequest);
                        collection.Add(partResult.PartETag);
                    }
                    var completeMultipartUploadRequest = new CompleteMultipartUploadRequest(_bucketName, fileName,
                        result.UploadId);
                    ((List<PartETag>)completeMultipartUploadRequest.PartETags).AddRange(collection);
                    _ossClient.CompleteMultipartUpload(completeMultipartUploadRequest);
                }
                else
                {
                    _ossClient.PutObject(_bucketName, fileName, stream, metadata);
                }
            }
            catch (WebException exception)
            {
                throw new Exception("上传失败，请重试", exception);
            }

        }
        /// <summary>
        /// 主机头
        /// </summary>
        public string Host { get; }
        /// <summary>
        /// 根节点名称
        /// </summary>
        public string Root { get; }
        /// <summary>
        /// 存储类型
        /// </summary>
        public StorageType StorageType
        {
            get
            {
                return StorageType.AliOssStorage;
            }
        }
        public IPictureProcess BeginProcessPicture()
        {
            return new AliOssPictureProcess();
        }
    }
}
