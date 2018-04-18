using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 玩法
    /// </summary>
    [Serializable]
    public class Play
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 玩法名称
        /// </summary>
        public string PlayName { get; set; }
    }
}
