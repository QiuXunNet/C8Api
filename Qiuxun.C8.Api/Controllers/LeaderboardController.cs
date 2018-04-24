using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LeaderboardController : QiuxunApiController
    {
        private LeaderboardService _service = new LeaderboardService();

        /// <summary>
        /// 获取积分榜数据
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<RankIntegralResDto>> GetIntegralList(string queryType)
        {
            var result = new ApiResult<List<RankIntegralResDto>>();
            result.Data = _service.GetIntegralList(queryType);
            return result;
        }

        /// <summary>
        /// 获取自己的积分排名
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<RankIntegralResDto> GetMyRank(string queryType)
        {
            var result = new ApiResult<RankIntegralResDto>();
            result.Data = _service.GetMyRank(queryType, (int)this.UserInfo.UserId);
            return result;
        }

        /// <summary>
        /// 获取一级彩种
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<LotteryType2>> GetLotteryTypeList()
        {
            var result = new ApiResult<List<LotteryType2>>();
            result.Data = _service.GetLotteryTypeList();
            return result;
        }

        /// <summary>
        /// 获取非一级彩种(小彩种)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<LotteryType2>> GetChildLotteryTypeList()
        {
            var result = new ApiResult<List<LotteryType2>>();
            result.Data = _service.GetChildLotteryTypeList();
            return result;
        }

        /// <summary>
        /// 根据彩种类型和查询类型获取高手榜单
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="lType">小彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<RankIntegralResDto>> GetHighMasterList(string queryType, int lType)
        {
            var result = new ApiResult<List<RankIntegralResDto>>();
            result.Data = _service.GetHighMasterList(queryType, lType);
            return result;
        }

        /// <summary>
        /// 查询当前登录人在高手榜的分数
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="lType">小彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<RankIntegralResDto> GetMyHighMaster(string queryType, int lType)
        {
            var result = new ApiResult<RankIntegralResDto>();
            int userId = (int)UserInfo.UserId;
            result.Data = _service.GetMyHighMaster(queryType, lType, userId);
            return result;
        }

        /// <summary>
        /// 获取盈利打赏榜数据
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="RType">4:盈利榜   9:打赏榜</param>
        /// <param name="lType">小彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<RankIntegralResDto>> GetProfitRewardList(string queryType, int RType, int lType)
        {
            var result = new ApiResult<List<RankIntegralResDto>>();
            result.Data = _service.GetProfitRewardList(queryType, RType, lType);
            return result;
        }

        /// <summary>
        /// 获取盈利打赏榜数据
        /// </summary>
        /// <param name="queryType">榜单类型 day week month all</param>
        /// <param name="RType">4:盈利榜   9:打赏榜</param>
        /// <param name="lType">小彩种类型</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<RankIntegralResDto> GetMyProfitReward(string queryType, int RType, int lType)
        {
            var result = new ApiResult<RankIntegralResDto>();
            int userId = (int)UserInfo.UserId;
            result.Data = _service.GetMyProfitReward(queryType, RType, lType, userId);
            return result;
        }
    }
}
