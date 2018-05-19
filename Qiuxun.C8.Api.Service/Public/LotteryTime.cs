using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Cache;
using Qiuxun.C8.Api.Service.Caching;

namespace Qiuxun.C8.Api.Service.Public
{
    public static class LotteryTime
    {

        public static List<LotteryTimeModel> GetLotteryTimeList()
        {

            var list = CacheHelper.GetCache<List<LotteryTimeModel>>("GetLotteryTimeListss");

            if (list == null || list.Count == 0)
            {
                list = new List<LotteryTimeModel>();

                string path = HttpContext.Current.Server.MapPath("/App_Data/LotteryTime.xml");

                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNode root = xml.SelectSingleNode("/LotteryRule");
                XmlNodeList childlist = root.ChildNodes;


                foreach (XmlNode child in childlist)
                {
                    list.Add(new LotteryTimeModel()
                    {
                        BeginTime = child.Attributes["BeginTime"].Value,
                        EndTime = child.Attributes["EndTime"].Value,
                        IsStop = child.Attributes["IsStop"].Value,
                        LotteryInterval = child.Attributes["LotteryInterval"].Value,
                        LType = child.Attributes["LType"].Value,
                        Name = child.Attributes["Name"].Value,
                        TimeInterval = child.Attributes["TimeInterval"].Value,
                        BeginIssue = child.Attributes["BeginIssue"].Value
                    });
                }

                CacheHelper.AddCache("GetLotteryTimeListss", list, 8 * 60);
            }

            return list;
        }

        /// <summary>
        /// 根据彩种，获取彩种开奖倒计时间
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static string GetTime(string lType)
        {
            var nowTime = DateTime.Now;
            var nowTimeStr = nowTime.ToString("yyyy-MM-dd");
            int nextLotteryTimeSeconds = 0; //下次开奖间隔秒数

            if (int.Parse(lType) < 9)
            {
                #region 全国彩

                DateTime target = CacheHelper.GetCache<DateTime>("LotteryTypeLotteryTimeCache" + lType);
                if (target == null || target == default(DateTime))
                {
                    string sql = "select OpenLine from DateLine where lType = " + lType;
                    target = (DateTime)SqlHelper.ExecuteScalar(sql);

                    CacheHelper.SetCache<DateTime>("LotteryTypeLotteryTimeCache" + lType, target, target.AddMinutes(-5));
                }

                if (nowTime > target) return "正在开奖";

                return Util.GetTwoDateCha(nowTime, target);

                #endregion
            }

            var list = GetLotteryTimeModelList();

            var lotteryTimeModel = list.FirstOrDefault(e => e.LType == lType &&
            nowTime > e.BeginTimeDate && nowTime < e.EndTimeDate);

            //如果当前时间不在配置的彩种开奖时间范围
            if (lotteryTimeModel == null)
            {
                var list2 = list.Where(e => e.LType == lType);
                if (!list2.Any())
                    return "找不到彩种";



                //如果只有一条记录
                if (list2.Count() == 1)
                {
                    var model = list2.FirstOrDefault();

                    //用于最后一期倒计时完成后显示配置时长的“正在开奖”文字
                    if (Math.Abs((int)(nowTime - model.EndTimeDate).TotalSeconds) < int.Parse(model.LotteryInterval))
                    {
                        return "正在开奖";
                    }

                    //如果当前时间小于开奖开始时间，即当前时间和开始开奖时间为同一天
                    if (nowTime < model.BeginTimeDate)
                    {
                        nextLotteryTimeSeconds = (int)(model.BeginTimeDate - nowTime).TotalSeconds;
                    }
                    // 如果当前时间大于开奖开始时间，即当前时间和开始开奖时间不为同一天
                    else
                    {
                        nextLotteryTimeSeconds = (int)(model.BeginTimeDate.AddDays(1) - nowTime).TotalSeconds;
                    }

                    nextLotteryTimeSeconds += int.Parse(model.TimeInterval) * 60;
                }
                else //如果存在多条记录，则取离当前时间最近的一条
                {
                    //查询开奖开始时间和当前时间在同一天的记录
                    var model = list2.Where(e => nowTime < e.BeginTimeDate).OrderBy(e => e.BeginTimeDate).FirstOrDefault();

                    //如果查不到,则开始开奖时间则必定在后一天,查询到后给开奖时间加一天
                    if (model == null)
                    {
                        model = list2.Where(e => nowTime < e.BeginTimeDate.AddDays(1)).OrderBy(e => e.BeginTimeDate.AddDays(1)).FirstOrDefault();
                        model.BeginTimeDate = model.BeginTimeDate.AddDays(1);
                    }

                    //用于最后一期倒计时完成后显示配置时长的“正在开奖”文字
                    if (Math.Abs((int)(nowTime - model.EndTimeDate).TotalSeconds) < int.Parse(model.LotteryInterval))
                    {
                        return "正在开奖";
                    }

                    nextLotteryTimeSeconds = (int)(model.BeginTimeDate - nowTime).TotalSeconds;

                    //第一期开奖时间需要加上时间间隔时长
                    nextLotteryTimeSeconds += int.Parse(model.TimeInterval) * 60;
                }
                return (nextLotteryTimeSeconds / 3600).ToString("00") + "&" + (nextLotteryTimeSeconds % 3600 / 60).ToString("00") + "&" + (nextLotteryTimeSeconds % 60).ToString("00");
            }

            //获取当前时间已过首次开奖计时时间的秒数
            int timeLong = (int)(nowTime - lotteryTimeModel.BeginTimeDate).TotalSeconds;

            //if (timeLong < 0)
            //{
            //    return "休奖时间";
            //}
            //时间间隔秒数
            int timeIntervalSeconds = int.Parse(lotteryTimeModel.TimeInterval) * 60;

            if ((timeLong % timeIntervalSeconds) <= int.Parse(lotteryTimeModel.LotteryInterval) && timeLong / timeIntervalSeconds > 0)
            {
                return "正在开奖";
            }

            //特殊处理
            if (lType == "9" && nowTime > Convert.ToDateTime(nowTimeStr + " 22:00") && nowTime < Convert.ToDateTime(nowTimeStr + " 22:01"))
            {
                return "正在开奖";
            }

            nextLotteryTimeSeconds = timeIntervalSeconds - (timeLong % timeIntervalSeconds);

            return (nextLotteryTimeSeconds / 3600).ToString("00") + "&" + (nextLotteryTimeSeconds % 3600 / 60).ToString("00") + "&" + (nextLotteryTimeSeconds % 60).ToString("00");
        }

        /// <summary>
        /// 获取彩种开奖配置
        /// </summary>
        /// <param name="lType"></param>
        /// <returns></returns>
        public static LotteryTimeModel GetLotteryModel(string lType)
        {
            var nowTime = DateTime.Now;
            var list = GetLotteryTimeModelList();

            var lotteryTimeModel = list.FirstOrDefault(e => e.LType == lType &&
            nowTime > e.BeginTimeDate && nowTime < e.EndTimeDate);

            return lotteryTimeModel;
        }

        public static LotteryTimeModel GetLotteryModel(string lType, DateTime currentTime)
        {
            var list = GetLotteryTimeModelList();

            var lotteryTimeModel = list.FirstOrDefault(e => e.LType == lType &&
            currentTime > e.BeginTimeDate && currentTime < e.EndTimeDate);

            return lotteryTimeModel;
        }

        public static List<LotteryTimeModel> GetLotteryTimeModelList()
        {
            var nowTime = DateTime.Now;
            var nowTimeStr = nowTime.ToString("yyyy-MM-dd");

            var list = GetLotteryTimeList().Where(e => e.IsStop == "0");
            list.ToList().ForEach(e => e.EndTime = e.EndTime == "24:00" ? "00:00" : e.EndTime);
            foreach (var model in list)
            {
                model.BeginTimeDate = Convert.ToDateTime(nowTimeStr + " " + model.BeginTime);
                model.EndTimeDate = Convert.ToDateTime(nowTimeStr + " " + model.EndTime);

                //如果开奖的开始时间大于了结束时间，说明时间跨天了，需要对开始结束时间重新处理
                if (model.BeginTimeDate > model.EndTimeDate)
                {
                    //如果当前时间小于23:59:59  说明还在跨天的前一天，则EndTime加一天
                    if (nowTime >= Convert.ToDateTime(nowTimeStr + " " + model.EndTime))
                    {
                        model.EndTimeDate = model.EndTimeDate.AddDays(1);
                    }
                    //如果当前时间小于01:00:00  说明在跨天的后一天，则BeginTime减一天
                    if (nowTime < Convert.ToDateTime(nowTimeStr + " " + model.EndTime))
                    {
                        model.BeginTimeDate = model.BeginTimeDate.AddDays(-1);
                    }
                }
            }

            return list.ToList();
        }

        public static LotteryTimeModel GetModelUseIssue(string lType)
        {
            var nowTime = DateTime.Now;
            var nowTimeStr = nowTime.ToString("yyyy-MM-dd");

            var list = GetLotteryTimeModelList().Where(e => e.LType == lType);
            if (!list.Any())
                return null;

            //如果只有一条记录
            if (list.Count() == 1)
            {
                return list.FirstOrDefault();
            }
            else //如果存在多条记录，则取离当前时间最近的一条
            {
                var model = list.Where(e => nowTime < e.BeginTimeDate).OrderBy(e => e.BeginTimeDate).FirstOrDefault();

                return model;
            }
        }
    }

    public class LotteryTimeModel
    {
        /// <summary>
        /// 彩种编号
        /// </summary>
        public string LType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BeginIssue { get; set; }

        /// <summary>
        /// 彩种名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开奖开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 开奖结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 开奖开始时间
        /// </summary>
        public DateTime BeginTimeDate { get; set; }

        /// <summary>
        /// 开奖结束时间
        /// </summary>
        public DateTime EndTimeDate { get; set; }

        /// <summary>
        /// 开奖间隔(分)
        /// </summary>
        public string TimeInterval { get; set; }

        /// <summary>
        /// 摇奖时间(秒)
        /// </summary>
        public string LotteryInterval { get; set; }

        /// <summary>
        /// 是否停售
        /// </summary>
        public string IsStop { get; set; }
    }
}