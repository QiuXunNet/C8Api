using Newtonsoft.Json;
using Qiuxun.C8.Api.Public;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    public class ToolsHelper
    {
        #region 类型转换、字符串操作
        /// <summary>
        /// 字符串 转换 char数组
        /// </summary>
        /// <param name="in_str"></param>
        /// <param name="in_len"></param>
        /// <returns></returns>
        public static char[] string2chararray(string in_str, int in_len)
        {
            char[] ch = new char[in_len];
            //in_str.ToCharArray().CopyTo(ch, 0);
            return ch;
        }

        /// <summary>
        /// char数组 转换 字符串
        /// </summary>
        /// <param name="in_str"></param>
        /// <returns></returns>        
        public static string chararray2string(char[] in_str)
        {
            string out_str;
            out_str = new string(in_str);
            int i = out_str.IndexOf('\0', 0);
            if (i == -1)
                i = 16;
            return out_str.Substring(0, i);
        }

        /// <summary>
        /// byte数组 转换 字符串
        /// </summary>
        /// <param name="in_str"></param>
        /// <returns></returns>
        public static string bytearray2string(byte[] in_str)
        {
            string out_str;
            out_str = System.Text.Encoding.Default.GetString(in_str);
            return out_str.Substring(0, out_str.IndexOf('\0', 0));

        }

        /// <summary>
        /// 字符串 转换 byte数组  注意转换出来会使原来的bytearray长度变短
        /// </summary>
        /// <param name="in_str"></param>
        /// <returns></returns>
        public static byte[] string2bytearray(string in_str)
        {
            return System.Text.Encoding.Default.GetBytes(in_str);
        }

        /// <summary>
        /// 字符串 转换 byte数组  长度为传如的长度
        /// </summary>
        /// <param name="in_str">传入字符串</param>
        /// <param name="iLen">目标字节数组长度</param>
        /// <returns></returns>
        public static byte[] string2bytearray(string in_str, int iLen)
        {
            byte[] bytes = new byte[iLen];
            byte[] bsources = System.Text.Encoding.Default.GetBytes(in_str);
            Array.Copy(bsources, bytes, bsources.Length);


            return bytes;
        }

        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encode(string str)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decode(string str)
        {
            byte[] barray;
            try
            {
                barray = Convert.FromBase64String(str);
                return Encoding.Default.GetString(barray);
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// 图片 转换 byte数组
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static byte[] image_Image2Byte(Image pic, System.Drawing.Imaging.ImageFormat fmt)
        {
            MemoryStream mem = new MemoryStream();
            pic.Save(mem, fmt);
            mem.Flush();
            return mem.ToArray();
        }
        /// <summary>
        /// byte数组 转换 图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image image_Byte2Image(byte[] bytes)
        {
            MemoryStream mem = new MemoryStream(bytes, true);
            mem.Read(bytes, 0, bytes.Length);
            mem.Flush();
            Image aa = Image.FromStream(mem);
            return aa;
        }

        /// <summary>
        /// ip 转换 长整形
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static long IP2Long(string strIP)
        {

            long[] ip = new long[4];

            string[] s = strIP.Split('.');
            ip[0] = long.Parse(s[0]);
            ip[1] = long.Parse(s[1]);
            ip[2] = long.Parse(s[2]);
            ip[3] = long.Parse(s[3]);

            return (ip[0] << 24) + (ip[1] << 16) + (ip[2] << 8) + ip[3];
        }

        /// <summary>
        /// 长整形 转换 IP
        /// </summary>
        /// <param name="longIP"></param>
        /// <returns></returns>
        public static string Long2IP(long longIP)
        {


            StringBuilder sb = new StringBuilder("");
            sb.Append(longIP >> 24);
            sb.Append(".");

            //将高8位置0，然后右移16为


            sb.Append((longIP & 0x00FFFFFF) >> 16);
            sb.Append(".");


            sb.Append((longIP & 0x0000FFFF) >> 8);
            sb.Append(".");

            sb.Append((longIP & 0x000000FF));


            return sb.ToString();
        }

        /// <summary>
        /// 将8位日期型整型数据转换为日期字符串数据
        /// </summary>
        /// <param name="date">整型日期</param>
        /// <param name="chnType">是否以中文年月日输出</param>
        /// <returns></returns>
        public static string FormatDate(int date, bool chnType)
        {
            string dateStr = date.ToString();

            if (date <= 0 || dateStr.Length != 8)
                return dateStr;

            if (chnType)
                return dateStr.Substring(0, 4) + "年" + dateStr.Substring(4, 2) + "月" + dateStr.Substring(6) + "日";

            return dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" + dateStr.Substring(6);
        }


        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型,转换失败返回0
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str)
        {
            return StrToInt(str, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(str, defValue));
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
                return defValue;

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object strValue)
        {
            return ObjectToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue)
        {
            if ((strValue == null))
                return 0;

            return StrToFloat(strValue.ToString(), 0);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(strValue, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }
        #endregion

        #region Sql
        /// <summary>
        /// 分页查询
        /// </summary>
        public static List<T> QueryPage<T>(int pageIndex, int pageSize,string totalsql, string pagesql, string orderby, string sort, ref int total, bool hasTotal = false, bool hasPage = false, params SqlParameter[] paramters)
        {

            string totalquerysql = "";
            string pagequerysql = "";

            int start = pageSize * (pageIndex - 1) + 1;
            int end = pageSize * pageIndex;
            if (!hasTotal)
            {
                totalquerysql = string.Format("SELECT COUNT(1) FROM ({0}) temp", pagesql);
            }
            else
            {
                totalquerysql = totalsql;
            }
            total = ObjectToInt(SqlHelper.ExecuteScalar(totalquerysql, paramters));
            if (!hasPage)
            {
                pagequerysql = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {3} {4}) AS rownumber,* FROM ({0}) temp) t WHERE t.rownumber >= {1} AND t.rownumber <= {2}", pagesql, start, end, orderby, sort);
            }
            else
            {
                pagequerysql = pagesql;
            }
            return Util.ReaderToList<T>(pagequerysql, paramters);
        }
        #endregion

        #region DataTable转Array
        public static ArrayList Dtb2JsonDic(DataTable dtb)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            foreach (DataRow row in dtb.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    drow.Add(col.ColumnName, row[col.ColumnName]);
                }
                dic.Add(drow);
            }
            return dic;
        }
        #endregion
    }

    public class PagedListP<T>
    {
        public PagedListP()
        {

        }

        public PagedListP(int pageIndex, int pageSize, int totalCount, List<T> pageData)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.totalCount = totalCount;
            this.PageData = pageData;
        }
        private int totalCount;

        public List<T> PageData { get; set; }
        public object ExtraData { get; set; }

        [JsonIgnore]
        public int StartIndex
        {
            get { return (PageIndex - 1) * PageSize + 1; }
        }

        [JsonIgnore]
        public int EndIndex
        {
            get { return PageIndex * PageSize; }
        }

        public bool HasNextPage
        {
            get
            {
                return this.PageIndex < this.TotalPages;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 1);
            }
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount
        {
            get
            {
                return this.totalCount;
            }
            set
            {
                this.totalCount = value;
                this.TotalPages = this.TotalCount / this.PageSize;
                if ((this.TotalCount % this.PageSize) > 0)
                {
                    this.TotalPages++;
                }
            }
        }

        public int TotalPages { get; private set; }
    }
}

