using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FluentSharp.ExtensionMethods;
using System.Windows.Forms;
using O2.Views.ASCX.classes.MainGUI;
using System.Collections;

namespace FluentSharp.ExtensionMethods
{
    public static class ComObjects_ExtensionMethods
    {
        
		public static T get_Value<T>(this object _comObject, string name)
		{
			return _comObject.prop_ComObject<T>(name);
		}
	
		public static T prop_ComObject<T>(this object _comObject, string name)
		{
			var value = _comObject.prop_ComObject(name);
			if (value is T)
				return (T)value;
			return default(T);
		}
		
		public static object prop_ComObject(this object _comObject, string name)
		{
			return _comObject.prop_ComObject_Value(name);
		}
		
		public static void prop_ComObject(this object _comObject, string name, object value)
		{
			_comObject.prop_ComObject_Value(name, value);
		}
		
		public static List<object> prop_ComObject_toList(this object _comObject, string name)
		{
			return _comObject.prop_ComObject_Value(name).extractList();
		}
		
		public static List<PropertyDescriptor> prop_ComObject_Properties(this object _comObject)
		{
			var properties = new List<PropertyDescriptor>();
			foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(_comObject))
				properties.Add(property);
			return properties;
		}
		
		public static PropertyDescriptor prop_ComObject_Property(this object _comObject, string name)			
		{
			foreach(var property in _comObject.prop_ComObject_Properties())
				if (property.Name.str() == name)
					return property;
			return null;								
		}
		
		public static object prop_ComObject_Value(this object _comObject, string name)			
		{
			var propertyDescriptor = _comObject.prop_ComObject_Property(name);
			if (propertyDescriptor.notNull())
				return propertyDescriptor.GetValue(_comObject);
			"[prop_ComObject_Value (getter)] didn't find property called {0} in provided object".error(name);	
			return null;
		}
		
		public static void prop_ComObject_Value(this object _comObject, string name, object value)	
		{
			var propertyDescriptor = _comObject.prop_ComObject_Property(name);
			if (propertyDescriptor.notNull())
				propertyDescriptor.SetValue(_comObject, value);
			else
				"[prop_ComObject_Value (setter)] didn't find property called {0} in provided object".error(name);
		}	
			
		
		
		//note:the results of this are not consistent
		public static TreeView showInfo_ComObject(this  object _rootObject)
		{
			var treeView = O2Gui.open<Panel>("showInfo_ComObject",400,400).add_TreeView();
			var propertyGrid = treeView.insert_Below<Panel>().add_PropertyGrid();
			
			Action<TreeNode, object> add_Object =
				(treeNode,_object)=>{
									treeNode.clear();									
									//treeNode.add_Node(_object.str(), _object, true);
									WinForms_ExtensionMethods_TreeView.add_Node(treeNode,_object.str(), _object, true);
								  };
			Action<TreeNode, IEnumerable> add_Objects = 
				(treeNode,items)=>{
									treeNode.clear();
									foreach(var item in items)
										//treeNode.add_Node(item.str(), item, true);
										WinForms_ExtensionMethods_TreeView.add_Node(treeNode, item.str(), item, true);
								  };
			
			 
			treeView.beforeExpand<object>(
				(treeNode, _object)=>{		
										if (_object is String)
											treeNode.add_Node(_object); 
										else
										{
											if (_object is IEnumerable)
												add_Objects(treeNode, _object as IEnumerable);
											else
												foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(_object))
												{
													try
													{
														var value =  property.GetValue(_object);
														treeNode.add_Node(property.Name.str(),value,true);
													}
													catch(Exception ex)
													{
														treeNode.add_Node(property.Name.str(),"O2 ERROR:".format(ex.Message) ,false);
													}
												}
										}
									 });
			
			treeView.afterSelect<object>(
				(_object)=> propertyGrid.show(_object));
				
			if(_rootObject is IEnumerable)
				add_Objects(treeView.rootNode(), _rootObject as IEnumerable);  
			else
				add_Object(treeView.rootNode(), _rootObject);  
			return treeView;
		}
    }

    public static class Lists_for_ComObject_ExtensionMethods
    { 
        //these helps with COM Objects received from IE	
		public static List<string> extractList_String(this object _object)
		{
			return _object.extractList<string>();
		}
		
		public static List<T> extractList<T>(this object _object)
		{
			return _object.extractList<T>(true);
		}
		
		public static List<T> extractList<T>(this object _object, bool logCastErrors)
		{
			var results = new List<T>();
			if (_object is IEnumerable)
			{
				foreach(var item in (IEnumerable)_object)
					if (item is T)
						results.Add((T)item);
					else
						if(logCastErrors)
							"[extractList] inside the IEnumerable, this item was not of type '{0}' it was of type: '{1}'".error(typeof(T), item.comTypeName());
			}
			else
				"[extractList] the provided object was not IEnumerable: {0}".error(_object);
			return results;
		}
		
		public static List<object> extractList(this object _object)
		{
			var results = new List<object>();
			if (_object is IEnumerable)
			{
				foreach(var item in (IEnumerable)_object)					
					results.Add(item);
			}
			else
				"[extractList] the provided object was not IEnumerable: {0}".error(_object);
			return results;
		}
    }
}
