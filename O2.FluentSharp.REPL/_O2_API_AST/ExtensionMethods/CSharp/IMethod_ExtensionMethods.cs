using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpDevelop.Dom;
using O2.DotNetWrappers.ExtensionMethods;

using ICSharpCode.SharpDevelop.Dom.CSharp;

namespace O2.API.AST.ExtensionMethods.CSharp
{
    public static class IMethod_ExtensionMethods
    {
        public static string name(this IMethod iMethod)
        {
            return iMethod.Name;
        }

        public static string fullName(this IMethodOrProperty iMethodOrProperty)
        {
            if (iMethodOrProperty is IMethod)
                return (iMethodOrProperty as IMethod).fullName();
            else if (iMethodOrProperty is IProperty)
                return (iMethodOrProperty as IProperty).fullName();
            return "[null value]";
        }

        public static string fullName(this IProperty iProperty)
        {
            if (iProperty == null)
                return "[null value]";
            CSharpAmbience ambience = new CSharpAmbience();
            ambience.ConversionFlags = ConversionFlags.StandardConversionFlags | ConversionFlags.UseFullyQualifiedMemberNames | ConversionFlags.UseFullyQualifiedTypeNames;
            return ambience.Convert(iProperty);
        }

        public static string fullName(this IMethod iMethod)
        {
            if (iMethod == null)
                return "[null value]";
            CSharpAmbience ambience = new CSharpAmbience();
            ambience.ConversionFlags = ConversionFlags.StandardConversionFlags | ConversionFlags.UseFullyQualifiedMemberNames;
            return ambience.Convert(iMethod);
        }

        public static string @namespace(this IMethod iMethod)
        {
            return iMethod.DeclaringType.Namespace;
        }

        public static string typeName(this IMethod iMethod)
        {
            return iMethod.DeclaringType.Name;
        }

        public static string csharpCode(this IMethod iMethod)
        {
            var @namespace = iMethod.@namespace();
            var typeName = iMethod.typeName();
            var ambiance = new CSharpAmbience();
            var csharpCode = ("\t\t{0}".line() +
                              "\t\t{{".line() +
                              "\t\t\tthrow new System.Exception(\"O2 Auto Generated Method\");".line() +
                              "\t\t}}".line())
                             .format(ambiance.Convert(iMethod));

            if (typeName.valid())
                csharpCode = ("\tclass {0}".line() +
                              "\t{{".line() +
                              "{1}".line() +
                              "\t}}".line())
                             .format(typeName, csharpCode);
            if (@namespace.valid())
                csharpCode = ("namespace {0}".line() +
                              "{{".line() +
                              "{1}".line() +
                              "}}".line())
                             .format(@namespace, csharpCode);
            //if (@namespace.valid());

            return csharpCode;
        }
    }
}
