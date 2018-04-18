using System;

namespace Qiuxun.C8.Api.Model.News
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描 述：新闻评论实体
    /// </summary>
    public class Comment
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

        public string SubTimeStr {
            get { return SubTime.ToString("yyyy-MM-dd HH:mm"); }
        }
        /// <summary>
        /// 更改时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 评论类型 1=计划 2=文章
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 当前用户是否点赞
        /// </summary>
        public int CurrentUserLikes { get; set; }

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
        public Comment ParentComment { get; set; }

        /// <summary>
        /// 彩种类型名称
        /// </summary>
        public string LotteryTypeName { get; set; }
        

    }
}
