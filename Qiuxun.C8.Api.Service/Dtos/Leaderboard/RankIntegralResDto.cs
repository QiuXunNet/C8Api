using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 积分榜信息
    /// </summary>
    public class RankIntegralResDto
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { set; get; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string NickName { set; get; }

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string Avater { set; get; }
    }
}
