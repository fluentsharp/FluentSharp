using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Ctor
    {
        public static T                     ctor<T>(this Type type, params object[] constructorParams)
        {
            var newObject = type.ctor(constructorParams);
            if (newObject.notNull() && newObject is T)
                return (T) newObject;
            return default(T);
        }

        public static object                ctor(this string className, string assembly, params object[] parameters)
        {
            var obj = PublicDI.reflection.createObject(assembly, className, parameters);
            if (PublicDI.reflection.verbose)
                if (obj == null)
                    PublicDI.log.error("in ctor, could not created object: {0}!{1}", assembly, className);
                else
                    PublicDI.log.debug("in ctor, created object of type: {0}", obj.GetType());
            return obj;
        }        
        public static object                ctor(this Type type, params object[] constructorParams)
        {
            return PublicDI.reflection.createObject(type, constructorParams);
        }
        public static List<ConstructorInfo> ctors(this Type type)
        {
            return type.GetConstructors(BindingFlags.NonPublic | 
                                        BindingFlags.Public | 
                                        BindingFlags.Instance).toList();
        }		    
        public static Array                 createArray<T>(this Type arrayType,  params T[] values)			
        {
            try
            {
                if (values.isNull())
                    return  Array.CreateInstance (arrayType,0);	
                    
                var array =  Array.CreateInstance (arrayType,values.size());	
                
                if (values.notNull())
                    for(int i=0 ; i < values.size() ; i ++)
                        array.SetValue(values[i],i);
                return array;				 				
            }
            catch(Exception ex)
            {
                ex.log("in Array.createArray");
            }
            return null;
        }	
    }
}