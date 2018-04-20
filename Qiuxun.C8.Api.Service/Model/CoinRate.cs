using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// CoinRate:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CoinRate
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Num
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Rate
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int GradeId
        {
            set;
            get;
        }

    }
}
