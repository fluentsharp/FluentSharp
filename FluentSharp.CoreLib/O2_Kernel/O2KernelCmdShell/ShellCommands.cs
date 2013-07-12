// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

//O2File:../PublicDI.cs


namespace FluentSharp.CoreLib.API
{
    public class ShellCommands
    {
        
/*        public static string hello()
        {
            sendMessage("in hello");
            return "World";
        }

        public static string sendMessage(string messageText)
        {            
            PublicDI.o2MessageQueue.sendMessage(messageText);
            return "message sent: " + messageText;
        }*/

        public static string getName()
        {
            return PublicDI.O2KernelProcessName;
        }

        public static void setName(string newName)
        {
            PublicDI.O2KernelProcessName = newName;
        }

        public static void echo(string echoMessage)
        {
            if (PublicDI.o2Shell != null)
                PublicDI.o2Shell.shellIO.writeLine(echoMessage);
        }

        public static void appDomains()
        {
            PublicDI.o2Shell.shellIO.writeLine("\nThere are {0} AppDomains currently hosted by this O2Kernel process", O2AppDomainFactory.AppDomains_ControledByO2Kernel.Count);
            foreach (var hostedAppDomains in O2AppDomainFactory.AppDomains_ControledByO2Kernel.Keys)
                PublicDI.o2Shell.shellIO.writeLine("\t{0}", hostedAppDomains);
        }

        public static void filesInAppDomain(string appDomainName)
        {
            if (false == O2AppDomainFactory.AppDomains_ControledByO2Kernel.ContainsKey(appDomainName))
                PublicDI.o2Shell.shellIO.writeLine("AppDomain not found {0}", appDomainName);
            {
                var assemblies = O2AppDomainFactory.AppDomains_ControledByO2Kernel[appDomainName].FilesInAppDomainBaseDirectory;
                PublicDI.o2Shell.shellIO.writeLine("\nThere are {0} files in the AppDomain ", assemblies.Count , appDomainName);
                foreach(var assemblyName in assemblies)
                    PublicDI.o2Shell.shellIO.writeLine("\t{0}", assemblyName);
            }
        }

    }
}
