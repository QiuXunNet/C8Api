using System;

namespace Qiuxun.C8.Api.Service.Model
{
    public class Plan
    {
        public int Id { get; set; }
        public int lType { get; set; }
        public string Issue { get; set; }
        public int Sort { get; set; }
        public string Num { get; set; }
        public string Result { get; set; }


        //---------------------

        public string ShowSort
        {
            get
            {
                string result = "";

                //时时彩
                if (lType == 5)
                {
                    #region 六合

                    if (Sort == 0)
                    {
                        result = "平码";
                    }
                    else if (Sort == 1)
                    {
                        result = "尾数";
                    }
                    else if (Sort == 2)
                    {
                        result = "特肖";
                    }
                    else if (Sort == 3)
                    {
                        result = "一肖";
                    }
                    else if (Sort == 4)
                    {
                        result = "特码大小";
                    }
                    else if (Sort == 5)
                    {
                        result = "特码单双";
                    }
                    else if (Sort == 6)
                    {
                        result = "特码20码";
                    }
                    else if (Sort == 7)
                    {
                        result = "五不中";
                    }

                    #endregion
                }
                else if ((lType >= 51 && lType < 60) || (lType == 2 || lType == 4 || lType == 8))
                {
                    #region 快乐十分

                    if (Sort == 0)
                    {
                        result = "第一球";
                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "第六球";
                    }
                    else if (Sort == 6)
                    {
                        result = "第七球";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八球";
                    }

                    #endregion
                }
                else if (lType < 9 || (lType >= 9 && lType < 51) || lType == 65 || (lType >= 60 && lType < 63))
                {
                    #region 时时彩

                    if (Sort == 0)
                    {
                        result = "第一球";
                    }
                    else if (Sort == 1)
                    {
                        result = "第二球";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三球";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四球";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五球";
                    }
                    else if (Sort == 5)
                    {
                        result = "龙虎";
                    }

                    #endregion
                }
                else if (lType == 63 || lType == 64)
                {
                    #region PK10

                    if (Sort == 0)
                    {
                        result = "冠军";
                    }
                    else if (Sort == 1)
                    {
                        result = "亚军";
                    }
                    else if (Sort == 2)
                    {
                        result = "第三名";
                    }
                    else if (Sort == 3)
                    {
                        result = "第四名";
                    }
                    else if (Sort == 4)
                    {
                        result = "第五名";
                    }
                    else if (Sort == 5)
                    {
                        result = "第六名";
                    }
                    else if (Sort == 6)
                    {
                        result = "第七名";
                    }
                    else if (Sort == 7)
                    {
                        result = "第八名";
                    }
                    else if (Sort == 8)
                    {
                        result = "第九名";
                    }
                    else if (Sort == 9)
                    {
                        result = "第十名";
                    }


                    #endregion
                }



                return result;
            }
        }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string OpenNum { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime OpenTime { get; set; }
    }
}