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
        /// 广告图片
        /// </summary>
        public List<string> ThumbList { get; set; }

        /// <summary>
        /// 位置(栏目ID 1,2,3 逗号分隔)
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 广告名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Layer { get; set; }
        /// <summary>
        /// 缩略图类型 0=无图 1=1张小图 2=1张大图 3=大于1张小图
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
    }
}
