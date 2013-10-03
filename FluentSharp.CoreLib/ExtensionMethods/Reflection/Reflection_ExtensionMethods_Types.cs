using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Types
    { 
        public static Type          type(this Assembly assembly, string typeName)
        {
            return PublicDI.reflection.getType(assembly, typeName);
        }
        public static Type          type(this string assemblyName, string typeName)
        {
            return PublicDI.reflection.getType(assemblyName, typeName);
        }
        public static Type          type(this object _object)
        {
            if (_object.isNull())
                return null;
            return _object.GetType();
        }
        public static List<Type>    types(this Assembly assembly)
        {
            return PublicDI.reflection.getTypes(assembly);
        }
        public static List<Type>    types(this Type type)
        {
            return PublicDI.reflection.getTypes(type);
        }

        public static List<Type>	types<T>(this List<T> list)
        {
            return list.select((item) => type(item)).toList();
        }
        public static object		infoTypeName(this object _object)
        {
            if (_object.notNull())
                _object.typeName().info();
            else
                "in infoTypeName _object was null".error();
            return _object;
        }
        public static string        typeName(this object target)
        {            
            return target.isNull() ? "" : target.type().Name;            
        }
        public static string        typeFullName(this object target)
        {
            return target.isNull() ? "" : target.type().FullName;
        }
        public static string        name(this Type type)
        {
            return type.isNull() ? "": type.Name;
        }
        public static string        fullName(this Type type)
        {
            return type.isNull() ? "" : type.FullName;
        }

        public static List<string> names(this List<Type> types)
        {
            return types.isNull() ? new List<string>()
                                  : types.Select((type) => type.Name).toList();
        }
        
        public static List<Type>    baseTypes(this Type type)
        {
            var baseType = new List<Type>();
            if (type.BaseType.notNull())
            {
                baseType.Add(type.BaseType);
                baseType.AddRange(baseTypes(type.BaseType));                
            }
            return baseType;
        }
        public static List<Type>    withBaseType<T>(this Assembly assembly)
        {
            return assembly .types()
                            .Where(type => type.baseTypes()
                                               .Contains(typeof (T)))
                            .ToList();
        }

        public static List<string>  typesNames(this Assembly assembly)
        {
            return (from type in assembly.types()
                    select type.Name).ToList();
        }    
    }
}