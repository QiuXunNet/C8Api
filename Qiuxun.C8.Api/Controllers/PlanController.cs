
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos.Plan.Request;
using System.Collections.Generic;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Cache;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 计划接口
    /// </summary>
    public class PlanController : QiuxunApiController
    {
        /// <summary>
        /// 获取官方计划推荐数据(分页)
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<PagedListP<Plan>> GetPlanData(int lType, int pageIndex = 1, int pageSize = 10)
        {
            PlanService service = new PlanService();
            PagedListP<Plan> list = service.GetPlanData(lType, pageIndex, pageSize);
            return new ApiResult<PagedListP<Plan>>() { Data = list };
        }

        /// <summary>
        /// 发布计划
        /// </summary>
        /// <param name="model">发布计划请求类</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult Bet(BetReqDto model)
        {
            PlanService service = new PlanService();
            return service.Bet(model.lType, model.currentIssue, model.betInfo, this.UserInfo.UserId);
        }

        /// <summary>
        /// 查询用户是否发表过计划
        /// </summary>
        /// <param name="model">查询用户是否发表过计划 请求类</param>
        [HttpPost]
        public ApiResult<bool> HasSubBet(HasSubBetReqDto model)
        {
            PlanService service = new PlanService();
            return service.HasSubBet(model.lType, model.userId, model.playName, model.type);
        }

        /// <summary>
        /// 获取用户点阅计划所需金币数\用户卡券数\用户金币数
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="userId">发布计划人ID</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<dynamic> GetReadCoin(int lType, long userId)
        {
            PlanService service = new PlanService();
            return service.GetReadCoin(lType, userId, UserInfo.UserId);
        }

        /// <summary>
        /// 检查是否看过该帖子，没看过，金币是否足够查看
        /// </summary>
        /// <param name="id">帖子Id</param>
        /// <param name="ltype">彩种Id</param>
        /// <param name="uid">用户Id</param>
        /// <param name="coin">查看所需金币</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult CanViewPlan(int id, int ltype, int uid, int coin)
        {
            PlanService service = new PlanService();
            return service.CanViewPlan(id, ltype, uid, coin);
        }

        /// <summary>
        /// 获取该用户最新计划(同时会插入点阅记录，收费专家扣除用户金币数、获得佣金)[paytype:1金币支付;2查看劵支付]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<BettingRecord> GetLastPlay(LastPlayReqDto model)
        {
            PlanService service = new PlanService();
            return service.GetLastPlay(model.lType, model.uid, model.playName, UserInfo.UserId, model.paytype ?? 1);
        }

        /// <summary>
        /// 获取专家列表
        /// </summary>
        /// <param name="model">lType:彩种Id playName:玩法名称 type:类型 1=高手推荐 2=免费专家 pageIndex:页码 pageSize:页数据量</param>      
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public ApiResult<PagedListP<Expert>> GetExpertList(ExpertListReqDto model)
        {
            PlanService service = new PlanService();
            PagedListP<Expert> list = service.GetExpertList(model.lType, model.playName, model.type, model.pageIndex, model.pageSize);
            return new ApiResult<PagedListP<Expert>>() { Data = list };
        }

        /// <summary>
        /// 获取用户近期竞猜记录(返回当前用户是否点阅过该记录)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<PagedListP<AchievementModel>> GetUserLastPlay(LastPlayReqDto model)
        {
            PlanService service = new PlanService();
            PagedListP<AchievementModel> list = service.GetUserLastPlay(model.uid, model.lType, model.playName, this.UserInfo.UserId);
            return new ApiResult<PagedListP<AchievementModel>>() { Data = list };
        }

        /// <summary>
        /// 根据彩种获取最新官方玩法推荐
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<Plan>> GetNewPlan(int lType)
        {
            PlanService service = new PlanService();
            return service.GetNewPlan(lType);
        }

        /// <summary>
        /// 获取用户彩种下前固定条数的打赏记录
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="uid">用户ID</param>
        /// <param name="top">固定条数</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<TipRecordModel>> GetTipRecord(int lType, int uid, int top)
        {
            PlanService service = new PlanService();
            return service.GetTipRecord(lType, uid, top);
        }

        /// <summary>
        /// 获取用户彩种下的打赏总金额
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<int> GetTotalTipMoeny(int lType, int uid)
        {
            PlanService service = new PlanService();
            return service.GetTotalTipMoeny(lType, uid);
        }

        /// <summary>
        /// 打赏金币
        /// </summary>
        /// <param name="id">计划Id</param>
        /// <param name="coin">金币数量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GiftCoin(int id, int coin)
        {
            PlanService service = new PlanService();
            return service.GiftCoin(id, coin, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取专家热搜前N条数据
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="top">N</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<ExpertSearchModel>> GetTopExpertSearch(int lType, int top)
        {
            PlanService service = new PlanService();
            return service.GetTopExpertSearch(lType, top, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取专家搜索历史记录
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<ExpertSearchModel>> GetHistoryExpertSearch(int lType)
        {
            string memberKey = "history_" + this.UserInfo.UserId + "_" + lType;
            List<ExpertSearchModel> historyList = CacheHelper.GetCache<List<ExpertSearchModel>>(memberKey) ?? new List<ExpertSearchModel>();
            return new ApiResult<List<ExpertSearchModel>>() { Data = historyList };
        }

        /// <summary>
        /// 插入搜索数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="lType">彩种</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult InsertHotSearch(long uid, int lType)
        {
            PlanService service = new PlanService();
            return service.InsertHotSearch(uid, lType, this.UserInfo.UserId);
        }
        /// <summary>
        /// 删除搜索历史记录
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="lType">彩种</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult DeleteHistory(int uid, int lType)
        {
            PlanService service = new PlanService();
            return service.DeleteHistory(uid, lType, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取专家搜索数据集合
        /// </summary>
        /// <param name="model">请求model</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<List<ExpertSearchModel>> GetExpertSearchList(ExpertSearchListReqDto model)
        {
            PlanService service = new PlanService();
            return service.GetExpertSearchList(model.lType, model.NickName, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取当前用户某个彩种当前期号计划集合
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<BettingRecord>> AlreadyPostData(int lType)
        {
            PlanService service = new PlanService();
            return service.AlreadyPostData(lType, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取可用查看劵数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<int> GetUserCouponNum()
        {
            PlanService service = new PlanService();

            var result = new ApiResult<int>();
            result.Data = service.GetUserCouponNum(this.UserInfo.UserId);
            return result;
        }


    }
}