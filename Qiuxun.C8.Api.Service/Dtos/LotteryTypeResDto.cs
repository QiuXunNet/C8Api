using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class LotteryTypeResDto
    {
        /// <summary>
		/// 类型Id
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 对应具体彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 彩种类型ID
        /// </summary>		
        public string LTypeName { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>		
        public int SortCode { get; set; }
    }
}
