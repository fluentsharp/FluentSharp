// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Threading;

namespace O2.Kernel.CodeUtils
{
    public class O2Kernel_O2Thread
    {
        #region Delegates

        public delegate void Func();

        #endregion

// they forgot to include this one :)

        public static Thread staThread(Func codeToExecute)
        {
            var staThread = new Thread(() => codeToExecute());
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            return staThread;
        }

        public static Thread mtaThread(Func codeToExecute)
        {
            var mtaThread = new Thread(() => codeToExecute());
            mtaThread.SetApartmentState(ApartmentState.MTA);
            mtaThread.Start();
            return mtaThread;
        }
    }
}
