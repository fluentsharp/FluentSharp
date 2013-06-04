using System;
using JetBrains.Annotations;
using O2.Kernel;
using System.Drawing;
using System.Threading;

namespace FluentSharp.ExtensionMethods
{
    public static class Misc_ExtensionMethods_Int
    {
        public static int       sleep(this int sleepPeriod)
        {
            Thread.Sleep(sleepPeriod);
            return sleepPeriod;
        }
        public static double    kBytes(this int value)
        {
            return (double)value / 1024;
        }
        public static double    mBytes(this int value)
        {
            return (double)value / (1024 * 1024);
        }
        public static double    gBytes(this int value)
        {
            return (double)value / (1024 * 1024 * 1024);
        }
        public static string    kBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.kBytes());
        }
        public static string    mBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.mBytes());
        }
        public static string    gBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.gBytes());
        }
        public static bool      eq(this int value1, int value2)
        {
            return value1 == value2;
        }
        public static bool      neq(this int value1, int value2)
        {
            return value1 != value2;
        }
        public static uint      uInt(this int _int)
        {
            return (uint)_int;
        }
        public static bool      isEven(this int value)
        {
            return (value % 2) == 0;
        }
        public static bool      isOdd(this int value)
        {
            return (value % 2) != 0;
        }

    }

    public static class Misc_ExtensionMethods_Bool
    {        
        [ContractAnnotation("false => true ; true => false")]
        public static bool      isFalse(this bool value)
        {
            return value == false;
        }
        [ContractAnnotation("true => true ; false => false")]
        public static bool      isTrue(this bool value)
        {
            return value;
        }
        public static bool      and(this bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }
        public static bool      or(this bool leftOperand, bool rightOperand)
        {
            return leftOperand || rightOperand;
        }
        public static bool      not(this bool value)
        {
            return !value;
        }
        public static bool      failed(this bool value)
        {
            return value.isFalse();
        }
        public static bool      ok(this bool value)
        {
            return value.isTrue();
        }
    }

    public static class Misc_ExtensionMethods_BitMap
    {
        public static Bitmap    bitmap(this string file)
        {
            if (file.fileExists())
                return new Bitmap(file);
            return null;
        }
        public static Bitmap    asBitmap(this Image image)
		{
			return image as Bitmap;				
		}				
    }


    public static class Misc_Extensionmethods_LiveObjects_and_Cache
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
		public static T         o2Cache<T>(this string key, Func<T> ctor)
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
		public static T			o2Cache<T>(this string key, bool resetValue, Func<T> ctor)
		{
			if (resetValue)
				key.o2Cache(null);
			return key.o2Cache(ctor);
		}
    }

    public static class Misc_Extensionmethods_AutoResetEvents
    {
        public static AutoResetEvent sync(this bool value)
        {
            return value.autoResetEvent();
        }
        public static AutoResetEvent autoResetEvent(this bool value)
        {
            return new AutoResetEvent(value);
        }

        public static AutoResetEvent reset(this AutoResetEvent autoResetEvent)
        {
            autoResetEvent.Reset();
            return autoResetEvent;
        }
        public static AutoResetEvent set(this AutoResetEvent autoResetEvent)
        {
            autoResetEvent.Set();
            return autoResetEvent;
        }
        public static bool waitOne(this AutoResetEvent autoResetEvent)
        {
            return autoResetEvent != null && autoResetEvent.WaitOne();
        }

        public static bool waitOne(this AutoResetEvent autoResetEvent, int timeOut)
        {
            return autoResetEvent != null && autoResetEvent.WaitOne(timeOut);
        }
    }
}
