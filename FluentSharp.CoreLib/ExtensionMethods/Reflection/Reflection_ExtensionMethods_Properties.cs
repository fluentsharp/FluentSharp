using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Properties
    { 
        public static List<PropertyInfo>    properties(this Type type)
        {
            return PublicDI.reflection.getProperties(type);
        }
        public static List<PropertyInfo>	properties<T>(this Type type)
        {
            return type.properties().where((prop)=>prop.PropertyType == typeof(T));
        }        
        public static List<PropertyInfo>    properties_public_declared(this Type type, BindingFlags bindingFlags)
        { 
            return type.properties(BindingFlags.Public | BindingFlags.DeclaredOnly);
        }
        public static List<PropertyInfo>    properties(this Type type, BindingFlags bindingFlags)
        {            
            return new List<PropertyInfo>(type.GetProperties(bindingFlags));
        }        
        public static object                property(this Type type, string propertyName)
        {
            return type.prop(propertyName);
        }
        public static T                     property<T>(this object _object, string propertyName)
        {						
            if (_object.notNull())
            {
                var result = _object.property(propertyName);
                if (result is T)
                    return (T)result;
            }
            return default(T);
        }		
        public static object                property(this object _object, string propertyName, object value)
        {
            var propertyInfo = PublicDI.reflection.getPropertyInfo(propertyName, _object.type());  
            if (propertyInfo.isNull())
                "Could not find property {0} in type {1}".error(propertyName, _object.type());  
            else
            {
                PublicDI.reflection.setProperty(propertyInfo, _object,value);    
                //		"set {0}.{1} = {2}".info(_object.type(),propertyName, value);
            }
            return _object;

        }		
        public static object                property(this object liveObject, string propertyName)
        {
            return liveObject.prop(propertyName);
        }        
        public static Type                  propertyType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }
        public static List<object>          propertyValues(this object _object)
        {
            /*var propertyValues = new List<object>();
            var names = _object.type().properties().names();
            foreach(var name in names)
                propertyValues.Add(_object.prop(name));
            return propertyValues;*/
            return _object.type()
                          .properties()
                          .names()
                          .select(name => _object.prop(name));            
        }
        public static List<T>               propertyValues<T>(this object _object)
        {            
            return _object.type         ()
                          .properties<T>()
                          .select       (property => (T) _object.prop(property.Name));                          
        }
        public static Dictionary<string,T> propertyValues_MappedBy_Name<T>(this object _object)
        {
            var propertyValues = new Dictionary<string,T>();
            var properties = _object.type().properties<T>();
            foreach (var property in properties)
                propertyValues.add(property.Name, (T)_object.prop(property.Name));
            return propertyValues;
        }
        public static object                prop(this Type type, string propertyName)
        {
            return PublicDI.reflection.getProperty(propertyName, type);
        }    
        public static T                     prop<T>(this object liveObject, string propertyName)
        {
            if (liveObject.notNull())
            {
                var value = liveObject.prop(propertyName);
                if (value is T)
                    return (T)value;
            }
            return default(T);
        }
        public static object                prop(this object liveObject, string propertyName)
        {            
            return PublicDI.reflection.getProperty(propertyName, liveObject);
        }
        public static object				prop(this object liveObject, PropertyInfo propertyInfo, object value)
        {
            PublicDI.reflection.setProperty(propertyInfo, liveObject, value);
            return liveObject;
        }        
        public static object                prop(this object liveObject, string propertyName, object value)
        {
            PublicDI.reflection.setProperty(propertyName, liveObject, value);
            return liveObject;
        }
        public static List<string>          names(this List<PropertyInfo> properties)
        {
            return (from property in properties
                    select property.Name).toList();
        }		
        
    }
}