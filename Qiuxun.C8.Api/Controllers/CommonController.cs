using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Caching;
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
        /// <param name="parentId">上级分类Id，可空。空时返回所有彩种</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<LotteryType2>> GetLotteryTypeList(int? parentId)
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
        /// 获取首页开奖列表
        /// </summary>
        /// <param name="parentId">彩种分类Id</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<IndexLotteryInfoResDto>> GetIndexLotteryList(int parentId)
        {
            return lotteryService.GetIndexLotteryList(parentId);
        }

        /// <summary>
        /// 获取彩种开奖信息
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<IndexLotteryInfoResDto> GetLotteryInfo(int lType)
        {
            return lotteryService.GetLotteryInfo(lType);
        }

        /// <summary>
        /// 获取玩法列表
        /// </summary>
        /// <param name="ltype">彩种Id，可空。空时返回所有彩种的玩法</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<PlayResDto>> GetPlayList(int? ltype)
        {
            return lotteryService.GetPlayList(ltype);
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

            return new ApiResult(100, "发送成功");
        }

        /// <summary>
        /// 上传资源
        /// </summary>
        /// <param name="reqDto">File:Base64图片信息；Type:资源类型，1=文章资源 2=用户头像，可空</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<UploadFileResDto> UploadFile(UploadFileReqDto reqDto)
        {
            #region 验证
            if (reqDto == null)
                throw new ApiException(10000, "验证参数失败");

            if (string.IsNullOrWhiteSpace(reqDto.File))
                throw new ApiException(10000, "验证参数File失败");
            #endregion

            var resourceService = new ResourceManagementService();

            long userId = UserInfo != null ? UserInfo.UserId : 0;
            int resType = reqDto.Type ?? 0;

            return resourceService.UploadFile(reqDto.File, resType, userId, this.Request);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult CleanCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new ApiResult(40000, "key不能为空");

            if (!CacheHelper.Exists(key))
                return new ApiResult(404, "没有该key的缓存");

            CacheHelper.DeleteCache(key);

            return new ApiResult();
        }

        /// <summary>
        /// 获取彩种时间配置
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<LotteryTimeModel>> GetLotteryTimeSettings(int ltype)
        {
            var list = LotteryTime.GetLotteryTimeModelList();
            return new ApiResult<List<LotteryTimeModel>>()
            {
                Data = list
            };
        }


    }
}