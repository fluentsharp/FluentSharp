using NUnit.Framework;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

namespace UnitTests.FluentSharp.CoreLib
{
	[TestFixture]
	public class ConfigFolder
	{
		[Test]
		public void CheckConfigFolder()
		{
			var o2TempDir = PublicDI.config.O2TempDir;
            Assert.NotNull(o2TempDir,"TempDir was fals");
			//o2TempDir.info();
		}
	}
}
