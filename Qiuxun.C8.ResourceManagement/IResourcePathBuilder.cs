using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Enum;

namespace Qiuxun.C8.ResourceManagement
{
    /// <summary>
    /// 资源Path构建接口
    /// </summary>
    public interface IResourcePathBuilder
    {
        /// <summary>
        /// 构建Path
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="resourceId">资源Id</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        string BuildPath(ResourceType type, Guid resourceId, string fileName);
    }
}
