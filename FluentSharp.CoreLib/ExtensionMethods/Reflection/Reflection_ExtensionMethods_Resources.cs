using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Resources
    {
        public static List<String>  embeddedResourceNames(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames().toList();
        }
        public static List<String>  embeddedAssembliesNames(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames().toList()
                                                      .where((name)=>name.ends(new [] {".dll",".exe",".dll.gz",".exe.gz"}));
        }
        public static byte[]        embeddedResource(this Assembly assembly, string name)
        {            
            if (assembly.isNull())
                return null;
            var assemblyStream = assembly.GetManifestResourceStream(name);
            var bytes = assemblyStream.isNull() 
                            ? null 
                            : new BinaryReader(assemblyStream).ReadBytes((int) assemblyStream.Length);
            return name.contains(".gz") 
                            ? bytes.gzip_Decompress() 
                            : bytes;
        }
    }

}
