using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    public class Temp_Clone_O2_Platform_Scripts
    {   
        public string   GIT_HUB_O2_PLATFORM_SCRIPTS { get; set; }

        public API_NGit nGit        { get; set; }
        public string   repoPath    { get; set; }        
        
        public Temp_Clone_O2_Platform_Scripts() 
        {
            repoPath = PublicDI.config.O2TempDir.pathCombine("Temp_Clone_O2_Platform_Scripts");
            GIT_HUB_O2_PLATFORM_SCRIPTS = "https://github.com/o2platform/O2.Platform.Scripts.git";
            nGit = new API_NGit().open_or_Clone(GIT_HUB_O2_PLATFORM_SCRIPTS, repoPath);
                        
            Assert.IsNotEmpty(repoPath.files());
            Assert.IsNotEmpty(repoPath.dirs());                        
            Assert.IsTrue    (repoPath.isGitRepository());
            /*if (Web.Online)
            {
                var pullResult = nGit.pull();
                Assert.IsTrue    (pullResult);
            }*/
            
        }        
        

        [Test]
        public void CheckThatRepoHasExpectedFiles()
        {
            var fullPath1 = nGit.file_FullPath("README");
            var fullPath2 = nGit.file_FullPath(@"_DataFiles\_Images\O2_Logo.gif");
            Assert.IsTrue(fullPath1.fileExists());
            Assert.IsTrue(fullPath2.fileExists());            
        }

    }
}
    