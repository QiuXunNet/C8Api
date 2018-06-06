using System;
using Qiuxun.C8.Api.Public;

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

                #region 保持与发帖得玩法一致
                //if (lType == 5)
                //{
                //    #region 六合彩

                //    if (Sort == 0)
                //    {
                //        result = "特码单双";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "特码大小";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "特尾大小";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "特合单双";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "特码波色";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "平特一肖";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "四肖中特";
                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "六肖中特";
                //    }
                //    else if (Sort == 8)
                //    {
                //        result = "二十码中特";
                //    }
                //    else if (Sort == 9)
                //    {
                //        result = "十码中特";
                //    }
                //    else if (Sort == 10)
                //    {
                //        result = "平五码";
                //    }
                //    else if (Sort == 11)
                //    {
                //        result = "五不中";
                //    }
                //    else if (Sort == 12)
                //    {
                //        result = "六不中";
                //    }

                //    #endregion
                //}
                //else if (lType == 1 || lType == 6)
                //{
                //    #region 3D 排列3
                //    if (Sort == 0)
                //    {
                //        result = "第一球";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "第二球";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三球";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "杀一码";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "杀二码";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "胆一码";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "胆二码";
                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "胆三码";
                //    }

                //    #endregion
                //}
                //else if (lType == 2)
                //{
                //    #region 双色球
                //    if (Sort == 0)
                //    {
                //        result = "红2胆";

                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "红3胆";

                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "杀3红";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "蓝单双";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "蓝大小";

                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "龙头单双";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "凤尾单双";
                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "蓝5码";
                //    }
                //    else if (Sort == 8)
                //    {
                //        result = "蓝8码";
                //    }
                //    #endregion
                //}
                //else if (lType == 4)
                //{
                //    #region 大乐透
                //    if (Sort == 0)
                //    {
                //        result = "前区4码";

                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "前区杀4码";

                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "前区龙头单双";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "前区凤尾单双";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "后区3码";

                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "后区杀3码";

                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "后区龙头单双";

                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "后区凤尾单双";

                //    }

                //    #endregion
                //}
                //else if (lType == 8)
                //{
                //    #region 七乐彩
                //    if (Sort == 0)
                //    {
                //        result = "第一球";

                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "第二球";

                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三球";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "第四球";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "第五球";

                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "第六球";

                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "第七球";

                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "胆一码";

                //    }
                //    else if (Sort == 8)
                //    {
                //        result = "胆二码";

                //    }
                //    else if (Sort == 9)
                //    {
                //        result = "胆三码";

                //    }
                //    else if (Sort == 10)
                //    {
                //        result = "杀三码";

                //    }
                //    else if (Sort == 11)
                //    {
                //        result = "杀五码";

                //    }

                //    #endregion
                //}
                //else if ((lType >= 51 && lType < 60) || lType == 3 || lType == 7)
                //{
                //    #region 快乐十分

                //    if (Sort == 0)
                //    {
                //        result = "第一球";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "第二球";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三球";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "第四球";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "第五球";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "第六球";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "第七球";
                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "第八球";
                //    }

                //    #endregion
                //}
                //else if (lType >= 9 && lType < 15)
                //{
                //    #region 时时彩

                //    if (Sort == 0)
                //    {
                //        result = "第一球";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "第二球";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三球";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "第四球";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "第五球";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "总和";
                //    }

                //    #endregion
                //}
                //else if ((lType >= 15 && lType < 38) || (lType >= 60 && lType < 63))
                //{
                //    #region 11选5 快乐12

                //    if (Sort == 0)
                //    {
                //        result = "第一球";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "第二球";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三球";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "第四球";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "第五球";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "总和";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "龙虎";
                //    }

                //    #endregion
                //}
                //else if ((lType >= 38 && lType < 51) || lType == 65)
                //{
                //    #region 快三 PC蛋蛋
                //    if (Sort == 0)
                //    {
                //        result = "和值";

                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "独胆";

                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "双胆";
                //    }

                //    #endregion
                //}
                //else if (lType == 63 || lType == 64)
                //{
                //    #region PK10

                //    if (Sort == 0)
                //    {
                //        result = "冠军";
                //    }
                //    else if (Sort == 1)
                //    {
                //        result = "亚军";
                //    }
                //    else if (Sort == 2)
                //    {
                //        result = "第三名";
                //    }
                //    else if (Sort == 3)
                //    {
                //        result = "第四名";
                //    }
                //    else if (Sort == 4)
                //    {
                //        result = "第五名";
                //    }
                //    else if (Sort == 5)
                //    {
                //        result = "第六名";
                //    }
                //    else if (Sort == 6)
                //    {
                //        result = "第七名";
                //    }
                //    else if (Sort == 7)
                //    {
                //        result = "第八名";
                //    }
                //    else if (Sort == 8)
                //    {
                //        result = "第九名";
                //    }
                //    else if (Sort == 9)
                //    {
                //        result = "第十名";
                //    }


                //    #endregion
                //}

                #endregion


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
        /// <summary>
        /// 开奖号码别名
        /// </summary>
        public string OpenNumAlias {
            get
            {
                if (string.IsNullOrWhiteSpace(OpenNum)) return OpenNum;

                return Util.GetShowInfo(lType, OpenNum,OpenTime);
            }
        }
    }
}