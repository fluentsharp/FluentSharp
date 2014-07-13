using System;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class Char_ExtensionMethods
    {
        public static byte     @byte(this char @char)
        {
            return (byte) @char;
        }        
        public static byte[]    bytes(this char[] chars)
        {
            return Encoding.Default.GetBytes(chars);            
        }        
        public static string    ascii(this char[] chars)
        {
            return Encoding.ASCII.GetString(chars.ascii_Bytes());            
        }
        public static byte[]    ascii_Bytes(this char[] chars)
        {
            return Encoding.ASCII.GetBytes(chars);            
        }
        public static string    repeat(this char charToRepeat, int count)
        {
            return count > 0 ? new String(charToRepeat, count) : "";
        }
        public static char[]    chars_Ascii(this string value)
        {
            return Encoding.ASCII.GetChars(value.bytes_Ascii());
        }
    }
}