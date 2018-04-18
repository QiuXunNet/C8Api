using System.Collections.Generic;
using System.Xml.Serialization;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{
    [XmlRoot("apiInterfaceConfig")]
    public class ApiInterfaceConfig
    {
        [XmlElement("blacklist")]
        public LimitListConfig Blacklist { get; set; }

        [XmlElement("defaultRequestLimit")]
        public RequestLimitConfig DefaultRequestLimit { get; set; }

        [XmlAttribute("disableAllBlacklist")]
        public bool DisableAllBlacklist { get; set; }

        [XmlAttribute("disableAllRequestLimit")]
        public bool DisableAllRequestLimit { get; set; }

        [XmlAttribute("disableAllRequestLog")]
        public bool DisableAllRequestLog { get; set; }

        [XmlAttribute("disableAllWhitelist")]
        public bool DisableAllWhitelist { get; set; }

        [XmlArrayItem("add"), XmlArray("interfaces")]
        public List<InterfaceConfig> InterfaceSettings { get; set; }

        [XmlElement("whitelist")]
        public LimitListConfig Whitelist { get; set; }
    }
}

