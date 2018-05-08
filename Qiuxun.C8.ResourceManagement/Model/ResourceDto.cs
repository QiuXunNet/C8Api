using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.ResourceManagement.Model
{
    public class ResourceDto
    {
        public long Id { get; set; }
        public string Extension { get; set; }

        public string RPath { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public int Type { get; set; }

        public long FkId { get; set; }

        public int RSize { get; set; }

        public DateTime SubTime { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}
