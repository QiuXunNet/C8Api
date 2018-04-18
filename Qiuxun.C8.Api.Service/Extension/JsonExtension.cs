using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Common;

namespace System
{
    public static class JsonExtension
    {
        public static string FormartJsonString(this string jsonData)
        {
            return JsonSerializerManager.JsonFormat(jsonData);
        }

        public static T FromJsonString<T>(this string jsonData)
        {
            return JsonSerializerManager.JsonDeserialize<T>(jsonData);
        }

        public static string ToJsonString(this object obj)
        {
            return JsonSerializerManager.JsonSerializer<object>(obj);
        }
    }
}
