using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Methods
    {        
        public static List<MethodInfo>  methods(this Type type)
        {
            return PublicDI.reflection.getMethods(type);
        }
        public static List<MethodInfo>  methods(this Assembly assembly)
        {
            return PublicDI.reflection.getMethods(assembly);
        }
        public static List<MethodInfo> methods(this Type type, string methodName)
        {
            return (from method in type.methods()
                    where method.Name == methodName
                    select method).toList();
        }
        public static MethodInfo method_bySignature(this Type type, string methodSignature)
        {
            return (from method in type.methods()
                    where method.str() == methodSignature
                    select method).first();
        }
        public static List<MethodInfo>  methods_public(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_private(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_declared(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_static(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
        }
        public static List<MethodInfo>  methods_instance(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
        }
        public static MethodInfo        firstMethod(this Assembly assembly)
        {
            // to skip the methods created by properties
            return assembly.methods().FirstOrDefault(method => false == method.IsSpecialName);
        }

        public static List<String>      names(this List<MethodInfo> methods)
        {
            var names = from method in methods select method.Name;
            return names.ToList();
        }
        public static MethodInfo        method(this Type type, string name)
        {
            return type.methods().first(method => method.Name == name);
        }
        public static MethodInfo        method(this Assembly assembly, string name)
        {
            return assembly.methods().first(method => method.Name == name);
        }
        public static string            signature(this MethodInfo methodInfo)
        {
            if (Environment.Version.Major == 4)
            {                
                var method = methodInfo.type().method("ConstructName");
                var result = method.Invoke(methodInfo, new object[] { });
                return result.str();        
            }
            return "mscorlib".assembly().type("RuntimeMethodInfo")
                             .method("ConstructName")
                             .invokeStatic(methodInfo)
                             .str();                
            
        }		    
        public static List<MethodInfo>  methods(this List<Type> types)
        {
            return (from type in types
                    from method in type.methods()
                    select method).toList();					
        }				
    }
}