using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.ResourceManagement.Model;

namespace Qiuxun.C8.ResourceManagement.Service
{

    public class PictureService : ResourceService
    {
        public PictureService()
        {
            base._publicHost = ConfigurationManager.AppSettings["picture_public_host"];
            if (string.IsNullOrWhiteSpace(base._publicHost))
            {
                base._publicHost = base._publicStorage.Host;
            }
            base._privateHost = ConfigurationManager.AppSettings["picture_private_host"];
            if (string.IsNullOrWhiteSpace(base._privateHost))
            {
                base._privateHost = base._privateStorage.Host;
            }
        }

        public Dictionary<Guid, List<ResourceDto>> BuildAllUrls(List<Guid> businessIds, int businessResourceType, bool isOrigin = false)
        {
            if ((businessIds == null) || (businessIds.Count == 0))
            {
                return new Dictionary<Guid, List<ResourceDto>>();
            }
            //return (from b in base.BuildUrl(businessIds, businessResourceType, false, isOrigin) group b by b.BusinessId).ToDictionary<IGrouping<Guid, ResourceDto>, Guid, List<ResourceDto>>(b => b.Key, b => b.ToList<BusinessResourceDto>());

            throw new NotImplementedException();
        }

        public Dictionary<Guid, ResourceDto> BuildUrls(List<Guid> businessIds, int businessResourceType, bool isOrigin = false)
        {
            if ((businessIds == null) || (businessIds.Count == 0))
            {
                return new Dictionary<Guid, ResourceDto>();
            }
            //return base.BuildUrl(businessIds, businessResourceType, true, isOrigin).ToDictionary<ResourceDto, Guid>(b => b.BusinessId);


            throw new NotImplementedException();
        }

        public List<string> BuildUrls(List<string> fileNames, bool isPrivate, bool isOrigin = false)
        {
            if (fileNames.Count == 0)
            {
                return null;
            }
            List<string> picUrls = new List<string>();
            fileNames.ForEach(d =>
            {
                string item = this.BuildUrlInner(d, isPrivate, isOrigin);
                picUrls.Add(item);
            });
            return picUrls;
        }

        public static string CutOutPicture(string picUrl, int width, int height)
        {
            string newValue = string.Format("@{0}w_{1}h?", width, height);
            return picUrl.Replace("?", newValue);
        }
    }
}
