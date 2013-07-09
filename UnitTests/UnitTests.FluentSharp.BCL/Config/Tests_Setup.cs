using FluentSharp.CoreLib.API;
using NUnit.Framework;

namespace UnitTests.FluentSharp_BCL
{
    [SetUpFixture]
    public class Tests_Setup
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            O2ConfigSettings.CheckForTempDirMaxSizeCheck = false;
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
