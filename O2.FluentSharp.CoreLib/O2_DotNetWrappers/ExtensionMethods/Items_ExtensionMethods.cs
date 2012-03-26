using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Items_ExtensionMethods
	{
		public static Item item(this Items items, string key)
		{
			foreach(var item in items)
				if (item.Key == key)
					return item;
			return null;				
		}
		
		public static string value(this Item item)
		{
			if(item.notNull())
				return item.Value;
			return null;
		}
		
		public static Item value(this Item item, string value)
		{
			if(item.notNull())
				item.Value = value;
			return item;
		}
		
		public static bool hasKey(this Items items, string key)
		{
			foreach(var item in items)
				if (item.Key == key)
					return true;
			return false;		
					
		}			
		
		public static Items add(this Items items, string key, string value)
		{
			items[key] = value;
			return items;
		}		
		
		public static Dictionary<string,string> toDictionary(this Items items)
		{
			var dictionary = new Dictionary<string,string>();
			foreach(var item in items)	
			{
				if(dictionary.hasKey(item.Key))
					"alert: in Items.toDictionary there was a duplicate key value for {0}, the original value ('{1}') will be overritten with '{2}')".debug(item.Key, dictionary[item.Key], item.Value);					
				dictionary.add(item.Key, item.Value);
			}
			return dictionary;
		}
		
		public static Items toItems(this Dictionary<string,string> dictionary)
		{
			var items = new Items();
			foreach(var keyValuePair in dictionary)
				items.add(keyValuePair.toItem());
			return items;
		}
		
		public static Item toItem(this KeyValuePair<string,string> keyValuePair)
		{
			return new Item(keyValuePair.Key,keyValuePair.Value);
		}
		
		public static Items remove(this Items items, string key)
		{
			var itemToRemove = items.item(key);
			if(itemToRemove.isNull())
				"in Items.remove, could not find item with key: '{0}'".error(key);
			else
				items.Remove(itemToRemove);
			return items;
		}
		
		public static Items set(this Items items, string key, string value)
		{
			return items.add(key,value);
		}
		
		public static string get(this Items items, string key)
		{
			return items[key];
		}
		
		public static List<string> keys(this Items items)
		{
			return (from item in items
					select item.Key).toList();
		}
		
		public static List<string> values(this Items items)
		{
			return (from item in items
					select item.Value).toList();
		}
	}

    //legacy
    public static class NameValuePair_ExtensionMethods
	{									
		public static List<NameValuePair<T,K>> add<T,K>(this List<NameValuePair<T,K>> list, T key, K value)
		{
			list.Add(new NameValuePair<T,K>(key,value));
			return list;
		}
	}
}
