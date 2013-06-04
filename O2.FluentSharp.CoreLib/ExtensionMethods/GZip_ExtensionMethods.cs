using System.Text;
using System.IO;
using System.IO.Compression;

namespace FluentSharp.ExtensionMethods
{

    public static class GZip_ExtensionMethods
    {
        public static byte[] gzip_Compress(this string _string)
        {
            var bytes = Encoding.ASCII.GetBytes(_string);
            var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                gzipStream.Write(bytes, 0, bytes.size());
            return outputStream.ToArray();
        }

        public static byte[] gzip_Decompress(this byte[] bytes)
        {
            var inputStream = new MemoryStream();
            inputStream.Write(bytes, 0, bytes.Length);
            inputStream.Position = 0;
            var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                var buffer = new byte[4096];
                int numRead;
                while ((numRead = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                    outputStream.Write(buffer, 0, numRead);
            }
            return outputStream.ToArray();
        }

        public static string gzip_Decompress_toString(this byte[] bytes)
        {
            return bytes.gzip_Decompress().ascii();
        }

        //based http://msdn.microsoft.com/en-us/library/system.io.compression.gzipstream(v=vs.90).aspx
        public static string compress(this FileInfo fi)
        {
            var targetFile = fi.FullName + ".gz";
            return fi.compress(targetFile);
        }
        public static string compress(this FileInfo fi, string targetFile)
        {

            targetFile.deleteIfExists();
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and already compressed files. 
                if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden)
                        != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file. 
                    using (FileStream outFile = File.Create(targetFile))
                    {
                        using (var compress = new GZipStream(outFile, CompressionMode.Compress))
                        {
                            // Copy the source file into the compression stream.
                            var buffer = new byte[4096];
                            int numRead;
                            while ((numRead = inFile.Read(buffer, 0, buffer.Length)) != 0)
                                compress.Write(buffer, 0, numRead);

                            "Compressed {0} from {1} to {2} bytes.".info(fi.Name, fi.Length.str(), outFile.Length.str());
                        }
                    }
                }
            }
            if (targetFile.fileExists())
                return targetFile;
            return null;
        }
        public static string decompress(this FileInfo fi)
        {
            string curFile = fi.FullName;
            string targetFile = curFile.Remove(curFile.Length - fi.Extension.Length);
            return fi.decompress(targetFile);
        }
        public static string decompress(this FileInfo fi, string targetFile)
        {
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                //Create the decompressed file. 
                using (FileStream outFile = File.Create(targetFile))
                {
                    using (var decompress = new GZipStream(inFile, CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        var buffer = new byte[4096];
                        int numRead;
                        while ((numRead = decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outFile.Write(buffer, 0, numRead);
                        }

                        "Decompressed: {0}".info(fi.Name);

                    }
                }
            }
            return targetFile;
        }

    }

}
