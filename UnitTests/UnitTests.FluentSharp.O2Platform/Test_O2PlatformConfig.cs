using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.NUnit;
using FluentSharp.O2Platform;
using NUnit.Framework;

namespace UnitTests.FluentSharp.O2Platform
{    
    public class Test_O2_Platform_Config : NUnitTests
    {
        [Test]
        public void O2PlatformConfig_Ctor()
        {
            var o2PlatformConfig = O2_Platform_Config.Current;
            var appData          = o2PlatformConfig.CurrentUser_AppData;
            var mainFolder         = o2PlatformConfig.Folder_Root;
            assert_Is_Not_Null  (o2PlatformConfig);
            assert_Folder_Exists(appData);
            assert_Folder_Exists(mainFolder);
        }
    }
    
    public class Test_O2_Platform_Scripts : NUnitTests
    {
        [Test]
        public void Clone_Or_Open_O2_Platform_Scripts()
        {
            var o2PlatformScripts = new O2_Platform_Scripts();
            var scriptsFolder     = o2PlatformScripts.ScriptsFolder();

            o2PlatformScripts.SetUp();
            
            scriptsFolder.assert_Folder_Exists()
                         .assert_Is_True      (path=>path.isGitRepository());
            
            //check that expected O2.Platform.Scripts files are in there:
            var nGit      = o2PlatformScripts.nGit;
            var fullPath1 = nGit.file_FullPath("README");
            var fullPath2 = nGit.file_FullPath(@"_DataFiles\_Images\O2_Logo.gif");

            Assert.IsTrue(fullPath1.fileExists());
            Assert.IsTrue(fullPath2.fileExists());   
        }

    }
}
