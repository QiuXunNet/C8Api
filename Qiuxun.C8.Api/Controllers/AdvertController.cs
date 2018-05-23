using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 广告
    /// </summary>
    public class AdvertController : QiuxunApiController
    {
        AdvertisementService advertService = new AdvertisementService();

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="location">栏目Id (type=3时传-1)</param>
        /// <param name="type">广告类型 1=栏目 2=文章 3=六彩</param>
        /// <param name="city">请求城市</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<AdvertisementResDto>> GetAdvertisementList(int location, int type, string city)
        {

            #region 校验请求参数
            if (type != 1 && type != 2 && type != 3)
                return new ApiResult<List<AdvertisementResDto>>(40000, "请求参数Type验证未通过");

            #endregion

            int deviceType = 0;
            if (this.RequestInfo.ClientType == DevicePlatform.Android)
            {
                deviceType = 3;
            }
            else if (this.RequestInfo.ClientType == DevicePlatform.Ios)
            {
                //Ios App Store不返回广告
                if (this.RequestInfo.ClientSourceId == 10)
                {
                    return new ApiResult<List<AdvertisementResDto>>();
                }

                deviceType = 2;
            }
            else if (this.RequestInfo.ClientType == DevicePlatform.Browser)
            {
                deviceType = 1;
            }
            else
            {
                deviceType = 1;
            }

            return advertService.GetAdvertList(location, type, deviceType, city, RequestInfo.ClientIP);
        }
    }
}
