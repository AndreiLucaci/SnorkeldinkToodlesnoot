using System.Collections.Generic;
using System.Linq;

namespace SnorkeldinkToodlesnoot.Helper
{
    public static class ListExtensions
    {
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var last = list.Last();
                list.Remove(last);
                return last;
            }
            return default(T);
        }
    }
}
