using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;

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
            string sql = "select * from LotteryType2 where PId = " + pid + " order by Position";
            return Util.ReaderToList<LotteryType2>(sql);
        }

        /// <summary>
        /// 获取玩法列表
        /// </summary>
        /// <param name="ltype"></param>
        /// <returns></returns>
        public ApiResult<List<PlayResDto>> GetPlayList(int? ltype)
        {
            string sql = "select Id,LType,PlayName from dbo.IntegralRule";
            if (ltype.HasValue)
            {
                sql += " where lType=" + ltype.Value;
            }

            var list = Util.ReaderToList<IntegralRule>(sql);

            var resDto = new List<PlayResDto>();

            list.ForEach(x =>
            {
                resDto.Add(new PlayResDto()
                {
                    Id = x.Id,
                    LType = x.lType,
                    PlayName = x.PlayName
                });
            });

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


            var resDto = new List<IndexLotteryInfoResDto>();


            lotteryList.ForEach(x =>
            {
                var info = new IndexLotteryInfoResDto()
                {
                    LType = x.lType,
                    LTypeName = Util.GetLotteryTypeName(x.lType),
                };

                var lastLotteryRecord = GetLotteryRecord(x.lType);
                if (lastLotteryRecord != null)
                {
                    info.OpenNum = lastLotteryRecord.Num;
                    info.Issue = lastLotteryRecord.Issue;
                    info.OpenTime = lastLotteryRecord.ShowOpenTime;
                }
                info.Logo = Util.GetLotteryIconUrl(x.lType);
                resDto.Add(info);
            });

            return new ApiResult<List<IndexLotteryInfoResDto>>()
            {
                Data = resDto
            };

        }

        public LotteryRecord GetLotteryRecord(int ltype)
        {
            string sql = "select top(1)* from LotteryRecord where lType = " + ltype + " order by Id desc";
            return Util.ReaderToModel<LotteryRecord>(sql);
        }
    }
}
