using System.Collections.Generic;
using JetBrains.Annotations;

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

        [ContractAnnotation("_object:null => true")]
        public static bool  isNull( this object _object)
        {
            return _object == null;
        }
        [ContractAnnotation("null => false")]
        public static bool  isNotNull(this object _object)
        {
            return _object != null;
        }
        [ContractAnnotation("null => false")]
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

		public static T backTo<T>(this object hostObject, T objectToGoBackTo)
		{
			return objectToGoBackTo;
		}
        // ReSharper disable RedundantAssignment
		public static T backTo<T, TK>(this TK hostObject, T objectToGoBackTo, ref TK hostObjectRef)         
		{
			hostObjectRef = hostObject;
			return objectToGoBackTo;
		}
        // ReSharper restore RedundantAssignment

		public static T log_Info<T>(this T hostObject, params object[] messageParams)
		{			
		    hostObject.str().info(messageParams);
			return hostObject;
		}

		public static T log_Debug<T>(this T hostObject,  params object[] messageParams)
		{			
            hostObject.str().debug(messageParams);
			return hostObject;
		}
		public static T log_Error<T>(this T hostObject, params object[] messageParams)
		{			
            hostObject.str().error(messageParams);
			return hostObject;
		}

        public static List<T> log_Info<T>(this List<T> list, params object[] messageParams)
		{			
		    list.forEach((item)=> item.str().info(messageParams));
			return list;
		}

		public static List<T> log_Debug<T>(this List<T> list,  params object[] messageParams)
		{			
            list.forEach((item)=> item.str().debug(messageParams));
			return list;
		}
		public static List<T> log_Error<T>(this List<T> list, params object[] messageParams)
		{			
            list.forEach((item)=> item.str().error(messageParams));
			return list;
		}
    }
}
