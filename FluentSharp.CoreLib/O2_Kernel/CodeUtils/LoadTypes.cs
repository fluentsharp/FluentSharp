// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;


namespace FluentSharp.CoreLib.API
{
    public class LoadTypes
    {        
        //public static object createObject()

        public static object loadTypeAndExecuteMethodInAppDomain(AppDomain appDomain, string dllWithType,
                                                                 string typeToCreateAndUnwrap, string methodToExecute,
                                                                 object[] methodParameters)
        {
            var appDomainHelper = new O2AppDomainFactory(appDomain);
            object proxyObject = appDomainHelper.appDomain().createAndUnWrap(dllWithType, typeToCreateAndUnwrap);
            if (proxyObject == null)
                PublicDI.log.error("in loadTypeAndExecuteMethodInAppDomain, proxy == null");                            
            else
            {
                MethodInfo methodInfo = PublicDI.reflection.getMethod(proxyObject.GetType(), methodToExecute, methodParameters);
                if (methodInfo == null)
                    PublicDI.log.error("in loadTypeAndExecuteMethodInAppDomain, methodInfo == null");
                else
                    return PublicDI.reflection.invoke(proxyObject, methodInfo, methodParameters);
            }
            return null;
        }
    }
}
