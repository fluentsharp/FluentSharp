using System.IO;
using System.Threading;

namespace FluentSharp.CoreLib
{
    public static class AutoResetEvents_Extensionmethods
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
            if (autoResetEvent.notNull())
                autoResetEvent.Reset();
            return autoResetEvent;
        }
        public static AutoResetEvent set(this AutoResetEvent autoResetEvent)
        {
            if (autoResetEvent.notNull())
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
