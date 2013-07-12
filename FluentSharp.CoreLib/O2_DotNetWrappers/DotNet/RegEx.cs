// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;



namespace FluentSharp.CoreLib.API
{
    public class RegEx
    {
        public Regex regExWithPatternToSearch;

        public RegEx(String matchPattern)
        {
            regExWithPatternToSearch = createRegEx(matchPattern);
        }

        public bool find(String stringToRunRegExOn)
        {
            return execRegExOnText_hasMatches(regExWithPatternToSearch, stringToRunRegExOn);
        }

        public static bool findStringInString(String sSource, String sRegExToFind)
        {
            if (sSource == "")
                return false;
            //PublicDI.dRegExes.add(sRegExToFind, createRegEx(sRegExToFind));
            if (PublicDI.dRegExes.ContainsKey(sRegExToFind) == false)
                Collections_ExtensionMethods_Dictionary.add(PublicDI.dRegExes, sRegExToFind, createRegEx(sRegExToFind));

            return execRegExOnText_hasMatches(PublicDI.dRegExes[sRegExToFind], sSource);
        }

        public static Regex createRegEx(String sMatchPattern)
        {
            return createRegEx(sMatchPattern, true, true, true);
        }

        public static Regex createRegEx(String sMatchPattern, bool bMultiline, bool bIgnoreCase, bool bExplicitCapture)
        {
            if (sMatchPattern != null)
                try
                {
                    var rRegexOptions = RegexOptions.Compiled;
                    if (bMultiline)
                        rRegexOptions = rRegexOptions | RegexOptions.Multiline;
                    if (bIgnoreCase)
                        rRegexOptions = rRegexOptions | RegexOptions.IgnoreCase;
                    if (bExplicitCapture)
                        rRegexOptions = rRegexOptions | RegexOptions.ExplicitCapture;
                    return new Regex(sMatchPattern, rRegexOptions);
                }
                catch (Exception ex)
                {
                    PublicDI.log.error("in createRegEx:{0}", ex.Message);
                }
            return null;
        }

        public static bool execRegExOnText_hasMatches(Regex rRegEx, String sText)
        {
            if (rRegEx != null && sText != null)
                return rRegEx.IsMatch(sText);
            return false;
        }

        // some code reuse from http://en.csharp-online.net/CSharp_Regular_Expression_Recipes%E2%80%94Returning_the_Entire_Line_in_Which_a_Match_Is_Found
        public static List<String> execRegExOnText_getLines(Regex rRegEx, String sText)
        {
            var lsMatches = new List<String>();
            if (rRegEx != null && sText != null)
            {
                MatchCollection mcMatchCollection = rRegEx.Matches(sText);
                foreach (Match mMatch in mcMatchCollection)
                {
                    int lineStartPos = GetBeginningOfLine(sText, mMatch.Index);
                    int lineEndPos = GetEndOfLine(sText, (mMatch.Index + mMatch.Length - 1));
                    string line = sText.Substring(lineStartPos, lineEndPos - lineStartPos);
                    lsMatches.Add(line);
                }
            }
            return lsMatches;
        }

        public static int GetBeginningOfLine(string text, int startPointOfMatch)
        {
            if (startPointOfMatch > 0)
            {
                --startPointOfMatch;
            }
            if (startPointOfMatch >= 0 && startPointOfMatch < text.Length)
            {
                // Move to the left until the first '\n char is found
                for (int index = startPointOfMatch; index >= 0; index--)
                {
                    if (text[index] == '\n')
                    {
                        return (index + 1);
                    }
                }
                return (0);
            }
            return (startPointOfMatch);
        }

        public static int GetEndOfLine(string text, int endPointOfMatch)
        {
            if (endPointOfMatch >= 0 && endPointOfMatch < text.Length)
            {
                // Move to the right until the first '\n char is found
                for (int index = endPointOfMatch; index < text.Length; index++)
                {
                    if (text[index] == '\n')
                    {
                        return (index);
                    }
                }
                return (text.Length);
            }
            return (endPointOfMatch);
        }

        public static string convertLinesIntoRegExes(string regex)
        {
            if (regex == "")
                return "";
            var splittedRegEx = regex.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            regex = "";
            foreach (var line in splittedRegEx)
                regex += string.Format("{0}({1})",
                                       (regex != "") ? "|" : "",            // only add the | if there are more than one entry
                                       line);
            return regex;
        }
    }
}
