using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Enum;

namespace Qiuxun.C8.ResourceManagement
{
    /// <summary>
    /// 精准到小时的资源目录构建类
    /// </summary>
    public class AccurateToHourPathBuilder : IResourcePathBuilder
    {
        /// <summary>
        /// 构建资源目录
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public string BuildPath(ResourceType type, Guid resourceId, string fileName)
        {
            string path = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.ToString("MMdd"), DateTime.Now.ToString("HH"));
            switch (type)
            {
                case ResourceType.Content:
                    return string.Format("content/{0}/{1}/{2}", path, resourceId.ToString().ToLower(), fileName);

                case ResourceType.Business:
                    return string.Format("business/{0}/{1}", path, resourceId.ToString().ToLower());
            }
            throw new Exception("不支持的类型");
        }
    }
}
