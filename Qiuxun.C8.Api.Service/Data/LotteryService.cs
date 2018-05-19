using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Cache;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Service.Data
{
    public class LotteryService
    {
        /// <summary>
        /// 获取所有彩种分类
        /// </summary>
        /// <returns></returns>
        public List<LotteryType2> GetAllLotteryTypeList()
        {
            string sql = "select * from LotteryType2 order by Position";
            return Util.ReaderToList<LotteryType2>(sql);
        }

        /// <summary>
        /// 根据上级Id获取彩种分类
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<LotteryType2> GetLotteryTypeList(int pid = 0)
        {
            List<LotteryType2> list = CacheHelper.GetCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pid);
            if (list == null)
            {
                string sql = "select * from LotteryType2 where PId = " + pid + " order by Position";

                list = Util.ReaderToList<LotteryType2>(sql);

                CacheHelper.AddCache<List<LotteryType2>>("GetChildLotteryTypeToWebSite" + pid, list, 30 * 24 * 60);
            }
            return list;
        }

        /// <summary>
        /// 获取彩种信息
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public LotteryType2 GetLotteryType2(int lType)
        {
            string sql = "select top 1 * from LotteryType2 where lType = " + lType + " order by Position";
            return Util.ReaderToModel<LotteryType2>(sql);
        }

        /// <summary>
        /// 获取玩法列表
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        public ApiResult<List<PlayResDto>> GetPlayList(int? ltype)
        {

            string memKey = "base_play_name";

            if (ltype.HasValue)
            {
                memKey = memKey + "_" + ltype;
            }
            var resDto = CacheHelper.GetCache<List<PlayResDto>>(memKey);

            if (resDto == null || resDto.Count < 1)
            {
                string sql = "select Id,LType,PlayName from dbo.IntegralRule";
                if (ltype.HasValue)
                {
                    sql += " where lType=" + ltype.Value;
                }

                var list = Util.ReaderToList<IntegralRule>(sql);

                resDto = new List<PlayResDto>();

                list.ForEach(x =>
                {
                    resDto.Add(new PlayResDto()
                    {
                        Id = x.Id,
                        LType = x.lType,
                        PlayName = x.PlayName
                    });
                });

                // CacheHelper.WriteCache(memKey, resDto);
                CacheHelper.AddCache(memKey, resDto);
            }

            return new ApiResult<List<PlayResDto>>()
            {
                Data = resDto
            };
        }

        /// <summary>
        /// 获取首页彩种信息列表
        /// </summary>
        /// <param name="lotteryTypePId">彩种分类Id</param>
        /// <returns></returns>
        public ApiResult<List<IndexLotteryInfoResDto>> GetIndexLotteryList(int lotteryTypePId)
        {
            var lotteryList = GetLotteryTypeList(lotteryTypePId);
            string lTypes = "";

            lotteryList.ForEach(e => lTypes += e.lType + ",");
            lTypes = lTypes.Trim(',');

            var dateTime = DateTime.Now.AddDays(-5);
            var resDto = GetLotteryRecordList(lTypes, dateTime);


            //lotteryList.ForEach(x =>
            //{
            //    var info = new IndexLotteryInfoResDto()
            //    {
            //        LType = x.lType,
            //        LTypeName = Util.GetLotteryTypeName(x.lType),
            //    };

            //    var lastLotteryRecord = GetLotteryRecord(x.lType);
            //    if (lastLotteryRecord != null)
            //    {
            //        info.OpenNum = lastLotteryRecord.Num;
            //        info.Issue = lastLotteryRecord.Issue;
            //        info.OpenTime = lastLotteryRecord.ShowOpenTime;
            //        info.OpenNumAlias = Util.GetShowInfo(lastLotteryRecord.lType, lastLotteryRecord.Num, lastLotteryRecord.SubTime);
            //       // info.CurrentIssue = LuoUtil.GetCurrentIssue(x.lType);
            //        info.CurrentIssue = "";
            //    }
            //    info.Logo = Util.GetLotteryIconUrl(x.lType);
            //    resDto.Add(info);
            //});

            return new ApiResult<List<IndexLotteryInfoResDto>>()
            {
                Data = resDto
            };
        }

        /// <summary>
        /// 获取彩种开奖信息
        /// </summary>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public ApiResult<IndexLotteryInfoResDto> GetLotteryInfo(int lType)
        {
            var lotteryInfo = GetLotteryType2(lType);

            if (lotteryInfo == null)
                throw new ApiException(40000, "该彩种不存在");

            var info = new IndexLotteryInfoResDto()
            {
                LType = lType,
                LTypeName = Util.GetLotteryTypeName(lType),
            };

            var lastLotteryRecord = GetLotteryRecord(lotteryInfo.lType);
            if (lastLotteryRecord != null)
            {
                info.OpenNum = lastLotteryRecord.Num;
                info.Issue = lastLotteryRecord.Issue;
                info.OpenTime = lastLotteryRecord.ShowOpenTime;
                info.OpenNumAlias = Util.GetShowInfo(lastLotteryRecord.lType, lastLotteryRecord.Num, lastLotteryRecord.SubTime);
                info.CurrentIssue = LuoUtil.GetCurrentIssue(lType);
            }
            info.Logo = Util.GetLotteryIconUrl(lotteryInfo.lType);

            return new ApiResult<IndexLotteryInfoResDto>()
            {
                Data = info
            };
        }

        /// <summary>
        /// 根据新闻彩种分类Id获取开奖信息
        /// </summary>
        /// <param name="lType">彩种Id</param>
        /// <returns></returns>
        public ApiResult<IndexLotteryInfoResDto> GetLotteryInfoByNewsLType(int lType)
        {
            int lotteryType = Util.GetlTypeById(lType);

            return GetLotteryInfo(lotteryType);
        }

        public LotteryRecord GetLotteryRecord(int ltype)
        {
            string sql = "select top(1)* from LotteryRecord where lType = " + ltype + " order by Issue desc";
            return Util.ReaderToModel<LotteryRecord>(sql);
        }

        public List<IndexLotteryInfoResDto> GetLotteryRecordList(string lTypes, DateTime dateTime)
        {
           var sql = @"select lr.* from LotteryRecord lr
                    join(
                    select lType, max(SubTime) SubTime from lotteryRecord where lType in(" + lTypes + ") and SubTime >'" + dateTime + @"' group by lType
                    ) tab on lr.lType = tab.lType and lr.SubTime = tab.SubTime
                    left join LotteryType2 lt on lr.lType = lt.lType
                    order by Position";

            var newList = new List<IndexLotteryInfoResDto>();

            Util.ReaderToList<LotteryRecord>(sql).ForEach(e => {
                IndexLotteryInfoResDto newModel = new IndexLotteryInfoResDto();
                newModel.OpenNum = e.Num;
                newModel.Issue = e.Issue;
                newModel.OpenTime = e.ShowOpenTime;
                newModel.OpenNumAlias = "";
                newModel.CurrentIssue = "";
                newModel.LType = e.lType;
                newModel.LTypeName = Util.GetLotteryTypeName(e.lType);
                newModel.Logo = Util.GetLotteryIconUrl(e.lType);
                newList.Add(newModel);
            });

            return newList;
        }
    }
}
