using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 接口版本服务类
    /// </summary>
    public class VersionService
    {
        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="version">客户端版本</param>
        /// <param name="clientType">设备类型</param>
        /// <param name="customerSourceId">客户来源Id</param>
        /// <param name="appName">App编码</param>
        /// <returns></returns>
        public CheckerResDto CheckVersion(ApiVersion version, DevicePlatform clientType, byte customerSourceId,
            string appName)
        {
            long versionCode = GetDefaultVersionCode(version.FullVersion, clientType);
            //客户端版本
            string clientSourceKey = string.Format("client_source_{0}_{1}_{2}_{3}",
                GetDefaultVersionCode(version.FullVersion, clientType),
                (int)clientType, customerSourceId, appName);

            var resDto = CacheHelper.GetCache<CheckerResDto>(clientSourceKey);

            if (resDto == null)
            {
                var sourceVersion = GetSourceVersion(clientType, customerSourceId, appName, versionCode);

                if (sourceVersion != null)
                {
                    resDto = new CheckerResDto()
                    {
                        Status = (int)sourceVersion.UpdateType,
                        Content = string.Empty,
                        Downurl = string.Empty
                    };

                    if ((sourceVersion.UpdateType == ClientUpdateStatus.Optional ||
                         sourceVersion.UpdateType == ClientUpdateStatus.Force) && sourceVersion.UpdateToVersionCode > 0)
                    {
                        var updateToSourceVersion = GetSourceVersion(clientType, customerSourceId, appName,
                            sourceVersion.UpdateToVersionCode);
                        if (updateToSourceVersion != null)
                        {
                            resDto.Content = string.IsNullOrWhiteSpace(updateToSourceVersion.ClientVersionDesc)
                                ? ""
                                : updateToSourceVersion.ClientVersionDesc;
                            resDto.Downurl = string.IsNullOrWhiteSpace(updateToSourceVersion.UpdateLink)
                                ? ""
                                : updateToSourceVersion.UpdateLink;

                        }
                    }

                    CacheHelper.WriteCache(clientSourceKey, resDto, 1440);
                }
                else
                {
                    resDto = new CheckerResDto()
                    {
                        Status = (int)ClientUpdateStatus.None,
                        Content = "",
                        Downurl = ""
                    };
                }
            }

            return resDto;

        }


        private long GetDefaultVersionCode(long fullVersion, DevicePlatform clientType)
        {
            if ((clientType == DevicePlatform.Android) && (fullVersion >= 540431955284459520))
            {
                return 108227128545247242;
            }
            if ((clientType == DevicePlatform.Ios) && (fullVersion < 216876491030396828))
            {
                return 216876491030396828;
            }
            return fullVersion;
        }


        /// <summary>
        /// 根据版本号获取版本升级信息
        /// </summary>
        /// <param name="clientType">客户端类型</param>
        /// <param name="customerSourceId">安装来源</param>
        /// <param name="appServerCode">app编码</param>
        /// <param name="versionCode">版本号</param>
        /// <returns></returns>
        public ClientSourceVersion GetSourceVersion(DevicePlatform clientType, byte customerSourceId,
            string appServerCode, long versionCode)
        {
            string sql = @"SELECT TOP 1 * FROM dbo.ClientSourceVersion 
WHERE [ClientSource]=@ClientSource AND [ClientType]=@ClientType AND [ServerCode]=@ServerCode AND [VersionCode]=@VersionCode";

            var param = new[]
            {
                new SqlParameter("@ClientSource",customerSourceId),
                new SqlParameter("@ClientType",(int)clientType),
                new SqlParameter("@ServerCode",appServerCode),
                new SqlParameter("@VersionCode",versionCode),
            };

            return Util.ReaderToModel<ClientSourceVersion>(sql, param);
        }

        /// <summary>
        /// 获取最新的升级版本信息
        /// </summary>
        /// <param name="clientType">客户端类型</param>
        /// <param name="customerSourceId">安装来源</param>
        /// <param name="appServerCode">app编码</param>
        /// <returns></returns>
        public ClientSourceVersion GetLastSourceVersion(DevicePlatform clientType, byte customerSourceId,
            string appServerCode)
        {
            string sql = @"SELECT TOP 1 * FROM dbo.ClientSourceVersion 
WHERE [ClientSource]=@ClientSource AND [ClientType]=@ClientType AND [ServerCode]=@ServerCode
ORDER BY VersionCode DESC";

            var param = new[]
            {
                new SqlParameter("@ClientSource",customerSourceId),
                new SqlParameter("@ClientType",(int)clientType),
                new SqlParameter("@ServerCode",appServerCode),
            };

            return Util.ReaderToModel<ClientSourceVersion>(sql, param);
        }
    }
}
