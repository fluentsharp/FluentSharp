using System;
using System.Collections;
using JetBrains.Annotations;

namespace FluentSharp.CoreLib
{
    public static class Object_ExtensionMethods
    {
        public static T     assign<T>(this T dummyObject, T newValue)
        {
            return newValue;
        }
        public static void  gcCollect(this object _object)
        {
            GC.Collect();
        }                
        public static int   hash(this object _object)
        {
            if (_object != null)
                return _object.GetHashCode();
            return default(int);
        }
        public static bool  is_Default<T>( this T _object) where T : class
        {
            return _object == default(T);
        }

        [ContractAnnotation("_object:null => true")]
        public static bool  is_Null<T>( this T _object) where T : class
        {
            return _object == null;
        }
        [ContractAnnotation("null => false")]
        public static bool  is_Not_Null(this object _object)
        {
            return _object != null;
        }
        [ContractAnnotation("_object:null => true")]
        public static bool  isNull<T>( this T _object) where T : class
        {
            return _object.is_Null();
        }
        [ContractAnnotation("null => false")]
        public static bool  isNotNull(this object _object)
        {
            return _object.is_Not_Null();
        }
        [ContractAnnotation("null => false")]
        public static bool  not_Null(this object _object)
        {
            return _object.is_Not_Null();
        }
        [ContractAnnotation("null => false")]
        public static bool  notNull(this object _object)
        {
            return _object.is_Not_Null();
        }
        /// <summary>
        /// Casts the provided object into the provided type.
        /// 
        /// This is a nice way to transform an plain object into strongly type object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_object"></param>
        /// <returns></returns>
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
		public static T backTo<T, TK>(this TK hostObject, T objectToGoBackTo, ref TK hostObjectRef)         
		{
			hostObjectRef = hostObject;
			return objectToGoBackTo;
		}        

		public static T log_Info<T>(this T hostObject, params object[] messageParams)
		{			
            if (hostObject.isIEnumerable())
                (hostObject as IEnumerable).toList().forEach<object>((item)=> item.str().info(messageParams));	
		    else
		        hostObject.str().info(messageParams);
			return hostObject;
		}
		public static T log_Debug<T>(this T hostObject,  params object[] messageParams)
		{	
		    if (hostObject.isIEnumerable())
                (hostObject as IEnumerable).toList().forEach<object>((item)=> item.str().debug(messageParams));					                    
		    else   
                hostObject.str().debug(messageParams);
			return hostObject;
		}
		public static T log_Error<T>(this T hostObject, params object[] messageParams)
		{			
            if (hostObject.isIEnumerable())
                (hostObject as IEnumerable).toList().forEach<object>((item)=> item.str().error(messageParams));			
            else
                hostObject.str().error(messageParams);
			return hostObject;
		}

        public static T clone<T>(this T objectToClone) where T : class
        {
            try
            {
                if (objectToClone.isNull())
                    "[object<T>.clone] provided object was null (type = {0})".error(typeof(T));
                else
                    return (T)objectToClone.invoke("MemberwiseClone");
            }
            catch (Exception ex)
            {
                ex.log();
                "[object<T>.clone]Faild to clone object {0} of type {1}".error(objectToClone.str(), typeof(T));
            }
            return default(T);
        }	

        public static bool isInstanceOf<T>(this object _object)
        {
            return _object is T;
        }

        public static bool isNotInstanceOf<T>(this object _object)
        {
            return _object.isInstanceOf<T>().isFalse();
        }
        /// <summary>
        /// This decasts an strongly type object into a pure C# object
        /// 
        /// Usefull when there is a need to actualy lose strong typing (at compiler level)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_object"></param>
        /// <returns></returns>
        public static object obj<T>(this T _object)    where T : class
        {
            return _object;
        }
    }
}
