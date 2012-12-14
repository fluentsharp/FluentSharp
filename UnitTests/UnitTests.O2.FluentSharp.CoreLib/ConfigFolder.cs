using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;

namespace UnitTests.O2.FluentSharp.CoreLib
{
	[TestClass]
	public class ConfigFolder
	{
		[TestMethod]
		public void CheckConfigFolder()
		{
			var o2TempDir = PublicDI.config.O2TempDir;
			o2TempDir.info();
		}
	}
}
