using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 友联访问记录
    /// </summary>
    public class LinkVisitRecord
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 友联表关联主键Id
        /// </summary>
        public int RefId { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int ClickCount { get; set; }
        /// <summary>
        /// UserView
        /// </summary>
        public int UV { get; set; }
        /// <summary>
        /// IP访问量
        /// </summary>
        public int IP { get; set; }
        /// <summary>
        /// PageView 
        /// </summary>
        public int PV { get; set; }
        /// <summary>
        /// 注册量
        /// </summary>
        public int RegCount { get; set; }
        /// <summary>
        /// 类型：1=进 2=出
        /// </summary>
        public int Type { get; set; }
    }
}
