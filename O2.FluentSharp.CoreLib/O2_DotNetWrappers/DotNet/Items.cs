using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using System.Xml.Serialization;

namespace O2.DotNetWrappers.DotNet
{
    [Serializable]
	public  class NameValueItems : Items
	{
		
	}

    [Serializable]
	public class Items : List<Item> 
	{
		public bool overrideIfExists = true;
		public bool alertOnOverriding = true;
		
		public string this[string key] 
		{
			get
			{
				foreach(var item in this)
					if (item.Key == key)
						return item.Value;
				return null;
					//return new Item(value,value);
			}	
			set
			{
				if (overrideIfExists)				
					foreach(var item in this)
						if (item.Key == key)
						{	
							if(alertOnOverriding)
								"Item Override: on key value '{0}', overriding the original value '{1}' with '{2}'".debug(item.Key,item.Value, value);					
							item.Value = value;
							return;
						}
				this.Add(new Item(key, value));
			}
		}				
	}
    
	[Serializable]
	public class Item : NameValuePair<string,string>
	{
		public Item()
		{}
		
		public Item(string key, string value) : base(key,value)
		{
			
		}
	}

    [Serializable]
	public  class NameValuePair<T,K>
	{
		[XmlAttribute] public T Key {get;set;}
		[XmlAttribute] public K Value {get;set;}
		
		public NameValuePair()
		{}
		
		public NameValuePair(T key, K value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return Key.str();
		}
	}
}
