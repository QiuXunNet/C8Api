using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 客户端版本
    /// </summary>
    public class VersionController : QiuxunApiController
    {
        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<CheckerResDto> Checkver()
        {
            var resDto = new CheckerResDto()
            {
                Status = (int)ClientUpdateStatus.None,
                Content = "",
                Downurl = ""
            };

            //VersionService service = new VersionService();
            //var version = new InkeyVersion(this.RequestInfo.ClientVersion);
            //var checkVerDto = service.CheckVersion(version, this.RequestInfo.ClientType, this.RequestInfo.ClientSourceId, CommonStr.OrgAppName);

            return new ApiResult<CheckerResDto>()
            {
                Data = resDto
            };

        }

    }
}