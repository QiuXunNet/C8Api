using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 广告
    /// </summary>
    public class AdvertController : QiuxunApiController
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="location">栏目Id (type=3时传-1)</param>
        /// <param name="type">广告类型 1=栏目 2=文章 3=六彩</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<AdvertisementResDto> GetAdvertisementList(int location, int type)
        {
            return new ApiResult<AdvertisementResDto>();
        }
    }
}
