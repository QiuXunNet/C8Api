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
        public IntegralRule()
        { }
        #region Model
        private int _id;
        private int _ltype;
        private string _playname;
        private int _addscore;
        private int _jianscore;
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
        public int lType
        {
            set { _ltype = value; }
            get { return _ltype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlayName
        {
            set { _playname = value; }
            get { return _playname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AddScore
        {
            set { _addscore = value; }
            get { return _addscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int JianScore
        {
            set { _jianscore = value; }
            get { return _jianscore; }
        }
        #endregion Model

    }
}
