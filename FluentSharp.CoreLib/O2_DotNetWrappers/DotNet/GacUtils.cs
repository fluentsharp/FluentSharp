using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.CoreLib.API
{
    public class GacUtils
    {
        static GacUtils()
        {
            PathToGac = Path.Combine(Environment.GetEnvironmentVariable("windir") ?? "", "Assembly");//\\GAC_MSIL");                        
        }
        public static string PathToGac { get; set; }

        public static List<string> chachedListOfGacAssembliesNames;

        public static string getPathToGac()
        {
            return GacUtils.PathToGac;
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
                                               select gacAssembly.Name).toList();
            return chachedListOfGacAssembliesNames;
        }


        public static List<IGacDll> currentGacAssemblies()
        {
            var gacAssemblies = new List<IGacDll>();
            foreach (var directory in Files.getListOfAllDirectoriesFromDirectory(GacUtils.PathToGac, true))
            {
                if (GacUtils.PathToGac != Path.GetDirectoryName(directory) && directory.contains("NativeImages").isFalse())
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