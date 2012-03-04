using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class RegEx_ExtensionMethods
    {
        public static bool nregEx(this string targetString, string regEx)
        {
            return !targetString.regEx(regEx);
        }

        public static bool regEx(this string targetString, string regEx)
        {
            return RegEx.findStringInString(targetString, regEx);
        }

        public static bool regEx(this List<string> targetStrings, string regEx)
        {
            foreach (var targetString in targetStrings)
                if (RegEx.findStringInString(targetString, regEx))
                    return true;
            return false;
        }

        public static bool regExOk(this string _string)
        {
            return (RegEx.createRegEx(_string) != null);
        }

        public static List<string> filterOnRegEx(this List<string> collection, string regEx)
        {
            return (from item in collection
                    where item.regEx(regEx)
                    select item).ToList(); ;
        }

        public static List<string> filter(this List<string> items, string textToFind)
        {
            textToFind = textToFind.lower();
            var filteredItems = from item in items
                                where item.lower().contains(textToFind)
                                select item;
            return filteredItems.toList();
        }
       
    }
}
