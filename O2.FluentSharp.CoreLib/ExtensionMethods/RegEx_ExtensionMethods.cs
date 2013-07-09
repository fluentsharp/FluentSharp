using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class RegEx_ExtensionMethods
    {
        public static bool          nregEx(this string targetString, string regEx)
        {
            return !targetString.regEx(regEx);
        }
        public static bool          regEx(this string targetString, string regEx)
        {
            return RegEx.findStringInString(targetString, regEx);
        }
        public static bool          regEx(this List<string> targetStrings, string regEx)
        {
            foreach (var targetString in targetStrings)
                if (RegEx.findStringInString(targetString, regEx))
                    return true;
            return false;
        }
        public static bool          regExOk(this string _string)
        {
            return (RegEx.createRegEx(_string) != null);
        }
        public static List<string>  filterOnRegEx(this List<string> collection, string regEx)
        {
            return (from item in collection
                    where item.regEx(regEx)
                    select item).ToList();
        }
        public static List<string>  filter(this List<string> items, string textToFind)
        {
            textToFind = textToFind.lower();
            var filteredItems = from item in items
                                where item.lower().contains(textToFind)
                                select item;
            return filteredItems.toList();
        }
		public static Regex         regEx(this string matchPattern, bool bMultiline = true, bool bIgnoreCase = true, bool bExplicitCapture = true)
		{
			return RegEx.createRegEx(matchPattern, bMultiline, bIgnoreCase, bExplicitCapture);			
		}
		
		public static List<Match>   matches(this string textToSearch, string matchPattern)
		{
			return textToSearch.regEx_Matches(matchPattern);
		}
		
		public static List<Match>   regEx_Matches(this string textToSearch, string matchPattern)
		{
			var matches = new List<Match>();
			foreach(Match match in matchPattern.regEx().Matches(textToSearch))
				matches.Add(match);
			return matches;
		}       
    }
}
