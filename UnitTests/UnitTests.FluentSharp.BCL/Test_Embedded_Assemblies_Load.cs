using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Platform.BCL.O2_Views_ASCX;

namespace UnitTests.FluentSharp_BCL
{
    [TestFixture]
    public class Test_Embedded_Assemblies_Load
    {
        public Test_Embedded_Assemblies_Load()
        {
            O2ConfigSettings.O2Version = "O2_UnitTests\\FluentSharp_BCL".append_O2Version();
        }

        [Test]
        public void GetAssembliesFromResources()
        {        
            var fluentSharpBcl = typeof (FormImages).Assembly;
            Assert.IsNotNull(fluentSharpBcl);

            var embeddedDlls = fluentSharpBcl.GetManifestResourceNames();
            foreach (var embeddedDll in embeddedDlls)
                embeddedDll.str().info();
        }
    }
}
