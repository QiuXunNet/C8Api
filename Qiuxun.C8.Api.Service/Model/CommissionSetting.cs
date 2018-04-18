using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 分成配置
    /// Author:LHS
    /// Date:2018年4月5日
    /// </summary>
    [Serializable]
    public class CommissionSetting
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 用户分成百分比
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
    }
}
