using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using FluentSharp.NUnit;
using FluentSharp.O2Platform;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Git
{
    public class Temp_Clone_O2_Platform_Scripts : NUnitTests
    {           
        public API_NGit nGit        { get; set; }
        public string   repoPath    { get; set; }        
        
        public Temp_Clone_O2_Platform_Scripts() 
        {
            var o2PlatformScripts = new O2_Platform_Scripts();
            o2PlatformScripts.Clone_Or_Open_O2_Platform_Scripts_Repository();
            repoPath = o2PlatformScripts.ScriptsFolder();
            nGit = o2PlatformScripts.nGit;

            assert_Is_Not_Null(nGit);
            Assert.IsNotEmpty(repoPath.files() , "[Temp_Clone_O2_Platform_Scripts] no files in repoPath:" + repoPath);
            Assert.IsNotEmpty(repoPath.dirs());                        
            Assert.IsTrue    (repoPath.isGitRepository());                       
        }        
        

        [Test]
        public void CheckThatRepoHasExpectedFiles()
        {
            var fullPath1 = nGit.file_FullPath("README.md");
            var fullPath2 = nGit.file_FullPath(@"_DataFiles\_Images\O2_Logo.gif");
            Assert.IsTrue(fullPath1.fileExists());
            Assert.IsTrue(fullPath2.fileExists());            
        }

    }
}
    