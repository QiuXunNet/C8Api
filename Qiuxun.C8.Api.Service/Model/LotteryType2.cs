using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// LotteryType2:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class LotteryType2
    {
        public LotteryType2()
        { }
        #region Model
        private int _id;
        private int _pid = 0;
        private int? _ltype;
        private string _name;
        private int? _position;

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
        public int PId
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? lType
        {
            set { _ltype = value; }
            get { return _ltype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Position
        {
            set { _position = value; }
            get { return _position; }
        }
     
        #endregion Model



        //------------扩展字段-------------------
        public int[] SonIdArr
        {
            get
            {
                if (this.PId == 0)
                {
                    string sql = "select lType from LotteryType2 where PId = " + this.Id + " order by Position";
                    List<LotteryType2> list = Util.ReaderToList<LotteryType2>(sql);
                    int[] idArr = new int[list.Count];
                    int count = 0;
                    foreach (LotteryType2 lotteryType2 in list)
                    {
                        idArr[count] = (int)lotteryType2.lType;
                        count ++;
                    }

                    return idArr;
                }

                return null;
            }
        }

    }
}
