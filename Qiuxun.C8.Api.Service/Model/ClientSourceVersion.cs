using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    /// 客户端版本
    /// </summary>
    public class ClientSourceVersion
    {
        /// <summary>
        /// 安装来源 1=官网 详见接口文档安装来源表
        /// </summary>
        public int ClientSource { get; set; }

        /// <summary>
        /// 客户端类型 0=未知 1=Android 2=IOS 3=Browser 4=Weixin
        /// </summary>
        public DevicePlatform ClientType { get; set; }

        /// <summary>
        /// 版本描述信息
        /// </summary>
        public string ClientVersionDesc { get; set; }
        /// <summary>
        /// 客户端版本ID
        /// </summary>
        public long ClientVersionId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新链接
        /// </summary>
        public string UpdateLink { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 升级至版本号
        /// </summary>
        public long UpdateToVersionCode { get; set; }
        /// <summary>
        /// 升级至的版本号名称
        /// </summary>
        public string UpdateToVersionName { get; set; }
        /// <summary>
        /// 升级方式 1=无需升级 2=用户可选择是否升级 3=强制用户升级
        /// </summary>
        public ClientUpdateStatus UpdateType { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>
        public long VersionCode { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 服务器编码
        /// </summary>
        public string ServerCode { get; set; }
    }
}
