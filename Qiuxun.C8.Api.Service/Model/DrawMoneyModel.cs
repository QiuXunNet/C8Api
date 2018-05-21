using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    public class DrawMoneyModel
    {
        public decimal XfYj { get; set; }//消费佣金
        public decimal MyYj { get; set; }//我的佣金
        public decimal Txing { get; set; }//提现中
        public decimal Txleiji { get; set; }//提现累计

        public decimal KeTx { get; set; } //可提现
    }
}
