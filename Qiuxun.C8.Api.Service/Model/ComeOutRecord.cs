using System;

namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    /// ComeOutRecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public partial class ComeOutRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            set;
            get;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 关联Id
        /// </summary>
        public string OrderId
        {
            set;
            get;
        }
        /// <summary>
        /// 记录类型 1=充值 2=提现 3=点阅 4=点阅佣金 5=打赏 6=受邀奖励 7=邀请奖励 8=任务奖励 9=打赏佣金
        /// </summary>
        public int Type
        {
            set;
            get;
        }
        /// <summary>
        /// 金额
        /// </summary>
        public int Money
        {
            set;
            get;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime
        {
            set;
            get;
        }
        /// <summary>
        /// 1微信 2支付宝 3银联
        /// </summary>
        public int? PayType
        {
            set;
            get;
        }

    }
}
