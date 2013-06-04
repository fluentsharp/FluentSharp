using System;
using System.Diagnostics;
using System.Threading;

namespace FluentSharp.ExtensionMethods
{
    public static class Thread_Info_ExtensionMethods
    {
        public static string name(this Thread thread)
        {
            if (thread.notNull())
                return thread.Name;
            return null;
        }
        public static StackTrace stackTrace(this Thread thread)
        {
            if (thread.notNull())
                try
                {
                    return new StackTrace(thread, true);
                }
                catch (Exception ex)
                {
                    ex.log("[thread][stacktrace]");
                }                            
            return null;
        }
    }
}