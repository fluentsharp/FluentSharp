namespace FluentSharp.CoreLib
{
    public static class Long_ExtensionMethods
    {		
        public static long      toLong(this double _double)
        {
            return (long)_double;
        }		
        public static long      add(this long _long, long value)
        {
            return _long + value;
        }				
    }
}