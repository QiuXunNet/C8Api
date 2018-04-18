using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// 请求日志
    /// </summary>
    public class RequestLog
    {
        public RequestLog()
        {
            this.Id = Guid.NewGuid();
            this.RequestTime = DateTime.Now;
        }

        public string ApiDesc { get; set; }

        public string ApiDescDetail { get; set; }

        public int? ApiStatus { get; set; }

        public string City { get; set; }

        public string CityCode { get; set; }

        public int ClientHeight { get; set; }

        public string ClientIp { get; set; }

        public string ClientIpHttp { get; set; }

        public string ClientNetType { get; set; }

        public int ClientType { get; set; }

        public string ClientVersion { get; set; }

        public int ClientWidth { get; set; }

        public Guid? CustomerGuid { get; set; }

        public int? CustomerId { get; set; }

        public string District { get; set; }

        public string DistrictCode { get; set; }

        public int ElapsedMilliseconds { get; set; }

        public DateTime? GenerateTime { get; set; }

        public bool HasRegionCodeInfo
        {
            get
            {
                if (string.IsNullOrEmpty(this.ProvinceCode) && string.IsNullOrEmpty(this.CityCode))
                {
                    return !string.IsNullOrEmpty(this.DistrictCode);
                }
                return true;
            }
        }

        public bool HasRegionInfo
        {
            get
            {
                if (string.IsNullOrEmpty(this.Province) && string.IsNullOrEmpty(this.City))
                {
                    return !string.IsNullOrEmpty(this.District);
                }
                return true;
            }
        }

        public string HttpMethod { get; set; }

        public int HttpStatus { get; set; }

        public Guid Id { get; set; }

        public string Imei { get; set; }

        public string InterfaceVersion { get; set; }

        public bool IsFake { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public int LocationType { get; set; }

        public string OtherHeader { get; set; }

        public string Province { get; set; }

        public string ProvinceCode { get; set; }

        public string RequestCookie { get; set; }

        public string RequestData { get; set; }

        public DateTime RequestTime { get; set; }

        public string ResponseCookie { get; set; }

        public string Route { get; set; }

        public string ServerName { get; set; }

        public string UserAgent { get; set; }

        public string UserName { get; set; }
    }
}
