using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.InterfacesBaseImpl;
using O2.Interfaces.Messages;

namespace O2.Kernel
{
    public class PublicDI_BCL
    {
        //public static IO2MessageQueue o2MessageQueue { get; set; }    
    
        static PublicDI_BCL()
        {
            o2MessageQueue = KO2MessageQueue.getO2KernelQueue();
            //o2MessageQueue = (KO2MessageQueue)PublicDI.o2MessageQueue;            
        }
        public static KO2MessageQueue o2MessageQueue { get; set; }    
    }
}
