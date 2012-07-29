// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using O2.DotNetWrappers.Windows;
using O2.Interfaces.O2Core;
using O2.Kernel;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.Views.ASCX
{
    internal class DI
    {        

        static DI()
        {
            config = PublicDI.config;   // need to use local copy
            log = PublicDI.log;

            reflection = new O2FormsReflectionASCX();                        
                       
        }

        // DI which will need to be injected 

        public static KO2Config config { get; set; } // = new O2ViewsAscxConfig();
        public static IO2Log log { get; set; }

        public static O2FormsReflectionASCX reflection; 
        
      //  public static IReflectionASCX reflectionASCX { get; set; }        
        
    }
}
