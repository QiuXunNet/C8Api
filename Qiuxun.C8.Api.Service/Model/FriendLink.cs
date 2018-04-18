using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    public class FriendLink
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }

        public string TransferUrl { get; set; }

        public int State { get; set; }

        public DateTime SubTime { get; set; }
        
    }
}
