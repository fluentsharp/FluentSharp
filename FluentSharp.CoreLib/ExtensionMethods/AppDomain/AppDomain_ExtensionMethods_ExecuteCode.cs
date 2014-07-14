using System;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class AppDomain_ExtensionMethods_ExecuteCode
    {
        public static O2Proxy	    executeScript   (this O2Proxy o2Proxy, string scriptToExecute)
        {
            o2Proxy.staticInvocation("FluentSharp.REPL", "FastCompiler_ExtensionMethods", "executeSourceCode", new object[] { scriptToExecute });
            return o2Proxy;
        }
        public static O2Proxy		executeScriptInAppDomain(this AppDomain appDomain, string scriptToExecute)
        {
            return appDomain.executeScriptInAppDomain(scriptToExecute, false, false);
        }
        public static O2Proxy		executeScriptInAppDomain(this AppDomain appDomain, string scriptToExecute, bool showLogViewer, bool openScriptGui)
        {
            var o2Proxy = (O2Proxy)appDomain.getProxyObject("O2Proxy");
            if (o2Proxy.isNull())
            {
                "in executeScriptInSeparateAppDomain, could not create O2Proxy object".error();
                return null;
            }
            o2Proxy.InvokeInStaThread = true;
            if (showLogViewer)
                o2Proxy.executeScript("open.logViewer();");
            if (openScriptGui)
                o2Proxy.executeScript("open.scriptEditor().inspector.set_Script(\"return 42;\");");            

            o2Proxy.executeScript(scriptToExecute);
            return o2Proxy;
        }
        
        /// <summary>
        /// Syntax: invokeMethod({method} {type} {assembly})=  
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="methodToInvoke"></param>
        /// <returns></returns>      
        public static object		invokeMethod            (this AppDomain appDomain, string methodToInvoke)
        {
            return appDomain.invokeMethod(methodToInvoke, new object[0]);
        }
        /// <summary>
        /// Syntax: getProxyMethod({method} {type} {assembly})
        /// </summary>
        /// <param name="appDomain"></param>
        /// <param name="methodToInvoke"></param>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        public static object		invokeMethod            (this AppDomain appDomain, string methodToInvoke, object[] methodParameters)
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
                    object proxyObject = appDomain.getProxyObject(typeName + " " + assemblyName);

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
        public static object		invoke                  (this AppDomain appDomain, string proxyObjectTypeAndAssembly, string methodToInvoke, object[] methodParameters)
        {
            var proxyObject = appDomain.getProxyObject(proxyObjectTypeAndAssembly);
            return appDomain.invoke(proxyObject, methodToInvoke, methodParameters);
        }
        public static object        invoke                  (this AppDomain appDomain, object proxyObject, string methodToInvoke, object[] methodParameters)
        {
            // if the proxyObject is a string we need to resolve it into its final O2 object
            if (proxyObject is string)
                proxyObject = appDomain.getProxyObject((string)proxyObject);
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
        public static object        invoke                  (this AppDomain appDomain,object proxyObject, string methodToInvoke)
        {
            return appDomain.invoke(proxyObject, methodToInvoke, new object[0]);
        }
    
        // these will invoke commands on the target AppDomain via the O2Proxy instanceInvocation method        

        // invoke instance methods
        public static object        proxyInvokeInstance     (this AppDomain appDomain, string assembly, string type, string method)
        {
            return appDomain.proxyInvokeInstance(assembly, type, method, new object[0]);
        }

        public static object        proxyInvokeInstance     (this AppDomain appDomain, string assembly, string type, string method, object[] methodParameters)
        {
            return appDomain.proxyInvokeInstance(new object[] {assembly, type, method, methodParameters});
        }

        public static object        proxyInvokeInstance(this AppDomain appDomain, object[] methodParameters)
        {
            return appDomain.invokeMethod("instanceInvocation O2Proxy O2_Kernel", methodParameters);
        }

        public static object        proxyInvokeStatic(this AppDomain appDomain, string assembly, string type, string method)
        {
            return appDomain.proxyInvokeStatic(assembly, type, method, new object[0]);
        }

        public static object        proxyInvokeStatic(this AppDomain appDomain, string assembly, string type, string method, object[] methodParameters)
        {
            return appDomain.proxyInvokeStatic(new object[] {assembly, type, method, methodParameters});
        }

        public static object        proxyInvokeStatic(this AppDomain appDomain, object[] methodParameters)
        {
            return appDomain.invokeMethod("staticInvocation O2Proxy O2_Kernel", methodParameters);
        }
    }
}