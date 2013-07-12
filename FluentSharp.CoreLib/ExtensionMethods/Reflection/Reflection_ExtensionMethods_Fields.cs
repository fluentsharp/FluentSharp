using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Fields
    {         
        public static object            field(this object liveObject, string fieldName)
        {
            return PublicDI.reflection.getFieldValue(fieldName, liveObject);
        }        
        public static object            field(this object liveObject, string fieldName, object value)
        {
            PublicDI.reflection.setField(fieldName, liveObject, value);
            return liveObject;
        }        
        public static T                 field<T>(this object _object, string fieldName)
        {
            var value = _object.field(fieldName);
            if (value is T)
                return (T) value;
            return default(T);
        }		
        public static object            field(this object _object, Type type, string fieldName)
        {						
            var fieldInfo =  (FieldInfo)type.field(fieldName);
            return PublicDI.reflection.getFieldValue(fieldInfo, type);
        }

        public static FieldInfo         fieldInfo(this object _object, string fieldName)
        {
            return _object.type().fieldInfo(fieldName);
        }
        public static FieldInfo         fieldInfo(this Type type, string fieldName)
        {            
            return PublicDI.reflection.getField(type, fieldName);
        }
        public static List<FieldInfo>   fieldInfos(this Type type)
        {
            return PublicDI.reflection.getFields(type);
        }
        public static List<FieldInfo>	fieldInfos<T>(this Type type)
        {
            return type.fieldInfos().where((field) => field.FieldType == typeof(T));
        }
        public static object            fieldValue(this Type type, string fieldName)
        {
            var fieldInfo = type.fieldInfo(fieldName);
            return PublicDI.reflection.getFieldValue(fieldInfo, null);
        }
        public static Type              fieldValue(this Type type, string fieldName, object value)
        {
            var fieldInfo = type.fieldInfo(fieldName);			
            PublicDI.reflection.setField(fieldInfo, fieldInfo.ReflectedType, value);
            return type;
        }		        
        
    }
}