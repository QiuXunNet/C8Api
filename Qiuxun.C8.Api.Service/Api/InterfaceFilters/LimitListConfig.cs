using System.Collections.Generic;
using System.Xml.Serialization;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{


    public class LimitListConfig
    {
        [XmlArray("ipLimit"), XmlArrayItem("ip")]
        public List<string> IpLimitConfigs { get; set; }
    }
}

