using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.NUnit;
using FluentSharp.O2Platform;
using NUnit.Framework;

namespace UnitTests.FluentSharp.O2Platform
{
    public class Test_O2_Platform_Scripts : NUnitTests
    {
        [Test]
        public void O2_Platform_Scripts_Ctor()
        {
            // the main  Setup will be called by the  Set_Test_Environment.SetupFixture_SetUp() method 
            // which should run before this one (the SetUp called bellow is only used to configure o2PlatformScripts.nGit
            
            var o2PlatformScripts = new O2_Platform_Scripts();
            var scriptsFolder     = o2PlatformScripts.ScriptsFolder();
            scriptsFolder.assert_Folder_Exists()
                         .assert_Is_True      (path=>path.isGitRepository());
            
            assert_True(o2PlatformScripts.SetUp());

            //check that expected O2.Platform.Scripts files are in there:
            var nGit      = o2PlatformScripts.nGit;            
            var fullPath1 = nGit.file_FullPath("README");
            var fullPath2 = nGit.file_FullPath(@"_DataFiles\_Images\O2_Logo.gif");
            
            assert_Not_Null   (nGit);
            assert_File_Exists(fullPath1);
            assert_File_Exists(fullPath2);    
        }

    }
}