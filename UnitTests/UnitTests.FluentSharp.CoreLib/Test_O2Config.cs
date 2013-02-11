using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Kernel.InterfacesBaseImpl;

namespace UnitTests.FluentSharp_CoreLib
{
	[TestFixture]
	public class Test_O2Config
	{
		[Test]
		public void CheckConfigFolder()
		{
		    var o2TempDir = PublicDI.config.O2TempDir;
            Assert.NotNull(o2TempDir,"TempDir was false");			

            //test checkForTempDirMaxSizeCheck  option
		    O2ConfigSettings.CheckForTempDirMaxSizeCheck = true;
		    PublicDI.config = new KO2Config();          
            var o2TempDir2 = PublicDI.config.O2TempDir.info();            

            //reset checkForTempDirMaxSizeCheck
            O2ConfigSettings.CheckForTempDirMaxSizeCheck = false;
		    PublicDI.config = new KO2Config();
		    var o2TempDir3 = PublicDI.config.O2TempDir;

            Assert.AreNotEqual(o2TempDir2, o2TempDir3 , "Should be different");

		    "O2TempDir is: {0}".info(o2TempDir3);
		}


	}
}
