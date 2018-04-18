using System.Collections.Generic;
using System.Xml.Serialization;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{
    

    public class InterfaceConfig
    {
        [XmlAttribute("disable")]
        public bool Disable { get; set; }

        [XmlAttribute("disableBlacklist")]
        public bool DisableBlacklist { get; set; }

        [XmlAttribute("disableLog")]
        public bool DisableLog { get; set; }

        [XmlAttribute("disableWhitelist")]
        public bool DisableWhitelist { get; set; }

        [XmlAttribute("forceBlacklist")]
        public bool ForceBlacklist { get; set; }

        [XmlAttribute("forceLog")]
        public bool ForceLog { get; set; }

        [XmlAttribute("forceWhitelist")]
        public bool ForceWhitelist { get; set; }

        [XmlAttribute("isRecordResponseData")]
        public bool IsRecordResponseData { get; set; }

        [XmlArray("otherRequestLimits"), XmlArrayItem("requestLimit")]
        public List<RequestLimitConfig> OtherRequestLimits { get; set; }

        [XmlElement("requestLimit")]
        public RequestLimitConfig RequestLimit { get; set; }

        [XmlAttribute("route")]
        public string Route { get; set; }
    }
}

