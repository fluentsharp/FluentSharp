using System;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class LiveObjects_And_Cache_Extensionmethods
    { 
        public static T         castIfType<T>(this object _object)
        {
            if (_object is T)
                return (T)_object;
            return default(T);
        }		
        public static object    liveObject(this string key, object value)
        {
            O2LiveObjects.set(key,value);
            return value;
        }		
        public static object    liveObject(this string key)
        {
            return O2LiveObjects.get(key);	
        }				
        public static object    o2Cache(this string key)
        {
            return key.liveObject();
        }		
        public static object    o2Cache(this string key, object value)
        {
            return key.liveObject(value);
        }		
        public static T         o2Cache<T>(this string key, T value)
        {
            O2LiveObjects.set(key,value);
            return value;
        }		
        public static T         o2Cache<T>(this string key)
        {
            var value =  O2LiveObjects.get(key);
            if (value is T)
                return (T)value;
            return default(T);
        }		
        public static T         o2Cache<T>(this string key, Func<T> ctor)  where T : class
        {
            try
            {
                if (key.o2Cache<T>().isNull())
                {
                    "there was no o2Chache object for type '{0}' so invoking the contructor callback".info(typeof(T));
                    key.o2Cache(ctor());
                }
                return key.o2Cache<T>();
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return default(T);			
        }
        public static T			o2Cache<T>(this string key, bool resetValue, Func<T> ctor) where T : class
        {
            if (resetValue)
                key.o2Cache(null);
            return key.o2Cache(ctor);
        }
    }
}