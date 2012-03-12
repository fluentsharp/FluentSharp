using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2.Kernel.ExtensionMethods
{
    public static class Object_ExtensionMethods
    {
        public static void gcCollect(this object _object)
        {
            System.GC.Collect();
        }                

        public static int hash(this object _object)
        {
            if (_object != null)
                return _object.GetHashCode();
            return default(int);
        }

        public static bool isNull(this object _object)
        {
            return _object == null;
        }

        public static bool notNull(this object _object)
        {
            return _object != null;
        }
 
    }
}
