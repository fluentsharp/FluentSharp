using System;
using NUnit.Framework;
using FluentSharp.NUnit;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace UnitTests.FluentSharp.CoreLib
{
	[TestFixture]
	public class Test_clr
	{
		[Test]
		public void mono()
		{
			var inMono = Type.GetType ("Mono.Runtime").isNotNull();
			clr.mono().assert_Is(inMono);
		}
	}
}

