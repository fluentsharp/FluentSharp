using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.InterfacesBaseImpl;
using O2.Interfaces.Messages;
using O2.Interfaces.O2Core;
using O2.Kernel.CodeUtils;

namespace O2.Kernel
{
    internal static class DI
    {
        static DI()
        {
            o2MessageQueue  = KO2MessageQueue.getO2KernelQueue();
            log             = new KO2Log();
            config          = O2ConfigLoader.getKO2Config();
            reflection      = new KReflection();            

        }

        public static IO2Config config          { get; set; }
        public static IO2Log log                { get; set; }
        public static IReflection reflection    { get; set; }        

        public static IO2MessageQueue o2MessageQueue { get; set; }        
    }
}
