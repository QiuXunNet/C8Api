﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Model;
using Qiuxun.C8.Api.Service.Model.News;
using Qiuxun.C8.Caching;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 新闻资讯业务类
    /// </summary>
    public class NewsService
    {
        ResourceManagementService sourceService = new ResourceManagementService();
        /// <summary>
        /// 获取新闻资讯彩种分类列表
        /// </summary>
        /// <returns></returns>
        public ApiResult<List<LotteryTypeResDto>> GetLotteryTypeList()
        {
            List<LotteryTypeResDto> resDto = CacheHelper.GetCache<List<LotteryTypeResDto>>("base_lottery_type");
            if (resDto == null)
            {
                var list = Util.GetEntityAll<LotteryType>().OrderBy(x => x.SortCode).ToList();

                resDto = list.Select(x => new LotteryTypeResDto()
                {
                    Id = x.Id,
                    LType = Util.GetlTypeById(x.Id.ToInt32()),
                    LTypeName = x.TypeName,
                    SortCode = x.SortCode
                }).ToList();

                CacheHelper.WriteCache("base_lottery_type", resDto);
            }

            return new ApiResult<List<LotteryTypeResDto>>()
            {
                Data = resDto
            };
        }

        /// <summary>
        /// 获取资讯栏目
        /// </summary>
        /// <param name="ltype">彩种Id</param>
        /// <param name="layer">层级</param>
        /// <returns></returns>
        public ApiResult<List<NewsTypeResDto>> GetNewsTypeList(long ltype, int layer = 1)
        {
            string memKey = "base_news_type_" + ltype;
            var resDto = CacheHelper.GetCache<List<NewsTypeResDto>>(memKey);

            if (resDto == null)
            {
                string newsTypeSql =
                    "SELECT TOP 500 [Id],[TypeName],[ShowType],[lType] FROM [dbo].[NewsType] WHERE [lType]=" +
                    ltype + " AND [Layer]=" + layer + " ORDER BY SortCode ";
                var list = Util.ReaderToList<NewsType>(newsTypeSql) ?? new List<NewsType>();

                resDto = list.Select(x => new NewsTypeResDto()
                {
                    Id = x.Id,
                    LType = Util.GetlTypeById((int)x.LType),
                    Layer = x.Layer,
                    LTypeName = x.LTypeName,
                    ParentId = x.ParentId,
                    ShowType = x.ShowType,
                    SortCode = x.SortCode,
                    TypeName = x.TypeName,
                    GroupType = GetGroupType(x.LType, x.TypeName)
                }).ToList();

                CacheHelper.WriteCache(memKey, resDto);
            }

            return new ApiResult<List<NewsTypeResDto>>()
            {
                Data = resDto
            };
        }

        /// <summary>
        /// 处理六合彩特殊栏目分组
        /// </summary>
        /// <param name="ltype"></param>
        /// <param name="newsTypeName"></param>
        /// <returns></returns>
        private int GetGroupType(long ltype, string newsTypeName)
        {
            if (ltype != 5) return 0;

            if (newsTypeName == "看图解码" || newsTypeName == "幸运彩图"
                || newsTypeName == "精选彩图" || newsTypeName == "香港图库")
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 查询当前分类的玄机图库
        /// </summary>
        /// <param name="newsId">新闻Id</param>
        /// <param name="newsTitle">新闻标题</param>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public List<Gallery> GetGalleries(long newsId, string newsTitle, int lType)
        {
            string memKey = "base_gallery_id_" + newsId;
            var list = CacheHelper.GetCache<List<Gallery>>(memKey);

            if (list != null && list.Any()) return list;

            string sql = @"select a.Id, a.FullHead as Name,a.[TypeId],right(ISNULL(a.LotteryNumber,''),3) as Issue, c.RPath as Picture 
from News a
left join ResourceMapping c on c.FkId=a.Id and c.[Type]=1
where a.FullHead=@FullHead and a.[TypeId]=@TypeId and DeleteMark=0 and EnabledMark=1  
order by a.LotteryNumber desc";

            var parameters = new[]
            {
                new SqlParameter("@FullHead",SqlDbType.NVarChar),
                new SqlParameter("@TypeId",SqlDbType.Int)
            };
            parameters[0].Value = newsTitle;
            parameters[1].Value = lType;

            list = Util.ReaderToList<Gallery>(sql, parameters) ?? new List<Gallery>();

            CacheHelper.WriteCache(memKey, list);
            return list;
        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="ltype">新闻彩种分类Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ApiResult<PagedListDto<NewsListResDto>> GetNewsList(int ltype, int pageIndex, int pageSize)
        {

            string sql = @"SELECT * FROM ( 
SELECT row_number() over(order by SortCode ASC, ReleaseTime DESC ) as rowNumber,
[Id],[FullHead],[SortCode],[Thumb],TypeId,[ReleaseTime],[ThumbStyle],(SELECT COUNT(1) FROM [dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
FROM [dbo].[News] a
WHERE [TypeId]=@TypeId and DeleteMark=0 and EnabledMark=1 ) T
WHERE rowNumber BETWEEN @Start AND @End";
            SqlParameter[] parameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@Start",SqlDbType.Int),
                new SqlParameter("@End",SqlDbType.Int),
            };
            parameters[0].Value = ltype;
            parameters[1].Value = (pageIndex - 1) * pageSize + 1;
            parameters[2].Value = pageSize * pageIndex;
            var list = Util.ReaderToList<News>(sql, parameters) ?? new List<News>();

            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            var pageData = list.Select(x => new NewsListResDto()
            {
                Id = x.Id,
                ParentId = x.ParentId,
                CommentCount = x.CommentCount,
                ReleaseTime = x.ReleaseTime,
                SortCode = x.SortCode,
                ThumbStyle = x.ThumbStyle,
                Title = x.FullHead,
                TypeId = x.TypeId,
                ThumbList = sourceService.GetResources(sourceType, x.Id)
                    .Select(n => n.RPath).ToList()
            }).ToList();



            //查询总数量
            string countSql = @"SELECT count(1) FROM [dbo].[News]
WHERE [TypeId]=@TypeId and DeleteMark=0 and EnabledMark=1 ";
            object obj = SqlHelper.ExecuteScalar(countSql, new SqlParameter("@TypeId", ltype));

            var data = new PagedList<NewsListResDto>(pageData, pageIndex, pageSize, obj.ToInt32())
                .GetPagedListDto();

            return new ApiResult<PagedListDto<NewsListResDto>>()
            {
                Data = data
            };
        }

        /// <summary>
        /// 获取图库类型列表
        /// </summary>
        /// <param name="ltype">彩种分类Id</param>
        /// <param name="newsTypeId">新闻栏目Id</param>
        /// <returns></returns>
        public ApiResult<PagedListDto<GalleryTypeResDto>> GetGalleryTypeList(long ltype, int newsTypeId)
        {
            string sql = @" SELECT Max(a.Id) as Id, FullHead as Name, right(Max(a.LotteryNumber),3) as LastIssue,isnull(a.QuickQuery,'#') as QuickQuery
 from News  a
 left join NewsType b on b.Id= a.TypeId
 where a.TypeId=@NewsTypeId and b.lType=@LType and DeleteMark=0 and EnabledMark=1
 group by a.FullHead,a.QuickQuery
 order by a.QuickQuery";
            SqlParameter[] parameters =
            {
                new SqlParameter("@NewsTypeId",SqlDbType.Int),
                new SqlParameter("@LType",SqlDbType.BigInt)
            };
            parameters[0].Value = newsTypeId;
            parameters[1].Value = ltype;

            var list = Util.ReaderToList<GalleryTypeResDto>(sql, parameters) ?? new List<GalleryTypeResDto>();

            var page = new PagedList<GalleryTypeResDto>(list, 1, 10000, list.Count)
                .GetPagedListDto();

            //查询推荐图
            string recGallerySql = @" SELECT TOP 3 a.Id,FullHead as Name,LotteryNumber as Issue FROM News a 
 left join NewsType b on b.Id= a.TypeId
 where a.RecommendMark=1 and DeleteMark=0 and EnabledMark=1 and TypeId=@NewsTypeId and b.lType=@LType order by ModifyDate DESC";
            var recGalleryList = Util.ReaderToList<Gallery>(recGallerySql, parameters);

            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            recGalleryList.ForEach(x =>
            {
                var pic = sourceService.GetResources(sourceType, x.Id);
                if (pic.Count > 0)
                {
                    x.Picture = pic[0].RPath;
                }
            });
            page.ExtraData = recGalleryList;

            return new ApiResult<PagedListDto<GalleryTypeResDto>>()
            {
                Data = page
            };
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResult<NewsResDto> GetNewsDetal(int id)
        {
            //获取新闻实体

            string sql = @"SELECT 
[Id],[FullHead],[SortCode],[Thumb],[TypeId],[ReleaseTime],[ThumbStyle],NewsContent,
(SELECT COUNT(1) FROM [dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
FROM [dbo].[News] a
WHERE [Id]=@Id and DeleteMark=0 and EnabledMark=1 ";
            SqlParameter[] parameters =
            {
                new SqlParameter("@Id",id),
            };

            var list = Util.ReaderToList<News>(sql, parameters);

            var model = list.FirstOrDefault();

            if (model == null)
                return new ApiResult<NewsResDto>(40000, "文章不存在或已删除");

            //var thumbList = sourceService.GetResources((int)ResourceTypeEnum.新闻缩略图, model.Id);
            //if (thumbList.Any())
            //    model.Thumb = thumbList.First().RPath;

            var resDto = new NewsResDto()
            {
                CommentCount = model.CommentCount,
                Id = model.Id,
                Content = HttpUtility.UrlDecode(model.NewsContent),
                ReleaseTime = model.ReleaseTime,
                Title = model.FullHead,
                TypeId = model.TypeId
            };

            #region 上一篇 下一篇
            //查询上一篇
            string preSql = @"SELECT TOP 1
[Id],[FullHead] as Title
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND [Id] > @CurrentId 
ORDER BY SortCode,Id";
            SqlParameter[] preParameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@CurrentId",SqlDbType.Int),
            };
            preParameters[0].Value = model.TypeId;
            preParameters[1].Value = id;
            var preview = Util.ReaderToList<PrevNewsInfo>(preSql, preParameters);

            if (preview != null && preview.Count > 0)
            {
                resDto.Previous = preview[0];
            }

            //查询下一篇
            string nextsql = @"SELECT TOP 1
[Id],[FullHead] as Title
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND [Id] < @CurrentId 
ORDER BY SortCode desc,Id DESC";
            SqlParameter[] nextparameters =
            {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@CurrentId",SqlDbType.Int),
            };
            nextparameters[0].Value = model.TypeId;
            nextparameters[1].Value = id;
            var nextview = Util.ReaderToList<NextNewsInfo>(nextsql, nextparameters);

            if (nextview != null && nextview.Count > 0)
            {
                resDto.Next = nextview[0];
            }
            #endregion

            return new ApiResult<NewsResDto>()
            {
                Data = resDto
            };

        }


        /// <summary>
        /// 根据新闻栏目Id获取推荐阅读文章列表
        /// </summary>
        /// <param name="newsTypeId"></param>
        /// <returns></returns>
        public ApiResult<List<NewsListResDto>> GetRecommendNewsList(int newsTypeId)
        {
            string recommendArticlesql = @"SELECT TOP 3 [Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],
(SELECT COUNT(1) FROM[dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
FROM [dbo].[News] a
WHERE [TypeId] = @TypeId AND DeleteMark=0 AND EnabledMark=1
ORDER BY ModifyDate DESC,SortCode ASC ";
            //AND RecommendMark = 1

            var recommendArticleParameters = new[]
            {
                new SqlParameter("@TypeId",newsTypeId),
            };

            var list = Util.ReaderToList<News>(recommendArticlesql, recommendArticleParameters);


            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            var data = list.Select(x => new NewsListResDto()
            {
                Id = x.Id,
                ParentId = x.ParentId,
                CommentCount = x.CommentCount,
                ReleaseTime = x.ReleaseTime,
                SortCode = x.SortCode,
                ThumbStyle = x.ThumbStyle,
                Title = x.FullHead,
                TypeId = x.TypeId,
                ThumbList = sourceService.GetResources(sourceType, x.Id)
                                .Select(n => n.RPath).ToList()
            }).ToList();

            return new ApiResult<List<NewsListResDto>>()
            {
                Data = data
            };
        }

        /// <summary>
        /// 根据文章Id获取该文章所属所有图库
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns></returns>
        public ApiResult<List<Gallery>> GetGalleryList(int articleId)
        {
            var news = Util.GetEntityById<News>(articleId);

            var galleryList = GetGalleries(news.Id, news.FullHead, (int)news.TypeId);

            return new ApiResult<List<Gallery>>()
            {
                Data = galleryList
            };
        }

        /// <summary>
        /// 查询图库详情推荐列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ApiResult<List<RecommendGalleryResDto>> GetRecommendGalleryList(int articleId, int count)
        {

            var news = Util.GetEntityById<News>(articleId);
            string recGallerySql = " SELECT TOP " + count + @" a.Id,FullHead as Name,LotteryNumber as Issue FROM News a 
 join NewsType b on b.Id= a.TypeId
 where  b.lType in
 (select ltype from News a join NewsType b on b.Id=a.TypeId
 where a.Id=" + articleId + " ) and a.TypeId=" + news.TypeId
 + @" and DeleteMark=0 and EnabledMark=1 
 order by RecommendMark DESC,LotteryNumber DESC,ModifyDate DESC";
            var recGalleryList = Util.ReaderToList<RecommendGalleryResDto>(recGallerySql);

            return new ApiResult<List<RecommendGalleryResDto>>()
            {
                Data = recGalleryList
            };
        }


    }
}
