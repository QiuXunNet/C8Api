using Qiuxun.C8.Api.Model;

namespace Qiuxun.C8.Api.Service.Model
{
    public class DynamicMessage : UserInternalMessage
    {
        /// <summary>
        /// 关联评论Id
        /// </summary>
        public int RefId { get; set; }
        /// <summary>
        /// 来源用户Id
        /// </summary>
        public int RefUserId { get; set; }
        /// <summary>
        /// 来源用户昵称
        /// </summary>
        public string RefNickName { get; set; }
        /// <summary>
        /// 评论人头像
        /// </summary>
        public string FromAvater { get; set; }
        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string FromNickName { get; set; }
        /// <summary>
        /// 评论人用户Id
        /// </summary>
        public string FromUserId { get; set; }
        /// <summary>
        /// 评论类型
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 评论上级Id
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 我的评论内容
        /// </summary>
        public string MyContent { get; set; }
        /// <summary>
        /// 评论关联Id （计划/文章/评论 Id）
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string LotteryTypeName { get; set; }
    }
}
