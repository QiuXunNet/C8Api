using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 评论响应类
    /// </summary>
    public class CommentResDto
    {
        /// <summary>
		/// 评论ID
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>		
        public string Content { get; set; }
        /// <summary>
        /// 上级评论ID
        /// </summary>		
        public long PId { get; set; }
        /// <summary>
        /// 新闻ID
        /// </summary>		
        public int ArticleId { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>		
        public int StarCount { get; set; }
        /// <summary>
        /// 评论人Id
        /// </summary>		
        public int UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime SubTime { get; set; }
        /// <summary>
        /// 评论类型 1=计划 2=文章
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 当前用户是否点赞
        /// </summary>
        public bool IsLike { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avater { get; set; }
        /// <summary>
        /// 回复数量
        /// </summary>
        public int ReplayCount { get; set; }

        /// <summary>
        /// 关联评论Id ， 0=1级评论 other=关联的上级评论Id
        /// </summary>
        public int RefCommentId { get; set; }

        /// <summary>
        /// 关联评论对象
        /// </summary>
        public ParentCommentResDto ParentComment { get; set; }
        /// <summary>
        /// 评论图片Url
        /// </summary>
        public List<string> Pictures { get; set; }
    }

    /// <summary>
    /// 上级评论实体
    /// </summary>
    public class ParentCommentResDto
    {
        public long Id { get; set; }


        /// <summary>
        /// 评论人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>		
        public string Content { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
