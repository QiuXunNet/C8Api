using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api
{
    [QiuxunApiFilter, QiuxunTokenAuth]
    public class QiuxunApiController : ApiController
    {
        protected static readonly ClientIpType _clientIpType;
        private LocationSearchCriteria baiduLocation;
        private bool isRequestBaiduLocation;
        private static ILog logger = LogManager.GetLogger(typeof(QiuxunApiController));

        static QiuxunApiController()
        {
            if (System.Enum.TryParse(ConfigurationManager.AppSettings["client_ip_switch"], out _clientIpType))
            {
                _clientIpType = ClientIpType.TcpIp;
            }
        }

        public LocationSearchCriteria BaiduLocation
        {
            get
            {
                if (!this.isRequestBaiduLocation)
                {
                    if ((this.RequestInfo != null) && this.RequestInfo.HasRegionInfo)
                    {
                        LocationSearchCriteria criteria = new LocationSearchCriteria
                        {
                            Lat = this.RequestInfo.Lat,
                            Lng = this.RequestInfo.Lng,
                            Province = this.RequestInfo.Province,
                            City = this.RequestInfo.City,
                            District = this.RequestInfo.District
                        };
                        this.baiduLocation = criteria;
                    }
                    else if ((this.RequestInfo != null) && this.RequestInfo.HasLocation)
                    {
                        //TODO:解析地址
                        //LoGeoResult result = null;
                        //if (this.UserInfo != null)
                        //{
                        //    result = LoGeoFactory.DefaultGeoTransform.TransformWithUserLastLocation(this.UserInfo.UserId, this.RequestInfo.Lat, this.RequestInfo.Lng, this.RequestInfo.LocationType);
                        //}
                        //else
                        //{
                        //    result = LoGeoFactory.DefaultGeoTransform.Transform(this.RequestInfo.Lat, this.RequestInfo.Lng, this.RequestInfo.LocationType);
                        //}
                        //if ((result != null) && result.HasAddress)
                        //{
                        //    LocationSearchCriteria criteria2 = new LocationSearchCriteria
                        //    {
                        //        Lat = result.Lat,
                        //        Lng = result.Lng,
                        //        Province = result.Province,
                        //        City = result.City,
                        //        District = result.District
                        //    };
                        //    this.baiduLocation = criteria2;
                        //}
                    }
                    this.isRequestBaiduLocation = true;
                }
                return this.baiduLocation;
            }
        }

        /// <summary>
        /// 请求信息
        /// </summary>
        public RequestInfo RequestInfo
        {
            get
            {
                return base.Request.GetProperty<RequestInfo>("");
            }
        }

        public IdentityInfo UserInfo
        {
            get
            {
                return base.Request.GetProperty<IdentityInfo>("");
            }
        }
    }
}
