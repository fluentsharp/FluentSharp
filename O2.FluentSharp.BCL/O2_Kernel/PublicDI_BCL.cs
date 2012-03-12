using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.Kernel
{
    public class PublicDI_BCL
    {

        static PublicDI_BCL()
        {
            o2MessageQueue = (KO2MessageQueue)DI.o2MessageQueue;            
        }
        public static KO2MessageQueue o2MessageQueue { get; set; }    
    }
}
