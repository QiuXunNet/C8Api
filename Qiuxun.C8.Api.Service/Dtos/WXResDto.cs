using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Dtos
{
    public class WXResDto
    {
        private string appid = "wx226d38e96ed8f01e";
        private string mchid = "1375852802";     

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get { return appid; } }

        /// <summary>
        /// MchId
        /// </summary>
        public string MchId { get { return mchid; } }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }
    }
}
