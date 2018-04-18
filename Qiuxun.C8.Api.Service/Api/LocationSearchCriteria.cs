using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Api
{
    /// <summary>
    /// 定位搜索标准
    /// </summary>
    public class LocationSearchCriteria
    {
        public string Province { get; set; }

        public string ProvinceCode { get; set; }

        public string City { get; set; }

        public string CityCode { get; set; }

        public string District { get; set; }

        public string DistrictCode { get; set; }

        public string RegionCode { get; set; }

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

        public double Lat { get; set; }

        public double Lng { get; set; }


    }
}
