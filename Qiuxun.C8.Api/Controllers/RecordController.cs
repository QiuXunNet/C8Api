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
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">默认20[不必填]</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<PagedList<LotteryRecordRseDto>> GetRecordList(int lType,int pageIndex = 1,int pageSize=20)
        {
            var totalCount = 0;
            var result = new ApiResult<PagedList<LotteryRecordRseDto>>();

            if (lType >= 9)
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");

                var beginDate = date;
                var endDate = date + " 23:59:59";

                var list = _service.GetRecordList(ref totalCount,lType, pageIndex, pageSize, beginDate, endDate);

                var pager = new PagedList<LotteryRecordRseDto>(list,pageIndex,pageSize,totalCount);
                result.Data = pager;
            }
            else
            {
                int year = DateTime.Now.Year;
                var beginDate = year + "-1-1";
                var list = _service.GetRecordList(ref totalCount,lType, pageIndex, pageSize, beginDate);

                var pager = new PagedList<LotteryRecordRseDto>(list, pageIndex, pageSize, totalCount);
                result.Data = pager;
            }

            return result;
        }

        /// <summary>
        /// 当彩种类型大于等于9时，需要获取查询的时间列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<dynamic>> GetQueryDate()
        {
            var strs = Util.GetQueryDate();

            var result = new ApiResult<List<dynamic>>();
            List<dynamic> list = new List<dynamic>();

            foreach (var s in strs.Split(','))
            {
                list.Add(new { Date = s });
            }
            result.Data = list;

            return result;
        }

        /// <summary>
        /// 根据彩种类型获取彩种名称
        /// </summary>
        /// <param name="lType">彩种类型</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<string> GetLotteryTypeName(int lType)
        {
            var result = new ApiResult<string>();
            result.Data = Util.GetLotteryTypeName((int)lType);
            return result;
        }
    }
}
