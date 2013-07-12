// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FluentSharp.CoreLib.API
{
    public class O2Proxy : MarshalByRefObject
    {
        public bool InvokeInStaThread { get; set; }
        public bool InvokeInMtaThread { get; set; }
        //  public AppDomain appDomain = AppDomain.CurrentDomain;

        /*  public O2AppDomainFactory getO2AppDomainFactoryWithCurrentAppDomain() // use this to get an proxy into the current AppDomain
        {
            return new O2AppDomainFactory(AppDomain.CurrentDomain );
        }*/

        public override string ToString()
        {
        	var currentDomain = nameOfCurrentDomain();            
            return currentDomain ?? "";  // to deal with '...Attempted to read or write protected memory..' issue 
        }

        // reflection helpers

        // need to convert it to strings since we can't send the Assembly Objects to the calling appDomain
        public List<String> getAssemblies()
        {
            return getAssemblies(false);
        }

        public List<String> getAssemblies(bool showFulName)
        {
            var assemblies = new List<String>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (showFulName)
                    assemblies.Add(assembly.FullName);
                else
                    assemblies.Add(assembly.GetName().Name);
            return assemblies;
        }

        public List<String> getTypes(string assemblyName)
        {
            var types = new List<String>();
            try
            {
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
                foreach (Type type in PublicDI.reflection.getTypes(assembly))
                    types.Add(type.Name);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in getTypes");
            }
            return types;
        }

        public List<String> getMethods(string assemblyName, string typeName)
        {
            return getMethods(assemblyName, typeName, false, false);
        }

        public List<String> getMethods(string assemblyName, string typeName, bool onlyReturnStaticMethods,
                                       bool onlyReturnMethodsWithNoParameters)
        {
            var methods = new List<String>();
            try
            {
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
                Type type = PublicDI.reflection.getType(assembly, typeName);
                foreach (MethodInfo method in PublicDI.reflection.getMethods(type))
                    if ((false == onlyReturnStaticMethods || method.IsStatic) &&
                        (false == onlyReturnMethodsWithNoParameters || method.GetParameters().Length == 0))
                        methods.Add(method.Name);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in getTypes");
            }
            return methods;
        }

        #region Methods to help basic testing

        public string Property { get; set; }

        public string returnConcatParamData(string paramDataString, int paramDataInt)
        {
            return paramDataString + paramDataInt;
        }

        public string nameOfCurrentDomain()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        public static string nameOfCurrentDomainStatic()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        public static void logInfo(string infoMessageToLog)
        {
            PublicDI.log.info(infoMessageToLog);
        }

        public static void logDebug(string debugMessageToLog)
        {
            PublicDI.log.debug(debugMessageToLog);
        }
        public static void logError(string errorMessageToLog)
        {
            PublicDI.log.error(errorMessageToLog);
        }

        /*public static void showMessageBox(string messageBoxText)
        {
            PublicDI.log.showMessageBox(messageBoxText);
        }

        public static DialogResult showMessageBox(string message, string messageBoxTitle,
                                                MessageBoxButtons messageBoxButtons)
        {
            return PublicDI.log.showMessageBox(message, messageBoxTitle, messageBoxButtons);
        }*/
        
        #endregion

        #region InstanceInvocation

        public object instanceInvocation(string typeToLoad, string methodToExecute, object[] methodParams)
        {
            string assembly = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            return instanceInvocation(assembly, typeToLoad, methodToExecute, methodParams);
        }

        public object instanceInvocation(string assemblyToUse, string typeToLoad, string methodToExecute,
                                         object[] methodParams)
        {
            try
            {
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyToUse);
                if (assembly.isNull())
                    PublicDI.log.error("in instanceInvocation assembly was null : {0} {1}", assemblyToUse);
                else
                {
                    Type type = PublicDI.reflection.getType(assembly, typeToLoad);
                    if (type == null)
                        PublicDI.log.error("in instanceInvocation type was null : {0} {1}", assembly, typeToLoad);
                    else
                    {
                        object typeObject = PublicDI.reflection.createObject(assembly, type, methodParams);
                        if (typeObject == null)
                            PublicDI.log.error("in dynamicInvocation typeObject was null : {0} {1}", assembly, type);
                        else
                        {
                            if (methodToExecute == "")
                                // means we don't want to execute a method (i.e we called the constructore) so just want the current proxy
                                return typeObject;
                            MethodInfo method = PublicDI.reflection.getMethod(type, methodToExecute, methodParams);
                            if (method == null)
                                PublicDI.log.error("in instanceInvocation method was null : {0} {1}", type, methodToExecute);
                            else
                            {
                                object returnValue = null;
                                if (InvokeInStaThread)
                                {
                                    O2Thread.staThread(() =>
                                        {
                                            returnValue = PublicDI.reflection.invoke(typeObject, method, methodParams);
                                        }).Join();
                                    return returnValue;
                                }
                                if (InvokeInMtaThread)
                                {
                                    O2Thread.mtaThread(() =>
                                        {
                                            returnValue = PublicDI.reflection.invoke(typeObject, method, methodParams);
                                        }).Join();
                                    return returnValue;
                                }
                                return PublicDI.reflection.invoke(typeObject, method, methodParams);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in instanceInvocation");
            }
            return null;
        }

        #endregion

        #region staticInvocation

        public object staticInvocation(string typeToLoad, string methodToExecute, object[] methodParams)
        {
            string assembly = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            return staticInvocation(assembly, typeToLoad, methodToExecute, methodParams);
        }

        public object staticInvocation(string assemblyToUse, string typeToLoad, string methodToExecute,
                                       object[] methodParams)
        {
            try
            {
                //Assembly assembly = AppDomain.CurrentDomain.Load(assemblyToUse);
                var assembly = assemblyToUse.assembly();
                if (assembly == null)
                    PublicDI.log.error("in staticInvocation assembly was null : {0} {1}", assemblyToUse);
                else
                {
                    Type type = PublicDI.reflection.getType(assembly, typeToLoad);
                    if (type == null)
                        PublicDI.log.error("in staticInvocation type was null : {0} {1}", assembly, typeToLoad);
                    else
                    {
                        MethodInfo method = PublicDI.reflection.getMethod(type, methodToExecute, methodParams);
                        if (method == null)
                            PublicDI.log.error("in staticInvocation method was null : {0} {1}", type, methodToExecute);
                        else
                        {
                            object returnValue = null;
                            if (InvokeInStaThread)
                            {
                                O2Thread.staThread(() =>
                                    {
                                        returnValue = PublicDI.reflection.invoke(null, method, methodParams);
                                    }).Join();
                                return returnValue;
                            }
                            if (InvokeInMtaThread)
                            {
                                O2Thread.mtaThread(() =>
                                    {
                                        returnValue = PublicDI.reflection.invoke(null, method, methodParams);
                                    }).Join();
                                return returnValue;
                            }                            
                            return PublicDI.reflection.invoke(null, method, methodParams);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in instanceInvocation");
            }
            return null;
        }

        #endregion


        public bool staThread()
        {
            return InvokeInStaThread;
        }
        public void staThread(bool value)
        {
            InvokeInStaThread = value;
        }
    }
}
