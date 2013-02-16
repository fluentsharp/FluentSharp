using NUnit.Framework;
using O2.Kernel;

namespace UnitTests.FluentSharp_REPL
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
