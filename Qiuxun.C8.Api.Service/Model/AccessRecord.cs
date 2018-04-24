using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qiuxun.C8.Api.Model
{
    public class AccessRecord
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
        /// 受访人用户Id
        /// </summary>
        public long RespondentsUserId { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public int Module { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime AccessTime { get; set; }
        /// <summary>
        /// 访问日期
        /// </summary>
        [JsonIgnore]
        public DateTime AccessDate { get; set; }
        /// <summary>
        /// 访问者昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 访问者头像
        /// </summary>
        public string Avater { get; set; }
       

        public string ModuleName {
            get
            {
                if (Module == 1) return "主页";
                else return "主页";
            }
        }
    }
}
