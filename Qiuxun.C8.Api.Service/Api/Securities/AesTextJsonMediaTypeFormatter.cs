using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.Securities
{
    public class AesTextJsonMediaTypeFormatter : AesJsonMediaTypeFormatter
    {
        public AesTextJsonMediaTypeFormatter(AesKeyIv keyIv) : base(keyIv)
        {
            base.SupportedMediaTypes.Clear();
            base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/jsonet"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        protected override byte[] GetResponseData(object value)
        {
            string s = JsonConvert.SerializeObject(value);
            string str2 = Convert.ToBase64String(AESHelper.EncryptBytes(Encoding.UTF8.GetBytes(s), base._aesKey, base._aesIv));
            return Encoding.UTF8.GetBytes(str2);
        }
    }
}
