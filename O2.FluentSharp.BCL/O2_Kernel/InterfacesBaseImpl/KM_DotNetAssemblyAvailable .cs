// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using O2.Interfaces.Messages;

namespace O2.Kernel.InterfacesBaseImpl
{
    public class KM_DotNetAssemblyAvailable : KO2Message, IM_DotNetAssemblyAvailable 
    {        
        public string pathToAssembly { get; set; }

        public KM_DotNetAssemblyAvailable(string _pathToAssembly)
        {            
            messageText = "KM_DotNetAssemblyAvailable";
            pathToAssembly = _pathToAssembly;
        }
    }
}
