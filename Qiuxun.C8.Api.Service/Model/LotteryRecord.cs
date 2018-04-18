using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Model
{
    public partial class LotteryRecord
    {
        public int Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public string Num { get; set; }
        public System.DateTime SubTime { get; set; }


        //扩展属性
        public string ShowTypeName
        {
            get { return Util.GetLotteryTypeName(lType); }
        }

        public string ShowIconName
        {
            get { return Util.GetLotteryIcon(lType); }
        }

        public string ShowOpenTime
        {
            get { return Util.GetOpenRemainingTimeWithHour(this.lType); }
        }

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

                if (lType >= 9 && lType < 15)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              int.Parse(arr[4]);

                    string daxiao = sum >= 23 ? "大" : "小";
                    string danshuang = sum % 2 == 0 ? "双" : "单";

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[4]);
                    string longhu = "";

                    if (a > b)
                    {
                        longhu = "龙";
                    }
                    else if (a < b)
                    {
                        longhu = "虎";
                    }
                    else if (a == b)
                    {
                        longhu = "和";
                    }

                    result = sum + "," + danshuang + "," + daxiao + "," + longhu;

                }
                else if (lType == 5)
                {
                    result += Util.GetShengxiaoByDigit(int.Parse(arr[0])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[1])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[2])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[3])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[4])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[5])) + "," +
                              Util.GetShengxiaoByDigit(int.Parse(arr[6]));
                }
                else if (lType == 63 || lType == 64)
                {
                    int guanyahe = int.Parse(arr[0]) + int.Parse(arr[1]);
                    string danshuang = guanyahe % 2 == 0 ? "双" : "单";
                    string daxiao = guanyahe > 11 ? "大" : "小";

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[1]);
                    int c = int.Parse(arr[2]);
                    int d = int.Parse(arr[3]);
                    int e = int.Parse(arr[4]);
                    int f = int.Parse(arr[5]);
                    int g = int.Parse(arr[6]);
                    int h = int.Parse(arr[7]);
                    int i = int.Parse(arr[8]);
                    int j = int.Parse(arr[9]);

                    string longhu1 = a > j ? "龙" : "虎";
                    string longhu2 = b > i ? "龙" : "虎";
                    string longhu3 = c > h ? "龙" : "虎";
                    string longhu4 = d > g ? "龙" : "虎";
                    string longhu5 = e > f ? "龙" : "虎";

                    result = guanyahe + "," + danshuang + "," + daxiao + "," + longhu1 + "," + longhu2 + "," + longhu3 +
                             "," + longhu4 + "," + longhu5;

                }
                else if (lType >= 15 && lType < 38)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              +int.Parse(arr[4]);

                    string danshuang = sum % 2 == 0 ? "双" : "单";
                    string daxiao = sum >= 31 ? "大" : "小";

                    if (sum == 30)
                    {
                        daxiao = "和";
                    }

                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[4]);

                    string longhu = a > b ? "龙" : "虎";

                    result = sum + "," + danshuang + "," + daxiao + "," + longhu;
                }
                else if (lType >= 51 && lType < 60)
                {
                    int sum = int.Parse(arr[0]) + int.Parse(arr[1]) + int.Parse(arr[2]) + int.Parse(arr[3]) +
                              int.Parse(arr[4]) + int.Parse(arr[5]) + int.Parse(arr[6]) + int.Parse(arr[7]);


                    string danshuang = sum % 2 == 0 ? "双" : "单";
                    string daxiao = "";
                    if (sum >= 85 && sum <= 132)
                    {
                        daxiao = "大";
                    }
                    else if (sum >= 36 && sum <= 83)
                    {
                        daxiao = "小";
                    }
                    else if (sum == 84)
                    {
                        daxiao = "和";
                    }

                    string weishu = sum.ToString();

                    int wei = int.Parse(weishu.Substring(weishu.Length - 1, 1));

                    string weidaxiao = wei >= 5 ? "尾大" : "尾小";


                    int a = int.Parse(arr[0]);
                    int b = int.Parse(arr[7]);

                    string longhu = a > b ? "龙" : "虎";

                    result = sum + "," + danshuang + "," + daxiao + "," + weidaxiao + "," + longhu;

                }

                return result;
            }
        }
    }
}
