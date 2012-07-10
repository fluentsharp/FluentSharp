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
            if (name.contains(".resources"))
                return null;

            return loadFromDiskOrResource(name); 
		}
        public static Assembly loadFromDiskOrResource(string name)
        {
            //"[AssemblyResolve] for name: {0}".debug(name);
            //first resolve by name/location
            var assembly = loadFromDisk(name);
            if (assembly.notNull())
                return assembly;

            //then by resources
            assembly = loadFromEmbededResources(name);

            return assembly;
        }
		public static Assembly loadFromDisk(string name)
		{
            //"[AssemblyResolve] loadFromDisk : {0}".info(name);
            if (name.valid() && CachedMappedAssemblies.hasKey(name))
                return CachedMappedAssemblies[name];
			
			var location = NameResolver(name);
			if (location.valid() && location.fileExists())
			{
				if (CachedMappedAssemblies.hasKey(location))
					return CachedMappedAssemblies[location];
				//"[AssemblyResolve] found location: {0}".info(location);
				Assembly assembly = null;
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
            
            var nameToFind = (name.isAssemblyName())
                    ? name.assemblyName().Name
                    : name.lower();

            foreach (var currentAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(currentAssembly.name().starts("O2"))
                    foreach (var resourceName in currentAssembly.GetManifestResourceNames())
                    {
                        if (resourceName.lower().contains(nameToFind.lower()))
                        {
                            "Found resource for {0} at {1} in {2}".info(name, resourceName, currentAssembly.name());
                            var assemblyStream = currentAssembly.GetManifestResourceStream(resourceName);
                            byte[] data = new BinaryReader(assemblyStream).ReadBytes((int)assemblyStream.Length);
                            var saveAssemblyTo = PublicDI.config.EmbeddedAssemblies.createDir().pathCombine(nameToFind);
                            if ((saveAssemblyTo.extension(".dll") || saveAssemblyTo.extension(".exe")).isFalse())
                                saveAssemblyTo+=".dll";
                            O2.DotNetWrappers.Windows.Files.WriteFileContent(saveAssemblyTo, data);
                            return loadFromDisk(saveAssemblyTo);
                        }
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
