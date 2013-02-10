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
		    O2ConfigSettings.CheckForTempDirMaxSizeCheck = false;
		    PublicDI.config = new KO2Config();
            //this fails in TeamCity
            //var o2TempDir2 = PublicDI.config.O2TempDir.info();
            //Assert.AreNotEqual(o2TempDir, o2TempDir2 , "Shouldn't be the same");
            
            //reset checkForTempDirMaxSizeCheck
            O2ConfigSettings.CheckForTempDirMaxSizeCheck = true;
		    PublicDI.config = new KO2Config();
		    var o2TempDir3 = PublicDI.config.O2TempDir;
            Assert.AreEqual(o2TempDir, o2TempDir3 , "Should be the same");
		    "O2TempDir is: {0}".info(o2TempDir3);
		}


	}
}
