using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 银行卡绑定提现接口服务
    /// </summary>
    public class AmountController : QiuxunApiController
    {
        AmountService _service = new AmountService();

        /// <summary>
        /// 根据用户Id获取用户的银行卡信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<BankInfoResDto> GetUserBankCard(int userId)
        {
            var result = new ApiResult<BankInfoResDto>();
            result.Data = _service.GetUserBankCard(userId);
            return result;
        }

        /// <summary>
        /// 获取所有可用银行
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetBankList()
        {
            var imgUrl ="";
            string host = ConfigurationManager.AppSettings["webHost"].ToLower();

            if (string.IsNullOrEmpty(host))
            {
                imgUrl = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/Images/";
            }
            else
            {
                imgUrl = host + "/Images/";
            }

            List<dynamic> list = new List<dynamic>();
            list.Add(new { Abbreviation= "ICBC", Name= "中国工商银行" ,BankImg = imgUrl + "ICBC.png" });
            list.Add(new { Abbreviation = "BOC", Name = "中国银行", BankImg = imgUrl + "BOC.png" });
            list.Add(new { Abbreviation = "CCB", Name = "中国建设银行", BankImg = imgUrl + "CCB.png" });
            list.Add(new { Abbreviation = "ABC", Name = "中国农业银行", BankImg = imgUrl + "ABC.png" });
            list.Add(new { Abbreviation = "PSBC", Name = "中国邮政储蓄银行", BankImg = imgUrl + "PSBC.png" });
            list.Add(new { Abbreviation = "CEB", Name = "中国光大银行", BankImg = imgUrl + "CEB.png" });
            list.Add(new { Abbreviation = "COMM", Name = "交通银行", BankImg = imgUrl + "COMM.png" });
            list.Add(new { Abbreviation = "CMB", Name = "招商银行", BankImg = imgUrl + "CMB.png" });
            list.Add(new { Abbreviation = "CMBC", Name = "中国民生银行", BankImg = imgUrl + "CMBC.png" });
            list.Add(new { Abbreviation = "CIB", Name = "兴业银行", BankImg = imgUrl + "CIB.png" });
            list.Add(new { Abbreviation = "CITIC", Name = "中信银行", BankImg = imgUrl + "CITIC.png" });
            list.Add(new { Abbreviation = "GDB", Name = "广发银行", BankImg = imgUrl + "GDB.png" });
            list.Add(new { Abbreviation = "HXBANK", Name = "华夏银行", BankImg = imgUrl + "HXBANK.png" });
            list.Add(new { Abbreviation = "SPDB", Name = "浦发银行", BankImg = imgUrl + "SPDB.png" });
            list.Add(new { Abbreviation = "SPABANK", Name = "平安银行", BankImg = imgUrl + "SPABANK.png" });

            ApiResult<List<dynamic>> result = new ApiResult<List<dynamic>>();
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 添加或修改银行卡信息
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="TrueName">用户姓名</param>
        /// <param name="BankAccount">银行卡号</param>
        /// <param name="BankName">银行名称</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<bool> AddBankCard(BankInfoResDto model)
        {
            var result = new ApiResult<bool>();
            result.Data = _service.AddBankCard(model);
            return result;
        }

        public static object _lock = new object();

        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="bankId">银行信息ID</param>
        /// <param name="money">提取的金额</param>
        /// <param name="userId">当前登录用户</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult AddExtractCash(int bankId, int money, int userId)
        {
            var result = new ApiResult();

            lock (_lock)
            {
                var minExtractCash = int.Parse(ConfigurationManager.AppSettings["minExtractCash"]);
                var myCommission = _service.GetMyCommission(userId);

                //后台判断提现佣金是否正确
                if (money > myCommission || money < minExtractCash)
                {
                    return new ApiResult(10000, "您的佣金出现变动,请重新输入");
                }
                else
                {
                    _service.AddExtractCash(bankId, money, userId);
                }
            }
            return result;
        }
    }
}
