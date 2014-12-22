using FluentSharp.CoreLib;
using FluentSharp.CoreLib.APIs;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.APIs
{
    [TestFixture]
    public class Test_API_NuGet
    {
        [Test] public void API_Nuget_Ctor()
        {
            var apiNuGet = new API_NuGet(); 

            apiNuGet.NuGet_Exe             .assert_Not_Null();
            apiNuGet.NuGet_Exe_Download_Uri.assert_Not_Null();
        }

        [Test] public void setup()
        {
            
            var apiNuGet = new API_NuGet(); 
            if(apiNuGet.NuGet_Exe.fileExists())
                apiNuGet.NuGet_Exe.assert_File_Deleted();

            apiNuGet.setup()  .assert_Not_Null();
            apiNuGet.NuGet_Exe.assert_File_Exists();

            apiNuGet.NuGet_Exe.assert_File_Deleted();
            apiNuGet.NuGet_Exe_Download_Uri = null;

            apiNuGet.setup() .assert_Is_Null();
            apiNuGet.NuGet_Exe.assert_File_Not_Exists();
        }
        [Test] public void help()
        {
            new API_NuGet().help().assert_Not_Null()
                                  .assert_Contains("usage: NuGet <command> [args] [options]");            
        }

        [Test] public void list()
        {
            var nuGet = new API_NuGet();

            nuGet.list("FluentSharp").assert_Not_Empty()
                                     .assert_Size_Is(28)
                                     .assert_Equal_To(nuGet.packages_FluentSharp());
        }
        [Test] public void install()
        {
            var nuGet = new API_NuGet();
            nuGet.install("FluentSharp.CoreLib").assert_Not_Null()         .assert_Folder_Exists()
                                                .pathCombine(@"lib\net35\").assert_Folder_Exists()
                                                .files().first().fileName().assert_Is_Equal_To  ("FluentSharp.CoreLib.dll");
        }
        [Test] public void extract_Installed_PackageName()
        {
            var nuGet = new API_NuGet();

            var message1 = @"Installing 'FluentSharp.CoreLib 5.5.172'.
Successfully installed 'FluentSharp.CoreLib 5.5.172'.";
            var message2 = "'FluentSharp.CoreLib 5.5.172' already installed.";

            var expectedName = "FluentSharp.CoreLib.5.5.172";

            nuGet.extract_Installed_PackageName(message1).assert_Is(expectedName);
            nuGet.extract_Installed_PackageName(message2).assert_Is(expectedName);
        }
        [Test] public void path_Package()
        {
            var packageName = "FluentSharp.CoreLib";
            var nuGet = new API_NuGet();
            nuGet.install(packageName);
            nuGet.path_Package(packageName).assert_Not_Null()
                                           .assert_Contains(packageName)
                                           .assert_Folder_Exists();
        }
        [Test] public void has_Package()
        {
            var packageName = "FluentSharp.CoreLib";
            var nuGet = new API_NuGet();
            nuGet.install(packageName);
            nuGet.has_Package(packageName).assert_True();
            
        }

    }
}
