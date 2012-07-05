using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.DotNet
{
	public class AssemblyResolver
	{
		public static Func<string, string> NameResolver						{ get; set; }
		public static Dictionary<string, Assembly> CachedMappedAssemblies	{ get; set; }

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
			return loadFromDisk(name);
		}

		public static Assembly loadFromDisk(string name)
		{
            if (name.valid() && CachedMappedAssemblies.hasKey(name))
                return CachedMappedAssemblies[name];
		/*	if (name.contains("SharpDevelop"))
			{
			
			}*/
			//"[AssemblyResolve] for name: {0}".debug(name);
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
	}
}
