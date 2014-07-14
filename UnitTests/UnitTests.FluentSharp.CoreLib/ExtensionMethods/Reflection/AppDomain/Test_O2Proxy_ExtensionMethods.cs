using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods.Reflection.AppDomain
{
    [TestFixture]
    public class Test_O2Proxy_ExtensionMethods
    {
        [Test]
        public void o2Proxy()
        {
            var appDomain = "Temp_AppDomain".appDomain_New().appDomain();
                
            if (appDomain.rootFolder().pathCombine("FluentSharp.CoreLib.dll").file_Not_Exists())
                appDomain.isAssemblyLoaded("FluentSharp.CoreLib").assert_False(); 

            var o2Proxy = appDomain.o2Proxy();
            
            o2Proxy.assert_Not_Null();

            appDomain.isAssemblyLoaded("FluentSharp.CoreLib").assert_True();            
        }
        [Test] public void assemblies()
        {
            var appDomain = "Temp_AppDomain".add_5_RandomLetters().appDomain_New().appDomain();           

            appDomain.o2Proxy().assert_Not_Null()
                               .assemblies().assert_Not_Empty()
                                            .assert_Contains("FluentSharp.CoreLib")
                                            .assert_Contains("mscorlib");
            
            appDomain.isAssemblyLoaded("FluentSharp.CoreLib").assert_True();            
        }
    }
}
