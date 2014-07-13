using System;

namespace FluentSharp.CoreLib
{
    public static class UInt_ExtensionMethods
    {
        public static uint      toUInt(this string value)
        {
            return UInt32.Parse(value);
        }
    }
}