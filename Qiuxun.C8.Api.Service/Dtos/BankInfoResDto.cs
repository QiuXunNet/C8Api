using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    /// <summary>
    /// 银行卡信息
    /// </summary>
    public class BankInfoResDto
    {
        /// <summary>
        /// 银行卡主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
    }
}
