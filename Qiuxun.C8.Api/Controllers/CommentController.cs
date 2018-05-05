using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentController : QiuxunApiController
    {
        CommentService commentService = new CommentService();


        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="reqDto">请求参数类。Id:评论对象的Id，[计划Id,文章Id，评论Id]；CommentType：评论类型 1=一级评论 2=回复；Content:内容;Type：类型 1=计划 2=文章</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<CommentResDto> Publish(CommentReqDto reqDto)
        {
            #region 验证
            if (reqDto == null)
                throw new ApiException(400, "验证参数失败");
            if (reqDto.CommentType != 1 && reqDto.CommentType != 2)
                throw new ApiException(40000, "评论类型超出业务范围");
            if (reqDto.Id <= 0)
                throw new ApiException(40000, "验证参数Id失败");

            if (string.IsNullOrWhiteSpace(reqDto.Content))
                throw new ApiException(40000, "内容不能为空");

            if (reqDto.Type != 1 && reqDto.Type != 2)
                throw new ApiException(40000, "评论对象类型超出业务范围");
            #endregion

            return commentService.Publish(reqDto, this.UserInfo);
        }

        /// <summary>
        /// 获取精彩评论
        /// </summary>
        /// <param name="id">彩种Id或文章Id</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="count">查询数量</param>
        /// <param name="refUserId">关联用户Id（Type=1时必填）</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<CommentResDto>> GetWonderfulComment(int id, int type, int count = 3, int refUserId = 0)
        {
            if (type != 1 && type != 2 && type != 3)
                throw new ApiException(40000, "超出业务范围");
            var userId = UserInfo != null ? UserInfo.UserId : 0;

            return commentService.GetWonderfulComment(id, type, count, userId, refUserId);
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="id">文章Id/彩种Id</param>
        /// <param name="type">类型1=计划 2=文章 </param>
        /// <param name="refUserId">关联用户Id（Type=1时必填）</param>
        /// <param name="lastId">每页拉取的最有一条Id</param>
        /// <param name="pageSize">页码</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<CommentResDto>> GetCommentList(int id, int type, int refUserId = 0, int lastId = 0,
            int pageSize = 20)
        {
            if (type != 1 && type != 2 && type != 3)
                throw new ApiException(40000, "超出业务范围");

            int userId = 0;
            if (UserInfo != null)
                userId = UserInfo.UserId.ToInt32();
            return commentService.GetCommentList(id, lastId, type, pageSize, userId, refUserId);
        }

        /// <summary>
        /// 获取回复列表
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="type">类型1=计划 2=文章</param>
        /// <param name="lastId">每页拉取的最有一条Id</param>
        /// <param name="pageSize">页码</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<CommentResDto>> GetReplayList(int id, int type, int lastId = 0,
            int pageSize = 20)
        {
            long userId = UserInfo != null ? UserInfo.UserId : 0;
            return commentService.GetReplayList(id, type, lastId, pageSize, userId);
        }

        /// <summary>
        /// 获取文章或计划的评论数
        /// </summary>
        /// <param name="id">文章Id/彩种Id</param>
        /// <param name="type">类型1=计划 2=文章 </param>
        /// <param name="refUserId">用户Id（Type=1时必填）</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<int> GetCommentCount(int id, int type, int refUserId = 0)
        {
            if (type != 1 && type != 2 && type != 3)
                throw new ApiException(40000, "超出业务范围");

            int count = commentService.GetCommentCount(id, type, refUserId);

            return new ApiResult<int>()
            {
                Data = count
            };
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult ClickLike(ClickLikeReqDto reqDto)
        {
            if (reqDto == null)
                return new ApiResult(40000, "验证参数失败");

            if (reqDto.Type != 1 && reqDto.Type != 2)
                return new ApiResult(40000, "超出业务范围");

            if (reqDto.OperationType != 1 && reqDto.OperationType != 2)
                return new ApiResult(40000, "超出业务范围");

            return commentService.ClickLike(reqDto.Id, reqDto.OperationType, reqDto.Type, UserInfo.UserId);
        }
    }
}