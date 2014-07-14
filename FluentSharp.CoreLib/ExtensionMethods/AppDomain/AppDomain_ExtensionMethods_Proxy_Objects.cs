using System;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class AppDomain_ExtensionMethods_Proxy_Objects
    {
        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly})
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="methodToCreate"></param>
        /// <returns></returns>
        public static object    getProxyMethod  (this AppDomain appDomain, string methodToCreate)
        {
            return appDomain.getProxyMethod(methodToCreate, new object[0]);
        }        
        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly}) 
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="methodToCreate"></param>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        public static object    getProxyMethod  (this AppDomain appDomain,string methodToCreate, object[] methodParameters)
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

                    var proxyObject = appDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
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
        /// <summary>
        /// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}    // Use this one for the cases where only the Type is needed
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <returns></returns>
        public static Type      getProxyType    (this AppDomain appDomain, string rawTypeOfProxyToCreate)
        {
            string assemblyName = "";
            string typeOfProxyToCreate = "";
            return appDomain.getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate); 
        }
        
        /// <summary>
        /// Syntax:   getProxyType({type} {assembly})  or getProxyType({type}}        
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeOfProxyToCreate"></param>
        /// <returns></returns>
        public static Type		getProxyType    (this AppDomain appDomain, string rawTypeOfProxyToCreate, ref string assemblyName, ref string typeOfProxyToCreate)
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
                foreach (Assembly assembly in appDomain.GetAssemblies())
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
        /// <summary>
        /// Syntax:   getProxyObject({type} {assembly})  or getProxyType({type}}  
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="rawTypeOfProxyToCreate"></param>
        /// <returns></returns>
        public static object 	getProxyObject  (this AppDomain appDomain, string rawTypeOfProxyToCreate)
        {

            string assemblyName = "";
            string typeOfProxyToCreate = "";
            try
            {
                var foundProxyType = appDomain.getProxyType(rawTypeOfProxyToCreate, ref assemblyName, ref typeOfProxyToCreate);
                if (foundProxyType != null)
                {
                    object proxy = appDomain.CreateInstanceAndUnwrap(assemblyName, typeOfProxyToCreate);
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
    }
}