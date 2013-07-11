using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Interfaces
    { 
        public static List<Type>    interfaces(this Type type)
        {
            return new List<Type>( type.GetInterfaces() );
        }
    }
}