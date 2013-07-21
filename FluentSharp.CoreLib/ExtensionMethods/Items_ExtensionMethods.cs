using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib.API;


namespace FluentSharp.CoreLib
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
		public static Items set_Value(this Items items, string key, string value)
		{
			return items.add(key,value);
		}		
		public static string get_Value(this Items items, string key)
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
		public static List<string> values(this Items items, List<string> columns)
		{
			return (from column in columns
					select items[column]).toList();
		}
		public static List<string> uniqueKeys(this List<Items> itemsList)
		{
			return (from items in itemsList
					from key in items.keys()
					select key).distinct();
		}
		public static List<string> uniqueKeys_WithValidValue(this List<Items> itemsList)
		{
			return (from items in itemsList
					from item in items
					where item.Value.valid()
					select item.Key).distinct();
		}		
		public static Dictionary<string, List<string>> indexBy_Key(this List<Items> itemsList)
		{
			var mappedData = new Dictionary<string, List<string>>();
			foreach (var items in itemsList)
				foreach (var item in items)
					mappedData.add(item.Key, item.Value);
			return mappedData;
		}
	}

    //legacy
    public static class NameValuePair_ExtensionMethods
	{									
		public static List<NameValuePair<T,TK>> add<T,TK>(this List<NameValuePair<T,TK>> list, T key, TK value)
		{
			list.Add(new NameValuePair<T,TK>(key,value));
			return list;
		}
	}
}
