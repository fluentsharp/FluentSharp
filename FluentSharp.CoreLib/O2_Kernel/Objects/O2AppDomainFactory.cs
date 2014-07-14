// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace FluentSharp.CoreLib.API
{
    [Serializable]
    public class O2AppDomainFactory
    {

        public static Dictionary<string, O2AppDomainFactory> AppDomains_ControledByO2Kernel { get; set; }

        public String BaseDirectory { get; set; }
        public AppDomain AppDomain { get; set; }

        static O2AppDomainFactory()
        {
            AppDomains_ControledByO2Kernel = new Dictionary<string, O2AppDomainFactory>();
        }

        public O2AppDomainFactory(AppDomain appDomain)
        {
            AppDomain = appDomain;
        }

        // if none is provide then create one a tempfolder in the O2 Temp Dir * give it a random name
        public O2AppDomainFactory() : this(PublicDI.config.TempFolderInTempDirectory, Path.GetFileName(PublicDI.config.TempFolderInTempDirectory))
        {
            /*string appDomainBaseDirectory = PublicDI.config.TempFolderInTempDirectory;
            var appDomainSetup = new AppDomainSetup {ApplicationBase = appDomainBaseDirectory};
            string appDomainName = Path.GetFileName(appDomainBaseDirectory);
            createAppDomain(appDomainName, appDomainSetup);*/
        }

        public O2AppDomainFactory(string appDomainName) : this (appDomainName, AppDomain.CurrentDomain.BaseDirectory)
        {        
        }

        public O2AppDomainFactory(string appDomainName, string baseDirectory)
        {
            var appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = baseDirectory;
			appDomainSetup.PrivateBinPath = baseDirectory; 
			appDomainSetup.ShadowCopyFiles = "true";                                                                                                               
            createAppDomain(appDomainName, appDomainSetup);            
        }

        public O2AppDomainFactory(string appDomainName, string baseDirectory, List<string> assembliesToLoadInNewAppDomain) : this(appDomainName, baseDirectory)
        {
            AppDomain.loadAssembliesIntoAppDomain(assembliesToLoadInNewAppDomain);
        }    
        
        // Properties

        public string Name
        {
            get { return AppDomain.notNull() ? AppDomain.FriendlyName : ""; }
        }

        public List<string> FilesInAppDomainBaseDirectory
        {
            get
            {
                if (BaseDirectory.notNull())
                    return new List<string>(Directory.GetFiles(BaseDirectory));
                return new List<string>();
            }
        }

        // Methods

        public AppDomain createAppDomain(string appDomainName)
        {
            return createAppDomain(appDomainName, null);
        }

        public AppDomain createAppDomain(string appDomainName, AppDomainSetup appDomainSetup)
        {
            try
            {
                if (AppDomains_ControledByO2Kernel.ContainsKey(appDomainName))
                    PublicDI.log.error("in createAppDomain, appDomainName provided has already been used, appDomainNames must be unique: {0}", appDomainName);
                else
                {


                    PublicDI.log.info("Creating AppDomain {0} with Base Directory {1}", appDomainName,
                                appDomainSetup.ApplicationBase);
                    // ensure target directory exits
                    O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(appDomainSetup.ApplicationBase);

                    // give our appDomain full trust :)
                    var permissionSet = new PermissionSet(PermissionState.Unrestricted);

                    AppDomains_ControledByO2Kernel.Add(appDomainName, this);
                    
                    //Create domain
                    AppDomain = AppDomain.CreateDomain(appDomainName, null, appDomainSetup, permissionSet);
            //        appDomain.AssemblyResolve += new ResolveEventHandler(assemblyResolve);
                    BaseDirectory = AppDomain.BaseDirectory;

                    AppDomain.DomainUnload += (sender, e) => this.removeDomainFromManagedList();
                    return AppDomain;
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "could not load createAppDomain: " + appDomainName);
            }
            return null;
        }

        /*static Assembly assemblyResolve(object sender, ResolveEventArgs args)
        {

            //return Assembly.LoadFile(GetAssemblyFileName());
            return null;
        } */

               
        /// <summary>
        /// This will generate an anonymous appDomain proxy (good for tests but _note that the AppDomain will not be terminated
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeToCreateSimpleName"></param>
        /// <returns></returns>
        public static object getProxy(string assemblyName, string typeToCreateSimpleName)
        {
            string appDomainName = Path.GetFileNameWithoutExtension(PublicDI.config.TempFileNameInTempDirectory) + "_" + typeToCreateSimpleName;        // make sure each appDomainName is unique
            var tempAppDomainFactory = new O2AppDomainFactory(appDomainName);
            return tempAppDomainFactory.appDomain().getProxyObject(typeToCreateSimpleName);
        }

        public static void create(string appDomainName)
        {
            new O2AppDomainFactory(appDomainName);
        }

        public void removeDomainFromManagedList()
        {
            if (AppDomains_ControledByO2Kernel.ContainsKey(this.Name))
            {
                "Removing AppDomain '{0}' from AppDomains_ControledByO2Kernel list".info(this.Name); 
                AppDomains_ControledByO2Kernel.Remove(this.Name);
            }
        }

        
    }
}
