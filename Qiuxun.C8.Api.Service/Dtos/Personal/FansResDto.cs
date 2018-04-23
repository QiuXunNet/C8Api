using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    public class FansResDto
    {
        public int Number { get; set; }//粉丝人数
        public int Rank { get; set; }//排名
        public int FollowedUserId { get; set; }//关注ID
        public string NickName { get; set; }//昵称
        public string Avater { get; set; }//头像
    }
}
