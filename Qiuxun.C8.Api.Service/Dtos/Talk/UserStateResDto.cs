﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 用户状态返回数据实体
    /// </summary>
    public class UserStateResDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 聊天是是否拉黑 1：拉黑   0：正常
        /// </summary>
        public int ChatBlack { get; set; }

        /// <summary>
        /// 评论是否拉黑  1：拉黑   0：正常
        /// </summary>
        public int CommentBlack { get; set; }

        /// <summary>
        /// 聊天室是否禁言 1：禁言   0：正常
        /// </summary>
        public int ChatShut { get; set; }

        /// <summary>
        /// 评论是否禁言 1：禁言   0：正常
        /// </summary>
        public int CommentShut { get; set; }

        /// <summary>
        /// 聊天室禁言开始时间
        /// </summary>
        public DateTime? ChatShutBegin { get; set; }

        /// <summary>
        /// 聊天是禁言结束时间
        /// </summary>
        public DateTime? ChatShutEnd { get; set; }

        /// <summary>
        /// 评论禁言开始时间
        /// </summary>
        public DateTime? CommentShutBegin { get; set; }

        /// <summary>
        /// 评论禁言结束时间
        /// </summary>
        public DateTime? CommentShutEnd { get; set; }

        /// <summary>
        /// 是否是聊天室管理员
        /// </summary>
        public int? IsChatAD { get; set; }

        public int? IsInfoAD { get; set; }

        public int? IsPlanAD { get; set; }

        /// <summary>
        /// 积分最高的彩种名称
        /// </summary>
        public string MasterLottery { get; set; }
    }
}
