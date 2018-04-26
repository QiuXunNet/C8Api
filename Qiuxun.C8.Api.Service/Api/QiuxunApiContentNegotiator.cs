using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Qiuxun.C8.Api.Service.Api.Securities;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Service.Api
{

    public class QiuxunApiContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter compatibleJsonFormatter;
        private readonly JsonMediaTypeFormatter jsonFormatter;

        public QiuxunApiContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            this.jsonFormatter = formatter;
            this.compatibleJsonFormatter = new JsonMediaTypeFormatter();
            this.compatibleJsonFormatter.SerializerSettings.Converters.Add(new CompatibleLongTypeConvert());
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            HttpContent content = request.Content;
            MediaTypeHeaderValue contentType = null;
            if (content != null)
            {
                contentType = request.Content.Headers.ContentType;
            }
            if (contentType != null)
            {
                string mediaType = contentType.MediaType;
                HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> accept = request.Headers.Accept;
                if (string.Equals("application/jsone", mediaType, StringComparison.OrdinalIgnoreCase) || string.Equals("application/jsonet", mediaType, StringComparison.OrdinalIgnoreCase))
                {
                    if (accept.Count > 0)
                    {
                        foreach (MediaTypeWithQualityHeaderValue value3 in accept)
                        {
                            if (string.Equals("application/jsone", value3.MediaType, StringComparison.OrdinalIgnoreCase))
                            {
                                return new ContentNegotiationResult(new AesJsonMediaTypeFormatter(request.GetProperty<AesKeyIv>("")), WebSecuritySettings._media_type_json_encrypt);
                            }
                            if (string.Equals("application/jsonet", value3.MediaType, StringComparison.OrdinalIgnoreCase))
                            {
                                return new ContentNegotiationResult(new AesTextJsonMediaTypeFormatter(request.GetProperty<AesKeyIv>("")), WebSecuritySettings._media_type_json_encrypt_text);
                            }
                        }
                    }
                    return new ContentNegotiationResult(new JsonWithRsaSignFormatter(), WebSecuritySettings._media_type_json);
                }
            }
            if (request.Headers.Contains("Compatible-LongType"))
            {
                return new ContentNegotiationResult(this.compatibleJsonFormatter, WebSecuritySettings._media_type_json);
            }
            return new ContentNegotiationResult(this.jsonFormatter, WebSecuritySettings._media_type_json);
        }
    }
}
