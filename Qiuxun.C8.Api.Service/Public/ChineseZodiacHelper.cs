using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Public
{

    public class ChineseZodiacHelper
    {
        public static string ChineseZodiacJsonData = "[{ 'BeginTime':'2015-02-19','EndTime':'2016-02-07','Data':[{'Name':'羊','Nums':'01,13,25,37,49'},{'Name':'马','Nums':'02,14,26,38'},{'Name':'蛇','Nums':'03,15,27,39'},{'Name':'龙','Nums':'04,16,28,40'},{'Name':'兔','Nums':'05,17,29,41'},{'Name':'虎','Nums':'06,18,30,42'},{'Name':'牛','Nums':'07,19,31,43'},{'Name':'鼠','Nums':'08,20,32,44'},{'Name':'猪','Nums':'09,21,33,45'},{'Name':'狗','Nums':'10,22,34,46'},{'Name':'鸡','Nums':'11,23,35,47'},{'Name':'猴','Nums':'12,24,36,48'}]},{'BeginTime':'2016-02-08','EndTime':'2017-01-27','Data':[{'Name':'猴','Nums':'01,13,25,37,49'},{'Name':'羊','Nums':'02,14,26,38'},{'Name':'马','Nums':'03,15,27,39'},{'Name':'蛇','Nums':'04,16,28,40'},{'Name':'龙','Nums':'05,17,29,41'},{'Name':'兔','Nums':'06,18,30,42'},{'Name':'虎','Nums':'07,19,31,43'},{'Name':'牛','Nums':'08,20,32,44'},{'Name':'鼠','Nums':'09,21,33,45'},{'Name':'猪','Nums':'10,22,34,46'},{'Name':'狗','Nums':'11,23,35,47'},{'Name':'鸡','Nums':'12,24,36,48'}]},{'BeginTime':'2017-01-28','EndTime':'2018-02-15','Data':[{'Name':'鸡','Nums':'01,13,25,37,49'},{'Name':'猴','Nums':'02,14,26,38'},{'Name':'羊','Nums':'03,15,27,39'},{'Name':'马','Nums':'04,16,28,40'},{'Name':'蛇','Nums':'05,17,29,41'},{'Name':'龙','Nums':'06,18,30,42'},{'Name':'兔','Nums':'07,19,31,43'},{'Name':'虎','Nums':'08,20,32,44'},{'Name':'牛','Nums':'09,21,33,45'},{'Name':'鼠','Nums':'10,22,34,46'},{'Name':'猪','Nums':'11,23,35,47'},{'Name':'狗','Nums':'12,24,36,48'}]},{'BeginTime':'2018-02-16','EndTime':'2019-02-04','Data':[{'Name':'狗','Nums':'01,13,25,37,49'},{'Name':'鸡','Nums':'02,14,26,38'},{'Name':'猴','Nums':'03,15,27,39'},{'Name':'羊','Nums':'04,16,28,40'},{'Name':'马','Nums':'05,17,29,41'},{'Name':'蛇','Nums':'06,18,30,42'},{'Name':'龙','Nums':'07,19,31,43'},{'Name':'兔','Nums':'08,20,32,44'},{'Name':'虎','Nums':'09,21,33,45'},{'Name':'牛','Nums':'10,22,34,46'},{'Name':'鼠','Nums':'11,23,35,47'},{'Name':'猪','Nums':'12,24,36,48'}]},{'BeginTime':'2019-02-05','EndTime':'2020-01-24','Data':[{'Name':'猪','Nums':'01,13,25,37,49'},{'Name':'狗','Nums':'02,14,26,38'},{'Name':'鸡','Nums':'03,15,27,39'},{'Name':'猴','Nums':'04,16,28,40'},{'Name':'羊','Nums':'05,17,29,41'},{'Name':'马','Nums':'06,18,30,42'},{'Name':'蛇','Nums':'07,19,31,43'},{'Name':'龙','Nums':'08,20,32,44'},{'Name':'兔','Nums':'09,21,33,45'},{'Name':'虎','Nums':'10,22,34,46'},{'Name':'牛','Nums':'11,23,35,47'},{'Name':'鼠','Nums':'12,24,36,48'}]},{'BeginTime':'2020-01-25','EndTime':'2021-02-11','Data':[{'Name':'鼠','Nums':'01,13,25,37,49'},{'Name':'猪','Nums':'02,14,26,38'},{'Name':'狗','Nums':'03,15,27,39'},{'Name':'鸡','Nums':'04,16,28,40'},{'Name':'猴','Nums':'05,17,29,41'},{'Name':'羊','Nums':'06,18,30,42'},{'Name':'马','Nums':'07,19,31,43'},{'Name':'蛇','Nums':'08,20,32,44'},{'Name':'龙','Nums':'09,21,33,45'},{'Name':'兔','Nums':'10,22,34,46'},{'Name':'虎','Nums':'11,23,35,47'},{'Name':'牛','Nums':'12,24,36,48'}]},{'BeginTime':'2021-02-12','EndTime':'2022-01-31','Data':[{'Name':'牛','Nums':'01,13,25,37,49'},{'Name':'鼠','Nums':'02,14,26,38'},{'Name':'猪','Nums':'03,15,27,39'},{'Name':'狗','Nums':'04,16,28,40'},{'Name':'鸡','Nums':'05,17,29,41'},{'Name':'猴','Nums':'06,18,30,42'},{'Name':'羊','Nums':'07,19,31,43'},{'Name':'马','Nums':'08,20,32,44'},{'Name':'蛇','Nums':'09,21,33,45'},{'Name':'龙','Nums':'10,22,34,46'},{'Name':'兔','Nums':'11,23,35,47'},{'Name':'虎','Nums':'12,24,36,48'}]},{'BeginTime':'2022-02-01','EndTime':'2023-01-21','Data':[{'Name':'虎','Nums':'01,13,25,37,49'},{'Name':'牛','Nums':'02,14,26,38'},{'Name':'鼠','Nums':'03,15,27,39'},{'Name':'猪','Nums':'04,16,28,40'},{'Name':'狗','Nums':'05,17,29,41'},{'Name':'鸡','Nums':'06,18,30,42'},{'Name':'猴','Nums':'07,19,31,43'},{'Name':'羊','Nums':'08,20,32,44'},{'Name':'马','Nums':'09,21,33,45'},{'Name':'蛇','Nums':'10,22,34,46'},{'Name':'龙','Nums':'11,23,35,47'},{'Name':'兔','Nums':'12,24,36,48'}]}]";
        //public static List<LunarCalendar<ChineseZodiac>> list = JsonHelper.JsonToObject<List<LunarCalendar<ChineseZodiac>>>(ChineseZodiacJsonData);
        public static List<LunarCalendar<ChineseZodiac>> list = JsonExtension.FromJsonString<List<LunarCalendar<ChineseZodiac>>>(ChineseZodiacJsonData);
        public static string GetChineseZodiac(DateTime date, int number)
        {
            var dateModel = list.Where(p => (date >= p.BeginTime && date < p.EndTime.AddDays(1))).FirstOrDefault();
            if (dateModel != null)
            {
                string tempnumber = number.ToString().PadLeft(2, '0');
                var ChineseZodiacModel = dateModel.Data.Where(p => p.Nums.Contains(tempnumber)).FirstOrDefault();
                if (ChineseZodiacModel != null) return ChineseZodiacModel.Name;
            }
            return "";
        }
    }

    public class LunarCalendar<T>
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<T> Data { get; set; }
    }

    public class ChineseZodiac
    {
        public string Name { get; set; }
        public string Nums { get; set; }
    }
}
