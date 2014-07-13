using System;

namespace FluentSharp.CoreLib
{
    public static class Int_ExtensionMethods
    {		
        public static int       toInt(this double _double)
        {
            return (int)_double;
        }		
        public static int       mod( this int num1, int num2)
        {
            return num1 % num2;
        }
        public static bool      mod0( this int num1, int num2)
        {
            return num1.mod(num2) ==0;
        }		
        public static string    intToBinaryString(this int number)
        {
            return Convert.ToString(number,2);
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
            return "{0:000.00} KB".format(value.kBytes());
        }
        public static string    mBytesStr(this int value)
        {
            return "{0:000.00} MB".format(value.mBytes());
        }
        public static string    gBytesStr(this int value)
        {
            return "{0:000.00} B".format(value.gBytes());
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
}