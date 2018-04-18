using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service.Api.Securities
{
    public class RsaTextJsonMediaTypeFormatter : RsaJsonMediaTypeFormatter
    {
        public RsaTextJsonMediaTypeFormatter()
        {
            base.SupportedMediaTypes.Clear();
            base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/jsonet"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        protected override byte[] GetRequestData(Stream readStream)
        {
            StreamReader reader = new StreamReader(readStream);
            return Convert.FromBase64String(reader.ReadToEnd());
        }
    }
}
