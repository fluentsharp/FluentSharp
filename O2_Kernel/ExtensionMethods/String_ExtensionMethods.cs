using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using O2.Kernel.CodeUtils;

//O2File:../DI.cs
//O2File:../PublicDI.cs
//O2File:../CodeUtils/O2Kernel_Processes.cs

namespace O2.Kernel.ExtensionMethods
{
    public static class String_ExtensionMethods
    {

        public static string str(this object _object)
        {
            return (_object != null) ? _object.ToString() : "[null value]";
        }
        
        public static string str(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }

        public static bool validString(this object _object)
        {
            return _object.str().valid();
        }

        public static bool eq(this string string1, string string2)
        {
            return (string1 == string2);
        }

        public static void eq(this string string1, string stringToFind, Action onMatch)
        {
            string1.eq(new [] {stringToFind}, onMatch);
        }
        
        public static void eq(this string string1, List<string> stringsToFind, Action onMatch)
        {
            string1.eq(stringsToFind.ToArray(), onMatch);
        }

        public static void eq(this string string1, string[] stringsToFind, Action onMatch) 
        {
            foreach(var stringToFind in stringsToFind)
                if (string1 == stringToFind)
                {
                    onMatch();
                    return;
                }
        }        

        public static bool neq(this string string1, string string2)
        {
            return (string1 != string2);
        }        

        public static bool contains(this string targetString, string stringToFind)
        {
            return (stringToFind != null)
                        ? targetString.Contains(stringToFind)
                        : false;
        }

        public static bool contains(this string targetString, List<string> stringsToFind)
        {
            return targetString.contains(stringsToFind.ToArray());
        }

        public static bool contains(this string targetString, params string[] stringsToFind)
        {
            if (stringsToFind.notNull())
            {
                foreach (var stringToFind in stringsToFind)
                    if (targetString.Contains(stringToFind))
                        return true;
            }
            return false;
        }
   
        public static bool starts(this string textToSearch, List<string> stringsToFind)
        {
            foreach(var stringToFind in stringsToFind)
                if (textToSearch.starts(stringToFind))
                    return true;
            return false;
        }

        public static bool starts(this string stringToSearch, string stringToFind)
        {
            return stringToSearch.StartsWith(stringToFind);
        }

        public static void starts(this string stringToSearch, string[] stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }

        public static void starts(this string stringToSearch, List<string> stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }

        public static void starts(this string stringToSearch, List<string> stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind.ToArray(), invokeOnMatchIfEqual, onMatch);
        }

        public static void starts(this string stringToSearch, string[] stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            foreach(var stringToFind in stringsToFind)
                if (stringToSearch.starts(stringToFind, invokeOnMatchIfEqual, onMatch))
                    return;
        }

        public static void starts(this string stringToSearch, string textToFind, Action<string> onMatch)
        {
            stringToSearch.starts(textToFind, true, onMatch);
        }

        public static bool starts(this string stringToSearch, string textToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            if (stringToSearch.starts(textToFind))
            {
                var textForCallback = stringToSearch.remove(textToFind);
                if (invokeOnMatchIfEqual || textForCallback.valid())
                {
                    onMatch(textForCallback);
                    return true;
                }
            }
            return false;
        }

        public static bool nstarts(this string stringToSearch, string stringToFind)
        {
            return ! starts(stringToSearch, stringToFind);
        }

        public static bool ends(this string string1, string string2)
        {
            return string1.EndsWith(string2);
        }

        public static bool inValid(this string _string)
        {
            return !_string.valid();
        }

        public static bool valid(this string _string)
        {
            if (_string != null && false == string.IsNullOrEmpty(_string))
                if (_string.Trim() != "")
                    return true;
            return false;
        }

        public static bool empty(this string _string)
        {
            return !(_string.valid());
        }

        public static string format(this string format, params object[] parameters)
        {
            if (format == null)
                return "";
            if (parameters == null)
                return format;
            try
            {
                return string.Format(format, parameters);
            }
            catch (Exception ex)
            {
                ex.log("error applying string format: " + format ?? "[null]");
                return "";
            }
        }

        public static string remove(this string _string, params string[] stringsToRemove)
        {
            return _string.replaceAllWith("", stringsToRemove);
        }

        public static string toSpace(this string _string, params string[] stringsToChange)
        {
            return _string.replaceAllWith(" ", stringsToChange);
        }

        public static string replace(this string targetString, string stringToFind, string stringToReplaceWith)
        {
            if (stringToFind.notNull())
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            // need to find a better way to do this replace (maybe using regex) since this pattern was causing some nasty side effects (for example when replacing \n with Environment.NewLine)
            //targetString = targetString.Replace(stringToFind.lower(), stringToReplaceWith);
            //targetString = targetString.Replace(stringToFind.upper(), stringToReplaceWith);
            return targetString;
        }

        public static string replaceAllWith(this string targetString, string stringToReplaceWith, params string[] stringsToFind)
        {
            foreach (var stringToFind in stringsToFind)
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            return targetString;
        }

        public static int size(this string _string)
        {
            if (_string.valid())
                return _string.Length;
            return 0;
        }

        public static string line(this string firstString, string secondString)
        {
            return firstString.line() + secondString;
        }

        public static string line(this string firstString)
        {
            return firstString + Environment.NewLine;
        }

        public static string lineBefore(this string targetString)
        {
            return Environment.NewLine + targetString;
        }

        public static string lineBeforeAndAfter(this string targetString)
        {
            return Environment.NewLine + targetString + Environment.NewLine;
        }

        public static bool isInt(this string value)
        {
            int a = 0;
            return Int32.TryParse(value, out a);
        }

        public static int toInt(this string _string)
        {
            Int32 value;
            Int32.TryParse(_string, out value);
            return value;
        }

        public static string hex(this byte value)
        {
            return Convert.ToString(value, 16).caps();
        }

        public static string hex(this int value)
        {
            return Convert.ToString(value, 16).caps();
        }

        public static string caps(this string value)
        {
            return value.ToUpper();
        }

        public static string lowerCaseFirstLetter(this string targetString)
        {
            if (targetString.valid())
                return targetString[0].str().lower() + targetString.removeFirstChar();
            return targetString;
        }

        public static string fixCRLF(this string stringToFix)
        {
            if (stringToFix.contains(Environment.NewLine))
                return stringToFix;
            if (stringToFix.contains("\n"))
                return stringToFix.Replace("\n", Environment.NewLine);
            return stringToFix;
        }
    
        public static string ascii(this byte value)
        {
            return Encoding.ASCII.GetString(new[] { value });
        }        

        public static string ascii(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static string unicode(this byte value)
        {
            return Encoding.Unicode.GetString(new[] { value });
        }

        public static string unicode(this byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes);
        }

        //this method is only really good to find ASCII binary strings
        public static List<string> strings(this byte[] bytes, bool ignoreCharZeroAfterValidChar, int minimumStringSize)
        {
            var extractedStrings = new List<string>();
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                var value = bytes[i];
                if (value > 31 && value < 127) // see http://www.asciitable.com/
                {
                    var str = value.ascii();
                    stringBuilder.Append(str);
                    if (ignoreCharZeroAfterValidChar)
                        if (bytes[i + 1] == 0)
                            i++;
                }
                else
                {
                    if (stringBuilder.Length > 0)
                    {
                        if (minimumStringSize == -1 || stringBuilder.Length > minimumStringSize)
                            extractedStrings.Add(stringBuilder.ToString());
                        stringBuilder = new StringBuilder();
                    }
                }
            }            
            return extractedStrings;
        }

        public static void removeLastChar(this StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        public static string removeLastChar(this string _string)
        {
            if (_string.Length > 0)
                return _string.Remove(_string.Length - 1, 1);
            return _string;
        }

        public static string removeFirstChar(this string _string)
        {
            return (_string.Length > 0)
                ? _string.Substring(1)
                : _string;

        }

        public static string replaceLast(this string stringToSearch, string findString, string replaceString)
        {
            var lastIndexOf = stringToSearch.LastIndexOf(findString);
            lastIndexOf.str().info();
            if (lastIndexOf > -1)
            {
                var beforeSubstring = stringToSearch.Substring(0, lastIndexOf);
                var afterString_StartPosition = (lastIndexOf + findString.size());
                var afterString = (afterString_StartPosition < stringToSearch.size())
                                    ? stringToSearch.Substring(afterString_StartPosition)
                                    : "";
                return "{0}{1}{2}".format(beforeSubstring, replaceString, afterString);
            }
            return "";
        }

        public static string appendGuid(this string _string)
        {
            return "{0} {1}".format(_string, Guid.NewGuid());
        }

        public static string lower(this string _string)
        {
            return _string.ToLower();
        }

        public static string upper(this string _string)
        {
            return _string.ToUpper();
        }

        public static string trim(this string _string)
        {           
            return _string.Trim();
        }

        public static int index(this string targetString, string stringToFind)
        {
            return targetString.IndexOf(stringToFind);
        }

        public static int indexLast(this string targetString, string stringToFind)
        {
            return targetString.LastIndexOf(stringToFind);
        }

        public static string pad(this string targetString, int totalWidth)
        {
            return targetString.PadLeft(totalWidth);
        }

        public static string add(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }

        public static string insertAfter(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }

        public static string insertBefore(this string targetString, string stringToAdd)
        {
            return stringToAdd + targetString;
        }      

        public static int toIntFromHex(this string hexValue)
        {
            try
            {
                return Convert.ToInt32(hexValue, 16);
            }
            catch (Exception ex)
            {
                ex.log("in toIntFromHex when converting string: {0}".format(hexValue));
                return -1;
            }
        }

        public static string repeat(this char charToRepeat, int count)
        {
            if (count > 0)
                return new String(charToRepeat, count);
            return "";
        }

        public static string tempFile(this string postfixString)
        {
            return PublicDI.config.getTempFileInTempDirectory(postfixString);
        }


        public static string o2Temp2Dir(this string tempFolderName)
        {
            return tempFolderName.o2Temp2Dir(true);
        }
        public static string o2Temp2Dir(this string tempFolderName, bool appendRandomStringToFolderName)
        {
            if (tempFolderName.valid())
                if (appendRandomStringToFolderName)
                    return PublicDI.config.getTempFolderInTempDirectory(tempFolderName);
                else
                {
                    var tempFolder = Path.Combine(PublicDI.config.O2TempDir, tempFolderName);
                    O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(tempFolder);
                    return tempFolder;
                }
            return PublicDI.config.O2TempDir;
        }

        public static string tempO2Dir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }

        public static string tempDir(this string tempFolderName, bool appendRandomStringToFolderName)
        {
            return o2Temp2Dir(tempFolderName, appendRandomStringToFolderName);
        }

        public static string tempDir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }
    }
}