using System;
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
using Qiuxun.C8.Api.Service.Cache;
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
            List<LotteryType> list = CacheHelper.GetCache<List<LotteryType>>("base_lottery_type");
            if (list == null)
            {
                list = Util.GetEntityAll<LotteryType>().OrderBy(x => x.SortCode).ToList();

                //CacheHelper.WriteCache("base_lottery_type", list);
                CacheHelper.AddCache("base_lottery_type", list);
            }
            var resDto = list.Select(x => new LotteryTypeResDto()
            {
                Id = x.Id,
                LType = Util.GetlTypeById(x.Id.ToInt32()),
                LTypeName = x.TypeName,
                SortCode = x.SortCode
            }).ToList();


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

            var list = CacheHelper.GetCache<List<NewsType>>(memKey);

            if (list == null)
            {
                string newsTypeSql =
                    "SELECT TOP 500 [Id],[TypeName],[ShowType],[lType] FROM [dbo].[NewsType] WHERE [lType]=" +
                    ltype + " AND [Layer]=" + layer + " ORDER BY SortCode ";
                list = Util.ReaderToList<NewsType>(newsTypeSql) ?? new List<NewsType>();


                // CacheHelper.WriteCache(memKey, list);
                CacheHelper.AddCache(memKey, list);
            }


            var resDto = list.Select(x => new NewsTypeResDto()
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

            //CacheHelper.WriteCache(memKey, list);
            CacheHelper.AddCache(memKey, list);
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
            List<News> list = CacheHelper.GetCache<List<News>>(string.Format("z_newslist_{0}_{1}", ltype, pageIndex));
            if (list == null || list.Count <= 0)
            {
                string sql = @"SELECT * FROM ( 
                            SELECT row_number() over(order by SortCode ASC, LotteryNumber DESC, ReleaseTime DESC ) as rowNumber,
                            [Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],(SELECT COUNT(1) FROM [dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
                            ,STUFF((SELECT ',' + RPath FROM  dbo.ResourceMapping WHERE  Type=1 AND FkId=a.Id FOR XML PATH('')), 1, 1, '') AS ThumbListStr
                            FROM [dbo].[News] a
                            WHERE [TypeId]=@TypeId and DeleteMark=0 and EnabledMark=1 ) T
                            WHERE rowNumber BETWEEN @Start AND @End 
                            ORDER BY rowNumber";
                SqlParameter[] parameters =
                {
                new SqlParameter("@TypeId",SqlDbType.BigInt),
                new SqlParameter("@Start",SqlDbType.Int),
                new SqlParameter("@End",SqlDbType.Int),
                };
                parameters[0].Value = ltype;
                parameters[1].Value = (pageIndex - 1) * pageSize + 1;
                parameters[2].Value = pageSize * pageIndex;
                list = Util.ReaderToList<News>(sql, parameters) ?? new List<News>();
            }
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
                ThumbList = !(string.IsNullOrWhiteSpace(x.ThumbListStr)) ? x.ThumbListStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()
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

            List<GalleryTypeResDto> list = CacheHelper.GetCache<List<GalleryTypeResDto>>(string.Format("z_GalleryTypeList_{0}_{1}", ltype, newsTypeId));
            SqlParameter[] parameters =
                {
                new SqlParameter("@NewsTypeId",SqlDbType.Int),
                new SqlParameter("@LType",SqlDbType.BigInt)
            };
            parameters[0].Value = newsTypeId;
            parameters[1].Value = ltype;
            if (list == null)
            {
                string sql = @" SELECT Max(a.Id) as Id, FullHead as Name, right(Max(a.LotteryNumber),3) as LastIssue,isnull(a.QuickQuery,'#') as QuickQuery
                                 from News  a
                                 left join NewsType b on b.Id= a.TypeId
                                 where a.TypeId=@NewsTypeId and b.lType=@LType and DeleteMark=0 and EnabledMark=1
                                 group by a.FullHead,a.QuickQuery
                                 order by a.QuickQuery";


                list = Util.ReaderToList<GalleryTypeResDto>(sql, parameters) ?? new List<GalleryTypeResDto>();
            }
            var page = new PagedList<GalleryTypeResDto>(list, 1, 10000, list.Count)
                .GetPagedListDto();

            //查询推荐图
            List<Gallery> recGalleryList = CacheHelper.GetCache<List<Gallery>>(string.Format("z_GalleryList_{0}_{1}", ltype, newsTypeId));
            if (recGalleryList == null)
            {
                string recGallerySql = @" SELECT TOP 3 a.Id,FullHead as Name,LotteryNumber as Issue,
                                            STUFF(( SELECT  ',' + RPath
                                                            FROM    dbo.ResourceMapping
                                                            WHERE   Type = 1
                                                                    AND FkId = a.Id
                                                          FOR
                                                            XML PATH('')
                                                          ), 1, 1, '') AS Picture
                                        FROM News a 
                                         left join NewsType b on b.Id= a.TypeId
                                         where a.RecommendMark=1 and DeleteMark=0 and EnabledMark=1 and TypeId=@NewsTypeId and b.lType=@LType order by ModifyDate DESC";
                recGalleryList = Util.ReaderToList<Gallery>(recGallerySql, parameters);
            }
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
            AddNewsPv(id);
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

            #region 设置分享信息

            string webHost = BaseService.WebHost;
            resDto.Share = new ShareDto()
            {
                Title = model.FullHead,
                Describe = model.FullHead,
                Link = string.Format("{0}/News/NewsDetail/{1}", webHost, model.Id)
            };

            int sourceType = (int)ResourceTypeEnum.新闻缩略图;
            List<string> thumbList = sourceService.GetResources(sourceType, model.Id)
                .Select(n => n.RPath).ToList();
            if (thumbList.Count > 0)
                resDto.Share.Icon = thumbList.FirstOrDefault();
            else
                resDto.Share.Icon = string.Format("{0}/images/c8.png", webHost);

            #endregion

            #region 上一篇 下一篇
            //查询上一篇
            string preSql = @"SELECT TOP 1
[Id],[FullHead] as Title
FROM [dbo].[News] 
WHERE [TypeId]=@TypeId AND DeleteMark=0 AND EnabledMark=1 AND [Id] > @CurrentId 
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
WHERE [TypeId]=@TypeId AND DeleteMark=0 AND EnabledMark=1 AND [Id] < @CurrentId 
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
        /// 添加新闻PV计数
        /// </summary>
        /// <param name="id"></param>
        //        private void AddNewsPv(int id)
        //        {
        //            try
        //            {
        //                string pvSql = @"if exists(
        //	select 1 from dbo.PageView where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
        //  )
        //  begin
        //   update dbo.PageView set ViewTotal+=1 where [Type]=@Type and FkId=@Id and ViewDate=@ViewDate
        //  end
        //  else
        //  begin
        //  insert into dbo.PageView(ViewDate,ViewTotal,[Type],FkId) values(GETDATE(),1,@Type,@Id)
        //  end;
        //UPDATE dbo.News SET PV+=1 WHERE Id=@Id";
        //                var pvParam = new[]
        //                {
        //                        new SqlParameter("@Type",1),//新闻类型=1
        //                        new SqlParameter("@Id",id),
        //                        new SqlParameter("@ViewDate",DateTime.Today),
        //                    };
        //                SqlHelper.ExecuteNonQuery(pvSql, pvParam);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.ErrorFormat("新闻{2}PV增加异常，Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace, id);
        //            }
        //        }

        public void AddNewsPv(int id)
        {
            var pageViewList = CacheHelper.GetCache<List<PageView>>("SavePageViewList");

            if (pageViewList == null || !pageViewList.Any(e => e.FkId == id && e.Type == 1))
            {
                var pageView = new PageView()
                {
                    FkId = id,
                    Type = 1,
                    ViewTotal = 1
                };

                if (pageViewList == null)
                    pageViewList = new List<PageView>();

                pageViewList.Add(pageView);
            }
            else
            {
                pageViewList.FirstOrDefault(e => e.FkId == id && e.Type == 1).ViewTotal++;
            }

            CacheHelper.SetCache<List<PageView>>("SavePageViewList", pageViewList, DateTime.Now.AddDays(2));
        }

        /// <summary>
        /// 根据新闻栏目Id获取推荐阅读文章列表
        /// </summary>
        /// <param name="newsTypeId"></param>
        /// <returns></returns>
        public ApiResult<List<NewsListResDto>> GetRecommendNewsList(int newsTypeId)
        {
            List<News> list = CacheHelper.GetCache<List<News>>(("z_newstop3list_" + newsTypeId));
            if (list == null || list.Count <= 0)
            {
                string recommendArticlesql = @"SELECT TOP 3 [Id],TypeId,[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],
                                                (SELECT COUNT(1) FROM[dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
                                                FROM [dbo].[News] a
                                                WHERE [TypeId] = @TypeId AND DeleteMark=0 AND EnabledMark=1
                                                ORDER BY ModifyDate DESC,SortCode ASC ";
                //AND RecommendMark = 1

                var recommendArticleParameters = new[]
                {
                    new SqlParameter("@TypeId",newsTypeId),
                };

                list = Util.ReaderToList<News>(recommendArticlesql, recommendArticleParameters);
                CacheHelper.AddCache<List<News>>(("z_newstop3list_" + newsTypeId), list, 120);
            }


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
            AddNewsPv(articleId);

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
            List<RecommendGalleryResDto> recGalleryList = CacheHelper.GetCache<List<RecommendGalleryResDto>>(("z_recGalleryList_" + news.TypeId));
            if (recGalleryList == null || recGalleryList.Count <= 0)
            {
                string recGallerySql = " SELECT TOP " + count + @" FullHead as Name, Id,LotteryNumber as Issue 
                                         from News where Id  in(
	                                        select max(id) from News where TypeId=" + news.TypeId + @" group by FullHead having count(FullHead)>=1
                                         )
                                         and DeleteMark=0 and EnabledMark=1 
                                         order by RecommendMark DESC,LotteryNumber DESC,ModifyDate DESC";
                recGalleryList = Util.ReaderToList<RecommendGalleryResDto>(recGallerySql);
                CacheHelper.AddCache<List<RecommendGalleryResDto>>(("z_recGalleryList_" + news.TypeId), recGalleryList, 60);
            }
            return new ApiResult<List<RecommendGalleryResDto>>()
            {
                Data = recGalleryList
            };
        }


        /// <summary>
        /// 获取热门新闻
        /// </summary>
        /// <param name="count">查询数量</param>
        /// <returns></returns>
        public ApiResult<List<NewsListResDto>> GetHotNewsList(int count)
        {
            List<NewsListResDto> data = CacheHelper.GetCache<List<NewsListResDto>>("GetNewListToWebAPI");
            if (data == null)
            {
                //string hotArticlesql = "SELECT TOP " + count + @" [Id],[FullHead],[SortCode],[Thumb],[ReleaseTime],[ThumbStyle],[TypeId],
                //(SELECT COUNT(1) FROM[dbo].[Comment] WHERE [ArticleId]=a.Id and RefCommentId=0) as CommentCount
                //FROM [dbo].[News] a
                //WHERE  DeleteMark=0 AND EnabledMark=1
                //ORDER BY CommentCount DESC,SortCode ASC ";

                string hotArticlesql = @"select top " + count + @" n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle ,TypeId,
                    count(c.id) as CommentCount
                    from News n
                    left join Comment c on n.id = c.ArticleId and RefCommentId = 0
                    where n.id in(
                    select max(n.Id) from News n 
                    join newsType nt on n.TypeId = nt.Id
                    where nt.lType in (1,2,3,4,6) and nt.SortCode = 1 and n.DeleteMark=0 and n.EnabledMark = 1
                    group by nt.lType
                    )
                    group by n.Id,n.FullHead,n.SortCode,n.Thumb,n.ReleaseTime,n.ThumbStyle";

                var list = Util.ReaderToList<News>(hotArticlesql);

                //int sourceType = (int)ResourceTypeEnum.新闻缩略图;
                data = list.Select(x => new NewsListResDto()
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    CommentCount = x.CommentCount,
                    ReleaseTime = x.ReleaseTime,
                    SortCode = x.SortCode,
                    ThumbStyle = x.ThumbStyle,
                    Title = x.FullHead,
                    TypeId = x.TypeId,
                    //ThumbList = sourceService.GetResources(sourceType, x.Id)
                    //                .Select(n => n.RPath).ToList()
                    ThumbList = new List<string>()
                }).ToList();

                //新闻缓存2小时
                CacheHelper.AddCache<List<NewsListResDto>>("GetNewListToWebAPI", data, 2 * 60);
            }

            return new ApiResult<List<NewsListResDto>>()
            {
                Data = data
            };
        }

        /// <summary>
        /// 获取新闻栏目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApiResult<NewsTypeResDto> GetNewsType(int id)
        {
            var newsType = Util.GetEntityById<NewsType>(id);

            if (newsType == null) throw new ApiException(40000, "资讯栏目不存在");


            var resDto = new NewsTypeResDto()
            {
                Id = newsType.Id,
                TypeName = newsType.TypeName,
                Layer = newsType.Layer,
                LType = newsType.LType,
                ParentId = newsType.ParentId,
                ShowType = newsType.ShowType,
                SortCode = newsType.SortCode
            };

            var lotteryType = Util.GetEntityById<LotteryType>(newsType.LType.ToInt32());

            if (lotteryType != null)
                resDto.LTypeName = lotteryType.TypeName;

            return new ApiResult<NewsTypeResDto>()
            {
                Data = resDto
            };

        }
    }
}
