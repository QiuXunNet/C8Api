using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    ///  充值接口控制器
    /// </summary>
    public class RechargeController : QiuxunApiController
    {
        private static object _lock = new object();

        RechargeService _service = new RechargeService();

        private string GetRandom()
        {
            lock (_lock)
            {
                return "T" + DateTime.Now.Ticks;
            }
        }


        #region 微信支付代码

        private string key = "1de60212dceafe2b4fdb621bd6f04288";
        /// <summary>
        /// 获取微信充值必要的数据和订单号
        /// </summary>
        /// <param name="money">充值金额</param>
        /// <param name="userId">充值人</param>
        /// <returns>返回微信充值必要的数据和订单号</returns>
        [HttpGet]
        public ApiResult GetWxInfo(int money, int userId)
        {
            var result = new ApiResult<WXResDto>();

            WXResDto model = new WXResDto();
            model.Key = key.Insert(5, "a");
            model.Key = key.Insert(12, "s");
            model.Key = key.Insert(25, "d");

            model.OrderId = GetRandom();

            if (_service.AddComeOutRecord(model.OrderId, money, 1, userId))
            {
                result.Data = model;
                return result;
            }
            else
            {
                return new ApiResult(10000, "生成订单失败");
            }
        }

        [AllowAnonymous]
        public string WxNotify()
        {
            Senparc.Weixin.MP.TenPayLibV3.ResponseHandler payNotifyRepHandler = new Senparc.Weixin.MP.TenPayLibV3.ResponseHandler(null);
            payNotifyRepHandler.SetKey(key);

            string return_code = payNotifyRepHandler.GetParameter("return_code");
            string return_msg = payNotifyRepHandler.GetParameter("return_msg");
            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", return_code, return_msg);

            //   log.Info(xml);
            if (return_code.ToUpper() != "SUCCESS")
            {
                return xml;
            }

            string out_trade_no = payNotifyRepHandler.GetParameter("out_trade_no");

            //log.Info("微信回调订单号：-" + out_trade_no);
            //微信服务器可能会多次推送到本接口，这里需要根据out_trade_no去查询订单是否处理，如果处理直接返回：return Content(xml, "text/xml"); 不跑下面代码
            //if (false)
            //{
            if (_service.AlertComeOutRecord(out_trade_no, 1))
            {
                return xml;
            }
            else
            {
                //如果订单修改失败，需要微信再次发送请求
                xml = string.Format(@"<xml><return_code><![CDATA[FAIL]]></return_code><return_msg><![CDATA[]]></return_msg></xml>");
                return xml;
            }
        }

        #endregion

        #region 支付宝支付代码

        public string app_id = "2018041302550006";   //开发者的应用ID
        public string format = "JSON";
        public string charset = "utf-8";
        public string sign_type = "RSA2";  //签名格式
        public string version = "1.0";
        //商户私钥
        public string merchant_private_key = "MIIEpQIBAAKCAQEA0wSiER8RQygVK1/1j5nYAzDJwI6Tx9NL3j7YfecRZvfXCJ8oTysByONG/loL0vdl/Ih0oeIwDcNbUr1F4AgjnQYRz8l16UVu8AwYShqf29MGfSAQLdvzqG3/oqkJpNwaBMALtyq1qwklga5fEgH8/l+pFc+LN2aTaxtWFShaDQRpMuVGsE3CwzALK+XWZxm49XcbBikH04F65Kx0NmC4FziBT/HS/kCC5atJ2JiUoa2VM8j00CBM0On/vHodjU3HVmmBg1LBVoTi9+FbaJT+Hdqy4H0In1V+2K9OVREe8ZrURT6S0zAeTgrjAUTKt9PMYrpkCaDkVqHzIhOB2gruvwIDAQABAoIBAQDBSVciE7D+MLLjXixR8vs4QPIsXOzkdpjh4/LtsD/yb0YacZ68lYo29mfLB7QY8+AJJvyeY87cbHs0GIbupMXqSOr7x28n0x/A5XNCPYz8EBm7dykauIRBXTBxUCCzT6DNhRO2HXr2RZSDarNOjV+tqPX6Mnc0sdKKoymAi8ugayUaqn8dTajoqg+drk6L1IgZCMK/CmVYPYJIQ+VrvRgwQ7bYiC1BHCme7I/n43rxGOQaXB9wRK5/LSD9O0tR9QktvhbsmtwW+e5TvoAGpy9p0LQMhLSdj5CocnM0G11dIboECrYc+SYD841EJF4J2pm18VIPp3rvoYZxRXrFleSxAoGBAPkGkmnawaHk/IhHyX0ZuHVMwBf8l4ZK3ducpBLKRdtLQvW2XOuoQhNiuKXg7KOW9npvN/GwKC50V8l7RblbEoZngyJ8rjm00sRF0UtSbWOdyII5WF0/mwJGm8yiwq8GuSyHj0xAdeJopH53ybuejBfN8VeBzv4H1G9dUuLnbCxVAoGBANjtj1oKsE7LrjNkiIN1+3P+wKIy/TwqA5bLwUILnzVW92IC2gHQLN+NoksKngkgH7m8Xnp0Eue3anUNfeAgdJKar94bKiBNest4ZOk288w0E5kRr2X/UtlVLIYcuJ4o9bx+QDn+ftMlWk90WOQI14QQxswZCa1fHe9b/60VmoLDAoGBAJqxSWx2VsiB3Zmutmx++MXtGnsMDvh+M1lD8ew2OLTkCMFoOkqtp/Yw4jExCu8ITS57Pk5ltmA9J3dim0psV5KkZKKcvwHb4P3JvRzEJG24SyESDGFIrLr6L7gr9zIQxCD0SMD+Xfx6MozZTri84Zu788r/OR02sfFIEMAhMGJNAoGAc4GK4xbt6gbqKtNNHTKlQY5UZAlibbaxUooLzW8CxxQXhUifbHe8bQytbeepXpKMUgnLBMjpiBhRxyH39G9TovxayJkORUT8LXtdwBBSoFjaVpbkHhtlsfN4UbDZXN3Sext+d2LbhPJOtB/vdPyARQHp2KM8U+RhvCHwceke7KECgYEAsdikO13ps6ML6buBTOZ3/3lqJVmNkUo9YG6gpxzhq/RQ2dXPqRqvJtDr/qmjP1+kigOi9xr8ZifjBEUI1ymTv/Th3X3Rq+o6Hrdfr1TpqE8ADlnJQYc6noV0qdwkd8lsnToLCrpUNLmU0EKWaIbiRv3+E55H80A3oyKGufqIlBo=";
        //支付宝公钥
        public string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAydy5QG+mxrN77GIiLLEis/cVYleYn8hg5CesKKT73WkswNSr4ohTfV/BVvHdURec395Ex2T230mKu/QSbbY7Ahi3k+EWufIX6TYz/Q28grV4BrtF1kIcNvQUHgE/zEGL+nVQhfEH/uBjZQ84ue4S8ywnVBovndB2rJOExr1SBS5iI1yzYxdvpHRGTHHMIF0MInW96dRU1t3XqGut6e4YvsZj8x3tEprNBSF5MIQ+BRz9KrdplIRpzR/sPXenxbgGjBTj5Fus/a625Oofb23F6W8qPG86m9RgPo14BWmAi5SoVa7FAYubN2p3OBpnEhxkDqSq5LmtVHkXUKOjxaIQxwIDAQAB";

        public string apiUrl = "https://openapi.alipay.com/gateway.do";

        /// <summary>
        /// 获取支付宝充值必要的数据和订单号
        /// </summary>
        /// <param name="money">充值金额</param>
        /// <param name="userId">充值人</param>
        /// <returns>返回支付宝充值必要的数据和订单号</returns>
        [HttpGet]
        public ApiResult GetZfbInfo(int money, int userId)
        {
            var zFBResDto = new ZFBResDto();
            zFBResDto.OrderId = GetRandom();

            if (_service.AddComeOutRecord(zFBResDto.OrderId, money, 2, userId))
            {
                IAopClient client = new DefaultAopClient(apiUrl, app_id, merchant_private_key, format, version, sign_type, alipay_public_key, charset, false);
                //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
                AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
                AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                model.Body = "金币充值";
                model.Subject = "金币充值";
                model.TotalAmount = money.ToString();
                model.ProductCode = "QUICK_MSECURITY_PAY";
                model.OutTradeNo = zFBResDto.OrderId;
                model.TimeoutExpress = "5m";
                request.SetBizModel(model);
                request.SetNotifyUrl("http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port+ "/api/Recharge/AsyncPay");
                //这里和普通的接口调用不同，使用的是sdkExecute
                AlipayTradeAppPayResponse response = client.SdkExecute(request);
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
                zFBResDto.ResponseBody = HttpUtility.HtmlEncode(response.Body);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题

                var result = new ApiResult<ZFBResDto>();
                result.Data = zFBResDto;
                return result;
            }
            else
            {
                return new ApiResult(10000, "生成订单失败");
            }
        }

        /// <summary>
        /// 验证通知数据的正确性
        /// </summary>
        /// <returns></returns> 
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="inputPara"></param>
        /// <returns></returns>
        private bool Verify(SortedDictionary<string, string> inputPara)
        {
            ZFBResDto model = new ZFBResDto();

            Dictionary<string, string> sPara = new Dictionary<string, string>();
            Boolean verifyResult = AlipaySignature.RSACheckV1(inputPara, model.AlipayPublicKey, model.CharSet, model.SignType, false);
            return verifyResult;
        }

        /// <summary>
        /// 支付宝异步回调方法
        /// </summary>
        [AllowAnonymous]
        public void AsyncPay()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();//将post请求过来的参数传化为SortedDictionary
                                                                      // log.Info("sPara.Count:" + sPara.Count);
            if (sPara.Count > 0)
            {
                bool bo = Verify(sPara);
                if (bo)//验签if (VerifyResult)
                {
                    try
                    {
                        //商户订单号
                        string out_trade_no = HttpContext.Current.Request.Form["out_trade_no"];

                        string trade_status = HttpContext.Current.Request.Form["trade_status"];


                        if (trade_status == "TRADE_FINISHED") //支持退款订单，如果超过可退款日期，支付宝发送一条请求并走这个代码
                        {
                            //  log.Info("该订单不可退款");
                        }
                        else if (trade_status == "TRADE_SUCCESS")
                        {

                            if (_service.AlertComeOutRecord(out_trade_no, 2))
                            {
                                HttpContext.Current.Response.Write("success");  //请不要修改或删除
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("fail");  //如果订单修改失败，则要求支付宝再次发送请求
                            }
                        }
                        else
                        {

                        }

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        HttpContext.Current.Response.Write("success");  //请不要修改或删除

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                    catch (Exception ex)
                    {

                        //  log.Error(ex.Message);
                    }
                }
                else//验证失败
                {
                    // log.Info("验证失败");
                    HttpContext.Current.Response.Write("fail");
                }
            }
            else
            {
                // log.Info("无通知参数");
                HttpContext.Current.Response.Write("无通知参数");
            }
        }

        #endregion
    }
}
