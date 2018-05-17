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
    /// 彩种历史记录接口控制器
    /// </summary>
    public class RecordController : QiuxunApiController
    {
        private RecordService _service = new RecordService();

        /// <summary>
        /// 根据彩种类型获取彩种记录
        /// </summary>
        /// <param name="lType">彩种类型</param>
        /// <param name="date">查询日期(传查询时间列表的Key)</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">默认20[不必填]</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiResult<PagedList<LotteryRecordRseDto>> GetRecordList(int lType, string date = "", int pageIndex = 1, int pageSize = 20)
        {
            var totalCount = 0;
            var result = new ApiResult<PagedList<LotteryRecordRseDto>>();

            if (lType >= 9)
            {
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }

                var beginDate = date;
                var endDate = date + " 23:59:59";

                var list = _service.GetRecordList(ref totalCount, lType, pageIndex, pageSize, beginDate, endDate);

                var pager = new PagedList<LotteryRecordRseDto>(list, pageIndex, pageSize, totalCount);
                result.Data = pager;
            }
            else
            {
                if (string.IsNullOrEmpty(date))
                {
                    date = DateTime.Now.ToString("yyyy-01-01");
                }

                var beginDate = date;
                var endDate = Convert.ToDateTime(beginDate).AddYears(1).ToString("yyyy-01-01");
                var list = _service.GetRecordList(ref totalCount, lType, pageIndex, pageSize, beginDate, endDate);

                var pager = new PagedList<LotteryRecordRseDto>(list, pageIndex, pageSize, totalCount);
                result.Data = pager;
            }

            return result;
        }

        /// <summary>
        /// 获取查询的时间列表
        /// </summary>
        /// <param name="lType">彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiResult<List<dynamic>> GetQueryDate(int lType)
        {
            var list = Util.GetQueryDate(lType);

            var result = new ApiResult<List<dynamic>>();
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 根据彩种类型获取彩种名称
        /// </summary>
        /// <param name="lType">彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiResult<string> GetLotteryTypeName(int lType)
        {
            var result = new ApiResult<string>();
            result.Data = Util.GetLotteryTypeName((int)lType);
            return result;
        }
    }
}
