using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class ProcessingRecordsResDto
    {
        /// <summary>
        /// 处理记录Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
    }
}
