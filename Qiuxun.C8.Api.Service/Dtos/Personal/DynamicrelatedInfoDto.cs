using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// 动态关联信息
    /// </summary>
    public class DynamicRelatedInfoDto
    {
        /// <summary>
        /// 关联Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 彩种Id
        /// </summary>
        public int LType { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 关联类型 1=计划 2=资讯
        /// </summary>
        public int RelatedType { get; set; }
    }
}
