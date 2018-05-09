using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class WXResDto
    {
        /*
          appid = mchappid,
                    body = body,
                    CreatedOn = DateTime.Now,
                    mch_id = mchid,
                    spbill_create_ip = ip,
                    nonce_str = nonce,
                    timeStamp = TenPayV3Util.GetTimestamp(),
                    out_trade_no = orderno,
                    time_start = start,
                    time_expire = end,
                    total_fee = total,
             */
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// MchId
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        public string PrepayId { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }

        public string Package { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }
}
