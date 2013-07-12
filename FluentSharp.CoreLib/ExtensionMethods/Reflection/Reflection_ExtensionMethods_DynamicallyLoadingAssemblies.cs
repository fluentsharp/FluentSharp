using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_DynamicallyLoadingAssemblies
    {
        public static Assembly resolveAssembly(this string nameOrPath)
        {
            return nameOrPath.resolveAssembly((nameToResolve)=> nameToResolve);
        }		
        public static Assembly resolveAssembly(this string name, string resolvedPath)
        {
            return name.resolveAssembly((nameToResolve)=> resolvedPath);
        }		
        public static Assembly resolveAssembly(this string nameOrPath, Func<string,string> resolveName)
        {
            ResolveEventHandler assemblyResolve =  
                (sender, args)=>{						
                                    var name = args.prop("Name").str();
                                    //"[AssemblyResolve] for name: {0}".debug(name);
                                    var location = resolveName(name);						
                                    if (location.valid())
                                    { 								
                                        //"[AssemblyResolve] found location: {0}".info(location);
                                        var assembly = Assembly.Load(location.fileContents_AsByteArray());
                                        if (assembly.notNull())
                                        {										
                                            //"[AssemblyResolve] loaded Assembly: {0}".info(assembly.FullName);
                                            return assembly;
                                        }                                        
                                        "[AssemblyResolve] failed to load Assembly from location: {0}".error(location);
                                    }
                                    else
                                        "[AssemblyResolve] could not find a location for assembly with name: {0}".error(name);
                                    return null;
                };
             
            Func<string,Assembly> loadAssembly = 
                (assemblyToLoad)=>{
                                      AppDomain.CurrentDomain.AssemblyResolve += assemblyResolve;
                                      var assembly = assemblyToLoad.assembly();
                                      AppDomain.CurrentDomain.AssemblyResolve -= assemblyResolve;
                                      return assembly;
                };
            return loadAssembly(nameOrPath);
        }				
        public static Assembly loadAssemblyAndAllItsDependencies(this string pathToAssemblyToLoad) 	
        {
            var referencesFolder = pathToAssemblyToLoad.directoryName();
            var referencesFiles = referencesFolder.files(true,"*.dll", "*.exe");
            
            Func<string,string> resolveAssemblyName = 
                (name)=>{   			
                            if (name.starts("System"))
                                return name;
                            if (name.isAssemblyName())
                                name = name.assembly_Name();
                            
                            var resolvedPath = referencesFiles.find_File_in_List(name, name+ ".dll", name+ ".exe");					
                            
                            if(resolvedPath.fileExists())
                            {
                                //"**** Found match:{0}".info(resolvedPath);	 
                                return resolvedPath;
                            }				 
                            
                            //"**** Couldn't match:{0}".error(resolvedPath);	
                            return null;  
                };
            
            
            var loadedAssemblies = new Dictionary<string, Assembly>();
            // ReSharper disable ImplicitlyCapturedClosure
            // ReSharper disable AccessToModifiedClosure
            Action<Assembly> loadReferencedAssemblies = (assembly) => { };
            Func<string, Assembly> loadAssembly = null;
            loadAssembly = 
                (assemblyPathOrName) => {
                                            if (loadedAssemblies.hasKey(assemblyPathOrName))
                                                return loadedAssemblies[assemblyPathOrName];
                                            var assembly = assemblyPathOrName.resolveAssembly(resolveAssemblyName);											
                                            if(assembly.notNull())
                                            {
                                                loadedAssemblies.add(assemblyPathOrName, assembly);                                                
                                                loadReferencedAssemblies(assembly); 												                                                
                                                if (assembly.Location.valid().isFalse())
                                                {
                                                    loadAssembly(assembly.FullName.assembly_Name());
                                                    loadAssembly(assembly.ManifestModule.Name != "<Unknown>"
                                                                     ? assembly.ManifestModule.Name
                                                                     : assembly.ManifestModule.ScopeName);
                                                }
                                                //loadAssembly(assembly.ManifestModule.Name);
                                                 
                                            } 
                                            return assembly;
                };
                                        
            loadReferencedAssemblies = 
                (assembly)=>{
                                var referencedAssemblies =  assembly.referencedAssemblies();								
                                foreach(var referencedAssembly in referencedAssemblies)
                                {									
                                    var assemblyName = referencedAssembly.str();																																				
                                    if (loadAssembly(assemblyName).isNull())
                                        "COULD NOT LOAD Referenced Assembly: {0}".error(assemblyName);
                                }
                };										
            // ReSharper restore ImplicitlyCapturedClosure
            // ReSharper restore AccessToModifiedClosure                                                
            var mainAssembly = loadAssembly(pathToAssemblyToLoad);
        
            "[loadAssemblyAndAllItsDependencies] there were {0} references loaded/mapped from '{1}'".info(loadedAssemblies.size(), pathToAssemblyToLoad);
            //show.info(loadedAssemblies);			
            
            return mainAssembly;
        }		
    }
}