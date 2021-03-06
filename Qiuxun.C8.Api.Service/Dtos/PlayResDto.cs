﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 玩法响应类
    /// </summary>
    public class PlayResDto
    {
        /// <summary>
        /// 玩法Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 玩法名称
        /// </summary>
        public string PlayName { get; set; }
    }
}
