using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class CollectionExtension
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            foreach (T local in items)
            {
                list.Add(local);
            }
            return list;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (T local in source)
                {
                    action(local);
                }
            }
            return source;
        }
        
    }
}
