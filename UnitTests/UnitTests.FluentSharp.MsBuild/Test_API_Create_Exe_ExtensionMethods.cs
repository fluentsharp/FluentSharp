using System.Drawing;
using FluentSharp.CoreLib;
using FluentSharp.MsBuild;
using FluentSharp.NUnit;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp.MsBuild
{
    [TestFixture]
    public class Test_API_Create_Exe_ExtensionMethods : NUnitTests
    {
        public API_Create_Exe apiCreateExe = new API_Create_Exe();

        [Test]
        public void path_FileWithStartupCode()
        {
            var resourceName = apiCreateExe.RESOURCE_FILE_WITH_STARTUP_CODE.assert_Not_Null();
            
            //check that we can get it from the embedded resource
            var assembly      = apiCreateExe.type().assembly()             .assert_Not_Null();
            var from_Resource = assembly    .resource_GetFile(resourceName).assert_Not_Null();
            
            assembly.embeddedResource(resourceName).assert_Not_Null();
            assembly.resource_GetFile(resourceName).assert_Not_Null()
                                                   .assert_Is_Equal_To(from_Resource);
            
            
            
            var codeFileWithStartupCode = apiCreateExe.path_FileWithStartupCode();
            codeFileWithStartupCode.assert_Not_Null()
                                   .assert_Equal_To(from_Resource);

            assert_Are_Equal(codeFileWithStartupCode.file_Contents(), assembly.resourceStream(resourceName).bytes().ascii());
        }

        [Test]
        public void path_O2Logo_Icon()
        {
            var resourceName = apiCreateExe.RESOURCE_FILE_O2_LOGO.assert_Not_Null();
            apiCreateExe.type().assembly()  
                               .embeddedResource(resourceName).assert_Not_Null();

            apiCreateExe.path_O2Logo_Icon()
                        .assert_Not_Null()
                        .assert_File_Exists()
                        .assert_Not_Null(path=>path.icon())
                        .assert_Are_Equal(path=>path.icon().type(), typeof(Icon));
        }

    }
}
