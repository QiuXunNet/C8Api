using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos.Personal;
using Qiuxun.C8.Api.Service.Dtos.Personal.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 个人中心接口
    /// </summary>
    public class PersonalController : QiuxunApiController
    {
        /// <summary>
        /// 获取个人中心首页展示数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>IndexResDto：个人中心首页数据对象</returns>
        [HttpGet]
        public ApiResult<IndexResDto> PersonalIndexData(long uid)
        {
            PersonalService service = new PersonalService();
            uid = uid > 0 ? uid : UserInfo.UserId;
            IndexResDto resDto = service.GetPersonalIndexData(uid);

            if (resDto == null)
            {
                return new ApiResult<IndexResDto>(60000, "登录超时，需要重新登录");
            }

            return new ApiResult<IndexResDto>() { Data = resDto };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">修改密码对象</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ApiResult ModifyPWD(ModifyPwdReqDto model)
        {
            if (string.IsNullOrWhiteSpace(model.oldpwd))
            {
                return new ApiResult(60001, "旧密码不能为空");
            }
            if (string.IsNullOrWhiteSpace(model.newpwd))
            {
                return new ApiResult(60002, "新密码不能为空");
            }
            if (model.newpwd.Length < 6 || model.newpwd.Length > 12)
            {
                return new ApiResult(60003, "新密码长度要在6-12位之间");
            }

            PersonalService service = new PersonalService();

            return service.ModifyPWD(model.oldpwd, model.newpwd, this.UserInfo.UserId);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">修改用户数据请求类</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ApiResult EditUserInfo(EditUserInfoReqDto model)
        {
            if (string.IsNullOrWhiteSpace(model.value))
            {
                return new ApiResult(60005, "新值不能为空");
            }
            if (model.type != 1 && model.type != 2 && model.type != 3)
            {
                return new ApiResult(60006, "类型在1-3之间");
            }
            PersonalService service = new PersonalService();
            return service.EditUserInfo(model.value, model.type, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取赚钱任务列表
        /// </summary>
        /// <returns>TaskResDto集合</returns>
        [HttpGet]
        public ApiResult<List<TaskResDto>> GetTaskList()
        {
            PersonalService service = new PersonalService();
            List<TaskResDto> resDto = service.GetTaskList(this.UserInfo.UserId);
            return new ApiResult<List<TaskResDto>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取我的关注列表(分页)
        /// </summary>
        /// <param name="lastId">最后一次拉取Id</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpGet]
        public ApiResult<List<MyFollowResDto>> GetMyFollow(int lastId = 0, int pageSize = 10)
        {

            PersonalService service = new PersonalService();
            var resDto = service.GetMyFollow(pageSize, this.UserInfo.UserId, lastId);
            return new ApiResult<List<MyFollowResDto>>() {Data = resDto };
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="followedUserId">关注的会员Id</param>
        /// <returns>取消结果</returns>
        [HttpGet]
        public ApiResult UnFollow(long followedUserId)
        {
            if (followedUserId <= 0)
            {
                return new ApiResult(60009, "已关注的会员ID不正确");
            }
            PersonalService service = new PersonalService();
            return service.UnFollow(followedUserId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="followedUserId">关注的会员Id</param>
        /// <returns>关注结果</returns>
        [HttpGet]
        public ApiResult IFollow(long followedUserId)
        {
            if (followedUserId <= 0)
            {
                return new ApiResult(60009, "已关注的会员ID不正确");
            }
            PersonalService service = new PersonalService();
            return service.IFollow(followedUserId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取我的粉丝列表(分页)
        /// </summary>
        /// <param name="lastId">上次拉取的Id最小值</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpGet]
        public ApiResult<List<MyFanResDto>> GetMyFans(int lastId = 0, int pageSize = 10)
        {

            PersonalService service = new PersonalService();
            List<MyFanResDto> resDto = service.GetMyFans(pageSize, this.UserInfo.UserId, lastId);
            return new ApiResult<List<MyFanResDto>>() {Data = resDto };
        }

        /// <summary>
        /// 获取已邀请的人数和总奖励金币数
        /// </summary>
        /// <returns>InvitationRegResDto</returns>
        [HttpGet]
        public ApiResult<InvitationRegResDto> GetInvitationReg()
        {
            PersonalService service = new PersonalService();
            return service.GetInvitationReg(this.UserInfo.UserId);
        }

        /// <summary>
        /// 粉丝榜数据 只取前100条（分页）
        /// </summary>
        /// <param name="type">day=日榜 week=周榜 month=月榜 all=总榜</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpGet]
        public ApiResult<List<FansResDto>> GetFansBangList(string type, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            return service.GetFansBangList(type, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取当前用户在粉丝榜的排名数据
        /// </summary>
        /// <param name="type">day=日榜 week=周榜 month=月榜 all=总榜</param>
        /// <returns>用户数据</returns>
        [HttpGet]
        public ApiResult<FansResDto> GetMyFanBangRank(string type)
        {
            PersonalService service = new PersonalService();
            return service.GetMyFanBangRank(type, this.UserInfo.UserId);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>修改结果</returns>
        [HttpGet]
        public ApiResult ReplaceHeadImg(long resourceId)
        {
            PersonalService service = new PersonalService();
            return service.ReplaceHeadImg(resourceId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取是否关注过指定用户
        /// </summary>
        /// <param name="id">受访问人ID</param>
        /// <returns>是否关注</returns>
        [HttpGet]
        public ApiResult<HasFollowResDto> GetHasFollow(long id)
        {
            PersonalService service = new PersonalService();
            return service.GetHasFollow(id, this.UserInfo.UserId);
        }


        /// <summary>
        /// 添加个人中心访问记录
        /// </summary>
        /// <param name="reqDto">Id:受访用户Id;Module:受访模块，默认=1</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult AddVisitRecord(AddVisitRecordReqDto reqDto)
        {
            #region 校验

            if (reqDto == null)
                return new ApiResult(40000, "验证参数失败");

            #endregion

            PersonalService service = new PersonalService();
            return service.AddVisitRecord(reqDto.Id, this.UserInfo.UserId, reqDto.Module);
        }

        /// <summary>
        /// 获取计划列表
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户计划列表</param>
        /// <param name="ltype">彩种类型Id</param>
        /// <param name="winState">开奖状态 1=未开奖 2=已开奖</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<BettingRecord>> GetPlan(long uid = 0, int ltype = 0, int winState = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<BettingRecord> resDto = service.GetPlan(uid, ltype, winState, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<BettingRecord>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取动态
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户动态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<Comment>> GetDenamic(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<Comment> resDto = service.GetDenamic(uid, pageIndex, pageSize, this.UserInfo.UserId);


            return new ApiResult<PagedListP<Comment>>() { Data = resDto };
        }

        /// <summary>
        /// 获取访问记录
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户访问记录</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<AccessRecord>> GetVisitRecord(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<AccessRecord> resDto = service.GetVisitRecord(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<AccessRecord>>() { Code = 100, Desc = "", Data = resDto };
        }

        ///// <summary>
        ///// 获取我的计划
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public ApiResult<List<LotteryType>> GetMyPlan()
        //{
        //    PersonalService service = new PersonalService();
        //    return service.GetMyPlan(this.UserInfo.UserId);
        //}

        /// <summary>
        /// 获取未读通知消息数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<NoReadResDto> GetNoticeNoRead()
        {
            PersonalService service = new PersonalService();
            return service.GetNoticeNoRead(this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取评论提醒
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户评论提醒</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<DynamicMessage>> GetCommentNotice(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<DynamicMessage> resDto = service.GetCommentNotice(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<DynamicMessage>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取系统消息
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户系统消息</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<SystemMessage>> GetSysMessage(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<SystemMessage> resDto = service.GetSysMessage(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<SystemMessage>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取竞猜数据
        /// </summary>
        /// <param name="pid">彩种分类Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<BetModel>> GetBet(int pid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<BetModel> resDto = service.GetBet(pid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<BetModel>>() { Data = resDto };
        }

        /// <summary>
        /// 获取我的成绩
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="PlayName">玩法名称</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<AchievementModel>> GetMyBet(int lType, string PlayName, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<AchievementModel> resDto = service.GetMyBet(lType, PlayName, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<AchievementModel>>() { Data = resDto };
        }

        /// <summary>
        /// 获取交易记录数据
        /// </summary>
        /// <param name="Type">1=充值 2=消费 3=赚钱</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<ComeOutRecordModel>> GetRecordList(int Type, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<ComeOutRecordModel> resDto = service.GetRecordList(Type, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<ComeOutRecordModel>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取我的佣金
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<DrawMoneyResDto> GetMyCommission()
        {
            PersonalService service = new PersonalService();
            return service.GetMyCommission(this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取提现、收入明细
        /// </summary>
        /// <param name="Type">1=收入明细 2=提现明细</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedListP<ComeOutRecordModel>> GetMyCommissionList(int Type, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<ComeOutRecordModel> resDto = service.GetMyCommissionList(Type, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<ComeOutRecordModel>>() { Code = 100, Desc = "", Data = resDto };
        }
    }
}
