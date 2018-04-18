using System.Xml.Serialization;

namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{
    

    public class RequestLimitConfig
    {
        [XmlAttribute("checkIp")]
        public bool CheckIp { get; set; }

        [XmlAttribute("checkTime")]
        public int CheckTime { get; set; }

        [XmlAttribute("checkUser")]
        public bool CheckUser { get; set; }

        [XmlAttribute("forceLimit")]
        public bool ForceLimit { get; set; }

        [XmlAttribute("limitCount")]
        public int LimitCount { get; set; }

        [XmlAttribute("limitCount_Lv2")]
        public int LimitCount_Lv2 { get; set; }

        [XmlAttribute("limitTime")]
        public int LimitTime { get; set; }

        [XmlAttribute("limitTime_Lv2")]
        public int LimitTime_Lv2 { get; set; }
    }
}

