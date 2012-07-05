using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.H2Scripts;

namespace O2.DotNetWrappers.ExtensionMethods
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
                return H2.load(file).SourceCode;
            }
            return null;
        }	
    }
}
