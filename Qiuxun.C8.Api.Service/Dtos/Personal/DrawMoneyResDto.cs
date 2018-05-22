using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    public class DrawMoneyResDto
    {
        /// <summary>
        /// 我的佣金
        /// </summary>
        public decimal MyYj { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal Txing { get; set; }
        /// <summary>
        /// 累计提现
        /// </summary>
        public decimal Txleiji { get; set; }
        /// <summary>
        /// 可提现
        /// </summary>
        public decimal KeTx { get; set; }
    }
}
