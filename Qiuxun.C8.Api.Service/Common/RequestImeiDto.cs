using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    public class RequestImeiDto
    {
        public RequestImeiDto(string encryptedImei)
        {
            this.EncryptedImei = encryptedImei;
        }
        /// <summary>
        /// 加密设备识别码
        /// </summary>
        public string EncryptedImei { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime? GenerateTime { get; set; }
        /// <summary>
        /// 是否伪造
        /// </summary>
        public bool IsFake { get; set; }
        /// <summary>
        /// 真实设备识别码
        /// </summary>
        public string RealImei { get; set; }
    }
}
