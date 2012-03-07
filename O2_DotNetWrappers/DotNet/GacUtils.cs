using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using O2.Interfaces.DotNet;
using O2.Kernel;
using O2.DotNetWrappers.Windows;
using O2.Kernel.InterfacesBaseImpl;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.DotNet
{
    public class GacUtils
    {
        public static List<string> chachedListOfGacAssembliesNames;

        public static string getPathToGac()
        {
            return DI.PathToGac;
        }
        public static List<string> assemblyNames()
        {
            return assemblyNames(true);
        }

        public static List<string> assemblyNames(bool useCachedVersion)        
        {
            if (useCachedVersion && chachedListOfGacAssembliesNames.notNull())
                return chachedListOfGacAssembliesNames;

            chachedListOfGacAssembliesNames = (from gacAssembly in currentGacAssemblies()
											   select gacAssembly.name).toList();
			return chachedListOfGacAssembliesNames;
        }


        public static List<IGacDll> currentGacAssemblies()
        {
            var gacAssemblies = new List<IGacDll>();
            foreach (var directory in Files.getListOfAllDirectoriesFromDirectory(DI.PathToGac, true))
            {
                if (DI.PathToGac != Path.GetDirectoryName(directory) && directory.contains("NativeImages").isFalse())
                {                    
                    var name = Path.GetFileName(Path.GetDirectoryName(directory));
                    var version = Path.GetFileName(directory);
                    var fullPath = Path.Combine(directory, name + ".dll");
                    if (File.Exists(fullPath))
                        gacAssemblies.Add(new KGacDll(name, version, fullPath));
                    else
                    {
                        // handle the rare cases when the assembly is an *.exe
                        fullPath = Path.Combine(directory, name + ".exe");
                        if (File.Exists(fullPath))
                            gacAssemblies.Add(new KGacDll(name, version, fullPath));
                        //else
                        //   PublicDI.log.error("ERROR in currentGacAssemblies: could not find: {0}", fullPath);
                    }
                }
            }
            return gacAssemblies;
        }
    }
}        