using System;

namespace Qiuxun.C8.Api.Model.News
{
    /// <summary>
    /// 评论点赞记录表
    /// </summary>
    public class LikeRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 评论Id
        /// </summary>
        public long CommentId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 评论类型 1=计划 2=文章
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态 1=点赞 2=取消点赞
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
