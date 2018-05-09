using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 广告管理服务类
    /// </summary>
    public class AdvertisementService
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="location">栏目Id</param>
        /// <param name="adtype">广告类型 1=栏目 2=文章 3=六彩栏目</param>
        /// <param name="deviceType">设备类型 1=网页等其他 2=ios 3=安卓</param>
        /// <param name="city">所在城市</param>
        /// <returns></returns>
        public ApiResult<List<AdvertisementResDto>> GetAdvertList(int location, int adtype, int deviceType, string city, string reqIp)
        {
            string memKey = string.Format("advertisement_{0}_{1}_{2}", location, adtype, deviceType);

            var list = CacheHelper.GetCache<List<Advertisement>>(memKey);
            if (list == null)
            {
                string strsql;
                if (adtype == 3)
                {
                    //六彩栏目列表
                    strsql = string.Format(@"select * from [dbo].[Advertisement] where charindex(',{1},',','+[where]+',')>0 and 
                   AdType={0} and (State=1 or (State=0 and getdate()>=BeginTime and EndTime>getdate()))", adtype, deviceType);

                }
                else
                {
                    strsql = string.Format(@"select * from [dbo].[Advertisement] where charindex(',{2},',','+[where]+',')>0   
            and charindex(',{0},',','+[Location]+',')>0 and AdType={1} and (State=1 or (State=0 and getdate()>=BeginTime and EndTime>getdate()))", location, adtype, deviceType);

                }
                list = Util.ReaderToList<Advertisement>(strsql);

                if (list != null)
                {
                   // CacheHelper.WriteCache(memKey, list);
                    CacheHelper.AddCache(memKey, list);
                }

            }

            if (list != null)
            {
                string cityId = Tool.GetCityIdByCityandIp(city, reqIp);
                var resDto = list.Select(x => new AdvertisementResDto()
                {
                    Title = x.Title,
                    CommentsNumber = x.CommentsNumber,
                    Company = x.Company,
                    Layer = x.Layer,
                    ThumbStyle = x.ThumbStyle,
                    TransferUrl = x.TransferUrl,
                    ThumbList = GetAdvertisementPictures(x.ThumbStyle, x.Id),
                    SubTime = x.SubTime,
                    Display = IsShow(cityId, x.RestrictedAreas)
                }).ToList();



                return new ApiResult<List<AdvertisementResDto>>()
                {
                    Data = resDto
                };
            }

            return new ApiResult<List<AdvertisementResDto>>();
        }

        /// <summary>
        /// 获取广告图片
        /// </summary>
        /// <param name="thumbStyle">显示方式 0=无图</param>
        /// <param name="id">广告Id</param>
        /// <returns></returns>
        private List<string> GetAdvertisementPictures(int thumbStyle, int id)
        {
            if (thumbStyle == 0) return null;

            int resourceType = (int)ResourceTypeEnum.广告图;
            ResourceManagementService resourceService = new ResourceManagementService();

            return resourceService.GetResources(resourceType, id).Select(y => y.RPath).ToList();
        }

        /// <summary>
        /// 是否显示广告
        /// </summary>
        /// <param name="cityId">当前访问者城市Id</param>
        /// <param name="restrictedAreas">广告受限制城市Id集合</param>
        /// <returns></returns>
        private bool IsShow(string cityId, string restrictedAreas)
        {
            if (string.IsNullOrWhiteSpace(restrictedAreas)) return true;
            var list = restrictedAreas.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            //城市Id不包含在受限制城市Id集合，则显示
            return !list.Contains(cityId);
        }
    }
}
