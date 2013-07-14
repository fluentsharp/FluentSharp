using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
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
                    PublicDI.log.info("executing method: {0}", filteredSignature.sSignature);
                    PublicDI.reflection.invoke(Method, parameters);
                });
        }

        public void raiseO2MDbgDebugMethodInfoRequest(string loadDllsFrom)
        {
            if (Method != null)
                O2Messages.raiseO2MDbgDebugMethodInfoRequest(Method, loadDllsFrom);
        }
    }
}
