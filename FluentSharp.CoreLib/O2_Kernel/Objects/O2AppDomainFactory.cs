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
            loadAssembliesIntoAppDomain(assembliesToLoadInNewAppDomain);
        }               

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

        public void loadAssembliesIntoAppDomain(List<string> assembliesToLoad)
        {
            var assembliesInNewAppDomain = new List<String>();
            // first copy the assemblies
            foreach(var assemblyToLoad in assembliesToLoad)
            {                
                if (Path.GetExtension(assemblyToLoad).ToLower() == ".dll" || Path.GetExtension(assemblyToLoad).ToLower() == ".exe") // since other wise these might be GAC assemblies
                {
                    if (File.Exists(assemblyToLoad))
                    {
                        var targetFileName = Path.Combine(AppDomain.BaseDirectory, assemblyToLoad.fileName());
						if (targetFileName.fileExists().isFalse())
							O2Kernel_Files.Copy(assemblyToLoad, targetFileName);
                        assembliesInNewAppDomain.Add(targetFileName);
                    }
                    else
                    {
                        var resolvedName = Path.Combine(PublicDI.config.CurrentExecutableDirectory, assemblyToLoad);
                        if (File.Exists(resolvedName))
                        {
                            var targetFileName = Path.Combine(AppDomain.BaseDirectory, resolvedName.fileName());
							if (targetFileName.fileExists().isFalse())
								O2Kernel_Files.Copy(resolvedName, targetFileName);
                            assembliesInNewAppDomain.Add(targetFileName);
                        }
                        else
                            PublicDI.log.error("in loadAssembliesIntoAppDomain , could not find dll to copy: {0}",
                                         assemblyToLoad);
                    }
                }
            }
            // then load them (and if there are no missing dependencies ALL should load ok
            foreach (var assemblyToLoad in assembliesInNewAppDomain)
                try
                {
                    AppDomain.Load(Path.GetFileNameWithoutExtension(assemblyToLoad));
                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex,
                              "in O2AppDomainFactory.loadAssembliesIntoAppDomain, could not load assembly: " +
                              assemblyToLoad);                    
                }
                
        }

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
        /// Syntax: getProxyMethod({method} {type} {assembly})
        public object getProxyMethod(string methodToCreate)
        {
            return getProxyMethod(methodToCreate, new object[0]);
        }        
        /// Syntax: getProxyMethod({method} {type} {assembly})        
        public object getProxyMethod(string methodToCreate, object[] methodParameters)
        {
            try
            {
                string[] splitedType = methodToCreate.Split(' ');
                if (splitedType.Length != 3)
                    PublicDI.log.error(
                        "in getProxyMethod, wrong format provided (syntax:  getProxyMethod({method} {type} {assembly}) ) :  " +
                        methodToCreate);
                else
                {
                    string methodName = splitedType[0];
                    string typeName = splitedType[1];
                    string assemblyName = splitedType[2];

                    var proxyObject = AppDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
                    if (proxyObject.isNull())
                        PublicDI.log.error("in getProxyMethod, could not create proxy:{0} in assembly {1}", typeName,
                                  assemblyName);
                    else
                    {
                        MethodInfo methodInfo = PublicDI.reflection.getMethod(proxyObject.GetType(), methodName,
                                                                        methodParameters);
                        if (methodInfo == null)
                            PublicDI.log.error("in getProxyMethod, could not find method {0} in type {1}", methodName,
                                      proxyObject.GetType());

                        return methodInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in getProxyMethod, error creating: " + methodToCreate);
            }
            return null;
        }        
		/// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}    // Use this one for the cases where only the Type is needed
        public Type getProxyType(string rawTypeOfProxyToCreate)
        {
            string assemblyName = "";
            string typeOfProxyToCreate = "";
            return getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate); 
        }
        
        /// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}        
        public Type		getProxyType(string rawTypeOfProxyToCreate, ref string assemblyName, ref string typeOfProxyToCreate)
        {
            try
            {

                string[] splitedType = rawTypeOfProxyToCreate.Split(' ');
                if (splitedType.Length == 2)
                {
                    rawTypeOfProxyToCreate = splitedType[0];
                    assemblyName = splitedType[1];
                    // check if we can load the assembly and the requested type is there
                    Assembly loadedAssembly = AppDomain.Load(assemblyName);
                    Type foundProxyType = PublicDI.reflection.getType(loadedAssembly, rawTypeOfProxyToCreate);
                    if (foundProxyType == null)
                    {
                        PublicDI.log.error("Could not find type {0} in assembly {1}", rawTypeOfProxyToCreate, assemblyName);
                        return null;
                    }
                    typeOfProxyToCreate = foundProxyType.FullName;
                    return foundProxyType;

                }

                // add support for just passing in the simple name of the type to create (this is will use the first one found)
                foreach (Assembly assembly in AppDomain.GetAssemblies())
                {
                    Type foundProxyType = PublicDI.reflection.getType(assembly, rawTypeOfProxyToCreate);
                    if (foundProxyType != null && assembly != null)
                    {
                        assemblyName = assembly.FullName;
                        typeOfProxyToCreate = foundProxyType.FullName;
                        return foundProxyType;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.log("error creating: " + typeOfProxyToCreate);
            }
            "could not create object: ".error(typeOfProxyToCreate);
            return null;
        }        
        /// Syntax:   getProxyObject({type} {assembly})  or getProxyType({type}}        
        public object		getProxyObject(string rawTypeOfProxyToCreate)
        {

            string assemblyName = "";
            string typeOfProxyToCreate = "";
            try
            {
                var foundProxyType = getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate);
                if (foundProxyType != null)
                {
                    object proxy = AppDomain.CreateInstanceAndUnwrap(assemblyName, typeOfProxyToCreate);
                    if (proxy.notNull())
                        return proxy;
                }
                "could not create object: ".error(rawTypeOfProxyToCreate);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "error creating object: " + rawTypeOfProxyToCreate);
            }            
            return null;
            //return appDomain.CreateInstanceAndUnwrap(dllToLoad, typeToCreateAndUnwrap);
        }        
        /// Syntax: invokeMethod({method} {type} {assembly})=        
        public object		invokeMethod(string methodToInvoke)
        {
            return invokeMethod(methodToInvoke, new object[0]);
        }
		/// Syntax: getProxyMethod({method} {type} {assembly})
        public object		invokeMethod(string methodToInvoke, object[] methodParameters)
        {
            try
            {
                string[] splitedType = methodToInvoke.Split(' ');
                if (splitedType.Length != 3)
                    PublicDI.log.error(
                        "in invokeMethod, wrong format provided (syntax:  getProxyMethod({method} {type} {assembly}) ) :  " +
                        methodToInvoke);
                else
                {
                    string methodName = splitedType[0];
                    string typeName = splitedType[1];
                    string assemblyName = splitedType[2];
                    object proxyObject = getProxyObject(typeName + " " + assemblyName);

                    // var proxyObject = appDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
                    if (proxyObject == null)
                        PublicDI.log.error("in invokeMethod, could not create proxy:{0} in assembly {1}", typeName, assemblyName);
                    else
                    {
                        MethodInfo methodInfo = PublicDI.reflection.getMethod(proxyObject.GetType(), methodName,
                                                                        methodParameters);
                        if (methodInfo == null)
                            PublicDI.log.error("in invokeMethod, could not find method {0} in type {1}", methodName,
                                      proxyObject.GetType());

                        return PublicDI.reflection.invoke(proxyObject, methodInfo, methodParameters);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in invokeMethod, error creating: " + methodToInvoke);
            }
            return null;
        }
        public bool			load(string assemblyName)
        {
            return load(assemblyName, false);
        }
        public bool			load(string assemblyName, bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            return load(assemblyName, assemblyName, copyToAppDomainBaseDirectoryBeforeLoad);
        }
        public bool			load(string fullAssemblyName, string pathToAssemblyToLoad,bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            try
            {
                // copy if asked to
                if (copyToAppDomainBaseDirectoryBeforeLoad)
                    try
                    {
                        if (File.Exists(pathToAssemblyToLoad))
                            O2Kernel_Files.Copy(pathToAssemblyToLoad, AppDomain.BaseDirectory);
                        else
                            PublicDI.log.error(
                                "copyToAppDomainBaseDirectoryBeforeLoad was set but pathToAssemblyToLoad was set to a file that didn't exist: {0}",
                                pathToAssemblyToLoad);
                    }
                    catch (Exception ex)
                    {
                        ex.log("in load copyToAppDomainBaseDirectoryBeforeLoad");
                    }
                // load assembly into AppDomain
                //First try directly
                AppDomain.Load(fullAssemblyName);                
            }
            catch (Exception ex1)
            {
                //then try using AssemblyName
                try
                {
                    AppDomain.Load(AssemblyName.GetAssemblyName(fullAssemblyName));
                }
                catch (Exception ex2)
                {
                    // last change load assembly into current appdomain to get its full name and try again
                    try
                    {
                        AppDomain.Load(fullAssemblyName.assembly().FullName);
                    }
                    catch (Exception ex3)
                    {
                        PublicDI.log.ex(ex1, "could not load assembly (method1): " + fullAssemblyName);
                        PublicDI.log.ex(ex2, "could not load assembly (method2): " + fullAssemblyName);
                        PublicDI.log.ex(ex3, "could not load assembly (method3): " + fullAssemblyName);
                        return false;
                    }
                }
            }
            return true;
        }
        public object		createAndUnWrap(string dllWithType, string typeToCreateAndUnwrap)
        {
            return AppDomain.CreateInstanceAndUnwrap(dllWithType, typeToCreateAndUnwrap);
        }
        public List<String> getAssemblies(bool showFulName)
        {
            var assemblies = new List<String>();
            try
            {
                if (AppDomain != null)
                    foreach (Assembly assembly in AppDomain.GetAssemblies())
                        if (showFulName)
                            assemblies.Add(assembly.FullName);
                        else
                            assemblies.Add(assembly.GetName().Name);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex,"in O2AppDomainFactory getAssemblies");
            }
            return assemblies;
        }
        public object		invoke(string proxyObjectTypeAndAssembly, string methodToInvoke, object[] methodParameters)
        {
            var proxyObject = getProxyObject(proxyObjectTypeAndAssembly);
            return invoke(proxyObject, methodToInvoke, methodParameters);
        }
        public object invoke(object proxyObject, string methodToInvoke, object[] methodParameters)
        {
            // if the proxyObject is a string we need to resolve it into its final O2 object
            if (proxyObject is string)
                proxyObject = getProxyObject((string)proxyObject);
            if (proxyObject == null)
                PublicDI.log.error("Provided proxyObject variable was null!");
            else
            {
                MethodInfo methodInfo = PublicDI.reflection.getMethod(proxyObject.GetType(), methodToInvoke, methodParameters);
                if (methodInfo == null)
                    PublicDI.log.error("in invoke, could not find the requested method (please check the method name & provided parameters).  methodInfo == null for proxyObjectTypeAndAssembly:{0} , methodToInvoke {1}", proxyObject.ToString(), methodToInvoke);
                else
                    return PublicDI.reflection.invoke(proxyObject, methodInfo, methodParameters);
            }
            return null;
        }
        public object invoke(object proxyObject, string methodToInvoke)
        {
            return invoke(proxyObject, methodToInvoke, new object[0]);
        }


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
            return tempAppDomainFactory.getProxyObject(typeToCreateSimpleName);
        }

        public Dictionary<string, string> load(Dictionary<string, string> assemblyDependencies)
        {
            var assemblyDependenciesNotLoaded = new Dictionary<string, string>();
            foreach (string assemblyDependency in assemblyDependencies.Keys)
                if (!load(assemblyDependency, assemblyDependencies[assemblyDependency], true))
                    assemblyDependenciesNotLoaded.Add(assemblyDependency, assemblyDependencies[assemblyDependency]);
            return assemblyDependenciesNotLoaded;
        }

        #region SimpleProxy Invoke Helper methods

        // these will invoke commands on the target AppDomain via the O2Proxy instanceInvocation method        

        // invoke instance methods
        public object proxyInvokeInstance(string assembly, string type, string method)
        {
            return proxyInvokeInstance(assembly, type, method, new object[0]);
        }

        public object proxyInvokeInstance(string assembly, string type, string method, object[] methodParameters)
        {
            return proxyInvokeInstance(new object[] {assembly, type, method, methodParameters});
        }

        public object proxyInvokeInstance(object[] methodParameters)
        {
            return invokeMethod("instanceInvocation O2Proxy O2_Kernel", methodParameters);
        }

        // invoke static methods

        public object proxyInvokeStatic(string assembly, string type, string method)
        {
            return proxyInvokeStatic(assembly, type, method, new object[0]);
        }

        public object proxyInvokeStatic(string assembly, string type, string method, object[] methodParameters)
        {
            return proxyInvokeStatic(new object[] {assembly, type, method, methodParameters});
        }

        public object proxyInvokeStatic(object[] methodParameters)
        {
            return invokeMethod("staticInvocation O2Proxy O2_Kernel", methodParameters);
        }

        #endregion

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

        public bool unLoadAppDomain()
        {
            try
            {
                PublicDI.log.info("Forcibly Unloading appDomain: {0}", Name);
                //removeDomainFromManagedList();
                AppDomain.Unload(AppDomain);                
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in O2AppDomainFactory.unLoadAppDomain");
                return false;
            }            
        }

        public bool unLoadAppDomainAndDeleteTempFolder()
        {
            try
            {
                var appDomainBaseDirectory = AppDomain.BaseDirectory;// we need to get this value before we unload the appDomain
               if (unLoadAppDomain())
                {
                    PublicDI.log.info("Deleting (recursively) appDomain Base Directory: {0}", appDomainBaseDirectory);
                    Directory.Delete(appDomainBaseDirectory, true);
                }
                return false;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in O2AppDomainFactory.unLoadAppDomainAndDeleteTempFolder");
                return false;
            }            
        }
    }
}
