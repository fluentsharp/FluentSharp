using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Object_ExtensionMethods
    {
        public static void  gcCollect(this object _object)
        {
            System.GC.Collect();
        }                
        public static int   hash(this object _object)
        {
            if (_object != null)
                return _object.GetHashCode();
            return default(int);
        }
        public static bool  isNull(this object _object)
        {
            return _object == null;
        }
        public static bool  notNull(this object _object)
        {
            return _object != null;
        }
        public static T     cast<T>(this object _object) 
        {
            if (_object is T)
                return (T)_object;
            return default(T);
        }

		public static T backTo<T>(this object _hostObject, T objectToGoBackTo)
		{
			return objectToGoBackTo;
		}
		public static T backTo<T, K>(this K hostObject, T objectToGoBackTo, ref K hostObjectRef)
		{
			hostObjectRef = hostObject;
			return objectToGoBackTo;
		}
		public static T log_Info<T>(this T _hostObject, string infoMessage, params string[] messageParams)
		{
			infoMessage.info(messageParams);
			return _hostObject;
		}

		public static T log_Debug<T>(this T _hostObject, string debugMessage, params string[] messageParams)
		{
			debugMessage.info(messageParams);
			return _hostObject;
		}
		public static T log_Error<T>(this T _hostObject, string errprMessage, params string[] messageParams)
		{
			errprMessage.info(messageParams);
			return _hostObject;
		}

    }
}
