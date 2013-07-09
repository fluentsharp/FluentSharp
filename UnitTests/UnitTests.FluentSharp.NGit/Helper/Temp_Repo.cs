using FluentSharp;
using FluentSharp.CoreLib;
using FluentSharp.ExtensionMethods;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    public class Temp_Repo
    {
        public API_NGit nGit        { get; set; }
        public string   repoPath    { get; set; }
        public bool     deleteRepo  { get; set; }

        public Temp_Repo()
        {
            deleteRepo = true;
        }

        public Temp_Repo(Temp_Repo tempRepo)
        {
            nGit        = tempRepo.nGit;
            repoPath    = tempRepo.repoPath;
            deleteRepo  = tempRepo.deleteRepo;
        }

        [SetUp]
        public void SetUp()
        {            
            repoPath = "_tempRepo".tempDir();
            Assert.IsFalse(repoPath.isGitRepository(), " repoPath should not be a repo");
            Assert.IsEmpty(repoPath.files()          , " repoPath should be empty");
            Assert.IsEmpty(repoPath.dirs()           , " repoPath should be empty");
            nGit = repoPath.git_Init();

            Assert.IsTrue(repoPath.isGitRepository());
            Assert.IsNotNull(nGit);
            Assert.IsNotNull(nGit.Git);
            Assert.IsNotNull(nGit.Repository);

        }

        [TearDown]
        public void TearDown()
        {
            if (deleteRepo)
            {
                Assert.IsTrue(repoPath.dirExists());
                nGit.close()
                    .delete_Repository_And_Files();
                Assert.IsFalse(repoPath.dirExists());
            }
        }

        [Test]
        public void CreateRepoUsingNGit()
        {
            Assert.IsTrue(repoPath.isGitRepository(), "Should be a repo");
            Assert.IsNotNull(nGit);
            Assert.AreEqual(nGit.files_Location(), repoPath);
        }

    }
}
    