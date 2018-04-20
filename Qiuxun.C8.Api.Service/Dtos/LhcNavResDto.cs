using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 导航响应类
    /// </summary>
    public class NavigationResDto
    {
        /// <summary>
        /// Logo地址
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 导航名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 连接Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 类型 1=开奖记录 2=新闻 3=开奖时间
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 关联Id
        /// </summary>
        public int Id { get; set; }
    }
}
