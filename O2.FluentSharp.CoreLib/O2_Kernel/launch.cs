using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.Kernel
{
    public class launch
    {
        public static string O2_Execution_Folder { get; set; }

        static launch()
        {
            O2_Execution_Folder = AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void o2Gui(string[] args)
        {
            var assembly = "O2_FluentSharp_REPL.dll".assembly();
            var type = assembly.type("ascx_Execute_Scripts");
            type.invokeStatic("startControl_With_Args",new object[] { args});
        }

/*        public static Assembly load_O2_Assembly(string assemblyName)
        {
            var fullPath = Path.Combine(O2_Execution_Folder, assemblyName);

            return Assembly.LoadFrom(fullPath);

        }

        public void loadDependencies()
        {
            
        }

        

        public void startO2(string[] args)
        {
            loadDependencies();
            //complileO2StartupScriptAndExecuteIt(args);
        }*/
    }
}
