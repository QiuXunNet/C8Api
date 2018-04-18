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
        public CoinRate()
        { }
        #region Model
        private int _id;
        private int _num;
        private decimal _rate;
        private int _gradeid;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Rate
        {
            set { _rate = value; }
            get { return _rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GradeId
        {
            set { _gradeid = value; }
            get { return _gradeid; }
        }
        #endregion Model

    }
}
