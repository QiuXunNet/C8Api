using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Common
{
    /// <summary>
    /// 请求信息
    /// </summary>
    public class RequestInfo
    {
        public RequestInfo()
        {
            this.Id = Guid.NewGuid();
        }
        /// <summary>
        /// Action参数
        /// </summary>
        internal Dictionary<string, object> ActionArguments { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 客户端高度
        /// </summary>
        public int ClientHeight { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIpHttp { get; set; }
        /// <summary>
        /// 客户端网络类型
        /// </summary>
        public string ClientNetType { get; set; }
        /// <summary>
        /// 客户端来源
        /// </summary>
        public byte ClientSourceId { get; set; }
        /// <summary>
        /// 客户端类型
        /// </summary>
        public DevicePlatform ClientType { get; set; }
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }
        /// <summary>
        /// 客户端宽度
        /// </summary>
        public int ClientWidth { get; set; }

        public string Cookie { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 地区编码
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// 是否有定位地址
        /// </summary>
        public bool HasLocation
        {
            get
            {
                return ((this.Lat > 1.0) && (this.Lng > 1.0));
            }
        }
        /// <summary>
        /// 是否有地区编码信息
        /// </summary>
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
        /// <summary>
        /// 是否有地区信息
        /// </summary>
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
        /// <summary>
        /// 请求方式
        /// </summary>
        public System.Net.Http.HttpMethod HttpMethod { get; set; }
        /// <summary>
        /// 请求Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        public RequestImeiDto ImeiInfo { get; set; }
        /// <summary>
        /// 接口版本
        /// </summary>
        public string InterfaceVersion { get; set; }
        /// <summary>
        /// 是否Https
        /// </summary>
        public bool IsHttps { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 定位类型
        /// </summary>
        public LocationType LocationType { get; set; }
        /// <summary>
        /// 其他Header
        /// 
        /// </summary>
        public string OtherHeader { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 省编码
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime RequestStartTime { get; set; }
        /// <summary>
        /// 加密数据
        /// </summary>
        public string RSAData { get; set; }
        /// <summary>
        /// token
        /// </summary>
        internal string Token { get; set; }
        /// <summary>
        /// 代理
        /// </summary>
        public string UserAgent { get; set; }
    }
}
