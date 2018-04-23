using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 用户关注粉丝
    /// </summary>
    [Serializable]
    public partial class Follow
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            set;
            get;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 关注ID
        /// </summary>
        public long Followed_UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 是否关注  1、关注 0、取消关注 默认1
        /// </summary>
        public int Status
        {
            set;
            get;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname
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
        /// 头像
        /// </summary>
        public string Avater
        {
            set;
            get;
        }

        public DateTime Followtime
        {
            set;
            get;
        }

        public int Isfollowed
        {
            set;
            get;
        }

    }
}
