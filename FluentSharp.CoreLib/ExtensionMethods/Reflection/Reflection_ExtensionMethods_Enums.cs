using System;
using System.Reflection;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Enums
    { 
        public static object    enumValue(this Type enumType, string value)
        {
            return enumType.enumValue<object>(value);
        }
        public static T         enumValue<T>(this Type enumType, string value)
        {
            var fieldInfo = (FieldInfo) enumType.field(value);
            if (fieldInfo.notNull())
                return (T)fieldInfo.GetValue(enumType);
            return default(T);
        }		
    }
}