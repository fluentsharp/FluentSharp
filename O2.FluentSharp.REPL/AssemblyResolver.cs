using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using O2.Kernel;

namespace O2.FluentSharp.REPL
{
    public class AssemblyResolver
    {
        public static bool saveEmbededLibrariesToDisk = true;        

        public static void setAssemblyResolver()
        {
            var startAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            //saveEmbededLibrariesToFolder = Path.Combine(saveEmbededLibrariesToFolder, "_Files For " + startAssemblyName);
            //saveEmbededLibrariesToFolder = Path.GetFullPath(saveEmbededLibrariesToFolder);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);
        }

        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var nameToFind = args.Name;
            if (nameToFind.IndexOf(",") > -1)
                nameToFind = nameToFind.Substring(0, nameToFind.IndexOf(","));
            return getAssemblyfromResources(nameToFind, saveEmbededLibrariesToDisk);
        }

        public static Assembly getAssemblyfromResources(string nameToFind, bool saveToDisk)
        {
            nameToFind = nameToFind.ToLower();
            var targetAssembly = Assembly.GetExecutingAssembly();
            //var nameToFind = "O2_FluentSharp_CoreLib.dll";
            foreach (var resourceName in targetAssembly.GetManifestResourceNames())
                if (resourceName.ToLower().Contains(nameToFind) &&
                    (resourceName.ToLower().Contains(nameToFind + ".dll") || resourceName.ToLower().Contains(nameToFind + ".exe")))
                {
                    var assemblyStream = targetAssembly.GetManifestResourceStream(resourceName);
                    byte[] data = new BinaryReader(assemblyStream).ReadBytes((int)assemblyStream.Length);
                    if (saveToDisk)
                    {
                        //var targetDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                        //targetDir = Path.Combine(targetDir, saveEmbededLibrariesToFolder);
                        var targetDir = PublicDI.config.EmbededLibrariesFolder;
                        if (Directory.Exists(targetDir) == false)
                            Directory.CreateDirectory(targetDir);
                        var targetFile = Path.Combine(targetDir, nameToFind + (resourceName.Contains(".dll") ? ".dll" : ".exe"));
                        if (File.Exists(targetFile) == false)
                        {
                            using (FileStream fs = File.Create(targetFile))
                            {
                                fs.Write(data, 0, data.Length);
                                fs.Close();
                            }
                        }
                        return Assembly.LoadFrom(targetFile);
                    }
                    return Assembly.Load(data);
                }
            return null;
        }
    }
}
