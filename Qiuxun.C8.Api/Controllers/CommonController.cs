using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 通用模块
    /// </summary>
    public class CommonController : QiuxunApiController
    {
        LotteryService lotteryService = new LotteryService();
        /// <summary>
        /// 获取彩种大分类
        /// </summary>
        /// <param name="parentId">上级分类Id，可空。空是返回所有彩种</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult GetLotteryTypeList(int? parentId)
        {
            var result = new ApiResult<List<LotteryType2>>();

            if (parentId.HasValue)
            {
                result.Data = lotteryService.GetLotteryTypeList(parentId.Value);
            }
            else
            {
                result.Data = lotteryService.GetAllLotteryTypeList();
            }

            return result;
        }
    }
}