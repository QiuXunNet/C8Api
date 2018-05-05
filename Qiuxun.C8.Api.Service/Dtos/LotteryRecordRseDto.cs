using Qiuxun.C8.Api.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 开间记录实体
    /// </summary>
    public class LotteryRecordRseDto
    {
        /// <summary>
        /// 彩种类型
        /// </summary>
        public int lType { get; set; }
        /// <summary>
        /// 期数
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime SubTime { get; set; }

        /// <summary>
        /// 球号
        /// </summary>
        public string Num { get; set; }

        public string ShowIssue
        {
            get
            {
                string result = "";
                int length = Issue.Length;

                if (lType == 10 || lType == 63 || lType == 65)
                {
                    result = Issue;
                }
                else if ((lType >= 9 && lType <= 14) || (lType >= 38 && lType <= 62) || lType == 64 || lType < 9)
                {
                    result = Issue.Substring(length - 3);
                }
                else if (lType >= 15 && lType <= 37)
                {
                    result = Issue.Substring(length - 2);
                }

                return result + "期";
            }
        }

        //开奖下面一排的信息
        public string ShowInfo
        {
            get
            {
                string result = "";
                string[] arr = this.Num.Split(',');

                if (lType == 5)  //只保留六合彩的生肖转换
                {
                    result += Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[6]));
                }              

                return result;
            }
        }
    }
}
