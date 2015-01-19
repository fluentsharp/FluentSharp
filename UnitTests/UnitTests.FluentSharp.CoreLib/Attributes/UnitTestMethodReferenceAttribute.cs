using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSharp.CoreLib;

namespace UnitTests.FluentSharp.CoreLib
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitTestMethodReferenceAttribute : Attribute
    {
        public MethodInfo MethodToInvoke { get; set; }

        public UnitTestMethodReferenceAttribute(string methodName)
        {
            MethodToInvoke =  this.type().assembly().method(methodName);
            if (MethodToInvoke.isNull())
                "[UnitTestMethodReferenceAttribute] Could not find method {0} in assembly {1}".error(methodName, this.type().assembly()); 
        }

    }
}
