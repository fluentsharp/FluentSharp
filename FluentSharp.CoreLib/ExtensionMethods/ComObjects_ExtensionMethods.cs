using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class ComObjects_ExtensionMethods
    {
        
        /*public static string        comTypeName(this object _object)
        {
            return _object.comObject_TypeName();
        }*/
        public static string        comObject_TypeName(this object _object)
        {
            return PublicDI.reflection.getComObjectTypeName(_object);
        }
		public static T             comObject_Get_Value<T>(this object comObject, string name)
		{
			return comObject.comObject_Prop<T>(name);
		}	
		public static T             comObject_Prop<T>(this object comObject, string name)
		{
			var value = comObject.comObject_Prop(name);
			if (value is T)
				return (T)value;
			return default(T);
		}		
		public static object        comObject_Prop(this object comObject, string name)
		{
			return comObject.comObject_Property_Value(name);
		}		
		public static void          comObject_Prop(this object comObject, string name, object value)
		{
			comObject.comObject_Property_Value(name, value);
		}		
		public static List<object>  comObject_Prop_ToList(this object comObject, string name)
		{
			return comObject.comObject_Property_Value(name).comObject_ExtractList();
		}		
		public static List<PropertyDescriptor>  comObject_PropertyDescriptors(this object comObject)
		{
			var properties = new List<PropertyDescriptor>();
			foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(comObject))
				properties.Add(property);
			return properties;
		}		
		public static PropertyDescriptor        comObject_PropertyDescriptor(this object comObject, string name)			
		{
			foreach(var property in comObject.comObject_PropertyDescriptors())
				if (property.Name.str() == name)
					return property;
			return null;								
		}		
		public static object                    comObject_Property_Value(this object comObject, string name)			
		{
			var propertyDescriptor = comObject.comObject_PropertyDescriptor(name);
			if (propertyDescriptor.notNull())
				return propertyDescriptor.GetValue(comObject);
			"[comObject_Property_Value (getter)] didn't find property called {0} in provided object".error(name);	
			return null;
		}		
		public static void                      comObject_Property_Value(this object comObject, string name, object value)	
		{
			var propertyDescriptor = comObject.comObject_PropertyDescriptor(name);
			if (propertyDescriptor.notNull())
				propertyDescriptor.SetValue(comObject, value);
			else
				"[comObject_Property_Value (setter)] didn't find property called {0} in provided object".error(name);
		}	
			
   
        // Lists_for_ComObject_ExtensionMethods to help (for example) with COM Objects received from IE	
		public static List<string>  comObject_ExtractList_String(this object _object)
		{
			return _object.comObject_ExtractList<string>();
		}		
		public static List<T>       comObject_ExtractList<T>(this object _object)
		{
			return _object.comObject_ExtractList<T>(true);
		}		
		public static List<T>       comObject_ExtractList<T>(this object _object, bool logCastErrors)
		{
			var results = new List<T>();
			if (_object is IEnumerable)
			{
				foreach(var item in (IEnumerable)_object)
					if (item is T)
						results.Add((T)item);
					else
						if(logCastErrors)
							"[comObject_ExtractList] inside the IEnumerable, this item was not of type '{0}' it was of type: '{1}'".error(typeof(T), item.comObject_TypeName());
			}
			else
				"[comObject_ExtractList] the provided object was not IEnumerable: {0}".error(_object);
			return results;
		}		
		public static List<object>  comObject_ExtractList(this object _object)
		{
			var results = new List<object>();
			if (_object is IEnumerable)
			{
				foreach(var item in (IEnumerable)_object)					
					results.Add(item);
			}
			else
				"[comObject_ExtractList] the provided object was not IEnumerable: {0}".error(_object);
			return results;
		}
    }
}
