namespace Qiuxun.C8.Api.Service.Api.InterfaceFilters
{

    public class RequestLimit
    {
        public bool CheckIp { get; set; }

        public int CheckTime { get; set; }

        public bool CheckUser { get; set; }

        public int LimitCount { get; set; }

        public int LimitCount_Lv2 { get; set; }

        public int LimitTime { get; set; }

        public int LimitTime_Lv2 { get; set; }
    }
}

