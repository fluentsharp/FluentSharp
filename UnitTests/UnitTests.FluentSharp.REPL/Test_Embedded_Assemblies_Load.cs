using System.Reflection;
using NUnit.Framework;
using FluentSharp.CoreLib;
using O2.External.SharpDevelop.AST;
using O2.Kernel;

namespace UnitTests.FluentSharp_REPL
{
    [TestFixture]
    public class Test_Embedded_Assemblies_Load
    {
        public Test_Embedded_Assemblies_Load()
        {
            O2ConfigSettings.O2Version = "O2_UnitTests\\FluentSharp_BCL".append_O2Version();            
        }

        [Test]
        public void GetAssemblyFromResources()
        {
            var testAssembly = "O2.FluentSharp.REPL._Embedded_Dlls.QuickGraph.dll";
            var fluentSharpREPL = typeof (CSharp_FastCompiler).Assembly;
            
            var resourceNames = fluentSharpREPL.embeddedResourceNames();
            var assemblyNames = fluentSharpREPL.embeddedAssembliesNames();
            Assert.NotNull    (resourceNames);
            Assert.IsNotEmpty (resourceNames);            
            Assert.NotNull    (assemblyNames);
            Assert.IsNotEmpty (assemblyNames);
            Assert.Contains   (testAssembly,assemblyNames);

            var assemblyBytes = fluentSharpREPL.embeddedResource(testAssembly);
            //var quickGraph = "QuickGraph.dll".assembly();
            Assert.NotNull(assemblyBytes);
            var assembly = assemblyBytes.assembly();
            Assert.NotNull(assembly);
            Assert.NotNull(assembly.Location);
            assembly.Location.info();
            //assemblyNames.log_Info();            
        }
    }
}
