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
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Public;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 通用模块
    /// </summary>
    public class CommonController : QiuxunApiController
    {
        LotteryService lotteryService = new LotteryService();
        SmsService smsService = new SmsService();
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

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="phone">接收人，可空。发送给登录用户传空值</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult GetSmsCode(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                if (this.UserInfo == null)
                {
                    throw new ApiException(1000, "验证参数Phone失败");
                }
                phone = UserInfo.UserAccount;
            }

            if (ValidateUtil.IsValidPhone(phone))
            {
                throw new ApiException(1001, "验证参数Phone失败");
            }

            bool sendResult = smsService.Send(new SmsSendDto()
            {
                Receiver = phone,
                Sender = "",
                Type = 1,
                UserId = UserInfo != null ? UserInfo.UserId : 0
            });


            if (!sendResult) return new ApiResult(10005, "发送失败，请稍后重试");

            return new ApiResult();
        }
    }
}