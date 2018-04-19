using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 短信验证参数类
    /// </summary>
    public class SmsSendDto
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 接收者手机号
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 发送者手机号
        /// </summary>
        public string Sender { get; set; }

        public long UserId { get; set; }
    }

    /// <summary>
    /// 消息发送日志
    /// </summary>
    public class SmsSendLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 消息类型 1=文本短信
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 短信码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 状态 0=发送 1=成功 2=失败 3=已使用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 发送次数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发送端口
        /// </summary>
        public int SendPort { get; set; }
        /// <summary>
        /// 发送结果 0=已发送 1=成功
        /// </summary>
        public int SendResult { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 手机验证码验证参数类
    /// </summary>
    public class SmsCodeValidateDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
