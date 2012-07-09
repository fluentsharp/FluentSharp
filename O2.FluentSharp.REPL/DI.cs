// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.DotNetWrappers.Windows;
using O2.Interfaces.O2Core;
using O2.Kernel;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.External.SharpDevelop
{
    internal class DI
    {        

        static DI()
        {
            config = PublicDI.config;
            log = PublicDI.log;
            //reflection = PublicDI.reflection;       
            reflection = new O2FormsReflectionASCX();

            new O2.DotNetWrappers.Zip.zipUtils();            // to force inclusion of the O2.FluentSharp.Misc in the compilation directory
        }

        // DI which will need to be injected 

        public static IO2Config config { get; set; } 
        public static IO2Log log { get; set; }

        public static O2FormsReflectionASCX reflection;                
        
        // public local global vars
        public static string sDefaultO2Scripts = @"_o2_Scripts\";
    }
}
