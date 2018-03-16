using System.Collections.Generic;
using System.Linq;

namespace SnorkeldinkToodlesnoot.Helper
{
    public static class HashSetExtensions
    {
        public static T Pop<T>(this HashSet<T> hashSet)
        {
            if (hashSet.Count > 0)
            {
                var last = hashSet.Last();
                hashSet.Remove(last);
                return last;
            }
            return default(T);
        }
    }
}
