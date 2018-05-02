using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 用户优惠券表
    /// </summary>
    public class UserCoupon
    {
        #region Model

        public int Id { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 优惠券编号
        /// </summary>
        public string CouponCode { get; set; }
        /// <summary>
        /// 计划ID 已使用情况下 记录在哪个计划下使用
        /// </summary>
        public long PlanId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 来源 1=注册 2=邀请注册
        /// </summary>
        public int FromType { get; set; }
        /// <summary>
        /// 状态 1=未使用 2=已使用
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SubTime { get; set; }
        #endregion Model

    }
}
