using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnitTests.FluentSharp.CoreLib
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitTestMethodReferenceAttribute : Attribute
    {
        public MethodInfo MethodToInvoke { get; set; }

        public UnitTestMethodReferenceAttribute(string methodName)
        {
            var method = Assembly.GetExecutingAssembly()
                                                  .methods().first(x => x.Name == methodName);
            if (method.isNotNull())
                MethodToInvoke = method;
            else
                "Method {0} does not exists ".error(methodName); 
        }

    }
}
