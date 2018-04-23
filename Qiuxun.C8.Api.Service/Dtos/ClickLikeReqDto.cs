using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 点赞请求参数类
    /// </summary>
    public class ClickLikeReqDto
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 类型 1=计划 =2文章
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 操作类型 1=点赞 2=取消
        /// </summary>
        public int OperationType { get; set; }
    }
}
