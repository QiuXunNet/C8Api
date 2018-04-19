using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 资源管理服务类
    /// </summary>
    public class ResourceManagementService
    {
        /// <summary>
        /// 上传资源
        /// </summary>
        /// <param name="base64Img"></param>
        /// <param name="type">资源类型</param>
        /// <param name="createUserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiResult<UploadFileResDto> UploadFile(string base64Img, int type, long createUserId, HttpRequestMessage request)
        {
            var result = new ApiResult<UploadFileResDto>();

            string url = request.RequestUri.GetLeftPart(UriPartial.Authority);
            string filePath = String.Format("/File/{0}/", DateTime.Now.ToString("yyyy/MM"));

            string errorMsg = "";
            var photo = Tool.SaveImage(HostingEnvironment.MapPath(filePath), base64Img, ref errorMsg);

            if (photo == null)
            {
                throw new ApiException(50000, "上传资源失败");
            }

            string rpath = url + filePath + photo.ImgName + photo.Extension;

            string strsql = @"insert into ResourceMapping(Extension, RPath, Title, SortCode, CreateTime, Type, FkId, RSize) 
                                   values(@Extension, @RPath, @Title, 1, getdate(), @Type, @FkId, @RSize);select @@identity";
            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@FkId",createUserId),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@Extension",photo.Extension),
                    new SqlParameter("@RPath",rpath),
                    new SqlParameter("@RSize",photo.RSize),
                    new SqlParameter("@Title",photo.ImgName)

                    };

            object data = SqlHelper.ExecuteScalar(strsql, sp);
            if (data == null)
                throw new ApiException(40000, "保存资源失败");

            int resId = Convert.ToInt32(data);

            UploadFileResDto resDto = new UploadFileResDto()
            {
                ResouceId = resId,
                Url = rpath
            };

            result.Data = resDto;
            return result;
        }
    }
}
