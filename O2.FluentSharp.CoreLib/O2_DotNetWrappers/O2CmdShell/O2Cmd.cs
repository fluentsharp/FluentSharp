// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using O2.DotNetWrappers.Filters;
using O2.Interfaces.O2Core;
using O2.Kernel;


namespace O2.DotNetWrappers.O2CmdShell
{
    public class O2Cmd
    {
        public static IO2Log log = PublicDI.log;

        public static string O2CmdName;
        public static string O2CmdPublishedDate;

        static O2Cmd()
        {
            O2CmdName = "O2 Cmd line tool";
            O2CmdPublishedDate = "2009";
            log.alsoShowInConsole = true;
        }
        //'O2 Cmd FindingsFilter' - Small tool to filter findings"
        public static void welcome()
        {
            log.write("\n\n\n           Welcome to " + O2CmdName);
            log.write("                                  http://www.o2-ounceopen.com  ");
            log.write("                                       " + O2CmdPublishedDate);
            log.write("  \n\n\n");
        }

        public static void help()
        {
            log.info("These are the commands available:");
            foreach(var type in O2CmdApi.typesWithCommands)
            {
                log.info("\n\ton type: {0}\n", type.Name);
                foreach (var methodAvailable in DI.reflection.getMethods(type, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    // make sure the O2CmdHide attribute is not set
                    if (methodAvailable.GetCustomAttributes(typeof(O2CmdHide),false).Length == 0)
                        log.info("\t\t{0}", new FilteredSignature(methodAvailable).sFunctionNameAndParams);                    
            }
        }
         
       /*public static void log(string messageFormat)
        {
            log(messageFormat, "");
        }*/
        
       
        /*[O2CmdHide]   
        public static void log(string messageFormat, params object[] parameters)
        {
            string message = string.Format(messageFormat, parameters);
            DI.log.info(message);
            Console.WriteLine(message);
        }*/

        
    }
}
