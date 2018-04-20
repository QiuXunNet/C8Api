﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 评论业务类
    /// </summary>
    public class CommentService
    {
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="reqDto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApiResult Publish(CommentReqDto reqDto, IdentityInfo user)
        {
            #region step1.黑名单和禁言验证
            //step1.黑名单和禁言验证
            string validateSql = @"select count(1) from [dbo].[UserState]
	  where ([CommentBlack]=1 or ([CommentShut]=1 and GETDATE() between [CommentShutBegin] and [CommentShutEnd])) and UserId=" +
                user.UserId;

            object obj = SqlHelper.ExecuteScalar(validateSql);

            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return new ApiResult(10001, "你已被禁言或拉黑");
            }
            #endregion

            #region step2.评论/回复对象是否存在

            long refId = 0;
            long pid = 0;
            long refCommentId = 0;

            if (reqDto.CommentType == 1)
            {
                if (reqDto.Type == 1)
                {
                    //查询计划
                    var model = Util.GetEntityById<BettingRecord>(reqDto.Id);
                    if (model == null) return new ApiResult(400, "计划已不存在或已删除");

                    refId = model.Id;
                    //refCommentId=
                }
                else if (reqDto.Type == 2)
                {
                    //查询文章
                    var model = Util.GetEntityById<News>(reqDto.Id);
                    if (model == null) return new ApiResult(400, "资讯不存在或已删除");
                    refId = model.Id;
                }
            }
            else
            {
                //查询评论
                var model = Util.GetEntityById<Comment>(reqDto.Id);
                if (model == null) return new ApiResult(400, "评论不存在或已删除");
                refId = model.Id;
                pid = model.Id;

                if (model.RefCommentId > 0)
                {
                    //第N级回复，则关联评论Id=上一级评论关联Id
                    refCommentId = model.RefCommentId;
                }
                else
                {
                    //第一级回复，则关联评论Id=上一级评论Id
                    refCommentId = model.Id;
                }
            }


            #endregion

            #region step3.评论内容过滤
            //去Html标签
            string content = WebHelper.NoHtml(reqDto.Content);
            //脏字过滤
            content = WebHelper.FilterSensitiveWords(content, "*");
            #endregion


            #region step4.添加评论
            string sql = @"INSERT INTO [dbo].[Comment]
           ([PId],[UserId],[Content],[SubTime],[Type],[ArticleId]
           ,[StarCount],[IsDeleted],[RefCommentId])
     VALUES(@PId,@UserId,@Content,GETDATE()
           ,@Type,@ArticleId,0,0,@RefCommentId);";

            SqlParameter[] parameters ={
                new SqlParameter("@PID",pid),
                new SqlParameter("@UserId",user.UserId),
                new SqlParameter("@Content",content),
                new SqlParameter("@Type",reqDto.Type),
                new SqlParameter("@ArticleId",refId),
                new SqlParameter("@RefCommentId",refCommentId),
              };
            int row = SqlHelper.ExecuteNonQuery(sql, parameters);
            if (row <= 0)
            {
                return new ApiResult(500, "服务器繁忙");
            }

            #endregion

            return new ApiResult();
        }

        /// <summary>
        /// 获取精彩评论
        /// </summary>
        /// <param name="id">文章Id/计划Id</param>
        /// <param name="type">类型1=计划 2=文章</param>
        /// <param name="count">查询数量</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public ApiResult<List<CommentResDto>> GetWonderfulComment(int id, int type, int count, long userId)
        {
            string sql =
                "select top " + count + @"  a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater,
(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes,
(select count(1) from Comment where PId = a.Id ) as ReplayCount 
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.IsDeleted = 0 and a.RefCommentId=0  and a.ArticleId = @ArticleId and a.Type=@Type
  order by StarCount desc";
            var parameters = new[]
            {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.BigInt),
                new SqlParameter("@ArticleId",SqlDbType.BigInt),
                new SqlParameter("@Type",SqlDbType.Int),
            };

            parameters[0].Value = userId;
            parameters[1].Value = (int)ResourceTypeEnum.用户头像;
            parameters[2].Value = id;
            parameters[3].Value = type;


            var list = Util.ReaderToList<Comment>(sql, parameters);
            string webHost = ConfigurationManager.AppSettings["webHost"];
            string defaultAvater = string.Format("{0}/images/default_avater.png", webHost);
            var data = list.Select(x => new CommentResDto()
            {
                Id = x.Id,
                ArticleId = x.ArticleId,
                Avater = string.IsNullOrWhiteSpace(x.Avater) ? defaultAvater : x.Avater,
                Content = x.Content,
                IsLike = x.CurrentUserLikes > 0,
                NickName = x.NickName,
                PId = x.PId,
                RefCommentId = x.RefCommentId,
                ReplayCount = x.ReplayCount
            }).ToList();

            return new ApiResult<List<CommentResDto>>()
            {
                Data = data
            };
        }


        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="id">文章Id/计划Id</param>
        /// <param name="lastId">上次拉取的最后一条Id</param>
        /// <param name="type">类型1=计划 2=文章</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiResult<List<CommentResDto>> GetCommentList(int id, int lastId, int type, int pageSize)
        {
            string sql = @"select Top " + pageSize + @" a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
,(select count(1) from Comment where PId = a.Id ) as ReplayCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.ArticleId = @ArticleId and a.IsDeleted = 0 and a.RefCommentId=0 and a.Type=@Type";

            if (lastId > 0)
            {
                sql += " and a.Id <" + lastId;
            }

            sql += " order by Id DESC";

            var parameters = new[]
            {
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@ArticleId",id),
                new SqlParameter("@Type",type),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);

            string webHost = ConfigurationManager.AppSettings["webHost"];
            string defaultAvater = string.Format("{0}/images/default_avater.png", webHost);
            var pageData = list.Select(x => new CommentResDto()
            {
                Id = x.Id,
                ArticleId = x.ArticleId,
                Avater = string.IsNullOrWhiteSpace(x.Avater) ? defaultAvater : x.Avater,
                Content = x.Content,
                IsLike = x.CurrentUserLikes > 0,
                NickName = x.NickName,
                PId = x.PId,
                RefCommentId = x.RefCommentId,
                ReplayCount = x.ReplayCount
            }).ToList();

            return new ApiResult<List<CommentResDto>>()
            {
                Data = pageData
            };
        }
    }
}
