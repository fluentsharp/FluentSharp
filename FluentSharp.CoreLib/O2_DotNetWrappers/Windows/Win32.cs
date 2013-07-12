// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;


namespace FluentSharp.CoreLib.API
{
    public class Win32
    {
        public static string findFileOnLocalPath(string fileToFind)
        {
            var pathEnvironmentVariable = Environment.GetEnvironmentVariable("Path");
            if (pathEnvironmentVariable != null)
                foreach (var directory in pathEnvironmentVariable.Split(';'))
                {
                    var file = Path.Combine(directory, fileToFind);
                    PublicDI.log.info(file);
                    if (File.Exists(file))
                        return file;
                }
            PublicDI.log.error("in findFileOnLocalPath, could not find file {0} in environment Path directories: {1}", fileToFind, pathEnvironmentVariable);
            return "";
        }
    }
}
