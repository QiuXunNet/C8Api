using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Model
{
    public class Advertisement
    {
        public List<string> ThumbList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 广告类型 1-栏目广告 2-文章广告
        /// </summary>
        public int AdType { get; set; }
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
        /// 有效期开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 0=有结束时间广告 1=无结束时间广告 2=已禁用 3=已删除
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SubTime { get; set; }
        /// <summary>
        /// 广告主
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 广告设备 (1,2,3)逗号分隔 1=网页 2=IOS 3=安卓
        /// </summary>
        public string Where { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TransferUrl { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentsNumber { get; set; }
        /// <summary>
        /// 屏蔽区域
        /// </summary>
        public string RestrictedAreas { get; set; }
    }
}
