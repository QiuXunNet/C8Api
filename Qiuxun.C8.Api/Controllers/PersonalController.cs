using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos.Personal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Qiuxun.C8.Api.Controllers
{
    public class PersonalController : QiuxunApiController
    {
        /// <summary>
        /// 获取个人中心首页展示数据
        /// </summary>
        /// <returns>IndexResDto：个人中心首页数据对象</returns>
        [HttpGet]
        public ApiResult<IndexResDto> PersonalIndexData()
        {
            PersonalService service = new PersonalService();
            IndexResDto resDto = service.GetPersonalIndexData(this.UserInfo.UserId);

            if (resDto == null)
            {
                return new ApiResult<IndexResDto>(60000, "登录超时，需要重新登录");
            }

            return new ApiResult<IndexResDto>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpwd">旧密码</param>
        /// <param name="newpwd">新密码</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ApiResult ModifyPWD(string oldpwd, string newpwd)
        {
            if (string.IsNullOrWhiteSpace(oldpwd))
            {
                return new ApiResult(60001, "旧密码不能为空");
            }
            if (string.IsNullOrWhiteSpace(newpwd))
            {
                return new ApiResult(60002, "新密码不能为空");
            }
            if (newpwd.Length < 6 || newpwd.Length > 12)
            {
                return new ApiResult(60003, "新密码长度要在6-12位之间");
            }

            PersonalService service = new PersonalService();

            return service.ModifyPWD(oldpwd, newpwd, this.UserInfo.UserId);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="value">新的值</param>
        /// <param name="type">1、昵称 2、签名 3、性别</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ApiResult EditUserInfo(string value, int type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new ApiResult(60005, "新值不能为空");
            }
            if (type != 1 || type != 2 || type != 3)
            {
                return new ApiResult(60006, "类型在1-3之间");
            }
            PersonalService service = new PersonalService();
            return service.EditUserInfo(value, type, this.UserInfo.UserId);
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
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpPost]
        public ApiResult<PagedListP<MyFollowResDto>> GetMyFollow(int pageIndex = 1, int pageSize = 10)
        {

            PersonalService service = new PersonalService();
            PagedListP<MyFollowResDto> resDto = service.GetMyFollow(pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<MyFollowResDto>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="followed_userId">关注的会员Id</param>
        /// <returns>取消结果</returns>
        [HttpPost]
        public ApiResult UnFollow(long followed_userId)
        {
            if (followed_userId <= 0)
            {
                return new ApiResult(60009, "已关注的会员ID不正确");
            }
            PersonalService service = new PersonalService();
            return service.UnFollow(followed_userId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="followed_userId">关注的会员Id</param>
        /// <returns>关注结果</returns>
        [HttpPost]
        public ApiResult IFollow(long followed_userId)
        {
            if (followed_userId <= 0)
            {
                return new ApiResult(60009, "已关注的会员ID不正确");
            }
            PersonalService service = new PersonalService();
            return service.IFollow(followed_userId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取我的粉丝列表(分页)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpPost]
        public ApiResult<PagedListP<MyFanResDto>> GetMyFans(int pageIndex = 1, int pageSize = 10)
        {

            PersonalService service = new PersonalService();
            PagedListP<MyFanResDto> resDto = service.GetMyFans(pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<MyFanResDto>>() { Code = 100, Desc = "", Data = resDto };
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
        /// <param name="typeId"></param>
        /// <param name="type">day=日榜 week=周榜 month=月榜 all=总榜</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <returns>分页集合</returns>
        [HttpPost]
        public ApiResult<List<FansResDto>> GetFansBangList(int typeId, string type, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            return service.GetFansBangList(typeId, type, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取当前用户在粉丝榜的排名数据
        /// </summary>
        /// <param name="type">day=日榜 week=周榜 month=月榜 all=总榜</param>
        /// <returns>用户数据</returns>
        [HttpPost]
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
        [HttpPost]
        public ApiResult ReplaceHeadImg(long resourceId)
        {
            PersonalService service = new PersonalService();
            return service.ReplaceHeadImg(resourceId, this.UserInfo.UserId);
        }

        /// <summary>
        /// 进入他人主页是否关注（调用同时插入一条访问记录）
        /// </summary>
        /// <param name="id">受访问人ID</param>
        /// <returns>是否关注</returns>
        [HttpPost]
        public ApiResult<HasFollowResDto> UserCenterGetHasFollow(long id)
        {
            PersonalService service = new PersonalService();
            return service.UserCenterGetHasFollow(id, this.UserInfo.UserId);
        }

        /// <summary>
        /// 获取计划列表
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户计划列表</param>
        /// <param name="ltype">彩种类型Id</param>
        /// <param name="winState">开奖状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpPost]
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
        [HttpPost]
        public ApiResult<PagedListP<Comment>> GetDenamic(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<Comment> resDto = service.GetDenamic(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<Comment>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取访问记录
        /// </summary>
        /// <param name="uid">用户Id，若用户Id为0时，则查询当前登录用户访问记录</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<PagedListP<AccessRecord>> GetVisitRecord(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<AccessRecord> resDto = service.GetVisitRecord(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<AccessRecord>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取我的计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<LotteryType>> GetMyPlan()
        {
            PersonalService service = new PersonalService();
            return service.GetMyPlan(this.UserInfo.UserId);
        }

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
        [HttpPost]
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
        [HttpPost]
        public ApiResult<PagedListP<SystemMessage>> GetSysMessage(long uid = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<SystemMessage> resDto = service.GetSysMessage(uid, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<SystemMessage>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取竞猜数据
        /// </summary>
        /// <param name="PId">父级ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<PagedListP<BetModel>> GetBet(int PId = 0, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();    
            PagedListP<BetModel> resDto = service.GetBet(PId, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<BetModel>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取我的成绩
        /// </summary>
        /// <param name="lType">彩种</param>
        /// <param name="PlayName">玩法名称</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<PagedListP<AchievementModel>> GetMyBet(int lType, string PlayName, int pageIndex = 1, int pageSize = 20)
        {
            PersonalService service = new PersonalService();
            PagedListP<AchievementModel> resDto = service.GetMyBet(lType, PlayName,pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<AchievementModel>>() { Code = 100, Desc = "", Data = resDto };
        }

        /// <summary>
        /// 获取交易记录数据
        /// </summary>
        /// <param name="Type">1=充值 2=消费 3=赚钱</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<PagedListP<ComeOutRecordModel>> GetRecordList(int Type, int pageIndex, int pageSize)
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
        [HttpPost]
        public ApiResult<PagedListP<ComeOutRecordModel>> GetMyCommissionList(int Type, int pageIndex, int pageSize)
        {
            PersonalService service = new PersonalService();
            PagedListP<ComeOutRecordModel> resDto = service.GetMyCommissionList(Type, pageIndex, pageSize, this.UserInfo.UserId);
            return new ApiResult<PagedListP<ComeOutRecordModel>>() { Code = 100, Desc = "", Data = resDto };
        }
    }
}
