using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 帖子信息
    /// </summary>
    public class BettingRecordViewModel : BettingRecord
    {
        /// <summary>
        /// 是否阅读过该帖子
        /// </summary>
        public bool IsRead { get; set; }
    }
}
