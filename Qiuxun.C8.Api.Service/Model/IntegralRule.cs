using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// IntegralRule:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class IntegralRule
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
        public int lType
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlayName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int AddScore
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int JianScore
        {
            set;
            get;
        }

    }
}
