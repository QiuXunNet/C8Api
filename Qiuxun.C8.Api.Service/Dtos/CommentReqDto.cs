﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 评论请求参数类
    /// </summary>
    public class CommentReqDto
    {
        /// <summary>
        /// 评论对象的Id，[彩种Id,文章Id，评论Id]
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 评论类型 1=一级评论 2=回复
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 类型 1=计划 2=文章
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 被评论相关用户Id
        /// </summary>
        public int RefUserId { get; set; } = 0;
        /// <summary>
        /// 评论图片
        /// </summary>
        public List<long> Pictures { get; set; }
    }
}
