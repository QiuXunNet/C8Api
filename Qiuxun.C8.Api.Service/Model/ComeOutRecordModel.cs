using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Model;

namespace Qiuxun.C8.Api.Model
{
    public class ComeOutRecordModel : ComeOutRecord
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
