using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    public class ExpertSearchModel
    {
        public int UserId { get; set; }
        public int lType { get; set; }
        public int isFollow { get; set; }
        public string Name { get; set; }
        public string Avater { get; set; }
    }
}
