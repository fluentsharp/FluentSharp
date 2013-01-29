using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
//using O2.Kernel;
//using O2.DotNetWrappers.ExtensionMethods;

namespace O2.FluentSharp.REPL
{
    //this is just to make that O2.FluentSharp.CoreLib.dll exists in the current folder(if not create it)
    //there must be no dependencies on an O2 dll here
    public class ResolveO2CoreLib
    {
    
        public static void resolve()
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load("FluentSharp.CoreLib");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            if (assembly == null)
                assembly = getAssemblyfromResources("FluentSharp.CoreLib.dll");
          //  assembly = Assembly.Load("O2_FluentSharp_CoreLib");
        }
        

        public static Assembly getAssemblyfromResources(string nameToFind)
        {
            try
            {
                var targetAssembly = Assembly.GetExecutingAssembly();
                var test = targetAssembly.GetManifestResourceStream(nameToFind);
                var nameToFind_Lower = nameToFind.ToLower();
                foreach (var resourceName in targetAssembly.GetManifestResourceNames())
                {
                    if (resourceName.ToLower().Contains(nameToFind_Lower))
                    {
                        var name = resourceName;
                        var assemblyStream = targetAssembly.GetManifestResourceStream(resourceName);
                        byte[] data = new BinaryReader(assemblyStream).ReadBytes((int)assemblyStream.Length);

                        var targetDir = Path.GetDirectoryName(targetAssembly.Location);
                        var targetFile = Path.Combine(targetDir, nameToFind);
                        using (FileStream fs = File.Create(targetFile))
                        {
                            fs.Write(data, 0, data.Length);
                            fs.Close();
                        }
                        return Assembly.LoadFrom(targetFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
