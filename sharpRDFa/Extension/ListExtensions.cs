using System.Collections.Generic;
using System.Linq;

namespace sharpRDFa.Extension
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this IList<T> list) where T : class 
        {
            return list == null || list.Count == 0;
        }

        public static T FirstNonEmptyOrDefault<T>(this IList<T> list) where T : class
        {
            if (list == null || list.Count == 0) return null;
            return list.FirstOrDefault(item => item != null);
        }
    }
}
