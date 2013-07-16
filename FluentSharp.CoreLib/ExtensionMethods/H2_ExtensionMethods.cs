using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class H2_ExtensionMethods
    {
        public static string scriptSourceCode(this string file)
        {
            if (file.extension(".h2"))
                return file.h2_SourceCode();
            return file.fileContents();
        }
        public static string h2_SourceCode(this string file)
        {
            if (file.extension(".h2"))
            {
                //"return source code of H2 file".info();
                if (file.fileExists())
                    return H2.load(file).SourceCode.fix_CRLF();
            }
            return null;
        }

        public static H2 h2(this string code)
        {
            var h2 = new H2();
            h2.SourceCode = code;
            return h2;
        }

        public static string h2_File(this string code, string targetFile = null)
        {
            if (targetFile.valid().isFalse())
                targetFile = ".h2".tempFile();
            if (code.h2().saveAs(targetFile).isFalse())
                "Something when wrong saving this code as an h2 script: {0}".error(code);
            return targetFile;
        }			
    }
}
