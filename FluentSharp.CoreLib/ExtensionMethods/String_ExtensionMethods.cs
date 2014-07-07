using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;


//O2File:../PublicDI.cs
//O2File:../PublicDI.cs
//O2File:../CodeUtils/O2Kernel_Processes.cs

namespace FluentSharp.CoreLib
{
    public static class String_ExtensionMethods
    {
        /// <summary>
        /// Wrapper around string's ToString method        
        /// 
        /// The return value will be:
        /// 
        ///   - ToString() value of the target object 
        ///   - <value>[null value]</value>: if the target object is null
        ///   - <value>[ToString exeception]</value>: if the ToString methods throws an exception (which shouldn't really happen). see log for details
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string    str(this object target)
        {
            try
            {
                return (target != null) ? target.ToString() : "[null value]";
            }
            catch(Exception ex)
            {
                //This really shouldn't happen but its better to catch it here
                "[object][str] ToString threw an exception on provided object: {0}".error(ex.Message);
                return "[ToString exeception]";  
            }
        }        
        public static string    str(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }
        public static bool      eq(this string _string, params string[] values)
        {
            if (_string.isNull() || values.isNull())
                return false;
            return values.Any(value => _string == value);
        }        
        public static void      eq(this string string1, string stringToFind, Action onMatch)
        {
            string1.eq(new [] {stringToFind}, onMatch);
        }        
        public static void      eq(this string string1, List<string> stringsToFind, Action onMatch)
        {
            string1.eq(stringsToFind.ToArray(), onMatch);
        }
        public static void      eq(this string string1, string[] stringsToFind, Action onMatch)
        {
            if (stringsToFind.Any(stringToFind => string1 == stringToFind))
                onMatch();                
        }

        public static bool      neq(this string string1, string string2)
        {
            return (string1 != string2);
        }        
        public static bool      contains(this string targetString, string stringToFind)
        {
            return targetString.notNull() && 
                   stringToFind.notNull() && 
                   targetString.Contains(stringToFind);
        }
        public static bool      contains(this string targetString, List<string> stringsToFind)
        {
            return targetString.contains(stringsToFind.toArray());
        }
        public static bool      contains(this string targetString, params string[] stringsToFind)
        {
            if (stringsToFind.notNull())
            {
                foreach (var stringToFind in stringsToFind)
                    if (targetString.contains(stringToFind))
                        return true;
            }
            return false;
        }   

        public static bool      starts(this string textToSearch, List<string> stringsToFind)
        {
            if (textToSearch.valid() && stringsToFind.notNull())
                foreach(var stringToFind in stringsToFind)
                    if (textToSearch.starts(stringToFind))
                        return true;
            return false;
        }
        public static bool      starts(this string stringToSearch, string stringToFind)
        {
            if (stringToSearch.notValid() || stringToFind.notValid())
                return false;
            return stringToSearch.StartsWith(stringToFind);
        }
        public static bool      starts(this string stringToSearch, string[] stringsToFind)
        {
            if (stringToSearch.notValid() || stringsToFind.empty())
                return false;
            return stringsToFind.Any(stringToSearch.StartsWith);
        }

        public static void      starts(this string stringToSearch, string[] stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }
        public static void      starts(this string stringToSearch, List<string> stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }
        public static void      starts(this string stringToSearch, List<string> stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind.ToArray(), invokeOnMatchIfEqual, onMatch);
        }
        public static void      starts(this string stringToSearch, string[] stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            foreach(var stringToFind in stringsToFind)
                if (stringToSearch.starts(stringToFind, invokeOnMatchIfEqual, onMatch))
                    return;
        }
        public static void      starts(this string stringToSearch, string textToFind, Action<string> onMatch)
        {
            stringToSearch.starts(textToFind, true, onMatch);
        }
        public static bool      starts(this string stringToSearch, string textToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
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
        /*public static bool      nstarts(this string stringToSearch, string stringToFind)
        {
            return ! starts(stringToSearch, stringToFind);
        }*/

        public static bool ends(this string stringToSearch, string stringToFind)
        {
            if (stringToSearch.notValid() || stringToFind.notValid())
                return false;
            return stringToSearch.EndsWith(stringToFind);
        }

        public static bool      ends(this string stringToSearch, string[] stringsToFind)
        {
            foreach(var stringToFind in stringsToFind)
                if (stringToSearch.EndsWith(stringToFind))
                    return true;
            return false;            
        }

        public static List<string> containing(this List<string> strings, string stringToFind)
        {
            return strings.where((_string) => _string.contains(stringToFind));
        }
        public static List<string> starting(this List<string> strings, string stringToFind)
        {
            return strings.where((_string) => _string.starts(stringToFind));
        }
        public static List<string> ending(this List<string> strings, string stringToFind)
        {
            return strings.where((_string) => _string.ends(stringToFind));
        }

        public static bool      inValid(this string _string)
        {
            return !_string.valid();
        }
        public static bool      notValid(this string _string)
        {
            return !_string.valid();
        }
        public static bool      valid(this string _string)
        {
            if (_string != null && false == string.IsNullOrEmpty(_string))
                if (_string.Trim() != "")
                    return true;
            return false;
        }
        public static bool      isEmpty(this string target)
        {
            return target.empty();
        }
        public static bool      empty(this string target)
        {
            return !(target.valid());
        }
        public static bool      validString(this object _object)
        {
            return _object.str().valid();
        }

        public static string    format(this string format, params object[] parameters)
        {
            if (format == null)
                return "";
            if (parameters.empty())
                return format;
            try
            {
                return string.Format(format, parameters);
            }
            catch (Exception ex)
            {
                ("Error applying format " + format + "\nexception message: " + ex.Message).error();
                //ex.log("error applying string format: " + format); // can't do this or we will have a recursive error call to this .format method
                return "";
            }
        }
        public static string    remove(this string _string, params string[] stringsToRemove)
        {
            return _string.replaceAllWith("", stringsToRemove);
        }
        public static string    toSpace(this string _string, params string[] stringsToChange)
        {
            return _string.replaceAllWith(" ", stringsToChange);
        }
        public static string    replace(this string targetString, string stringToFind, string stringToReplaceWith)
        {
            if (stringToFind.notNull() && targetString.notNull() && stringToReplaceWith.notNull())
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            // need to find a better way to do this replace (maybe using regex) since this pattern was causing some nasty side effects (for example when replacing \n with Environment.NewLine)
            //targetString = targetString.Replace(stringToFind.lower(), stringToReplaceWith);
            //targetString = targetString.Replace(stringToFind.upper(), stringToReplaceWith);
            return targetString;
        }
        public static string    replaceAllWith(this string targetString, string stringToReplaceWith, params string[] stringsToFind)
        {
            foreach (var stringToFind in stringsToFind)
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            return targetString;
        }
        public static int       size(this string _string)
        {
            if (_string.notNull())
                return _string.Length;
            return 0;
        }

        public static string    line(this string firstString, string secondString)
        {
            if (firstString.isNull())
                return secondString;
            return firstString.line() + secondString;
        }
        public static string    line(this string firstString)
        {
            if (firstString.isNull())
                return Environment.NewLine;
            return firstString + Environment.NewLine;
        }
        public static string    lineBefore(this string targetString)
        {
            if (targetString.isNull())
                return Environment.NewLine;
            return Environment.NewLine + targetString;
        }
        public static string    lineBeforeAndAfter(this string targetString)
        {
            if (targetString.isNull())
                return Environment.NewLine;
            return Environment.NewLine + targetString + Environment.NewLine;
        }

        public static bool      isInt(this string value)
        {
            int a;
            return Int32.TryParse(value, out a);
        }
        public static int       toInt(this string _string)
        {
            Int32 value;
            Int32.TryParse(_string, out value);
            return value;
        }

        
        public static string    hex(this int value)
        {
            return Convert.ToString(value, 16).caps();
        }
        public static byte[]    hexStringToByteArray(this string hex)
        {
            try
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }
            catch (Exception ex)
            {
                ex.log();
                return new byte[] { };
            }
        }
        
        public static string    caps(this string value)
        {
            if (value.isNull())
                return null;
            return value.ToUpper();
        }
        public static string    lower(this string value)
        {
            if (value.isNull())
                return null;
            return value.ToLower();
        }
        public static string    upper(this string value)
        {
            if (value.isNull())
                return null;
            return value.ToUpper();
        }

        public static string    lowerCaseFirstLetter(this string targetString)
        {
            if (targetString.valid())
                return targetString[0].str().lower() + targetString.removeFirstChar();
            return targetString;
        }

        public static string    fix_CRLF (this string stringToFix)
        {            
            if (stringToFix.isNull())
                return null;
            return stringToFix.Replace(Environment.NewLine, "\n")
                              .Replace("\n", Environment.NewLine);
        }            
        public static StringBuilder appendLine(this StringBuilder stringBuilder, string line)
        {
            if (stringBuilder.notNull() && line.notNull())
                stringBuilder.AppendLine(line);
            return stringBuilder;
        }
        public static StringBuilder appendLines(this StringBuilder stringBuilder, params string[] lines)
        {
            if (stringBuilder.notNull() && lines.notNull())
                foreach(var line in lines)
                    stringBuilder.appendLine(line);
            return stringBuilder;
        }
        public static void      removeLastChar(this StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
        public static string    removeLastChar(this string _string)
        {
            if (_string.Length > 0)
                return _string.Remove(_string.Length - 1, 1);
            return _string;
        }
        public static string    removeFirstChar(this string _string)
        {
            return (_string.Length > 0)
                ? _string.Substring(1)
                : _string;

        }
        public static string    replaceLast(this string stringToSearch, string findString, string replaceString)
        {
            var lastIndexOf = stringToSearch.LastIndexOf(findString, StringComparison.Ordinal);
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

        public static string    appendGuid(this string _string)
        {
            return "{0} {1}".format(_string, Guid.NewGuid());
        }

        public static string    trim(this string target)
        {           
            if (target.valid())
                return target.Trim();
            return target;
        }
        public static string    pad(this string targetString, int totalWidth)
        {
            return targetString.PadLeft(totalWidth);
        }
        public static int       index(this string targetString, string stringToFind)
        {
			if (targetString.notValid() || stringToFind.notValid())
				return -1;
            return targetString.IndexOf(stringToFind, StringComparison.Ordinal);
        }
        public static int       index(this string targetString, string stringToFind, int startIndex)
        {
			if (targetString.notValid() || stringToFind.notValid() || startIndex > targetString.size())
				return -1;
            return targetString.IndexOf(stringToFind, startIndex,StringComparison.Ordinal);
        }
        public static string    tabLeft(this string targetString)
        {
            var newLines = new List<string>();
            foreach (var line in targetString.lines())
                newLines.Add("\t{0}".format(line));
            return StringsAndLists.fromStringList_getText(newLines);

        }
        public static int       indexLast(this string targetString, string stringToFind)
        {
			if (targetString.notValid() || stringToFind.notValid())
				return -1;
            return targetString.LastIndexOf(stringToFind, StringComparison.Ordinal);
        }
        
        public static string    add(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }
        public static string    insertAfter(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }
        public static string    insertBefore(this string targetString, string stringToAdd)
        {
            return stringToAdd + targetString;
        }      
        public static int       toIntFromHex(this string hexValue)
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

        public static string    tempFile(this string postfixString)
        {
            return PublicDI.config.getTempFileInTempDirectory(postfixString);
        }
        public static string    o2Temp2Dir(this string tempFolderName)
        {
            return tempFolderName.o2Temp2Dir(true);
        }
        public static string    o2Temp2Dir(this string tempFolderName, bool appendRandomStringToFolderName)
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
        public static string    tempO2Dir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }
        public static string    tempDir(this string tempFolderName, bool appendRandomStringToFolderName)
        {
            return o2Temp2Dir(tempFolderName, appendRandomStringToFolderName);
        }
        public static string    tempDir(this string tempFolderName)
        {
            return o2Temp2Dir(tempFolderName);
        }


        public static Exception logStackTrace(this Exception ex)
		{
			"Error strackTrace: \n\n {0}".error(ex.StackTrace);
			return ex;
		}		
		public static string    ifEmptyUse(this string _string , string textToUse)
		{
			return (_string.empty() )
						?  textToUse
						: _string;
		}		
		public static string    upperCaseFirstLetter(this string _string)
		{
			if (_string.valid())
			{
				return _string[0].str().upper() + _string.subString(1); 
			}
			return _string;
		}										
		public static string    append(this string _string, string stringToAppend)
		{
			return _string + stringToAppend;
		}		
		public static string    insertAt(this string _string,  int index, string stringToInsert)
		{
			return _string.Insert(index,stringToInsert);
		}		
		public static string    subString(this string _string, int startPosition)
		{
			if (_string.size() < startPosition)
				return "";
			return _string.Substring(startPosition);
		}		
		public static string    subString(this string _string,int startPosition, int size)
		{
			var subString = _string.subString(startPosition);
			if (subString.size() < size)
				return subString;
			return subString.Substring(0,size);
		}		
		public static string    subString_After(this string value, string stringToFind)
		{
            if (value.notNull() && stringToFind.notNull())
            {
			    var index = value.IndexOf(stringToFind, StringComparison.Ordinal);
			    if (index > -1)			
				    return value.subString(index + stringToFind.size());
			}
			return "";
		}
        public static string    subString_After_Last(this string _string, string stringToFind)
        {
            var index = _string.LastIndexOf(stringToFind, StringComparison.Ordinal);
            if (index > 0)
            {
                return _string.subString(index + stringToFind.size());
            }
            return "";
        }

        public static string    subString_Before(this string stringToProcess, string untilString)
        {
            var lastIndex = stringToProcess.indexLast(untilString);
            return (lastIndex > 0)
                        ? stringToProcess.subString(0, lastIndex)
                        : stringToProcess;
        }

		public static string    lastChar(this string _string)
		{
			if (_string.size() > 0)
				return _string[_string.size()-1].str();
			return "";			
		}		
		public static bool      lastChar(this string _string, char lastChar)
		{
			return _string.lastChar(lastChar.str());
		}		
		public static bool      lastChar(this string _string, string lastChar)
		{
			return _string.lastChar() == lastChar;
		}		
		public static string    firstChar(this string _string)
		{
			if (_string.size() > 0)
				return _string[0].str();
			return "";			
		}		
		public static bool      firstChar(this string _string, char lastChar)
		{
			return _string.firstChar(lastChar.str());
		}		
		public static bool      firstChar(this string _string, string lastChar)
		{
			return _string.firstChar() == lastChar;
		}		
        public static string    add_5_RandomLetters(this string target)
        {
            return target.add_RandomLetters(5);
        }
		public static string    add_RandomLetters(this string target)
		{
			return target.add_RandomLetters(10);
		}		
		public static string    add_RandomLetters(this string _string, int count)
		{
			return "{0}_{1}".format(_string,count.randomLetters());
		}		
		public static int       randomNumber(this int max)
		{
			return max.random();
		}
		public static string random_Email(this string emailName)
        {
            emailName = emailName.safeFileName();
            if (emailName.notValid())
                emailName = 10.randomLetters();
            return "{0}@{1}.com".format(emailName, 10.randomLetters());
        }		
		public static string    ascii(this int _int)
		{
			try
			{				
				return (_int).str();					
			}
			catch(Exception ex)
			{
				ex.log();
				return "";
			}
		}		

		public static Guid      next(this Guid guid)
		{
			return guid.next(1);
		}		
		public static Guid      next(this Guid guid, int incrementValue)
		{			
			var guidParts = guid.str().split("-");
			var lastPartNextNumber = Int64.Parse(guidParts[4], System.Globalization.NumberStyles.AllowHexSpecifier);
			lastPartNextNumber+= incrementValue;
			var lastPartAsString = lastPartNextNumber.ToString("X12");
			var newGuidString = "{0}-{1}-{2}-{3}-{4}".format(guidParts[0],guidParts[1],guidParts[2],guidParts[3],lastPartAsString);
			return new Guid(newGuidString); 					
		}		
		public static Guid      emptyGuid(this Guid guid)
		{
			return Guid.Empty;
		}		
		public static Guid      newGuid(this string guidValue)
		{
			return Guid.NewGuid();
		}		
		public static Guid      guid(this string guidValue)
		{
			if (guidValue.inValid())
				return Guid.Empty;			
			return new Guid(guidValue);		
		}		
		public static bool      isGuid(this String guidValue)
		{
			try
			{                
				new Guid(guidValue);
				return true;
			}
			catch
			{
				return false;
			}
		}		

		public static bool      toBool(this string _string)
		{
			try
			{
				if (_string.valid())
					return bool.Parse(_string);				
			}
			catch(Exception ex)
			{
				"in toBool, failed to convert provided value ('{0}') into a boolean: {2}".format(_string, ex.Message);				
			}
			return false;
		}		
		public static double    toDouble(this string _string)
		{
			try
			{
				if (_string.valid())
					return Double.Parse(_string);				
			}
			catch(Exception ex)
			{
				"in toDouble, failed to convert provided value ('{0}') into a double: {2}".format(_string, ex.Message);				
			}
			return default(double);
		}		
		public static IPAddress toIPAddress(this string _string)
		{
			try
			{
				IPAddress ipAddress;
				IPAddress.TryParse(_string, out ipAddress);
				return ipAddress;
			}
			catch(Exception ex)
			{
				"Error in toIPAddress: {0}".error(ex.Message);
				return null;
			}
		}		
		public static byte      hexToByte(this string hexNumber)
		{
			try
			{
				return Byte.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToByte]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return default(byte);
			}
		}		
		public static byte[]    hexToBytes(this List<string> hexNumbers)
		{
			var bytes = new List<byte>();
			foreach(var hexNumber in hexNumbers)
				bytes.add(hexNumber.hexToByte());
			return bytes.ToArray();
		}		
		public static int       hexToInt(this string hexNumber)
		{
			try
			{
				return Int32.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToInt]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return -1;
			}
		}		
		public static long      hexToLong(this string hexNumber)
		{
			try
			{
				return long.Parse(hexNumber,System.Globalization.NumberStyles.HexNumber);
			}
			catch(Exception ex)
			{
				"[hexToLong]	Failed to convert {0} : {1}".error(hexNumber, ex.Message);
				return -1;
			}
		}		
		public static string    hexToAscii(this string hexNumber)
		{
			var value = hexNumber.hexToInt();
			if (value > 0)
				return value.ascii();
			return "";
		}		
		public static string    hexToAscii(this List<string> hexNumbers)
		{
			var asciiString = new StringBuilder();
			foreach(var hexNumber in hexNumbers)
				asciiString.Append(hexNumber.hexToAscii());
			return asciiString.str();
		}
    }


    public static class Long_ExtensionMethods
	{		
		public static long      toLong(this double _double)
		{
			return (long)_double;
		}		
		public static long      add(this long _long, long value)
		{
			return _long + value;
		}				
	}
	
	public static class Int_ExtensionMethods
	{		
		public static int       toInt(this double _double)
		{
			return (int)_double;
		}		
		public static int       mod( this int num1, int num2)
		{
			return num1 % num2;
		}
		public static bool      mod0( this int num1, int num2)
		{
			return num1.mod(num2) ==0;
		}		
		public static string    intToBinaryString(this int number)
		{
			return Convert.ToString(number,2);
		}			
	}	
	
	public static class UInt_ExtensionMethods
	{
		public static uint      toUInt(this string value)
		{
			return UInt32.Parse(value);
		}
	}    
}