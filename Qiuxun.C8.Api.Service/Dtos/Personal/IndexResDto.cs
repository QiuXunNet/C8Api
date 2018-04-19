using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// 个人中心首页数据实体
    /// </summary>
    public class IndexResDto
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public  string Headpath { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Autograph { get; set; }

        /// <summary>
        /// 金币
        /// </summary>
        public int Coin { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 关注
        /// </summary>
        public int Follow { get; set; }

        /// <summary>
        /// 粉丝
        /// </summary>
        public int Fans { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int NoticeCount { get; set; }
    }
}
