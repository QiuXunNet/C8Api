using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Qiuxun.C8.Api.Model;
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


            string rpath = url + filePath + photo.ImgName + "_Min" + photo.Extension;

            string path1 = Tool.UploadFileToOss(filePath + photo.ImgName + "_Min" + photo.Extension);
            string path2 = Tool.UploadFileToOss(filePath + photo.ImgName + photo.Extension);

            if (string.IsNullOrWhiteSpace(path1) || string.IsNullOrWhiteSpace(path2))
            {
                throw new ApiException(50000, "上传资源失败OSS");
            }

            StringBuilder strSql = new StringBuilder();

            if (type == (int)ResourceTypeEnum.用户头像)
            {
                strSql.Append("delete from dbo.ResourceMapping where [Type]=@Type and FkId=@FkId;");
            }

            strSql.Append(@"insert into ResourceMapping(Extension, RPath, Title, SortCode, CreateTime, Type, FkId, RSize) 
                                   values(@Extension, @RPath, @Title, 1, getdate(), @Type, @FkId, @RSize);select @@identity");

            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@FkId",createUserId),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@Extension",photo.Extension),
                    new SqlParameter("@RPath",path1),
                    new SqlParameter("@RSize",photo.RSize),
                    new SqlParameter("@Title",photo.ImgName)

                    };

            object data = SqlHelper.ExecuteScalar(strSql.ToString(), sp);
            if (data == null)
                throw new ApiException(40000, "保存资源失败");

            int resId = Convert.ToInt32(data);

            UploadFileResDto resDto = new UploadFileResDto()
            {
                ResouceId = resId,
                Url = path1
            };

            result.Data = resDto;
            return result;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="fkId">关联Id</param>
        /// <returns></returns>
        public List<ResourceMapping> GetResources(int type, long fkId)
        {
            string thumbSql = @"SELECT * FROM ResourceMapping WHERE [Type]=@type and [FKId]=@FKId ";
            SqlParameter[] parameters =
            {
                new SqlParameter("@type",SqlDbType.Int),
                new SqlParameter("@FKId",SqlDbType.BigInt),
            };
            parameters[0].Value = type;
            parameters[1].Value = fkId;

            return Util.ReaderToList<ResourceMapping>(thumbSql, parameters) ?? new List<ResourceMapping>();
        }

        /// <summary>
        /// 新增资源绑定
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="fkId">关联Id</param>
        /// <param name="resourceId">资源Id</param>
        public void InsertResource(int type, long fkId, int resourceId)
        {
            string updateSql = "UPDATE dbo.ResourceMapping SET FkId=@FkId WHERE [Type]=@Type AND Id=@Id";
            var parameters = new[]
            {
                new SqlParameter("@FkId",fkId),
                new SqlParameter("@Type",type),
                new SqlParameter("@Id",resourceId),
            };

            int result = SqlHelper.ExecuteNonQuery(updateSql, parameters);

            if (result <= 0) throw new ApiException(50000, "保存资源失败");
        }

        /// <summary>
        /// 新增资源绑定
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <param name="fkId">关联Id</param>
        /// <param name="resourceIds">资源Id列表</param>
        public void InsertResources(int type, long fkId, List<long> resourceIds)
        {
            if (resourceIds == null || resourceIds.Count < 1) return;

            string updateSql = string.Format("UPDATE dbo.ResourceMapping SET FkId=@FkId WHERE [Type]=@Type AND Id IN ({0})",
                string.Join(",", resourceIds));

            var parameters = new[]
           {
                new SqlParameter("@FkId",fkId),
                new SqlParameter("@Type",type),
            };

            SqlHelper.ExecuteNonQuery(updateSql, parameters);
        }
    }

}
