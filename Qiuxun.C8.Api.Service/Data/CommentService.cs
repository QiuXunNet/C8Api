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
        ResourceManagementService resourceService = new ResourceManagementService();
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="reqDto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApiResult<CommentResDto> Publish(CommentReqDto reqDto, IdentityInfo user)
        {
            #region step1.黑名单和禁言验证
            //step1.黑名单和禁言验证
            string validateSql = @"select count(1) from [dbo].[UserState]
	  where ([CommentBlack]=1 or ([CommentShut]=1 and GETDATE() between [CommentShutBegin] and [CommentShutEnd])) and UserId=" +
                user.UserId;

            object obj = SqlHelper.ExecuteScalar(validateSql);

            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                throw new ApiException(10001, "你已被禁言或拉黑");
            }
            #endregion

            #region step2.评论/回复对象是否存在

            long refId = 0;
            long pid = 0;
            long refCommentId = 0;
            int refUserId = reqDto.RefUserId;

            if (reqDto.CommentType == 1)
            {
                if (reqDto.Type == 1)
                {
                    //查询计划
                    //var model = Util.GetEntityById<BettingRecord>(reqDto.Id);
                    //if (model == null) throw new ApiException(400, "计划已不存在或已删除");

                    //id为彩种Id。对计划评论为用户在该彩种的评论
                    refId = reqDto.Id;
                    //refCommentId=
                }
                else if (reqDto.Type == 2)
                {
                    //查询文章
                    var model = Util.GetEntityById<News>(reqDto.Id);
                    if (model == null) throw new ApiException(400, "资讯不存在或已删除");
                    refId = model.Id;
                }
            }
            else
            {
                //查询评论
                var model = Util.GetEntityById<Comment>(reqDto.Id);
                if (model == null) throw new ApiException(400, "评论不存在或已删除");
                refId = model.Id;
                pid = model.Id;

                if (model.RefCommentId > 0)
                {
                    //第N级回复，则关联评论Id=上一级评论关联Id
                    refCommentId = model.RefCommentId;
                    refUserId = model.UserId;
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
           ,[StarCount],[IsDeleted],[RefCommentId],[ArticleUserId])
     VALUES(@PId,@UserId,@Content,GETDATE()
           ,@Type,@ArticleId,0,0,@RefCommentId,@ArticleUserId);SELECT IDENT_CURRENT('Comment')";

            SqlParameter[] parameters ={
                new SqlParameter("@PID",pid),
                new SqlParameter("@UserId",user.UserId),
                new SqlParameter("@Content",content),
                new SqlParameter("@Type",reqDto.Type),
                new SqlParameter("@ArticleId",refId),
                new SqlParameter("@RefCommentId",refCommentId),
                new SqlParameter("@ArticleUserId",refUserId),
              };
            int row = SqlHelper.ExecuteScalar(sql, parameters).ToInt32();
            if (row <= 0)
            {
                throw new ApiException(50000, "服务器繁忙");
            }

            //添加评论图资源关联
            resourceService.InsertResources((int)ResourceTypeEnum.评论图, row, reqDto.Pictures);

            #endregion

            #region 获取评论实体
            string getSql =
                @"select top 1 a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater
,0 as CurrentUserLikes,0 as ReplayCount 
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.IsDeleted = 0 and a.Id=@Id";
            var getParameters = new[]
            {
                new SqlParameter("@Id",row),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.评论图),
            };

            var list = Util.ReaderToList<Comment>(getSql, getParameters);
            string webHost = ConfigurationManager.AppSettings["webHost"];
            string defaultAvater = string.Format("{0}/images/default_avater.png", webHost);
            int resouceType = (int)ResourceTypeEnum.评论图;
            var resDto = list.Select(x => new CommentResDto()
            {
                Id = x.Id,
                ArticleId = x.ArticleId,
                Avater = string.IsNullOrWhiteSpace(x.Avater) ? defaultAvater : x.Avater,
                Content = x.Content,
                IsLike = x.CurrentUserLikes > 0,
                NickName = x.NickName,
                PId = x.PId,
                RefCommentId = x.RefCommentId,
                ReplayCount = x.ReplayCount,
                UserId = x.UserId,
                Pictures = resourceService.GetResources(resouceType, x.Id)
                    .Select(n => n.RPath).ToList(),
                SubTime = x.SubTime

            }).FirstOrDefault();



            #endregion

            return new ApiResult<CommentResDto>()
            {
                Data = resDto
            };
        }

        /// <summary>
        /// 获取精彩评论
        /// </summary>
        /// <param name="id">文章Id/彩种Id</param>
        /// <param name="type">类型1=计划 2=文章</param>
        /// <param name="count">查询数量</param>
        /// <param name="userId">当前用户Id</param>
        /// <param name="articleUserId">评论关联用户Id</param>
        /// <returns></returns>
        public ApiResult<List<CommentResDto>> GetWonderfulComment(int id, int type, int count, long userId, int articleUserId)
        {
            string sql = "select top " + count + string.Format(@"  a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater,
(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes,
(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount,
(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id) as StarCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.IsDeleted = 0 and a.RefCommentId=0  and a.ArticleId = @ArticleId {0} and a.Type=@Type
  order by a.StarCount desc", type == 1 ? "and a.ArticleUserId=@ArticleUserId" : "");
            var parameters = new[]
            {
                new SqlParameter("@UserId",SqlDbType.BigInt),
                new SqlParameter("@ResourceType",SqlDbType.BigInt),
                new SqlParameter("@ArticleId",SqlDbType.BigInt),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@ArticleUserId",SqlDbType.BigInt),
            };

            parameters[0].Value = userId;
            parameters[1].Value = (int)ResourceTypeEnum.用户头像;
            parameters[2].Value = id;
            parameters[3].Value = type;
            parameters[4].Value = articleUserId;


            var list = Util.ReaderToList<Comment>(sql, parameters);
            string webHost = ConfigurationManager.AppSettings["webHost"];
            string defaultAvater = string.Format("{0}/images/default_avater.png", webHost);
            int resouceType = (int)ResourceTypeEnum.评论图;
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
                ReplayCount = x.ReplayCount,
                UserId = x.UserId,
                SubTime = x.SubTime,
                StarCount = x.StarCount,
                Pictures = resourceService.GetResources(resouceType, x.Id)
                    .Select(n => n.RPath).ToList()
            }).ToList();

            return new ApiResult<List<CommentResDto>>()
            {
                Data = data
            };
        }


        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="id">文章Id/彩种Id</param>
        /// <param name="lastId">上次拉取的最后一条Id</param>
        /// <param name="type">类型1=计划 2=文章 3=该用户所有计划的评论数量</param>
        /// <param name="pageSize"></param>
        /// <param name="userId">当前登录用户Id</param>
        /// <param name="articleUserId">评论关联用户Id</param>
        /// <returns></returns>
        public ApiResult<List<CommentResDto>> GetCommentList(int id, int lastId, int type, int pageSize, long userId, int articleUserId)
        {
            string sql;
            if (type == 1)
            {
                sql = @"select Top " + pageSize + @" a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id) as StarCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.IsDeleted = 0 and a.RefCommentId=0 and a.Type=@Type and a.ArticleId = @ArticleId and a.ArticleUserId = @ArticleUserId";
            }
            else
            {
                sql = @"select Top " + pageSize + @" a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id) as StarCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.IsDeleted = 0 and a.RefCommentId=0 and a.Type=@Type and a.ArticleId = @ArticleId";
            }

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
                new SqlParameter("@ArticleUserId",articleUserId),
                new SqlParameter("@UserId",userId),
            };

            var list = Util.ReaderToList<Comment>(sql, parameters);

            string webHost = ConfigurationManager.AppSettings["webHost"];
            string defaultAvater = string.Format("{0}/images/default_avater.png", webHost);
            int resouceType = (int)ResourceTypeEnum.评论图;

            var pageData = list.Select(x => new CommentResDto()
            {
                Id = x.Id,
                ArticleId = x.ArticleId,
                Avater = string.IsNullOrWhiteSpace(x.Avater) ? defaultAvater : x.Avater,
                Content = x.Content,
                IsLike = x.CurrentUserLikes > 0,
                NickName = x.NickName,
                PId = x.PId,
                UserId = x.UserId,
                SubTime = x.SubTime,
                RefCommentId = x.RefCommentId,
                ParentComment = GetParentComment(x.PId),
                StarCount = x.StarCount,
                ReplayCount = x.ReplayCount,
                Pictures = resourceService.GetResources(resouceType, x.Id)
                    .Select(n => n.RPath).ToList()
            }).ToList();

            return new ApiResult<List<CommentResDto>>()
            {
                Data = pageData
            };
        }

        /// <summary>
        /// 获取评论数量
        /// </summary>
        /// <param name="id">文章Id/计划Id/用户Id</param>
        /// <param name="type">类型1=计划 2=文章</param>
        /// <returns></returns>
        public int GetCommentCount(int id, int type, int articleUserId)
        {
            string commentTotalCountSql;
            commentTotalCountSql = "select count(1) from Comment where IsDeleted = 0 and Type=" + type +
                                              " and ArticleId=" + id;
            if (type == 1)
            {
                //该用户所有计划的评论数量
                commentTotalCountSql += " AND ArticleUserId=" + articleUserId;
            }




            var obj = SqlHelper.ExecuteScalar(commentTotalCountSql);

            return obj.ToInt32();
        }

        /// <summary>
        /// 获取回复列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="lastId"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApiResult<List<CommentResDto>> GetReplayList(int id, int type, int lastId, int pageSize, long userId)
        {
            string sql = "select top " + pageSize + @" a.*,isnull(b.Name,'') as NickName,isnull(c.RPath,'') as Avater 
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id and UserId=@UserId) as CurrentUserLikes 
,(select count(1) from Comment where RefCommentId = a.Id ) as ReplayCount
,(select count(1) from LikeRecord where [Status]=1 and [Type]=a.[Type] and CommentId=a.Id) as StarCount
from Comment a
  left join UserInfo b on b.Id = a.UserId
  left join ResourceMapping c on c.FkId = a.UserId and c.Type = @ResourceType
  where a.RefCommentId = @RefCommentId and a.IsDeleted = 0 and a.Type=@Type";

            if (lastId > 0)
            {
                sql += " and a.Id <" + lastId;
            }

            sql += " order by Id DESC";

            var parameters = new[]
           {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@ResourceType",(int)ResourceTypeEnum.用户头像),
                new SqlParameter("@RefCommentId",id),
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
                ReplayCount = x.ReplayCount,
                StarCount = x.StarCount,
                ParentComment = GetParentComment(x.PId),
                SubTime = x.SubTime,
                UserId = x.UserId
            }).ToList();

            return new ApiResult<List<CommentResDto>>()
            {
                Data = pageData
            };
        }

        /// <summary>
        /// 获取评论信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ParentCommentResDto GetParentComment(long id)
        {
            string sql = @"select a.Id,a.UserId,a.[Content],isnull(b.Name,'') as NickName
 from Comment a
left join UserInfo b on b.Id=a.UserId
where  a.Id=@Id and a.IsDeleted = 0";
            var parameters = new[]
           {
                new SqlParameter("@Id",id),
            };

            var list = Util.ReaderToList<ParentCommentResDto>(sql, parameters);


            if (list != null && list.Any())
            {
                return list.FirstOrDefault();
            }

            return null;
        }


        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="ctype">操作类型 1=点赞 2=取消点赞</param>
        /// <param name="type">类型 1=计划 2=文章</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public ApiResult ClickLike(int id, int ctype, int type, long userId)
        {
            var result = new ApiResult();
            string sql = "select [Id],[CommentId],[UserId],[Status],[Type] from [dbo].[LikeRecord] where [Type]=" + type + " and CommentId=" + id + " and UserId=" + userId;

            var list = Util.ReaderToList<LikeRecord>(sql);

            if (ctype == 1)
            {
                if (list.Any())
                {
                    //已存在点赞记录
                    var likeRecord = list.FirstOrDefault();
                    if (likeRecord.Status == (int)LikeStatusEnum.Canceled)
                    {
                        //已存在的点赞记录为取消状态
                        //修改点赞状态
                        result = MoidfyLike(id, type, userId, (int)LikeStatusEnum.Clicked);
                    }
                    else
                    {
                        result = new ApiResult(10000, "你已经点过赞");
                    }
                }
                else
                {
                    #region 添加点赞
                    try
                    {
                        //添加点赞
                        //SqlHelper.ExecuteTransaction();
                        string insert = @"INSERT INTO [dbo].[LikeRecord]
           ([CommentId],[UserId],[CreateTime],[Status],[UpdateTime],[Type])
     VALUES(@CommentId,@UserId,GETDATE(),1,GETDATE(),@Type);
        UPDATE [dbo].[Comment] SET [StarCount]+=1 WHERE Id=@CommentId;";

                        var insertParameters = new[]
                        {
                        new SqlParameter("@CommentId", id),
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@Type", type),
                    };

                        SqlHelper.ExecuteTransaction(insert, insertParameters);
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.WriteLog($"点赞异常，用户：{UserHelper.GetUser().Name},点赞类型：{type},Id:{id}。堆栈：{ex.StackTrace}");
                        result = new ApiResult(500, ex.Message);
                    }
                    #endregion

                }
            }
            else if (ctype == 2)
            {
                //取消点赞
                if (list.Any())
                {
                    result = MoidfyLike(id, type, userId, (int)LikeStatusEnum.Canceled);
                }
            }
            else
            {
                result = new ApiResult(400, "超出业务范围");
            }

            return result;
        }


        /// <summary>
        /// 修改点赞
        /// </summary>
        /// <param name="id">评论Id</param>
        /// <param name="type">评论类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="likeStatus">点赞操作类型 </param>
        /// <returns></returns>
        private static ApiResult MoidfyLike(int id, int type, long userId, int likeStatus)
        {
            var result = new ApiResult();
            try
            {
                //修改点赞
                //SqlHelper.ExecuteTransaction();
                string updateSql = @"
        UPDATE [dbo].[LikeRecord] SET [Status]=@Status,[UpdateTime]=GETDATE() 
        WHERE [CommentId]=@CommentId AND [UserId]=@UserId AND [Type]=@Type;";

                if (likeStatus == 1)
                {
                    updateSql += "UPDATE [dbo].[Comment] SET [StarCount] +=1 WHERE [StarCount]>0 and Id=@CommentId;";
                }
                else
                {
                    updateSql += "UPDATE [dbo].[Comment] SET [StarCount] -=1 WHERE [StarCount]>0 and Id=@CommentId;";
                }

                var updateParameters = new[]
                {
                    new SqlParameter("@CommentId", id),
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@Status", likeStatus)
                };

                SqlHelper.ExecuteTransaction(updateSql, updateParameters);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog($"取消点赞异常，用户：{UserHelper.GetUser().Name},点赞类型：{type},Id:{id}。堆栈：{ex.StackTrace}");
                LogHelper.WriteLog("取消点赞异常，用户：" + userId + ",点赞类型：" + type + ", Id:" + id + "。堆栈：" + ex.StackTrace);

                result = new ApiResult(500, ex.Message);
            }
            return result;
        }
    }
}
