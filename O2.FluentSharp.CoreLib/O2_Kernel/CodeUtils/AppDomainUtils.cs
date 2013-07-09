// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentSharp.CoreLib.Interfaces;


namespace FluentSharp.CoreLib.API
{
    public class AppDomainUtils
    {
        private static IO2Log log = new KO2Log("AppDomainUtils");

        public static string findDllInCurrentAppDomain(string dllToFind)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (assembly.GetName().Name == dllToFind)
                    return assembly.Location;
            log.error("in findDllInCurrentAppDomain, could not find {0} in the current AppDomain assemblies", dllToFind);
            return "";
        }

        public static IEnumerable<string> getDllsInCurrentAppDomain_FullPath()
        {
            var dllList = new List<String>();
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
                dllList.Add(assembly.Location);
            return dllList;
            //return from assembly in AppDomain.CurrentDomain.GetAssemblies() select assembly.Location;                                            
        }

        public static void closeAppDomain(AppDomain appDomain, bool deleteFilesInBaseDirectory)
        {
            log.info("Unloading AppDomain:{0}", appDomain.FriendlyName);
            string baseDirectory = appDomain.BaseDirectory;
            AppDomain.Unload(appDomain);
            if (deleteFilesInBaseDirectory)
            {
                log.info("Deleting all files from AppDomain BaseDirectory : {0}", baseDirectory);
                Directory.Delete(baseDirectory, true);
            }
        }

        public static void renameCurrentO2KernelProcessName(string newO2KernelProcessName)
        {
            var o2AppDomainFactory = O2AppDomainFactory.AppDomains_ControledByO2Kernel[PublicDI.O2KernelProcessName];
            //o2AppDomainFactory.appDomain.FriendlyName = newAppDomainName; // can't do this since there is no Setter for the FiendlyName property
            O2AppDomainFactory.AppDomains_ControledByO2Kernel.Remove(newO2KernelProcessName);
            PublicDI.O2KernelProcessName = newO2KernelProcessName;
            O2AppDomainFactory.AppDomains_ControledByO2Kernel.Add(PublicDI.O2KernelProcessName, o2AppDomainFactory);
        }

        public static void registerCurrentAppDomain()
        {
            try
            {
                O2AppDomainFactory.AppDomains_ControledByO2Kernel.Add(PublicDI.O2KernelProcessName, new O2AppDomainFactory(AppDomain.CurrentDomain));
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in registerCurrentAppDomain: {0}", ex.Message);
            }
            
        }

        public static O2AppDomainFactory getO2AppDomainFactoryForCurrentO2Kernel()
        {
            return O2AppDomainFactory.AppDomains_ControledByO2Kernel[PublicDI.O2KernelProcessName];
        }        
    }
}
