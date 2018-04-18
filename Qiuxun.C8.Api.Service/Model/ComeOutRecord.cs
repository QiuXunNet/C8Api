using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// ComeOutRecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ComeOutRecord
    {
        public ComeOutRecord()
        { }
        #region Model
        private long _id;
        private int _userid;
        private string _orderid;
        private int _type;
        private int _money;
        private int _state = 1;
        private DateTime _subtime;
        private int? _paytype;
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 关联Id
        /// </summary>
        public string OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 记录类型 1=充值 2=提现 3=点阅 4=点阅佣金 5=打赏 6=受邀奖励 7=邀请奖励 8=任务奖励 9=打赏佣金
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public int Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        /// <summary>
        /// 1微信 2支付宝 3银联
        /// </summary>
        public int? PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        #endregion Model

    }
}
