// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;


namespace FluentSharp.CoreLib.API
{
    public class O2Cmd
    {
        public static KO2Log log = PublicDI.log;

        public static string O2CmdName          { get; set; }
        public static string O2CmdPublishedDate { get; set; }

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
                foreach (var methodAvailable in PublicDI.reflection.getMethods(type, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
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
            PublicDI.log.info(message);
            Console.WriteLine(message);
        }*/

        
    }
}
