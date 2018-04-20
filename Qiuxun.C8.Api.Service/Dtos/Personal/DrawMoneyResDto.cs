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
        public string MyYj { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public string Txing { get; set; }
        /// <summary>
        /// 累计提现
        /// </summary>
        public string Txleiji { get; set; }
        /// <summary>
        /// 可提现
        /// </summary>
        public string KeTx { get; set; }
    }
}
