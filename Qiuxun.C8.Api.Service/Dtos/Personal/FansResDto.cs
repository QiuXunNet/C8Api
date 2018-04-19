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
        public int Followed_UserId { get; set; }//关注ID
        public string Name { get; set; }//昵称
        public string HeadPath { get; set; }//头像
    }
}
