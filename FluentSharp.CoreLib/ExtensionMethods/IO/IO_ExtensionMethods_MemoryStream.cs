using System.IO;
using System.Text;

namespace FluentSharp.CoreLib
{
    public static class IO_ExtensionMethods_MemoryStream
    {
        public static MemoryStream  stream(this string initialValue)
        {
            return initialValue.memoryStream();
        }
        public static MemoryStream  memoryStream(this string initialValue)
        {
            return new MemoryStream().write(initialValue);
        }
        public static MemoryStream  writeLine(this MemoryStream memoryStream)
        {
            return memoryStream.writeLine("");
        }
        public static MemoryStream  writeLine(this MemoryStream memoryStream, string text)
        {
            return memoryStream.write(text.line());
        }
        public static MemoryStream  write(this MemoryStream memoryStream)
        {
            return memoryStream.write("");
        }
        public static MemoryStream  write(this MemoryStream memoryStream, string text)
        {
            var bytes = text.asciiBytes();
            "write size: {0} - {1} : {2}".debug(bytes.size(), text.size(), text);
            memoryStream.Write(bytes, 0, bytes.size());
            return memoryStream;
        }
        public static string        ascii(this MemoryStream memoryStream)
        {
            return memoryStream.ToArray().ascii();
        }
        public static MemoryStream  stream_UFT8(this string text)
		{
			var memoryStream = text.valid()
				       ? new MemoryStream(Encoding.UTF8.GetBytes(text))
				       : new MemoryStream();
			memoryStream.Flush();
			return memoryStream;
		}
    }
    
}