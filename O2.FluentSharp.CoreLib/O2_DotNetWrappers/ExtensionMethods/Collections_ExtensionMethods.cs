using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using O2.DotNetWrappers.DotNet;
using O2.Kernel.ExtensionMethods;
using System.Collections.Specialized;
using O2.Kernel.Objects;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class Collections_ExtensionMethods
    {
        #region IEnumerable

        public static string toString<T>(this IEnumerable<T> sequence) where T : class
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

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }

        public static void forEach<T>(this IEnumerable collection, Action<T> action)
        {
            foreach (var item in collection)
                if (item is T)
                    action((T)item);
        }

        public static List<T> toList<T>(this IEnumerable<T> collection)
        {
            return (collection != null) ? collection.ToList() : null;
        }

        public static List<T> toList<T>(this IEnumerable list)
        {
            var results = new List<T>();
            foreach (var item in list)
                results.Add((T)item);
            return results;
        }

        public static List<string> toList(this StringCollection stringCollection)
        {
            var results = new List<string>();
            foreach (var item in stringCollection)
                results.Add(item);
            return results;
        }


        #endregion
        
        #region List

        public static void createTypeAndAddToList<T>(this List<T> sequence, params object[] values)
        {
            var t = (T)typeof(T).ctor();
            var properties = t.type().properties();
            Loop.nTimes(values.Length, i => t.prop(properties[i], values[i]));            
            sequence.Add(t);
        }

        public static List<string> split_onLines(this string targetString)
        {
            return targetString.split(Environment.NewLine);
        }

        public static List<string> split_onSpace(this string targetString)
        {
            return targetString.split(" ");
        }

        public static List<string> split(this string targetString, string splitString)
        {
            var result = new List<string>();
            var splittedString = targetString.Split(new[] { splitString }, StringSplitOptions.None);
            result.AddRange(splittedString);
            return result;
        }

        public static List<List<string>> split_onSpace(this List<string> list)
        {
            return list.split(" ");
        }

        public static List<List<string>> split(this List<string> list, string splitString)
        {
            var result = new List<List<string>>();
            foreach (var item in list)
                result.Add(item.split(splitString));
            return result;
        }

        public static string tabRight(this string targetString)
        {
            var newLines = new List<string>();
            foreach (var line in targetString.lines())
                newLines.Add("\t{0}".format(line));
            return StringsAndLists.fromStringList_getText(newLines);

        }

        public static List<string> lines(this string targetString)
        {
            return StringsAndLists.fromTextGetLines(targetString);
        }

        public static string str(this List<String> list)
        {
            return StringsAndLists.fromStringList_getText(list);
        }

        public static T[] array<T>(this List<T> list)
        {
            return list.ToArray();
        }

        public static bool contains(this List<String> list, string text)
        {
            if (list != null)
                return list.Contains(text);
            return false;
        }

        public static List<string> add_OnlyNewItems(this List<string> targetList, List<string> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                if (targetList.Contains(item).isFalse())
                    targetList.add(item);
            return targetList;
        }

        public static List<T> add<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static List<T> add<T, T1>(this List<T> targetList, List<T1> sourceList) where T1 : T
        {
            foreach (var item in sourceList)
                targetList.Add(item);
            return targetList;
        }

        public static List<String> sort(this List<String> list)
        {
            list.Sort();
            return list;
        }

        public static List<String> lower(this List<String> list)
        {
            return (from item in list
                    select item.ToLower())
                    .toList();            
        }
        

        public static bool contains<T>(this List<T> list, T itemToFind)
        {
            return list.Contains(itemToFind);
        }

        public static List<T> remove<T>(this List<T> list, int index)
        {
            if (list.size() > index)
                list.RemoveAt(index);
            return list;
        }

        public static List<T> remove<T>(this List<T> list, T itemToRemove)
        { 
            if (list.contains(itemToRemove))
                list.Remove(itemToRemove);
            return list;
        }

        #endregion

        #region ICollection

        public static int size(this ICollection colection)
        {
            return colection.Count;
        }

        public static T first<T>(this ICollection<T> collection)
        {
            //collection.GetEnumerator().Reset();
            var enumerator = collection.GetEnumerator();
            enumerator.Reset();
            if (enumerator.MoveNext())            
                return enumerator.Current;
            return default(T);
        }
        
        public static bool size(this ICollection colection, int value)
        {
            return colection.size() == value;
        }

        #endregion 

        #region Dictionary

        public static List<T> keys<T, T1>(this Dictionary<T, T1> dictionary)
        {
            if (dictionary.notNull())
                return dictionary.Keys.toList();
            return new List<T>();
        }

        public static List<object> values(this Dictionary<string, object> dictionary)
        {
            var results = new List<object>();
            results.AddRange(dictionary.Values);
            return results;
        }

        public static object[] valuesArray(this Dictionary<string, object> dictionary)
        {
            return dictionary.values().ToArray();
        }

        public static List<T> add_Key<T>(this Dictionary<string, List<T>> items, string keyToAdd)
        {
            if (items.ContainsKey(keyToAdd).isFalse())
                items.Add(keyToAdd, new List<T>());
            return items[keyToAdd];
        }

        public static bool hasKey<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary != null && key != null)
                return dictionary.ContainsKey(key);
            return false;
        }

        public static Dictionary<T, T1> add<T, T1>(this Dictionary<T, T1> dictionary, T key, T1 value)
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

        public static Dictionary<T, List<T1>> add<T, T1>(this Dictionary<T, List<T1>> dictionary, T key, T1 value)
        {
            if (dictionary.hasKey(key).isFalse())
                dictionary[key] = new List<T1>();

            dictionary[key].Add(value);
            return dictionary;
        }

        public static Dictionary<string, T> filter_By_ToString<T>(this List<T> list)
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

        public static Dictionary<string, List<T>> indexOnToString<T>(this List<T> items)
        {
            return items.indexOnToString("");
        }

        public static Dictionary<string, List<T>> indexOnToString<T>(this List<T> items, string string_RegExFilter)
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

        public static Dictionary<string, List<T>> indexOnProperty<T>(this List<T> items, string propertyName, string string_RegExFilter)
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

        public static T1 value<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            return dictionary.get(key);
        }

        public static T1 get<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary.hasKey(key))
                return dictionary[key];
            return default(T1);
        }


        public static Dictionary<T, T1> remove<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            return dictionary.delete(key);
        }

        public static Dictionary<T, T1> delete<T, T1>(this Dictionary<T, T1> dictionary, T key)
        {
            if (dictionary.hasKey(key))
                dictionary.Remove(key);
            return dictionary;
        }

        public static Dictionary<string, string> clear(this Dictionary<string, string> dictionary)
        {
            if (dictionary.notNull())
                dictionary.Clear();
            return dictionary;
        }
        #endregion 

        #region KeyValuePair

        public static List<KeyValuePair<T, T1>> add<T, T1>(this List<KeyValuePair<T, T1>> valuePairList, T key, T1 value)
        {
            valuePairList.Add(new KeyValuePair<T, T1>(key, value));
            return valuePairList;
        }

        public static int totalValueSize(this List<KeyValuePair<string, string>> keyValuePairs)
        {
            var total = 0;
            foreach (var item in keyValuePairs)
                total += item.Value.size();
            return total;
        }

        public static List<T1> values<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs)
        {
            return (from item in keyValuePairs
                    select item.Value).toList();
        }

        public static List<T> keys<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs)
        {
            return (from item in keyValuePairs
                    select item.Key).toList();
        }

        public static T1 value<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs, int index)
        {
            if (index < keyValuePairs.size())
                return keyValuePairs[index].Value;
            return default(T1);
        }

        public static T key<T, T1>(this List<KeyValuePair<T, T1>> keyValuePairs, int index)
        {
            if (index < keyValuePairs.size())
                return keyValuePairs[index].Key;
            return default(T);
        }

        #endregion 

        #region KeyValueStrings

        public static Dictionary<string, string> toDictionary(this KeyValueStrings keyValueStrings)
        {
            if (keyValueStrings.isNull())
                return null;    
            var dictionary = new Dictionary<string, string>();            
            foreach (var item in keyValueStrings.Items)                
                dictionary.add(item.Key, item.Value);
            return dictionary;
        }

        public static KeyValueStrings toKeyValueStrings(this Dictionary<string,string> dictionary)
        {
            if (dictionary.isNull())
                return null;
            var keyValueStrings = new KeyValueStrings();
            foreach (var item in dictionary)
                keyValueStrings.add(item.Key, item.Value);
            return keyValueStrings;
        }

        public static KeyValueStrings toKeyValueStrings(this string file)
        {
            return file.load<KeyValueStrings>();
        }

        public static Dictionary<string, string> configLoad(this string file)
        {
            return file.toKeyValueStrings().toDictionary();
        }

        public static string configSave(this Dictionary<string, string> dictionary)
        {
            return dictionary.toKeyValueStrings().save();
        }
        public static Dictionary<string, string> configSave(this Dictionary<string, string> dictionary, string file)
        {
            dictionary.toKeyValueStrings().saveAs(file);
            return dictionary;
        }

        #endregion

    }
}