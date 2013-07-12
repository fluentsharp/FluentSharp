using System.Collections.Generic;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Parameters
    { 
        public static List<ParameterInfo> parameters(this MethodInfo methodInfo)
        {
            return PublicDI.reflection.getParameters(methodInfo);
        }
    }
}