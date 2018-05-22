using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Model
{
    public class ComeOutRecordModel
    {



        //针对消费记录

        /// <summary>
        /// 期号
        /// </summary>
        public string Issue { set; get; }
        /// <summary>
        /// 彩种图片
        /// </summary>
        public string LotteryIcon { get; set; }
        /// <summary>
        /// 彩种
        /// </summary>
        public int lType { get; set; }

        /// <summary>
        ///  打赏人、点阅人用户ID
        /// </summary>
        public int BUserId { get; set; }

        /// <summary>
        /// 打赏人、点阅人名字
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }


        public long Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 关联Id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 记录类型 1=充值 2=提现 3=点阅 4=点阅佣金 5=打赏 6=受邀奖励 7=邀请奖励 8=任务奖励 9=打赏佣金
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime { get; set; }
        /// <summary>
        /// 1微信 2支付宝 3银联
        /// </summary>
        public int? PayType { get; set; }

    }


    /// <summary>
    /// 打赏记录Model
    /// </summary>
    public class TipRecordModel : ComeOutRecord
    {
        /// <summary>
        /// 打赏人昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 打赏人头像
        /// </summary>
        public string Avater { get; set; }

        public string SubTimeStr
        {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
    }
}
