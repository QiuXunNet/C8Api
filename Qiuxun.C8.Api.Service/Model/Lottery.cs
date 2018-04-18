using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 版 本 1.0
    /// Copyright (c) 2018 
    /// 创建人：LHS
    /// 日 期：2018年3月8日
    /// 描 述：彩种实体
    /// </summary>	
    public class Lottery
    {

        /// <summary>
        /// 彩种ID
        /// </summary>		
        public long Id { get; set; }
        /// <summary>
        /// 彩种编码
        /// </summary>		
        public int LotteryCode { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>		
        public string LotteryName { get; set; }
        /// <summary>
        /// 彩种分类Id
        /// </summary>		
        public long LType { get; set; }
        /// <summary>
        /// 彩种图标路径
        /// </summary>		
        public string LotteryIcon { get; set; }
        /// <summary>
        /// 排序编码
        /// </summary>		
        public int SortCode { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>		
        public bool Enable { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>		
        public bool IsDeleted { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        public string SeoSubject { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public string SeoKeyword { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        public string SeoDescription { get; set; }
        /// <summary>
        /// 描述
        /// </summary>		
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

    }
}
