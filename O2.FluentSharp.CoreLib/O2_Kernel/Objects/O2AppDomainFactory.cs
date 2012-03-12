// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using O2.Kernel.CodeUtils;
using O2.Kernel.ExtensionMethods;

//O2File:../DI.cs
//O2File:../CodeUtils/O2Kernel_Files.cs
//O2File:../CodeUtils/O2Kernel_Serialize.cs
//O2File:../ExtensionMethods/Logging_ExtensionMethods.cs
//O2File:../ExtensionMethods/Object_ExtensionMethods.cs 
//O2File:../ExtensionMethods/Reflection_ExtensionMethods.cs 


namespace O2.Kernel.Objects
{
    [Serializable]
    public class O2AppDomainFactory
    {
        public String BaseDirectory { get; set; }
        public AppDomain appDomain { get; set; }

        public O2AppDomainFactory(AppDomain _appDomain)
        {
            appDomain = _appDomain;
        }

        // if none is provide then create one a tempfolder in the O2 Temp Dir * give it a random name
        public O2AppDomainFactory()
            : this(DI.config.TempFolderInTempDirectory, Path.GetFileName(DI.config.TempFolderInTempDirectory))
        {
            /*string appDomainBaseDirectory = DI.config.TempFolderInTempDirectory;
            var appDomainSetup = new AppDomainSetup {ApplicationBase = appDomainBaseDirectory};
            string appDomainName = Path.GetFileName(appDomainBaseDirectory);
            createAppDomain(appDomainName, appDomainSetup);*/
        }

        public O2AppDomainFactory(string appDomainName) : this (appDomainName, AppDomain.CurrentDomain.BaseDirectory)
        {        
        }

        public O2AppDomainFactory(string appDomainName, string baseDirectory)
        {
            var appDomainSetup = new AppDomainSetup {
                                                        ApplicationBase = baseDirectory,
                                                        PrivateBinPath = baseDirectory, //PublicDI.config.CurrentExecutableDirectory,
                                                        ShadowCopyFiles = "true"                                                        
                                                    };            
            createAppDomain(appDomainName, appDomainSetup);            
        }

        public O2AppDomainFactory(string appDomainName, string baseDirectory, List<string> assembliesToLoadInNewAppDomain)
            : this(appDomainName, baseDirectory)
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
                if (DI.appDomainsControledByO2Kernel.ContainsKey(appDomainName))
                    DI.log.error("in createAppDomain, appDomainName provided has already been used, appDomainNames must be unique: {0}", appDomainName);
                else
                {


                    DI.log.info("Creating AppDomain {0} with Base Directory {1}", appDomainName,
                                appDomainSetup.ApplicationBase);
                    // ensure target directory exits
                    O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(appDomainSetup.ApplicationBase);

                    // give our appDomain full trust :)
                    var permissionSet = new PermissionSet(PermissionState.Unrestricted);

                    DI.appDomainsControledByO2Kernel.Add(appDomainName,this);
                    
                    //Create domain
                    appDomain = AppDomain.CreateDomain(appDomainName, null, appDomainSetup, permissionSet);
            //        appDomain.AssemblyResolve += new ResolveEventHandler(assemblyResolve);
                    BaseDirectory = appDomain.BaseDirectory;
                    return appDomain;
                }
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "could not load createAppDomain: " + appDomainName);
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
                        var targetFileName = Path.Combine(appDomain.BaseDirectory, Path.GetFileName(assemblyToLoad));
                        O2Kernel_Files.Copy(assemblyToLoad, targetFileName);
                        assembliesInNewAppDomain.Add(targetFileName);
                    }
                    else
                    {
                        var resolvedName = Path.Combine(DI.config.CurrentExecutableDirectory, assemblyToLoad);
                        if (File.Exists(resolvedName))
                        {
                            var targetFileName = Path.Combine(appDomain.BaseDirectory, Path.GetFileName(resolvedName));
                            O2Kernel_Files.Copy(resolvedName, targetFileName);
                            assembliesInNewAppDomain.Add(targetFileName);
                        }
                        else
                            DI.log.error("in loadAssembliesIntoAppDomain , could not find dll to copy: {0}",
                                         assemblyToLoad);
                    }
                }
            }
            // then load them (and if there are no missing dependencies ALL should load ok
            foreach (var assemblyToLoad in assembliesInNewAppDomain)
                try
                {
                    appDomain.Load(Path.GetFileNameWithoutExtension(assemblyToLoad));
                }
                catch (Exception ex)
                {
                    DI.log.ex(ex,
                              "in O2AppDomainFactory.loadAssembliesIntoAppDomain, could not load assembly: " +
                              assemblyToLoad);                    
                }
                
        }

        public string Name
        {
            get { return appDomain.FriendlyName; }
        }


        public List<string> FilesInAppDomainBaseDirectory
        {
            get
            {
                return new List<string>(Directory.GetFiles(BaseDirectory));                
            }
        }

        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly})
        /// </summary>        
        public object getProxyMethod(string methodToCreate)
        {
            return getProxyMethod(methodToCreate, new object[0]);
        }

        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly})
        /// </summary>                
        public object getProxyMethod(string methodToCreate, object[] methodParameters)
        {
            try
            {
                string[] splitedType = methodToCreate.Split(' ');
                if (splitedType.Length != 3)
                    DI.log.error(
                        "in getProxyMethod, wrong format provided (syntax:  getProxyMethod({method} {type} {assembly}) ) :  " +
                        methodToCreate);
                else
                {
                    string methodName = splitedType[0];
                    string typeName = splitedType[1];
                    string assemblyName = splitedType[2];

                    object proxyObject = appDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
                    if (proxyObject == null)
                        DI.log.error("in getProxyMethod, could not create proxy:{0} in assembly {1}", typeName,
                                  assemblyName);
                    else
                    {
                        MethodInfo methodInfo = DI.reflection.getMethod(proxyObject.GetType(), methodName,
                                                                        methodParameters);
                        if (methodInfo == null)
                            DI.log.error("in getProxyMethod, could not find method {0} in type {1}", methodName,
                                      proxyObject.GetType());

                        return methodInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in getProxyMethod, error creating: " + methodToCreate);
            }
            return null;
        }

        /// <summary>
        /// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}
        /// Use this one for the cases where only the Type is needed
        /// </summary>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <returns></returns>
        public Type getProxyType(string rawTypeOfProxyToCreate)
        {
            string assemblyName = "";
            string typeOfProxyToCreate = "";
            return getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate); 
        }

        /// <summary>
        /// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}
        /// </summary>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeOfProxyToCreate"></param>
        /// <returns></returns> 
        public Type getProxyType(string rawTypeOfProxyToCreate, ref string assemblyName, ref string typeOfProxyToCreate)
        {
            try
            {

                string[] splitedType = rawTypeOfProxyToCreate.Split(' ');
                if (splitedType.Length == 2)
                {
                    rawTypeOfProxyToCreate = splitedType[0];
                    assemblyName = splitedType[1];
                    // check if we can load the assembly and the requested type is there
                    Assembly loadedAssembly = appDomain.Load(assemblyName);
                    Type foundProxyType = DI.reflection.getType(loadedAssembly, rawTypeOfProxyToCreate);
                    if (foundProxyType == null)
                    {
                        DI.log.error("Could not find type {0} in assembly {1}", rawTypeOfProxyToCreate, assemblyName);
                        return null;
                    }
                    typeOfProxyToCreate = foundProxyType.FullName;
                    return foundProxyType;

                }

                // add support for just passing in the simple name of the type to create (this is will use the first one found)
                foreach (Assembly assembly in appDomain.GetAssemblies())
                {
                    Type foundProxyType = DI.reflection.getType(assembly, rawTypeOfProxyToCreate);
                    if (foundProxyType != null && assembly != null && assembly.FullName != null)
                    {
                        assemblyName = assembly.FullName;
                        typeOfProxyToCreate = foundProxyType.FullName;
                        return foundProxyType;
                    }
                }

            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "error creating: " + typeOfProxyToCreate);
            }
            DI.log.e("could not create object: " + typeOfProxyToCreate);
            return null;
        }

        /// <summary>
        /// Syntax:   getProxyObject({type} {assembly})  or getProxyType({type}}
        /// </summary>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <returns></returns> 
        public object getProxyObject(string rawTypeOfProxyToCreate)
        {

            string assemblyName = "";
            string typeOfProxyToCreate = "";
            try
            {
                var foundProxyType = getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate);
                if (foundProxyType != null)
                {
                    object proxy = appDomain.CreateInstanceAndUnwrap(assemblyName, typeOfProxyToCreate);
                    if (proxy != null)
                        return proxy;
                }
                DI.log.e("could not create object: " + rawTypeOfProxyToCreate);
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "error creating object: " + rawTypeOfProxyToCreate);
            }            
            return null;
            //return appDomain.CreateInstanceAndUnwrap(dllToLoad, typeToCreateAndUnwrap);
        }


        /// <summary>
        /// Syntax: invokeMethod({method} {type} {assembly})
        /// </summary>        
        public object invokeMethod(string methodToInvoke)
        {
            return invokeMethod(methodToInvoke, new object[0]);
        }

        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly})
        /// </summary>                
        public object invokeMethod(string methodToInvoke, object[] methodParameters)
        {
            try
            {
                string[] splitedType = methodToInvoke.Split(' ');
                if (splitedType.Length != 3)
                    DI.log.error(
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
                        DI.log.error("in invokeMethod, could not create proxy:{0} in assembly {1}", typeName, assemblyName);
                    else
                    {
                        MethodInfo methodInfo = DI.reflection.getMethod(proxyObject.GetType(), methodName,
                                                                        methodParameters);
                        if (methodInfo == null)
                            DI.log.error("in invokeMethod, could not find method {0} in type {1}", methodName,
                                      proxyObject.GetType());

                        return DI.reflection.invoke(proxyObject, methodInfo, methodParameters);
                    }
                }
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in invokeMethod, error creating: " + methodToInvoke);
            }
            return null;
        }

        public bool load(string assemblyName)
        {
            return load(assemblyName, false);
        }

        public bool load(string assemblyName, bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            return load(assemblyName, assemblyName, copyToAppDomainBaseDirectoryBeforeLoad);
        }

        public bool load(string fullAssemblyName, string pathToAssemblyToLoad,
                         bool copyToAppDomainBaseDirectoryBeforeLoad)
        {
            try
            {
                // copy if asked to
                if (copyToAppDomainBaseDirectoryBeforeLoad)
                    try
                    {
                        if (File.Exists(pathToAssemblyToLoad))
                            O2Kernel_Files.Copy(pathToAssemblyToLoad, appDomain.BaseDirectory);
                        else
                            DI.log.error(
                                "copyToAppDomainBaseDirectoryBeforeLoad was set but pathToAssemblyToLoad was set to a file that didn't exist: {0}",
                                pathToAssemblyToLoad);
                    }
                    catch (Exception ex)
                    {
                        ex.log("in load copyToAppDomainBaseDirectoryBeforeLoad");
                    }
                // load assembly into AppDomain
                //First try directly
                appDomain.Load(fullAssemblyName);                
            }
            catch (Exception ex1)
            {
                //then try using AssemblyName
                try
                {
                    appDomain.Load(AssemblyName.GetAssemblyName(fullAssemblyName));
                }
                catch (Exception ex2)
                {
                    // last change load assembly into current appdomain to get its full name and try again
                    try
                    {
                        appDomain.Load(Kernel.ExtensionMethods.Reflection_ExtensionMethods.assembly(fullAssemblyName).FullName);
                    }
                    catch (Exception ex3)
                    {
                        DI.log.ex(ex1, "could not load assembly (method1): " + fullAssemblyName);
                        DI.log.ex(ex2, "could not load assembly (method2): " + fullAssemblyName);
                        DI.log.ex(ex3, "could not load assembly (method3): " + fullAssemblyName);
                        return false;
                    }
                }
            }
            return true;
        }

        public object createAndUnWrap(string dllWithType, string typeToCreateAndUnwrap)
        {
            return appDomain.CreateInstanceAndUnwrap(dllWithType, typeToCreateAndUnwrap);
        }

        public List<String> getAssemblies(bool showFulName)
        {
            var assemblies = new List<String>();
            try
            {
                if (appDomain != null)
                    foreach (Assembly assembly in appDomain.GetAssemblies())
                        if (showFulName)
                            assemblies.Add(assembly.FullName);
                        else
                            assemblies.Add(assembly.GetName().Name);
            }
            catch (Exception ex)
            {
                DI.log.ex(ex,"in O2AppDomainFactory getAssemblies");
            }
            return assemblies;
        }

        public object invoke(string proxyObjectTypeAndAssembly, string methodToInvoke, object[] methodParameters)
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
                DI.log.error("Provided proxyObject variable was null!");
            else
            {
                MethodInfo methodInfo = DI.reflection.getMethod(proxyObject.GetType(), methodToInvoke, methodParameters);
                if (methodInfo == null)
                    DI.log.error("in invoke, could not find the requested method (please check the method name & provided parameters).  methodInfo == null for proxyObjectTypeAndAssembly:{0} , methodToInvoke {1}", proxyObject.ToString(), methodToInvoke);
                else
                    return DI.reflection.invoke(proxyObject, methodInfo, methodParameters);
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
            string appDomainName = Path.GetFileNameWithoutExtension(DI.config.TempFileNameInTempDirectory) + "_" + typeToCreateSimpleName;        // make sure each appDomainName is unique
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

        public bool unLoadAppDomain()
        {
            try
            {
                DI.log.info("Unloading appDomain: {0}", Name);
                if (DI.appDomainsControledByO2Kernel.ContainsKey(this.Name))
                    DI.appDomainsControledByO2Kernel.Remove(this.Name);                
                AppDomain.Unload(appDomain);                
                return true;
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in O2AppDomainFactory.unLoadAppDomain");
                return false;
            }            
        }

        public bool unLoadAppDomainAndDeleteTempFolder()
        {
            try
            {
                var appDomainBaseDirectory = appDomain.BaseDirectory;// we need to get this value before we unload the appDomain
               if (unLoadAppDomain())
                {
                    DI.log.info("Deleting (recursively) appDomain Base Directory: {0}", appDomainBaseDirectory);
                    Directory.Delete(appDomainBaseDirectory, true);
                }
                return false;
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in O2AppDomainFactory.unLoadAppDomainAndDeleteTempFolder");
                return false;
            }            
        }
    }
}
