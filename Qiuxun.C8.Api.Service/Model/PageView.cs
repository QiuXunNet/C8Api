using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Model
{
    public class PageView
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime ViewDate{get;set;}

        /// <summary>
        /// 点击量
        /// </summary>
        public int ViewTotal { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 外键Id
        /// </summary>
        public int FkId { get; set; }
    }
}
