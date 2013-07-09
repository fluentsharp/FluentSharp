using System;

namespace FluentSharp.CoreLib.API
{
    public static class Loop
    {
        public static void nTimesWithDelay(int count, int delay, Action methodInvoker)
        {
            nTimesWithDelay(count, delay, true, methodInvoker);
        }

        public static void nTimesWithDelay(int count, int delay, bool runInMtaThread, Action methodInvoker)
        {
            if (runInMtaThread)
                O2Thread.mtaThread(() => nTimesWithDelay(count, delay, false, methodInvoker));
            else
                for (int i = 0; i < count; i++)
                {
                    methodInvoker();
                    Processes.Sleep(delay);
                }
        }

        public static void nTimes(int count, Action<int> methodInvoker)
        {
            for (int i = 0; i < count; i++)
                methodInvoker(i);
        }
    }

}
