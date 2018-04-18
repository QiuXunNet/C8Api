using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        public static string GetValue(this NameValueCollection collection, string key)
        {
            return collection[key];
        }

        public static T GetValue<T>(this NameValueCollection collection, string key) where T : struct
        {
            string str = collection[key];
            if (!string.IsNullOrEmpty(str))
            {
                return str.ParseTo<T>();
            }
            return default(T);
        }
    }
}
