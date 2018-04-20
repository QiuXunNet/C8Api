using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Model
{
    public class ResouceMapping
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 资源路径
        /// </summary>
        public string RPath { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 资源类型 1=文章资源 2=用户头像
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 资源关联外键
        /// </summary>
        public long FkId { get; set; }
        /// <summary>
        /// 资源大小
        /// </summary>
        public int RSize { get; set; }
    }
}
