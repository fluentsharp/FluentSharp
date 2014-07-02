using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.NUnit;
using FluentSharp.O2Platform;
using FluentSharp.Web35;
using NUnit.Framework;

namespace UnitTests.FluentSharp.O2Platform
{
    
    [SetUpFixture]
    public class Set_Test_Environment
    {
        [SetUp]
        public void SetupFixture_SetUp()
        {
            
            var o2PlatformScripts = new O2_Platform_Scripts();
            var scriptsFolder     = o2PlatformScripts.ScriptsFolder();
            
            if("".offline())                    // we can't do the sync when offline
                return;

            Assert.IsTrue(o2PlatformScripts.SetUp());  // call SetUp which will trigger the git clone (if needed)
            
            scriptsFolder.assert_Folder_Exists()
                         .assert_Is_True      (path=>path.isGitRepository());
            
            //check that expected O2.Platform.Scripts files are in there:
            var nGit      = o2PlatformScripts.nGit;
            var fullPath1 = nGit.file_FullPath("README");
            var fullPath2 = nGit.file_FullPath(@"_DataFiles\_Images\O2_Logo.gif");
            
            Assert.IsTrue(nGit.pull());
            Assert.IsTrue(fullPath1.fileExists());
            Assert.IsTrue(fullPath2.fileExists());
        }
    }
}
