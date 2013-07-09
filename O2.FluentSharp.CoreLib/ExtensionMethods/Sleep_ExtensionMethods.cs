using System;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Sleep_ExtensionMethods
    {                    
        public static void	sleep(this object _object, int miliseconds)
        {
            Processes.Sleep(miliseconds);
        }
        public static void	sleep(this object _object, int miliseconds, bool verbose)
        {                        
            Processes.Sleep(miliseconds, verbose);
        }
        public static void	sleep(this object _object, int miliSeconds, Action toInvokeAfterSleep)
        {
            _object.sleep(miliSeconds, false, toInvokeAfterSleep);
        }
        public static void	sleep(this object _object, int miliSeconds, bool verbose, Action toInvokeAfterSleep)
        {
            O2Thread.mtaThread(
                () =>
                {
                    _object.sleep(miliSeconds, verbose);
                    toInvokeAfterSleep();
                });
        }
        public static T		wait<T>(this T _object)
        {
            if (_object is Int32)
                return _object.wait(Int32.Parse(_object.str()), true);
            return _object.wait(1000, true);
        }
        public static T		wait<T>(this T _object, int length)
        {
            return _object.wait(length, true);
        }
        public static T		wait<T>(this T _object, int length, bool verbose)
        {
            _object.sleep(length, verbose);
            return _object;
        }
        public static T		wait_n_Seconds<T>(this T _object, int seconds)
        {
            return _object.wait(seconds * 1000);
        }
        
    }
}
