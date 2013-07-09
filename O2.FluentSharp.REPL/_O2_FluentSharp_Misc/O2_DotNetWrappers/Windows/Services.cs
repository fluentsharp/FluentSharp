using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//O2Ref:c:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\System.ServiceProcess.dll
using System.ServiceProcess;

namespace FluentSharp.REPL.Utils
{
    public class Services
    {
        public static List<ServiceController> getExistingServices()
        {
            return ServiceController.GetServices().ToList();
        }
    }
}
