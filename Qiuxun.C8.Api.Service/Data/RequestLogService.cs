using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 请求日志服务
    /// </summary>
    public class RequestLogService
    {
        /// <summary>
        /// 记录请求日志
        /// </summary>
        public void AddRequestLog(RequestLog logEntity)
        {
            string sql = @"INSERT INTO [dbo].[ApiRequestLog]
           ([Id],[CustomerId],[UserName],[Imei],[ClientIp],[Lng],[Lat],[LocationType],[ClientNetType],[InterfaceVersion],[ClientVersion]
           ,[ClientType],[ClientWidth],[ClientHeight],[UserAgent],[ServerName],[RequestTime],[HttpMethod],[Route],[RequestData]
           ,[HttpStatus],[ApiStatus],[ApiDesc],[ElapsedMs],[RequestCookie],[ResponseCookie],[ClientIpHttp],[CustomerGuid]
           ,[OtherHeader],[GenerateTime],[IsFake],[ProvinceCode],[CityCode],[DistrictCode],[CreateTime])
     VALUES
           (@Id,@CustomerId,@UserName,@Imei,@ClientIp,@Lng,@Lat,@LocationType,@ClientNetType,@InterfaceVersion,@ClientVersion,@ClientType
		   ,@ClientWidth,@ClientHeight,@UserAgent,@ServerName,@RequestTime,@HttpMethod,@Route,@RequestData,@HttpStatus,@ApiStatus,@ApiDesc
           ,@ElapsedMs,@RequestCookie,@ResponseCookie,@ClientIpHttp,@CustomerGuid,@OtherHeader,@GenerateTime,@IsFake,@ProvinceCode,@CityCode
           ,@DistrictCode,GETDATE())";

            var param = new[]
            {
                new SqlParameter("@Id",logEntity.Id),
                new SqlParameter("@CustomerId",logEntity.CustomerId ?? 0),
                new SqlParameter("@UserName",logEntity.UserName ?? ""),
                new SqlParameter("@Imei",logEntity.Imei ?? ""),
                new SqlParameter("@ClientIp",logEntity.ClientIp ?? ""),
                new SqlParameter("@Lng",logEntity.Lng),
                new SqlParameter("@Lat", logEntity.Lat),
                new SqlParameter("@LocationType",logEntity.LocationType),
                new SqlParameter("@ClientNetType",logEntity.ClientNetType??""),
                new SqlParameter("@InterfaceVersion",logEntity.InterfaceVersion??""),
                new SqlParameter("@ClientVersion",logEntity.ClientVersion??""),
                new SqlParameter("@ClientType",logEntity.ClientType),
                new SqlParameter("@ClientWidth",logEntity.ClientWidth),
                new SqlParameter("@ClientHeight",logEntity.ClientHeight),
                new SqlParameter("@UserAgent",logEntity.UserAgent ?? ""),
                new SqlParameter("@ServerName",logEntity.ServerName ?? ""),
                new SqlParameter("@RequestTime",logEntity.RequestTime),
                new SqlParameter("@HttpMethod",logEntity.HttpMethod ?? ""),
                new SqlParameter("@Route",logEntity.Route ?? ""),
                new SqlParameter("@RequestData",logEntity.RequestData ?? ""),
                new SqlParameter("@HttpStatus",logEntity.HttpStatus),
                new SqlParameter("@ApiStatus",logEntity.ApiStatus ?? 0),
                new SqlParameter("@ApiDesc",logEntity.ApiDesc ?? ""),
                new SqlParameter("@ElapsedMs",logEntity.ElapsedMilliseconds),
                new SqlParameter("@RequestCookie",logEntity.RequestCookie ?? ""),
                new SqlParameter("@ResponseCookie",logEntity.ResponseCookie??""),
                new SqlParameter("@ClientIpHttp",logEntity.ClientIpHttp ?? ""),
                new SqlParameter("@CustomerGuid",logEntity.CustomerGuid??Guid.Empty),
                new SqlParameter("@OtherHeader",logEntity.OtherHeader ?? ""),
                new SqlParameter("@GenerateTime",logEntity.GenerateTime??new DateTime(1970,1,1)),
                new SqlParameter("@IsFake",logEntity.IsFake),
                new SqlParameter("@ProvinceCode",logEntity.ProvinceCode.ToInt32(0)),
                new SqlParameter("@CityCode",logEntity.CityCode.ToInt32(0)),
                new SqlParameter("@DistrictCode",logEntity.DistrictCode.ToInt32(0)),
            };
            

            SqlHelper.ExecuteNonQueryForSupport(sql, param);
        }

    }
}
