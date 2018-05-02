using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 优惠券类型
    /// </summary>
    public class Coupon
    {
        #region Model
        public int Id { get; set; }
        /// <summary>
        /// 优惠券编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 优惠券类型 0=全部查看券
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 有效期 单位天
        /// </summary>
        public int ExpiryDate { get; set; }
        /// <summary>
        /// 彩种 小彩种ID
        /// </summary>
        public int? lType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubTime { get; set; }
        #endregion Model
    }
}
