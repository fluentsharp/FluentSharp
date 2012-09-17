using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using O2.DotNetWrappers.ExtensionMethods;
using System.IO;
using O2.Kernel;

namespace O2.DotNetWrappers.DotNet
{
    public class AssemblyResolver
    {
        public static Func<string, string> NameResolver						{ get; set; }
        public static Dictionary<string, Assembly> CachedMappedAssemblies	{ get; set; }
        public static bool  Initialized                                     { get; set; }

        static AssemblyResolver()
        {
            enable_AssemblyResolve();			
            NameResolver = resolve_using_CompilationReferencePath;
            CachedMappedAssemblies = new Dictionary<string, Assembly>();
        }

        public AssemblyResolver()
        {						
            
        }

        public static string resolve_using_CompilationReferencePath(string file)
        {
            return CompileEngine.resolveCompilationReferencePath(file);
        }

        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.prop("Name").str();
            if (name.contains(".resources",".XmlSerializers"))
                return null;

            return loadFromDiskOrResource(name); 
        }
        public static Assembly loadFromDiskOrResource(string name)
        {
            //first try by name/location
            var assembly = loadFromDisk(name);
            if (assembly.notNull())
                return assembly;

            //then see if we have it on the embeded resources
            assembly = loadFromEmbededResources(name);
            

            return assembly;
        }
        public static Assembly loadFromDisk(string name)
        {
            //"[AssemblyResolve] loadFromDisk : {0}".info(name);
            if (name.valid() && CachedMappedAssemblies.hasKey(name))
                return CachedMappedAssemblies[name];
            
            Assembly assembly = null;
            /*if( name.isAssemblyName())
            {
                var assemblyName = name.assemblyName();
                if(CachedMappedAssemblies.hasKey(assemblyName.Name))
                {
                    assembly = CachedMappedAssemblies[assemblyName.Name];
                    "[Assembly Resolved] A different version of an assembly was requested: {0}".debug(name);
                    "[Assembly Resolved] Returned current loaded version: {0} at {1}".debug(assembly.str(), assembly.location());
                    return assembly;
                }
            }*/

            var location = NameResolver(name);
            if (location.valid() && location.fileExists())
            {
                if (CachedMappedAssemblies.hasKey(location))
                    return CachedMappedAssemblies[location];
                //"[AssemblyResolve] found location: {0}".info(location);
                
                assembly = Assembly.LoadFrom(location);
                if (assembly.isNull())
                    assembly = Assembly.Load(location.fileContents_AsByteArray());
                if (assembly.notNull())
                {
                    //"[AssemblyResolve] loaded Assembly: {0}".info(assembly.FullName);
                    CachedMappedAssemblies.add(location, assembly);
                    return assembly;
                }
                else
                    "[AssemblyResolve] failed to load Assembly from location: {0}".error(location);
            }
            //else
            //	"[AssemblyResolve] could not find a location for assembly with name: {0}".error(name);
            return null;
        }

        public static Assembly loadFromEmbededResources(string name)
        {
            if (name.contains(".XmlSerializers"))           //deal with the weird auto load of assemblies called .XmlSerializers by the .NET Framework
                return null;

            var nameToFind = (name.isAssemblyName())
                    ? name.assemblyName().Name
                    : name;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().removeAssembliesSignedByMicrosoft();
            //first see if the one we're trying to find is already loaded in memory
            foreach (var assembly in assemblies)
                if (assembly.FullName == name)
                    return assembly;
             
            foreach (var currentAssembly in assemblies)      
            {
                try
                {
                   // var isDynamic = currentAssembly.prop("IsDynamic");
                    //if (isDynamic.str() == "True")
                    //    continue;                    
                        // if(currentAssembly.name().starts("O2"))                               // it shouldn't have a big performance hit to look in all assemblies
                    foreach (var resourceName in currentAssembly.GetManifestResourceNames())
                    {
                        if (resourceName.lower().contains(nameToFind.lower()) &&
                            (resourceName.extension(".dll") || resourceName.extension(".exe")))
                        {
                            "Found resource for {0} at {1} in {2}".info(name, resourceName, currentAssembly.name());
                            var assemblyStream = currentAssembly.GetManifestResourceStream(resourceName);
                            byte[] data = new BinaryReader(assemblyStream).ReadBytes((int)assemblyStream.Length);
                            if (resourceName.contains(".gz"))
                                data = data.gzip_Decompress();
                            var saveAssemblyTo = PublicDI.config.EmbeddedAssemblies.createDir().pathCombine(name);//nameToFind);
                           // if ((saveAssemblyTo.extension(".dll") || saveAssemblyTo.extension(".exe")).isFalse())
                           //     saveAssemblyTo += ".dll";
                            if (saveAssemblyTo.fileExists())
                                "Resource file already existed, so skipping it: {0}".info(saveAssemblyTo);
                            else
                                O2.DotNetWrappers.Windows.Files.WriteFileContent(saveAssemblyTo, data);
                            return loadFromDisk(saveAssemblyTo);
                        }
                    }
                }
                catch (Exception ex)
                { 
                  ex.log("[loadFromEmbededResources] while looking at assembly: {0}".format( currentAssembly.FullName));
                }
            }
            return null;            
        }
        
        public static void enable_AssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);			
        }

        public static void disable_AssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(AssemblyResolve);			
        }


        public static Assembly loadAssembly(string assemblyToLoad)
        {			
            return assemblyToLoad.assembly();			
        }

        //will setup the assembly resolver
        public static void Init()
        {
            Initialized = true;            
        }
    }
}
