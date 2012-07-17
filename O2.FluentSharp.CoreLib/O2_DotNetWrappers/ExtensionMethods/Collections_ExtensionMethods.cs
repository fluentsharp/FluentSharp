using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using O2.DotNetWrappers.DotNet;

using System.Collections.Specialized;
using O2.Kernel.Objects;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Collections_ExtensionMethods_IEnumerable
    {
        public static string asString<T>(this IEnumerable<T> sequence) where T : class
        {
            return sequence.toString();
        }

        public static string    toString<T>(this IEnumerable<T> sequence) where T : class
        {
            var value = "";
            foreach (var item in sequence)
            {
                if (value.valid())
                    value += " , ";
                value += " \"{0}\"".format(item != null ? item.ToString() : "");
            }
            value = "{{ {0} }}".format(value);
            return value;
        }
        public static void      ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }
        public static void      forEach<T>(this IEnumerable collection, Action<T> action)
        {
            foreach (var item in collection)
                if (item is T)
                    action((T)item);
        }
        
        public static bool      isIEnumerable(this object list)
		{
			return list.notNull() && list is IEnumerable;
		}		
		public static int       count(this object list)
		{
			if (list.isIEnumerable())
				return (list as IEnumerable).count();
			return -1;
		}		
		public static int       size(this IEnumerable list)
		{
			return list.count();
		}
        public static bool      empty(this IEnumerable list)
        {
            return list.size() < 1;
        }
		public static int       count(this IEnumerable list)
		{			
			var count = 0;
			if (list.notNull())
				foreach(var item in list)
					count++;
			return count;
		}		
		public static object    first(this IEnumerable list)
		{
			if(list.notNull())
				foreach(var item in list)
					return item;
			return null;
		}		
		public static T         first<T>(this IEnumerable<T> list)
		{
			try
			{
				if (list.notNull())
					return list.First();
			}
			catch(Exception ex)
			{	
				if (ex.Message != "Sequence contains no elements")
					"[IEnumerable.first] {0}".error(ex.Message);
			}
			return default(T);
		}								
		public static T         last<T>(this IEnumerable<T> list)
		{
			try
			{
				if (list.notNull())
					return list.Last();
			}
			catch(Exception ex)
			{	
				if (ex.Message != "Sequence contains no elements")
					"[IEnumerable.first] {0}".error(ex.Message);
			}
			return default(T);
		}		
		public static object    last(this IEnumerable list)
		{
			object lastItem = null;
			if(list.notNull())
				foreach(var item in list)
					lastItem= item;
			return lastItem;
		}				
		public static List<T>   insert<T>(this List<T> list, T value)
		{
			return list.insert(0, value);
		}		
		public static List<T>   insert<T>(this List<T> list, int position, T value)
		{
			list.Insert(position, value);
			return list;
		}

        public static List<object>  toList(this IEnumerable collection)
        {
            return collection.toList<object>();
        }

        public static List<T>       toList<T>(this IEnumerable<T> collection)
        {
            return (collection != null) ? collection.ToList() : null;
        }
        public static List<T>       toList<T>(this IEnumerable list)
        {
            var results = new List<T>();
            foreach (var item in list)
                results.Add((T)item);
            return results;
        }
        public static List<string>  toList(this StringCollection stringCollection)
        {
            var results = new List<string>();
            foreach (var item in stringCollection)
                results.Add(item);
            return results;
        }

    }

    public static class Collections_ExtensionMethods_List
    {        
        public static List<string>          split_onLines(this string targetString)
        {
            return targetString.split(Environment.NewLine);
        }
        public static List<string>          split_onSpace(this string targetString)
        {
            return targetString.split(" ");
        }
        public static List<string>          split(this string targetString, string splitString)
        {
            var result = new List<string>();
            var splittedString = targetString.Split(new[] { splitString }, StringSplitOptions.None);
            result.AddRange(splittedString);
            return result;
        }
        public static List<List<string>>    split_onSpace(this List<string> list)
        {
            return list.split(" ");
        }
        public static List<List<string>>    split(this List<string> list, string splitString)
        {
            var result = new List<List<string>>();
            foreach (var item in list)
                result.Add(item.split(splitString));
            return result;
        }
        
        public static List<string>      lines(this string targetString)
        {
            return StringsAndLists.fromTextGetLines(targetString);
        }
        public static string            str(this List<String> list)
        {
            return StringsAndLists.fromStringList_getText(list);
        }
        public static T[]               array<T>(this List<T> list)
        {
            return list.ToArray();
        }
        public static bool              contains(this List<String> list, string text)
        {
            if (list != null)
                return list.Contains(text);
            return false;
        }

        public static List<T> clear<T>(this List<T> list)
        {
            list.Clear();
            return list;
        }

        public static List<string>      add_OnlyNewItems(this List<string> targetList, List<string> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                if (targetList.Contains(item).isFalse())
                    targetList.add(item);
            return targetList;
        }
        public static List<T>           add<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
        public static List<T>           add<T, T1>(this List<T> targetList, List<T1> sourceList) where T1 : T
        {
            foreach (var item in sourceList)
                targetList.Add(item);
            return targetList;
        }
        public static List<String>      sort(this List<String> list)
        {
            list.Sort();
            return list;
        }
        public static List<String>      lower(this List<String> list)
        {
            return (from item in list
                    select item.ToLower())
                    .toList();            
        }        
        public static bool              contains<T>(this List<T> list, T itemToFind)
        {
            return list.Contains(itemToFind);
        }
        public static List<T>           remove<T>(this List<T> list, int index)
        {
            if (list.size() > index)
                list.RemoveAt(index);
            return list;
        }
        public static List<T>           remove<T>(this List<T> list, T itemToRemove)
        { 
            if (list.contains(itemToRemove))
                list.Remove(itemToRemove);
            return list;
        }
        public static string            tabRight(this string targetString)
        {
            var newLines = new List<string>();
            foreach (var line in targetString.lines())
                newLines.Add("\t{0}".format(line));
            return StringsAndLists.fromStringList_getText(newLines);

        }
        public static List<T>           wrapOnList<T>(this T item)
        {
            var list = new List<T>();
            list.add(item);
            return list;
        }        

        public static List<string> removeEmpty(this List<string> list)
		{
			return (from item in list
					where item.valid()
					select item).toList();
		}
				
		public static List<string> add_If_Not_There(this List<string> list, string item)
		{
			if (item.notNull())
				if (list.contains(item).isFalse())
					list.add(item);
			return list;
		}

		public static string    join(this List<string> list)
		{
			return list.join(",");
		}		
		public static string    join(this List<string> list, string separator)
		{
			if (list.size()==1)
				return list[0];
			if (list.size() > 1)
				return list.Aggregate((a,b)=> "{0} {1} {2}".format(a,separator,b));
			return "";
		}		
		public static T         item<T>(this List<T> list, int index)
		{
			return list.value(index);
		}		
		public static T         value<T>(this List<T> list, int index)
		{
			if (list.size() > index)
				return list[index];
			return default(T);
		}		
		public static List<T>   where<T>(this List<T> list, Func<T,bool> query)
		{
			return list.Where<T>(query).toList();
		}		
		public static T         first<T>(this List<T> list, Func<T,bool> query)
		{
			var results = list.Where<T>(query).toList();
			if (results.size()>0)
				return results.First();
			return default(T);
		}		
		public static T         second<T>(this List<T> list)
		{			
			if (list.notNull() && list.size()>1)
				return list[1];		
			return default(T);
		}		
		public static List<T>   removeRange<T>(this List<T> list, int start, int end)
		{
			list.RemoveRange(start,end);
			return list;
		}	
		public static List<T>   list<T>(this T item)
		{
			return item.wrapOnList<T>();
		}		
		public static List<T>   push<T>(this List<T> list, T value)
		{
			if (list.notNull())			
				list.add(value);
			return list;
		}
		public static T         pop<T>(this List<T> list)
		{			
			if (list.notNull() && list.Count > 0)
			{
				int valuePosition = list.Count - 1;
				var value = list[valuePosition];
				list.RemoveAt(valuePosition);
				return value;
			}
			return  default(T);
		}		
		public static T         shift<T>(this List<T> list)
		{			
			if (list.notNull() && list.Count > 0)
			{
				T value = list[0];
				list.RemoveAt(0);
				return value;
			}
			return default(T);			
		}	
		public static List<T>   unshift<T>(this List<T> list, T value)
		{
			if (list.notNull())			
				list.Insert(0, value);
			return list;
		}

        public static void      createTypeAndAddToList<T>(this List<T> sequence, params object[] values)
        {
            var t = (T)typeof(T).ctor();
            var properties = t.type().properties();
            Loop.nTimes(values.Length, i => t.prop(properties[i], values[i]));            
            sequence.Add(t);
        }
    }
    public static class Collections_ExtensionMethods_ICollection
    {
        public static int       size(this ICollection colection)
        {
            return colection.Count;
        }
        public static bool      empty(this ICollection colection)
        {
            return colection.size() < 1;
        }
        public static T         first<T>(this ICollection<T> collection)
        {
            //collection.GetEnumerator().Reset();
            try
            {
                var enumerator = collection.GetEnumerator();
                enumerator.Reset();
                if (enumerator.MoveNext())
                    return enumerator.Current;
            }
            catch { }
            return default(T);
        }        
        public static bool      size(this ICollection colection, int value)
        {
            return colection.size() == value;
        }       
    }

    public static class Collections_ExtensionMethods_Dictionary
    {
        public static List<T>       keys<T, T1>(this Dictionary<T, T1> dictionary)
        {
            if (dictionary.notNull())
                return dictionary.Keys.toList();
            return new List<T>();
        }
        public static List<object>  values(this Dictionary<string, object> dictionary)
        {
            var results = new List<object>();
            results.AddRange(dictionary.Values);
            return results;
        }
        public static object[]      valuesArray(this Dictionary<string, object> dictionary)
        {
            return dictionary.values().ToArray();
        }
        public static List<T>       add_Key<T>(this Dictionary<string, List<T>> items, string keyToAdd)
        {
            if (items.ContainsKey(keyToAdd).isFalse())
                items.Add(keyToAdd, new List<T>());
            return items[keyToAdd];
        }
        public static bool          hasKey<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary != null && key != null)
                return dictionary.ContainsKey(key);
            return false;
        }
        public static T1            value<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            return dictionary.get(key);
        }        
        public static T1            get<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary.hasKey(key))
                return dictionary[key];
            return default(T1);
        }
        public static T             value<T>(this Dictionary<string,object> dictionary, string name)
		{
			var value = dictionary.value(name);
			if (value.notNull() && value is T)
				return (T)value;
			return default(T);
		}

        public static Dictionary<T, T1>         add<T, T1>(this Dictionary<T, T1> dictionary, T key, T1 value)
        {
			if (dictionary.isNull ())
			    "[Dictionary<T, T1> add] dictionary object was null".error ();
			else
			{
            	if (dictionary.hasKey(key))
            	    dictionary[key] = value;
            	else
            	    dictionary.Add(key, value);
			}
            return dictionary;
        }
        public static Dictionary<T, List<T1>>   add<T, T1>(this Dictionary<T, List<T1>> dictionary, T key, T1 value)
        {
            if (dictionary.hasKey(key).isFalse())
                dictionary[key] = new List<T1>();

            dictionary[key].Add(value);
            return dictionary;
        }
        public static Dictionary<T, T1>         remove<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            return dictionary.delete(key);
        }
        public static Dictionary<T, T1>         delete<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary.hasKey(key))
                dictionary.Remove(key);
            return dictionary;
        }

        public static Dictionary<string, T>         filter_By_ToString<T>(this List<T> list)
        {
            var results = new Dictionary<string, T>();
            foreach (var item in list)
            {
                var key = item.str();
                if (key.notNull())
                    results.add(key, item);
            }
            return results;
        }        	
        public static Dictionary<string, List<T>>   indexOnToString<T>(this List<T> items)
        {
            return items.indexOnToString("");
        }
        public static Dictionary<string, List<T>>   indexOnToString<T>(this List<T> items, string string_RegExFilter)
        {
            var result = new Dictionary<string, List<T>>();
            foreach (var item in items)
            {
                if (item != null)
                {
                    var str = item.str();
                    if (string_RegExFilter.valid().isFalse() || str.regEx(string_RegExFilter))
                        result.add(str, item);
                }
            }
            return result;
        }
        public static Dictionary<string, List<T>>   indexOnProperty<T>(this List<T> items, string propertyName, string string_RegExFilter)
        {
            var result = new Dictionary<string, List<T>>();
            foreach (var item in items)
            {
                if (item != null)
                {
                    var propertyValue = item.prop(propertyName);

                    if (propertyValue != null)
                    {
                        var str = propertyValue.str();
                        if (string_RegExFilter.valid().isFalse() || str.regEx(string_RegExFilter))
                            result.add(str, item);
                    }
                }
            }
            return result;
        }
        public static Dictionary<string, string>    clear(this Dictionary<string, string> dictionary)
        {
            if (dictionary.notNull())
                dictionary.Clear();
            return dictionary;
        }        
        
        public static Dictionary<string,string>     toStringDictionary(this string targetString, string rowSeparator, string keySeparator)
		{
			var stringDictionary = new Dictionary<string,string>();
			try
			{
				foreach(var row in targetString.split(rowSeparator))
				{
					if(row.valid())
					{
						var splittedRow = row.split(keySeparator);
						if (splittedRow.size()!=2)
							"[toStringDictionary] splittedRow was not 2: {0}".error(row);
						else
						{
							if (stringDictionary.hasKey(splittedRow[0]))
								"[toStringDictionary] key already existed in the collection: {0}".error(splittedRow[0]);		
							else
								stringDictionary.Add(splittedRow[0], splittedRow[1]);
						}
					}
				}
			}
			catch(Exception ex)
			{
				"[toStringDictionary] {0}".error(ex.Message);
			}
			return stringDictionary;
		}		
		public static Dictionary<string,string>     remove(this Dictionary<string,string> dictionary, Func<KeyValuePair<string,string>, bool> filter)
		{
			var itemsToRemove = dictionary.Where(filter).toList();
			//var itemsToRemove = CompileEngine.CachedCompiledAssemblies.Where((item)=>item.Value.str().contains("O2_TeamMentor_AspNet"));  
			foreach(var itemToRemove in itemsToRemove)
				dictionary.Remove(itemToRemove.Key);    
			return dictionary;
		}		
		
        
    }
    public static class Collections_ExtensionMethods_KeyValuePair
    {        
        public static int       totalValueSize(this List<KeyValuePair<string, string>> keyValuePairs)
        {
            var total = 0;
            foreach (var item in keyValuePairs)
                total += item.Value.size();
            return total;
        }
        public static List<T1>  values<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs)
        {
            return (from item in keyValuePairs
                    select item.Value).toList();
        }
        public static List<T>   keys<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs)
        {
            return (from item in keyValuePairs
                    select item.Key).toList();
        }
        public static T1        value<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs, int index)
        {
            if (index < keyValuePairs.size())
                return keyValuePairs[index].Value;
            return default(T1);
        }
        public static T         key<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs, int index)
        {
            if (index < keyValuePairs.size())
                return keyValuePairs[index].Key;
            return default(T);
        }        

        public static List<KeyValuePair<T, T1>> add<T, T1>(this List<KeyValuePair<T, T1>> valuePairList, T key, T1 value)
        {
            valuePairList.Add(new KeyValuePair<T, T1>(key, value));
            return valuePairList;
        }
    }

    public static class Collections_ExtensionMethods_KeyValueStrings
    {
        public static Dictionary<string, string>    toDictionary(this KeyValueStrings keyValueStrings)
        {
            if (keyValueStrings.isNull())
                return null;    
            var dictionary = new Dictionary<string, string>();            
            foreach (var item in keyValueStrings.Items)                
                dictionary.add(item.Key, item.Value);
            return dictionary;
        }
        public static KeyValueStrings               toKeyValueStrings(this Dictionary<string,string> dictionary)
        {
            if (dictionary.isNull())
                return null;
            var keyValueStrings = new KeyValueStrings();
            foreach (var item in dictionary)
                keyValueStrings.add(item.Key, item.Value);
            return keyValueStrings;
        }
        public static KeyValueStrings               toKeyValueStrings(this string file)
        {
            return file.load<KeyValueStrings>();
        }
        public static Dictionary<string, string>    configLoad(this string file)
        {
            return file.toKeyValueStrings().toDictionary();
        }

        public static string                        configSave(this Dictionary<string, string> dictionary)
        {
            return dictionary.toKeyValueStrings().save();
        }
        public static Dictionary<string, string>    configSave(this Dictionary<string, string> dictionary, string file)
        {
            dictionary.toKeyValueStrings().saveAs(file);
            return dictionary;
        }
    }

    public static class Loop_ExtensionMethods
    { 
    	public static Action            loop(this int count , Action action)
		{
			return count.loop(0,action);
		}	
		public static Action            loop(this int count , int delay,  Action action)
		{
			"Executing provided action for {0} times with a delay of {1} milliseconds".info(count, delay);
			for(var i=0 ; i < count ; i ++)
			{
				action();
				if (delay > 0)
					count.sleep(delay);
			}
			return action;
		}		
		public static Action<int>       loop(this int count , Action<int> action)
		{
			return count.loop(0, action);
		}		
		public static Action<int>       loop(this int count , int start, Action<int> action)
		{
			return count.loop(start,1, action);
		}		
		public static Action<int>       loop(this int count, int start , int step, Action<int> action)
		{
			for(var i=start ; i < count ; i+=step)			
				action(i);							
			return action;
		}		
		public static Func<int,bool>    loop(this int count, Func<int,bool> action)
		{
			return count.loop(0, action);
		}
		public static Func<int,bool>    loop(this int count, int start , Func<int,bool> action)
		{
			for(var i=start ; i < count ; i++)			
				if (action(i).isFalse())
					break;
			return action;
		}
		public static List<T>           loopIntoList<T>(this int count , Func<int,T> action)
		{
			return count.loopIntoList(0, action);
		}	
		public static List<T>           loopIntoList<T>(this int count , int start, Func<int,T> action)
		{
			return count.loopIntoList(start,1, action);
		}		
		public static List<T>           loopIntoList<T>(this int count, int start , int step, Func<int,T> action)
		{
			var results = new List<T>();
			for(var i=start ; i < count ; i+=step)			
				results.Add(action(i));
			return results;
		}
    }
}