using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Attributes
    { 
        public static List<Attribute>           attributes      (this Assembly assembly)
        {
            return PublicDI.reflection.getAttributes(assembly);
       
        }		

        public static List<T>                   attributes<T>   (this Assembly assembly)			where T : Attribute
        {
            return assembly.attributes().attributes<T>();
        }	
        public static T                         attribute<T>    (this Assembly assembly)			where T : Attribute
        {
            return assembly.attributes<T>().first();
        }	        	
        public static bool                      hasAttribute<T>(this Assembly assembly)			    where T : Attribute
        {
            return assembly.attribute<T>().notNull();
        }
        public static List<Attribute>    attributes(this List<MethodInfo> methods)
        {
            return (from method in methods 
                    from attribute in method.attributes()
                    select attribute).toList();
        }
        public static List<Attribute>           attributes(this MethodInfo method)
        {
            return PublicDI.reflection.getAttributes(method);
        }		
        public static List<T>                   attributes<T>(this MethodInfo method)			        where T : Attribute
        {
            return method.attributes().attributes<T>();
        }		
        public static List<T>                   attributes<T>(this List<MethodInfo> methods)			where T : Attribute
        {
            var attributes = new List<T>();
            foreach(var method in  methods)
                foreach (var attribute in method.attributes<T>())
                    attributes.add(attribute);
            return attributes;
        }

        public static List<T>                   attributes<T>(this List<Attribute> attributes)			where T : Attribute
        {
            return (from attribute in attributes
                    where attribute is T
                    select (T)attribute).toList();
        }						
        public static List<Attribute>           attributes(this List<MethodInfo> methods, string name)
        {
            return methods.attributes().withName(name);
        }		
        public static Attribute                 attribute(this MethodInfo methodInfo, string name)
        {
            return methodInfo.attributes().FirstOrDefault(attribute => attribute.name() == name);
        }

        public static T                         attribute<T>(this MethodInfo methodInfo)			where T : Attribute
        {
            return methodInfo.attributes<T>().first();			
        }		
        public static List<MethodInfo>          methodsWithAttribute<T>(this Assembly assembly)			where T : Attribute
        {
            return assembly.methods().withAttribute<T>();
        }		
        public static List<MethodInfo>          withAttribute(this Assembly assembly, string attributeName)
        {
            return assembly.methods().withAttribute(attributeName);
        }		
        public static List<MethodInfo>          withAttribute(this List<MethodInfo> methods, string attributeName)
        { 
            return (from method in methods 
                    from attribute in method.attributes()
                    let type = attribute.TypeId as Type
                    where type != null && attributeName == type.Name.remove("Attribute")
                    select method).toList();						
        }				
        public static List<MethodInfo>          withAttribute<T>(this List<MethodInfo> methods)			where T : Attribute
        {
            return (from method in methods 
                    from attribute in method.attributes()		  
                    where attribute is T
                    select method).toList();						
        }		        
        public static List<Attribute>           withName(this List<Attribute> attributes, string name)
        {
            return (from attribute in attributes
                    where attribute.name() == name
                    select attribute).toList();
        }
        public static string                    name(this Attribute attribute)
        {
            return attribute.typeName().remove("Attribute");
        }		
        public static List<string>              names(this List<Attribute> attributes)
        {
            return (from attribute in attributes
                    select attribute.name()).toList();
        }		
        
    }
}