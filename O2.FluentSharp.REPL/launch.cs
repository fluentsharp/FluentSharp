

using FluentSharp.REPL.Controls;

namespace FluentSharp.REPL.Utils
{
    public class O2Launch
    {
        //public static string O2_Execution_Folder { get; set; }

        /*static O2Launch()
        {
            O2_Execution_Folder = AppDomain.CurrentDomain.BaseDirectory;
        }*/

        public static void o2Gui(string[] args)
        {
            //var assembly = "FluentSharp.REPL.exe".assembly();
            //var type = assembly.type("ascx_Execute_Scripts");
            //type.invokeStatic("startControl_With_Args",new object[] { args});
            ascx_Execute_Scripts.startControl_With_Args(args);
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
