﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// 个人中心用户数据实体
    /// </summary>
    public class IndexResDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id
        {
            set;
            get;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            set;
            get;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            set;
            get;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set;
            get;
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set;
            get;
        }
        /// <summary>
        /// 金币
        /// </summary>
        public int Coin
        {
            set;
            get;
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Money
        {
            set;
            get;
        }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral
        {
            set;
            get;
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime SubTime
        {
            set;
            get;
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            set;
            get;
        }
        /// <summary>
        /// 状态 0启用 1禁用
        /// </summary>
        public int State
        {
            set;
            get;
        }
        /// <summary>
		/// 性别
		/// </summary>
		public int? Sex
        {
            set;
            get;
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Autograph
        {
            set;
            get;
        }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avater
        {
            set;
            get;
        }

        public int? Pid
        {
            set;
            get;
        }

        public int Follow
        {
            set;
            get;
        }

        public int Fans
        {
            set;
            get;
        }

        public string Registerip
        {
            set;
            get;
        }

        public string Lastloginip
        {
            set;
            get;
        }

        public int NoticeCount { get; set; }

        /// <summary>
        /// 卡券数量
        /// </summary>
        public int UserCoupon { get; set; }
    }



}
