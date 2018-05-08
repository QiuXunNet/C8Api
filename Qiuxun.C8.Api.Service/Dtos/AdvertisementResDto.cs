using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class AdvertisementResDto
    {

        /// <summary>
        /// 广告名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 层级 (用于确定显示位置：1-7)
        /// </summary>
        public int Layer { get; set; }
        /// <summary>
        /// 图片显示方式 0=无图 1=1张小图 2=1张大图 3=大于1张小图 4=纯图片广告
        /// </summary>
        public int ThumbStyle { get; set; }
        /// <summary>
        /// 广告主
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string TransferUrl { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentsNumber { get; set; }
        /// <summary>
        /// 广告图片
        /// </summary>
        public List<string> ThumbList { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime SubTime { get; set; }
    }
}
