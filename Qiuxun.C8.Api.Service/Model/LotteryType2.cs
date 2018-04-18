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
    
    public class LotteryType2
    {
        #region Model

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Position { get; set; }

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
