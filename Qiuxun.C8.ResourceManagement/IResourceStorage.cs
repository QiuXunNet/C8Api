using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Enum;

namespace Qiuxun.C8.ResourceManagement
{
    /// <summary>
    /// 资源存储接口
    /// </summary>
    public interface IResourceStorage
    {
        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="originBucket"></param>
        /// <param name="destinationBucket"></param>
        /// <param name="path"></param>
        void CopyTo(string originBucket, string destinationBucket, string path);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fullFileName"></param>
        void Delete(string fullFileName);
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        Stream Download(string fullFileName);
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        Uri GetUrl(string key, int expireMinutes);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mimeType"></param>
        /// <param name="file"></param>
        void Upload(string fileName, string mimeType, Stream file);
        /// <summary>
        /// 主机头
        /// </summary>
        string Host { get; }
        /// <summary>
        /// 根节点
        /// </summary>
        string Root { get; }
        /// <summary>
        /// 存储类型
        /// </summary>
        StorageType StorageType { get; }
    }
}
