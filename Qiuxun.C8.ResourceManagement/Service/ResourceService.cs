using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Common;
using Qiuxun.C8.ResourceManagement.Enum;
using Qiuxun.C8.ResourceManagement.Model;

namespace Qiuxun.C8.ResourceManagement.Service
{
    public class ResourceService
    {
        private const int _expire_mins = 15;        //过期时长（分）
        private const long _max_len = 0x200000L;    //最大单次上传大小
        protected IResourcePathBuilder _pathBuilder;
        protected string _privateBucket;
        private HashSet<int> _privateBusinessTypes = new HashSet<int> { 0x3e9, 0x3ea, 0x44d, 0x44e, 0x44f, 0x450, 0x452, 0x453 };
        protected string _privateHost;
        protected IResourceStorage _privateStorage;
        protected string _privateWatermarkHost;
        protected string _publicBucket;
        protected string _publicHost;
        protected IResourceStorage _publicStorage;


        public ResourceService()
        {
            string accessKeyId = ConfigurationManager.AppSettings["oss_key_id"];
            string accessKeySecret = ConfigurationManager.AppSettings["oss_key_secret"];
            string format = ConfigurationManager.AppSettings["oss_host"];
            string root = ConfigurationManager.AppSettings["oss_root"];
            _publicBucket = ConfigurationManager.AppSettings["oss_public_bucket"];
            string host = string.Format(format, _publicBucket);
            _privateBucket = ConfigurationManager.AppSettings["oss_private_bucket"];
            string privateHost = string.Format(format, _privateBucket);
            _publicStorage = new AliOssStorage(accessKeyId, accessKeySecret, _publicBucket, host, root);
            _privateStorage = new AliOssStorage(accessKeyId, accessKeySecret, _privateBucket, privateHost, root);
            _pathBuilder = new AccurateToHourPathBuilder();
            _publicHost = ConfigurationManager.AppSettings["resource_public_host"];
            if (string.IsNullOrWhiteSpace(_publicHost))
            {
                _publicHost = _publicStorage.Host;
            }
            _privateHost = ConfigurationManager.AppSettings["resource_private_host"];
            if (string.IsNullOrWhiteSpace(_privateHost))
            {
                _privateHost = _privateStorage.Host;
            }
            _privateWatermarkHost = ConfigurationManager.AppSettings["watermark_host"];
        }

        protected string BuildPathWithTimestamp(string path, DateTime updateTime)
        {
            return string.Format("{0}?_t={1}", path, updateTime.ToString("yyyyMMddHHmmssfff"));
        }


        public virtual string BuildUrl(long resourceId, bool isPrivate, bool isOrigin = false)
        {
            //查询资源Id

            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成文件Url
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="isOrigin">是否为源文件</param>
        /// <returns></returns>
        public string BuildUrl(string fileName, bool isPrivate, bool isOrigin = false)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            return BuildUrlInner(fileName, isPrivate, isOrigin);

        }

        /// <summary>
        /// 生成URL
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="isOrigin">是否为源文件</param>
        /// <returns></returns>
        protected string BuildUrlInner(string fileName, bool isPrivate, bool isOrigin)
        {
            if (!isPrivate) return UrlHelper.BuildUrl(_publicHost, fileName);

            if (!isOrigin) return UrlHelper.BuildUrl(_privateWatermarkHost, fileName);

            int index = fileName.IndexOf('@');
            if (index > 0)
            {
                fileName = fileName.Substring(0, index);
            }
            else
            {
                int length = fileName.IndexOf('?');
                if (length > 0)
                {
                    fileName = fileName.Substring(0, length);
                }
            }
            Uri url = _privateStorage.GetUrl(fileName, 15);
            return url.ToString().Replace(url.Scheme + "://" + url.Host, _privateHost);
        }


        public List<ResourceDto> BuildUrls(Guid businessId, List<int> businessResourceTypes, bool isOrigin = false)
        {
            //if ((businessResourceTypes == null) || (businessResourceTypes.Count == 0))
            //{
            //    return new List<BusinessResourceDto>();
            //}
            //ResourceReferenceByTypesQueryDto parms = new ResourceReferenceByTypesQueryDto
            //{
            //    BusinessId = businessId,
            //    BusinessResourceTypes = businessResourceTypes
            //};
            //return (from r in this._resourceDao.GetAllReferencesByBusinessIdAndTypes(parms) select new BusinessResourceDto { ResourceId = r.ResourceId, BusinessId = r.BusinessId, Path = r.Path, BusinessResourceType = r.BusinessResourceType, Width = r.Width, Height = r.Height, Url = this._privateBusinessTypes.Any<int>(d => (d == r.BusinessResourceType)) ? this.BuildUrl(r.Path, true, isOrigin) : this.BuildUrl(r.Path, false, isOrigin) }).ToList<BusinessResourceDto>();

            return null;
        }

        public List<ResourceDto> BuildUrls(Guid businessId, int businessResourceType, bool isOrigin = false)
        {
            //return this.BuildUrl(new List<Guid> { businessId }, businessResourceType, false, isOrigin);

            return null;
        }



        /// <summary>
        /// 拷贝到私有
        /// </summary>
        /// <param name="path"></param>
        public void CopyToPrivate(string path)
        {
            string str = "";
            int index = path.IndexOf('@');
            if (index > 0)
            {
                str = path.Substring(0, index);
            }
            else
            {
                int length = path.IndexOf('?');
                if (length > 0)
                {
                    str = path.Substring(0, length);
                }
                else
                {
                    str = path;
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("path not found");
            }
            this._publicStorage.CopyTo(this._publicBucket, this._privateBucket, str);
        }

        public void Delete(Guid resourceId, bool isPrivate)
        {
            //查询OSS资源路径Path
            //ResourceDto resourceById = this._resourceDao.GetResourceById(resourceId);
            //if (isPrivate)
            //{
            //    this._privateStorage.Delete(resourceById.Path); 调用删除
            //}
            //else
            //{
            //    this._publicStorage.Delete(resourceById.Path);调用删除
            //}
            //this._resourceDao.DeleteResourceReferenceByResourceId(resourceById.Id);
            //this._resourceDao.DeleteResourceById(resourceById.Id);
        }

        public void DeleteByBusinessId(Guid businessId)
        {
            //List<Guid> list2 = (from d in this._resourceDao.GetAllReferenceByBusinessId(businessId) select d.Id).ToList<Guid>();
            //if (list2.Count > 0)
            //{
            //    GuidsQueryDto ids = new GuidsQueryDto
            //    {
            //        Ids = list2
            //    };
            //    this._resourceDao.DeleteReferencesByIds(ids);
            //}
        }

        public void DeleteReference(Guid businessId, Guid resourceId, int businessResourceType)
        {
            //RemoveResourceReferenceQueryDto parms = new RemoveResourceReferenceQueryDto
            //{
            //    BusinessId = businessId,
            //    BusinessResourceType = businessResourceType,
            //    ResourceId = resourceId
            //};
            //this._resourceDao.DeleteResourceReference(parms);
        }

        public void DeleteReferences(Guid businessId, int businessResourceType)
        {
            //RemoveResourceReferencesQueryDto parms = new RemoveResourceReferencesQueryDto
            //{
            //    BusinessId = businessId,
            //    BusinessResourceType = businessResourceType
            //};
            //this._resourceDao.DeleteResourceReferences(parms);
        }

        public Stream Download(Guid businessId, int businessResourceType)
        {
            //ResourceReferenceQueryDto parms = new ResourceReferenceQueryDto
            //{
            //    BusinessIds = new List<Guid> { businessId },
            //    BusinessResourceType = businessResourceType
            //};
            //ResourceReferenceDto dto = this._resourceDao.GetAllReferenceByTypeAndBusinessIds(parms).FirstOrDefault<ResourceReferenceDto>();
            //if (dto == null)
            //{
            //    throw new FileNotFoundException();
            //}
            //ResourceDto resourceById = this._resourceDao.GetResourceById(dto.ResourceId);
            //if (resourceById == null)
            //{
            //    throw new FileNotFoundException();
            //}

            //查询OSS路径Path下载
            //return this._publicStorage.Download(resourceById.Path);

            return null;
        }

        public Stream Download(string path, bool isPrivate)
        {
            string address = this.BuildUrl(path, isPrivate, false);
            byte[] buffer = new WebClient().DownloadData(address);
            if (buffer == null)
            {
                throw new FileNotFoundException();
            }
            return new MemoryStream(buffer);
        }

        public ResourceDto Insert(ResourceType type, byte[] bytes, bool isPrivate, string fileName = "df.jpg", long maxLength = 0x200000L)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return this.Insert(type, stream, isPrivate, fileName, maxLength);
            }
        }

        public ResourceDto Insert(ResourceType type, Stream stream, bool isPrivate, string fileName = "df.jpg", long maxLength = 0x200000L)
        {
            long length = stream.Length;
            if (length > maxLength)
            {
                throw new Exception(string.Format("上传的文件大小不能超过{0}MB", maxLength >> 20));
            }
            if (stream.CanSeek)
            {
                stream.Position = 0L;
            }
            string contentType = UrlHelper.GetContentType(fileName);
            Guid resourceId = Guid.NewGuid();// IdGen.NewSequentialGuid();
            string str2 = this._pathBuilder.BuildPath(type, resourceId, fileName);
            string str3 = UrlHelper.BuildUrl(this._publicStorage.Root, str2, new string[0]);
            if (isPrivate)
            {
                this._privateStorage.Upload(str3, contentType, stream);
            }
            else
            {
                this._publicStorage.Upload(str3, contentType, stream);
            }
            DateTime now = DateTime.Now;
            ResourceDto item = new ResourceDto
            {
                //Id = resourceId,
                //Type = (int)type,
                //Path = str3,
                //OriginName = fileName,
                //MimeType = contentType,
                //AddTime = now,
                //UpdateTime = now,
                //FileSize = length
            };
            if (!string.IsNullOrEmpty(UrlHelper.GetImageSurfix(contentType)))
            {
                stream.Position = 0;
                var image = Image.FromStream(stream);
                item.Width = image.Width;
                item.Height = image.Height;
            }
            //this._resourceDao.InsertResource(item);
            return item;
        }

        public List<string> InsertMultiReference(List<Guid> resourceIds, Guid businessId, int businessResourceType)
        {
            //RemoveResourceReferencesQueryDto parms = new RemoveResourceReferencesQueryDto
            //{
            //    BusinessId = businessId,
            //    BusinessResourceType = businessResourceType
            //};
            //this._resourceDao.DeleteResourceReferences(parms);
            //List<string> list = new List<string>();
            //if ((resourceIds != null) && (resourceIds.Count != 0))
            //{
            //    DateTime now = DateTime.Now;
            //    List<Guid> list2 = IdGen.NewSequentialGuids(resourceIds.Count);
            //    GuidsQueryDto ids = new GuidsQueryDto
            //    {
            //        Ids = resourceIds
            //    };
            //    IList<ResourceDto> resourcesByIds = this._resourceDao.GetResourcesByIds(ids);
            //    DateTime addTime = DateTime.Now;
            //    for (int i = 0; i < resourceIds.Count; i++)
            //    {
            //        Guid resId = resourceIds[i];
            //        Guid guid = list2[i];
            //        ResourceDto dto2 = resourcesByIds.FirstOrDefault<ResourceDto>(d => d.Id == resId);
            //        if (dto2 != null)
            //        {
            //            string item = this.BuildPathWithTimestamp(dto2.Path, dto2.UpdateTime);
            //            list.Add(item);
            //            ResourceReferenceDto dto3 = new ResourceReferenceDto
            //            {
            //                Id = guid,
            //                AddTime = addTime.AddMilliseconds(10.0),
            //                BusinessId = businessId,
            //                BusinessResourceType = businessResourceType,
            //                Path = item,
            //                ResourceId = resId,
            //                Width = dto2.Width,
            //                Height = dto2.Height
            //            };
            //            addTime = dto3.AddTime;
            //            this._resourceDao.InsertResourceReference(dto3);
            //        }
            //    }
            //}
            //return list;
            return null;
        }

        public string InsertSingleReference(Guid resourceId, Guid businessId, int businessResourceType)
        {
            List<Guid> resourceIds = new List<Guid> {
                resourceId
            };
            return this.InsertMultiReference(resourceIds, businessId, businessResourceType).FirstOrDefault<string>();
        }

        public ResourceDto Update(Guid resourceId, bool isPrivate, string originName, byte[] bytes, long maxLength = 0x200000L)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return this.Update(resourceId, isPrivate, originName, stream, maxLength);
            }
        }

        public ResourceDto Update(Guid resourceId, bool isPrivate, string originName, Stream stream, long maxLength = 0x200000L)
        {
            long length = stream.Length;
            if (length > maxLength)
            {
                throw new Exception(string.Format("上传的文件大小不能超过{0}MB", maxLength >> 20));
            }
            if (stream.CanSeek)
            {
                stream.Position = 0L;
            }
            ResourceDto resourceById = new ResourceDto();// this._resourceDao.GetResourceById(resourceId);
            if (resourceById != null)
            {
                int type = resourceById.Type;
                string contentType = UrlHelper.GetContentType(originName);
                if (isPrivate)
                {
                    this._privateStorage.Upload(resourceById.RPath, contentType, stream);
                }
                else
                {
                    this._publicStorage.Upload(resourceById.RPath, contentType, stream);
                }
                DateTime now = DateTime.Now;
                //UpdateResourceQueryDto item = new UpdateResourceQueryDto
                //{
                //    Id = resourceId,
                //    OriginName = originName,
                //    UpdateTime = now,
                //    FileSize = length,
                //    MimeType = contentType
                //};
                //if (!string.IsNullOrEmpty(UrlHelper.GetImageSurfix(contentType)))
                //{
                //    stream.Position = 0L;
                //    MediaTypeNames.Image image = MediaTypeNames.Image.FromStream(stream);
                //    item.Width = new int?(image.Width);
                //    item.Height = new int?(image.Height);
                //}
                //this._resourceDao.UpdateResource(item);
                //UpdateResourceReferenceQueryDto dto3 = new UpdateResourceReferenceQueryDto
                //{
                //    ResourceId = resourceById.Id,
                //    Path = this.BuildPathWithTimestamp(resourceById.Path, now)
                //};
                //dto3.Width = item.Width;
                //dto3.Height = item.Height;
                //this._resourceDao.UpdateResourceReference(dto3);
            }
            return resourceById;
        }



        public string InsertSingleReference(long resourceId, string businessId, int resourceType)
        {
            throw new NotImplementedException();
        }

        public List<string> InsertMultiReference(List<long> resouceId, string businessId, int resourceType)
        {

            throw new NotImplementedException();
        }




        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="bytes">资源字节</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void Upload(string path, byte[] bytes, bool isPrivate, long maxLength = 0x200000)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                Upload(path, stream, isPrivate, maxLength);
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stream">资源文件流</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void Upload(string path, Stream stream, bool isPrivate, long maxLength = 0x200000)
        {
            if (stream.Length > maxLength)
            {
                throw new ArgumentException($"上传的文件大小不能超过{maxLength >> 20}MB");
            }
            if (stream.CanSeek)
                stream.Position = 0;
            string contentType = UrlHelper.GetContentType(path);
            if (isPrivate)
            {
                _privateStorage.Upload(path, contentType, stream);
            }
            else
            {
                _publicStorage.Upload(path, contentType, stream);
            }
        }

        /// <summary>
        /// 上传并刷新CDN缓存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stream">资源文件流</param>
        /// <param name="isPrivate">是否私有</param>
        /// <param name="maxLength">最大大小</param>
        public void UploadAndRefreshCDN(string path, Stream stream, bool isPrivate, long maxLength = 0x200000)
        {
            Upload(path, stream, isPrivate, maxLength);
            AliCDNHelper.Refresh(path);
        }
    }
}
