using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class Reflection_ExtensionMethods_Assembly
    { 
        public static Assembly                  assembly                        (this string assemblyName)                  
        {
            return PublicDI.reflection.getAssembly(assemblyName);
        }
        public static Assembly                  assembly                        (this byte[] bytes)                         
        {
            try
            {
                return Assembly.Load(bytes);
            }
            catch (Exception ex)
            {
                ex.log("Failed to load assembly from provided bytes");
                return null;
            }            
        }
        public static Assembly                  assembly                        (this AssemblyName assemblyName)            
        {
            return assemblyName.str().assembly();
        }		        
        public static Assembly                  assembly                        (this Type type)
        {
            if (type.isNull()) 
                return null;
            return type.Assembly;
        }
        public static AssemblyName              assembly_AssemblyName           (this Type type)                            
        {
            if (type.notNull())
                try
                {
                    return type.Assembly.GetName();
                }
                catch (Exception ex)
                {
                    ex.log("[assembly_AssemblyName]");
                }
            return null;
        }
        public static AssemblyName              assembly_AssemblyName           (this string value)                         
        {
            if (value.notNull())
                try
                {
                    return new AssemblyName(value);
                }
                catch(Exception ex)
                {
                    ex.log("[assembly_AssemblyName]");                    
                }
            return null;
        }
        public static string                    assembly_Location               (this string assemblyName)                  
        {
            return assemblyName.assembly().location();
        }		
        public static string                    assembly_Location               (this Type type)                            
        {
            return type.isNull() ? null : type.Assembly.Location;
        }
        public static string                    assembly_Name                   (this string value)                         
        {

            return value.assembly_AssemblyName().name();
        }
        public static string                    assembly_Name                   (this Type type)                            
        {

            return type.assembly_AssemblyName().name();
        }
        public static string                    append_CurrentAssemblyVersion   (this string aString)                       
        {
            return Assembly.GetCallingAssembly().version();            
        }
        public static string                    append_O2Version                (this string aString)                       
        {
            return Assembly.GetExecutingAssembly().version();
        }        
        public static PortableExecutableKinds	assembly_PortableExecutableKind (this string assemblyLocation)              
        {
            try
            {
                return Assembly.ReflectionOnlyLoadFrom(assemblyLocation).portableExecutableKind();
            }
            catch(Exception ex)
            {
                ex.log("[assembly_PortableExecutableKind]");                
            }
            return PortableExecutableKinds.NotAPortableExecutableImage;
        }        
        public static string                    fullName                        (this Assembly assembly)                    
        {
            if (assembly.notNull())
                return assembly.GetName().str();
            return null;
        }
        public static string				    imageRuntimeVersion             (this Assembly assembly)                    
        {
            if (assembly.notNull())
                return assembly.ImageRuntimeVersion;
            return null;
        }        
        public static bool                      isAssemblyName                  (this string _string)                       
        {
            return _string.contains("PublicKeyToken") &&   // ensure these exists since .assemblyName() will work for simple filenames
                   _string.assembly_Name().notNull();
        }		        
        public static bool                      isDynamic                       (this Assembly assembly)                    
        {
            return assembly.prop<bool>("IsDynamic");
        }
        
        public static string                    location                        (this Assembly assembly)                    
        {
            if (assembly.notNull())
            {
                if (assembly.Location.notValid())                
                    return AssemblyResolver.saveEmbeddedAssemblyToDisk(assembly.GetName());
                return assembly.Location;
            }
            return null;
        }
        public static List<string>			    locations                       (this List<AssemblyName> assemblyNames)     
        {

            var locations = new List<string>();
            try
            {
                foreach (var assemblyName in assemblyNames)
                {
                    var location = assemblyName.assembly().Location;
                    locations.add(location);
                }
            }
            catch (Exception ex)
            {
                "[Reflection] locations, could not resolve {0}".error(ex.Message);
            }
            return locations;
        }
        
        public static string                    name                            (this Assembly assembly)                    
        {
            if (assembly.notNull())
                return assembly.GetName().Name;
            return null;
        }
        public static string                    name                            (this AssemblyName assemblyName)            
        {
            if(assemblyName.notNull())
                return assemblyName.Name;
            return null;
        }					        
        public static List<string>              names                           (this List<AssemblyName> assemblyNames)     
        {
            return (from assemblyName in assemblyNames
                    select assemblyName.name()).toList();
        }        
        public static List<Assembly>            notDynamic                      (this List<Assembly> assemblies)            
        {
            return (from assembly in assemblies                    
                    where assembly.isDynamic().isFalse()
                    select assembly).toList();
        }
        
        public static List<AssemblyName>        referencedAssemblies            (this AssemblyName assemblyName)            
        {
            return assemblyName.assembly().referencedAssemblies();
        }
        public static List<AssemblyName>        referencedAssemblies            (this Assembly assembly, bool recursiveSearch)                          
        {
            return assembly.referencedAssemblies(recursiveSearch, true);
        }
        public static List<AssemblyName>        referencedAssemblies            (this Assembly assembly, bool recursiveSearch, bool removeGacEntries)   
        {
            var mappedReferences = new List<string>();
            var resolvedAssemblies = new List<AssemblyName>();

            Action<List<AssemblyName>> resolve = null;

            resolve = (assemblyNames) =>
                {
                    if (removeGacEntries)
                        assemblyNames = assemblyNames.removeGacAssemblies();
                    if (assemblyNames.isNull())
                        return;
                    foreach (var assemblyName in assemblyNames)
                    {
                        if (mappedReferences.contains(assemblyName.str()).isFalse())
                        {
                            mappedReferences.add(assemblyName.str());
                            resolvedAssemblies.add(assemblyName);
                            resolve(assemblyName.referencedAssemblies());
                        }
                    }
                };

            resolve(assembly.referencedAssemblies());

            "there where {0} NonGac  assemblies resolved for {1}".debug(resolvedAssemblies.size(), assembly.Location);
            return resolvedAssemblies;
        }
        public static List<AssemblyName>        removeGacAssemblies             (this List<AssemblyName> assemblyNames)                                 
        {
            var systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            return (from assemblyName in assemblyNames
                    let assembly = assemblyName.assembly()
                    where assembly.notNull() && assembly.Location.starts(systemRoot).isFalse()
                    select assemblyName).toList();
        }        
                
        public static PortableExecutableKinds	portableExecutableKind          (this Assembly assembly)                    
        {
            PortableExecutableKinds peKind;
            ImageFileMachine imageFileMachine;
            assembly.ManifestModule.GetPEKind(out peKind, out imageFileMachine);
            return peKind;
        }
        
        public static List<Assembly>            removeAssembliesSignedByMsft    (this IEnumerable<Assembly> assemblies) 
        {
            return (from assembly in assemblies
                    where assembly.isDynamic().isFalse()
                    where assembly.FullName.contains("PublicKeyToken=b03f5f7f11d50a3a",         //need to identify other public keys used by Microsoft
                                                     "PublicKeyToken=b77a5c561934e089", 
                                                     "PublicKeyToken=31bf3856ad364e35").isFalse()
                    select assembly).toList();
        }                
        public static List<AssemblyName>        referencedAssemblies            (this Assembly assembly)                
        {
            return assembly.GetReferencedAssemblies().toList();
        }		

        public static string					value                           (this PortableExecutableKinds peKind)       
        {
            switch (peKind)
            {
                case PortableExecutableKinds.ILOnly:
                    return "AnyCPU";
                case PortableExecutableKinds.Required32Bit:
                    return "x86";
                case PortableExecutableKinds.PE32Plus:
                    return "x64";
                case PortableExecutableKinds.Unmanaged32Bit:
                    return "Unmanaged32Bit";
                case PortableExecutableKinds.NotAPortableExecutableImage:
                    return "NotAPortableExecutableImage";
                default:
                    return peKind.str();
                    //throw new ArgumentOutOfRangeException();
            }
        }
        public static string                    version                         (this Assembly assembly)                    
        {
            if (assembly.notNull())
                return assembly.GetName().Version.ToString();
            return "";
        }

        public static List<Assembly>            with_Valid_Location             (this List<Assembly> assemblies)            
        {
            return assemblies.notDynamic().where((assembly) => assembly.Location.valid()).toList();
        }
    }
}