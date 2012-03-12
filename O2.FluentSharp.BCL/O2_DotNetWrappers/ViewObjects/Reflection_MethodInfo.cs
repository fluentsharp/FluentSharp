using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using O2.DotNetWrappers.Filters;
using O2.DotNetWrappers.DotNet;
using O2.Kernel.CodeUtils;

namespace O2.DotNetWrappers.ViewObjects
{
    public class Reflection_MethodInfo
    {
        public MethodInfo Method { get; set; }
        public FilteredSignature filteredSignature  { get; set; }

        public Reflection_MethodInfo(MethodInfo method)
        {
            Method = method;
            filteredSignature = new FilteredSignature(method);
        }

        public override string ToString()
        {
            return filteredSignature.sFunctionNameAndParamsAndReturnClass;
        }


        public void invokeMTA(object[] parameters)
        {
            if (Method != null)
                O2Thread.mtaThread(
                () =>
                {
                    DI.log.info("executing method: {0}", filteredSignature.sSignature);
                    DI.reflection.invoke(Method, parameters);
                });
        }

        public void raiseO2MDbgDebugMethodInfoRequest(string loadDllsFrom)
        {
            if (Method != null)
                O2Messages.raiseO2MDbgDebugMethodInfoRequest(Method, loadDllsFrom);
        }
    }
}
