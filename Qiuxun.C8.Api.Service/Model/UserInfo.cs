using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：KCP
    /// 日 期：2018年3月20日
    /// 描 述：用户信息类
    /// </summary>	
    [Serializable]
    public partial class UserInfo
    {
        public UserInfo()
        { }
        #region Model
        private long _id;
        private string _username;
        private string _name;
        private string _password;
        private string _mobile;
        private int _coin;
        private decimal _money = 0M;
        private int _integral = 0;
        private DateTime _subtime;
        private DateTime _lastlogintime;
        private int _state;
        private int? _sex = 0;
        private string _autograph;
        private string _headpath;//头像地址 type=2
        private int? _pid = 0;//受邀ID 
        private int follow;//关注
        private int fans;//粉丝
        private string _registerip;//注册IP
        private string _lastloginip;//最后登录IP

        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Coin
        {
            set { _coin = value; }
            get { return _coin; }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money
        {
            set { _money = value; }
            get { return _money; }
        }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral
        {
            set { _integral = value; }
            get { return _integral; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime SubTime
        {
            set { _subtime = value; }
            get { return _subtime; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }
        /// <summary>
        /// 状态 0启用 1禁用
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
		/// 性别
		/// </summary>
		public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Autograph
        {
            set { _autograph = value; }
            get { return _autograph; }
        }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Headpath
        {
            get
            {
                return _headpath;
            }

            set
            {
                _headpath = value;
            }
        }

        public int? Pid
        {
            get
            {
                return _pid;
            }

            set
            {
                _pid = value;
            }
        }

        public int Follow
        {
            get
            {
                return follow;
            }

            set
            {
                follow = value;
            }
        }

        public int Fans
        {
            get
            {
                return fans;
            }

            set
            {
                fans = value;
            }
        }

        public string Registerip
        {
            get
            {
                return _registerip;
            }

            set
            {
                _registerip = value;
            }
        }

        public string Lastloginip
        {
            get
            {
                return _lastloginip;
            }

            set
            {
                _lastloginip = value;
            }
        }
    }
    #endregion Model
}
