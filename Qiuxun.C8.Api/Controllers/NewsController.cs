using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Model.News;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Common.Paging;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 新闻资讯
    /// </summary>
    public class NewsController : QiuxunApiController
    {
        private NewsService newsService = new NewsService();

        /// <summary>
        /// 获取新闻彩种分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<LotteryTypeResDto>> GetNewsLotteryTypeList()
        {
            return newsService.GetLotteryTypeList();
        }

        /// <summary>
        /// 根据彩种分类Id获取新闻栏目列表
        /// </summary>
        /// <param name="lType">彩种分类Id</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<NewsTypeResDto>> GetNewsTypeList(int lType)
        {
            return newsService.GetNewsTypeList(lType);
        }

        ///// <summary>
        ///// 获取六合彩新闻栏目列表
        ///// </summary>
        ///// <param name="lType"></param>
        ///// <returns></returns>
        //[HttpGet, AllowAnonymous]
        //public ApiResult GetNewsTypeListByLhc(int lType = 5)
        //{

        //}

        /// <summary>
        /// 获取彩种开奖信息（新闻资讯中的彩种分类Id）
        /// </summary>
        /// <param name="lType">新闻资讯彩种分类Id</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<IndexLotteryInfoResDto> GetLotteryInfo(int lType)
        {
            var lotteryService = new LotteryService();

            return lotteryService.GetLotteryInfoByNewsLType(lType);
        }

        /// <summary>
        /// 获取六合彩导航列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<NavigationResDto>> GetNavigationByLhc()
        {
            var list = new List<NavigationResDto>();

            list.Add(new NavigationResDto()
            {
                Name = "开奖",
                Id = 5,
                Logo = string.Format("{0}/images/icon_info_01.png", WebHost),
                Type = 1,
                Url = ""
            });
            list.Add(new NavigationResDto()
            {
                Name = "挂牌",
                Id = 123,
                Logo = string.Format("{0}/images/icon_info_02.png", WebHost),
                Type = 2,
                Url = ""
            });

            list.Add(new NavigationResDto()
            {
                Name = "跑狗",
                Id = 124,
                Logo = string.Format("{0}/images/icon_info_03.png", WebHost),
                Type = 2,
                Url = ""
            });

            list.Add(new NavigationResDto()
            {
                Name = "时间",
                Id = 0,
                Logo = string.Format("{0}/images/icon_info_04.png", WebHost),
                Type = 3,
                Url = string.Format("{0}/News/LotteryTime", WebHost)
            });

            return new ApiResult<List<NavigationResDto>>()
            {
                Data = list
            };

        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="newsTypeId">新闻彩种Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<PagedListDto<NewsListResDto>> GetNewsList(int newsTypeId, int pageIndex = 1, int pageSize = 20)
        {
            return newsService.GetNewsList(newsTypeId, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取图库列表
        /// </summary>
        /// <param name="ltype">新闻彩种分类Id</param>
        /// <param name="newsTypeId">新闻栏目Id</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<PagedListDto<GalleryTypeResDto>> GetGalleryTypeList(int ltype, int newsTypeId)
        {
            return newsService.GetGalleryTypeList(ltype, newsTypeId);
        }

        /// <summary>
        /// 获取图库详情
        /// </summary>
        /// <param name="id">当前图库分类Id(文章Id)</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<Gallery>> GetGalleryList(int id)
        {
            return newsService.GetGalleryList(id);
        }

        /// <summary>
        /// 获取图库详情页推荐内容
        /// </summary>
        /// <param name="id">当前图库Id(文章Id)</param>
        /// <param name="count">查询推荐数量，可空，默认=10</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<RecommendGalleryResDto>> GetRecommendGalleryList(int id, int count = 10)
        {
            return newsService.GetRecommendGalleryList(id, count);
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<NewsResDto> NewsDetail(int id)
        {
            return newsService.GetNewsDetal(id);
        }

        /// <summary>
        /// 获取推荐阅读
        /// </summary>
        /// <param name="newsTypeId">新闻栏目Id</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<NewsListResDto>> GetRecommendNewsList(int newsTypeId)
        {
            return newsService.GetRecommendNewsList(newsTypeId);
        }

        /// <summary>
        /// 获取热门新闻
        /// </summary>
        /// <param name="count">查询数量</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ApiResult<List<NewsListResDto>> GetHotNewsList(int count = 5)
        {
            return newsService.GetRecommendNewsList(count);
        }

    }
}