using System;

namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    /// 玩法
    /// </summary>
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
