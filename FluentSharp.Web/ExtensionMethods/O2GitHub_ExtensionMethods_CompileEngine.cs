using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
    public static class O2GitHub_ExtensionMethods_CompileEngine
    {
        /// <summary>
        /// Downloads an assembly from O2's https://github.com/o2platform/O2_Platform_ReferencedAssemblies repository
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly  download_Assembly_From_O2_GitHub(this string assemblyName)
        {
            return assemblyName.download_Assembly_From_O2_GitHub(false);
        }
        public static Assembly  download_Assembly_From_O2_GitHub(this string assemblyName, bool ignoreSslErrors)
        {
            if (assemblyName.assembly().notNull())
                "in download_Assembly_From_O2_GitHub, the requests assembly already exists".error(assemblyName);
            else
            {
                if (ignoreSslErrors)
                    Web.Https.ignoreServerSslErrors();

                //add option to ignore cache
                
                new O2GitHub().tryToFetchAssemblyFromO2GitHub(assemblyName,false);
            }
            return assemblyName.assembly();
        }
        public static void populateCachedListOfGacAssemblies()
        {
            if (O2GitHub.AssembliesCheckedIfExists.size() < 50)
            {
                var gacAssemblies = GacUtils.assemblyNames();   
                if (gacAssemblies.contains("Microsoft.mshtml"))     // have to hard-code this one since there are cases where this is in the GAC but the load fails
                    gacAssemblies.Remove("Microsoft.mshtml");
                O2GitHub.AssembliesCheckedIfExists.add_OnlyNewItems(gacAssemblies);
            }
        }
    }
}