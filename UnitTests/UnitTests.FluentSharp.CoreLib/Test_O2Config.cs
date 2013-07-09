using FluentSharp.CoreLib.API;
using NUnit.Framework;
using FluentSharp.CoreLib;

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

            if (o2TempDir2.size() > 120 || o2TempDir3.size() > 120)
                Assert.AreNotEqual(o2TempDir2, o2TempDir3 , "Should be different");
            else
                Assert.AreEqual   (o2TempDir2, o2TempDir3 , "Should be the same");
		    "O2TempDir is: {0}".info(o2TempDir3);
		}


	}
}
