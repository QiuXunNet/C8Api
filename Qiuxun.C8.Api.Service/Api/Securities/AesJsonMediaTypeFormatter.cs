using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api.Securities
{

    public class AesJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        protected byte[] _aesIv;
        protected byte[] _aesKey;

        public AesJsonMediaTypeFormatter(AesKeyIv keyIv)
        {
            this._aesKey = keyIv.Key;
            this._aesIv = keyIv.Iv;
            base.SupportedMediaTypes.Clear();
            base.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/jsone"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        protected virtual byte[] GetResponseData(object value)
        {
            string s = JsonConvert.SerializeObject(value);
            return AESHelper.EncryptBytes(Encoding.UTF8.GetBytes(s), this._aesKey, this._aesIv);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            byte[] responseData;
            ApiResult result = value as ApiResult;
            if ((result != null) && !result.IsSuccess)
            {

                ApiException exception = new ApiException(9001, "程序异常，请联系管理员");
                return base.WriteToStreamAsync(type, new ApiResult(exception.Code, exception.Desc), writeStream, content, transportContext);
            }
            try
            {
                responseData = this.GetResponseData(value);
            }
            catch (Exception exception2)
            {
                ApiException exception3 = new ApiException(9001, "程序异常，请联系管理员");

                return base.WriteToStreamAsync(type, new ApiResult(exception3.Code, exception3.Desc), writeStream, content, transportContext);
            }
            return Task.Run(delegate
            {
                using (new MemoryStream(responseData))
                {
                    writeStream.Write(responseData, 0, responseData.Length);
                }
            });
        }
    }
}
