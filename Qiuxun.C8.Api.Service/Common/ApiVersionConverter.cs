using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qiuxun.C8.Api.Service.Common
{

    public class ApiVersionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ApiVersion).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            string str = reader.Value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return new ApiVersion();
            }
            return new ApiVersion(str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as ApiVersion).ToString());
        }
    }
}
