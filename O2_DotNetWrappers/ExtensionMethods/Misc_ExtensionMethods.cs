using System;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using System.Drawing;
using O2.DotNetWrappers.DotNet;
using System.Threading;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Misc_ExtensionMethods
    {
        #region int

        public static int sleep(this int sleepPeriod)
        {
            Thread.Sleep(sleepPeriod);
            return sleepPeriod;
        }
        public static double kBytes(this int value)
        {
            return (double)value / 1024;
        }

        public static double mBytes(this int value)
        {
            return (double)value / (1024 * 1024);
        }

        public static double gBytes(this int value)
        {
            return (double)value / (1024 * 1024 * 1024);
        }

        public static string kBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.kBytes());
        }

        public static string mBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.mBytes());
        }

        public static string gBytesStr(this int value)
        {
            return "{0:000.00} kb".format(value.gBytes());
        }

        public static bool eq(this int value1, int value2)
        {
            return value1 == value2;
        }

        public static bool neq(this int value1, int value2)
        {
            return value1 != value2;
        }

        public static uint uInt(this int _int)
        {
            return (uint)_int;
        }

        public static bool isEven(this int value)
        {
            return (value % 2) == 0;
        }

        public static bool isOdd(this int value)
        {
            return (value % 2) != 0;
        }

        #endregion

        #region bool

        public static bool isFalse(this bool value)
        {
            return value == false;
        }

        public static bool isTrue(this bool value)
        {
            return value == true;
        }

        public static bool and(this bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }

        public static bool or(this bool leftOperand, bool rightOperand)
        {
            return leftOperand || rightOperand;
        }

        public static bool not(this bool value)
        {
            return !value;
        }

        #endregion

        public static Bitmap bitmap(this string file)
        {
            if (file.fileExists())
                return new Bitmap(file);
            return null;
        }
    }
}
