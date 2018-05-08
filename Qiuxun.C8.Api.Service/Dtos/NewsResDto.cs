using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 新闻详情响应类
    /// </summary>
    public class NewsResDto
    {
        /// <summary>
        /// 新闻主键
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>		
        public long TypeId { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>		
        public string NewsTypeName { get; set; }
        /// <summary>
        /// 完整标题
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>		
        public string Thumb { get; set; }
        /// <summary>
        /// 新闻内容
        /// </summary>		
        public string Content { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>		
        public int PV { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 上一篇
        /// </summary>
        public PrevNewsInfo Previous { get; set; }
        /// <summary>
        /// 下一篇
        /// </summary>
        public NextNewsInfo Next { get; set; }

        public ShareDto Share { get; set; }
    }

    /// <summary>
    /// 上一篇
    /// </summary>
    public class PrevNewsInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
    /// <summary>
    /// 下一篇
    /// </summary>
    public class NextNewsInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// 新闻列表响应类
    /// </summary>
    public class NewsListResDto
    {
        /// <summary>
		/// 新闻主键
        /// </summary>		
        public long Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>		
        public long TypeId { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        public string ParentId { get; set; }

        /// <summary>
        /// 完整标题
        /// </summary>		
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime ReleaseTime { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// 缩率图样式 0=无图 1=1张小图 2=1张大图 3=大于1张小图 
        /// </summary>
        public int ThumbStyle { get; set; }

        public List<string> ThumbList { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
    }


    public class GalleryNewsListResDto : NewsListResDto
    {

        /// <summary>
        /// 完整期号
        /// </summary>
        public int LotteryNumber { get; set; }

        /// <summary>
        /// 当年期号
        /// </summary>
        public string Issue
        {
            get
            {
                string issue = LotteryNumber.ToString();
                if (issue.Length > 4)
                {
                    issue = issue.Substring(4, issue.Length - 4);
                }
                return issue;
            }
        }
    }

}
