using System;

namespace sharpRDFa.Extension
{
    public static class ObjectExtensions
    {
        public static void RequireNotNull(this object obj, string msg)
        {
            if (obj == null) throw new Exception(msg);
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}
