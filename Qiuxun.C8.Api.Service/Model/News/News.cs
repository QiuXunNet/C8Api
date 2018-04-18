using System;
using System.Collections.Generic;

namespace Qiuxun.C8.Api.Model.News
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描述：新闻实体类
    /// </summary>
    public class News
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
        /// 所属类别
        /// </summary>		
        public string Category { get; set; }
        /// <summary>
        /// 完整标题
        /// </summary>		
        public string FullHead { get; set; }
        /// <summary>
        /// 标题颜色
        /// </summary>		
        public string FullHeadColor { get; set; }
        /// <summary>
        /// 简略标题
        /// </summary>		
        public string BriefHead { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>		
        public string Thumb { get; set; }
        /// <summary>
        /// 作者
        /// </summary>		
        public string AuthorName { get; set; }
        /// <summary>
        /// 编辑
        /// </summary>		
        public string CompileName { get; set; }
        /// <summary>
        /// Tag词
        /// </summary>		
        public string TagWord { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>		
        public string Keyword { get; set; }
        /// <summary>
        /// 来源
        /// </summary>		
        public string SourceName { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>		
        public string SourceAddress { get; set; }
        /// <summary>
        /// 新闻内容
        /// </summary>		
        public string NewsContent { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>		
        public int PV { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>		
        public DateTime ReleaseTime { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        public int EnabledMark { get; set; }
        /// <summary>
        /// 推荐标记
        /// </summary>		
        public bool RecommendMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }

        public string ReleaseTimeStr
        {
            get { return ReleaseTime.ToString("MM-dd HH:mm"); }
        }
        /// <summary>
        /// 缩率图样式 0=无图 1=1张小图 2=1张大图 3=大于1张小图 
        /// </summary>
        public int ThumbStyle { get; set; }

        public List<string> ThumbList { get; set; }
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
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
    }
}
