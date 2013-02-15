// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Interfaces.O2Core;

namespace O2.Views.ASCX.SourceCodeEdit.ScriptSamples
{
    public class HelloWorlds
    {
        public static IO2Log log = O2.Kernel.PublicDI.log;

      
        public static string sayHelloO2World()
        {
            string message = string.Format("Hello O2 World");
            log.info(message);
            return message;
        }

        public static string sayHellotoMe(string myName, bool showInMessageBox)
        {
            string message = string.Format("Hello {0}", myName);
            log.info(message);
            //if (showInMessageBox)
            //    log.showMessageBox(message);
            return message;
        }

        // chose EXE option on the C#Scripts GUI to start invocation from this method
        public static void Main()
        {
            log.info("from main {0}", sayHelloO2World());
        }
    }

}
