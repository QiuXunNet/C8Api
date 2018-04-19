using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    public class BetModel
    {
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 大类ID
        /// </summary>		
        public int Id { get; set; }
        /// <summary>
        /// 彩种编码
        /// </summary>		
        public int PId { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>		
        public int lType { get; set; }
        /// <summary>
        /// 彩种分类Id
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 彩种图标路径
        /// </summary>		
        public string LotteryIcon { get; set; }


    }
}
