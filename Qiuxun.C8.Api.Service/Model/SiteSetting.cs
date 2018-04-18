using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 站点设置
    /// Author:LHS
    /// Date:2018年4月5日
    /// </summary>
    [Serializable]
    public class SiteSetting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string KeyWord { get; set; }

        public string Description { get; set; }

        public string LogoPath { get; set; }
    }
}
