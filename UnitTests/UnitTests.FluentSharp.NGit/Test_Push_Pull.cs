using FluentSharp;
using FluentSharp.ExtensionMethods;
using NGit.Revwalk;
using NGit.Transport;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Push_Pull
    {
        public string    repoPath1;
        public string    repoPath2;
        public API_NGit  nGit1;
        public API_NGit  nGit2;
        public RevCommit revCommit1;
    
        [SetUp]
        public void setUp()
        {
            //create repos
            repoPath1 = "repo1".tempDir();
            repoPath2 = "repo2".tempDir();

            nGit1 = repoPath1.git_Init();
            nGit2 = repoPath2.git_Init();

            Assert.AreNotEqual(repoPath1, repoPath2);
            Assert.IsTrue     (repoPath1.isGitRepository());
            Assert.IsTrue     (repoPath2.isGitRepository());

            //add a file to repo 1            
            revCommit1 = nGit1.add_and_Commit_Random_File();
        }

        [SetUp]
        public void tearDown()
        {
            //delete repos
            nGit1.delete_Repository_And_Files();
            nGit2.delete_Repository_And_Files();

            Assert.IsFalse(repoPath1.dirExists());
            Assert.IsFalse(repoPath2.dirExists());
        }

        [Test(Description = "Push the current branch into a remote")]
        public void push()
        {
            //set repo2 remote and pull
            var result_RemoteAdd   = nGit2.remote_Add("origin", repoPath1);//.pathCombine(".git"));
            var result_Pull_Origin = nGit2.pull();
            //var commits_after_Pull = nGit2.commits();

            //repoPath1.startProcess();
            //repoPath2.startProcess();
            Assert.IsNull(nGit2.Last_Exception);
            Assert.IsTrue(result_RemoteAdd);
            Assert.IsTrue(result_Pull_Origin);

            
        }

        [Test(Description = "Pulls from a remote into current branch ")]
        public void pull()
        {
            //push()
        }

        [Test(Description = "Pulls from a remote into current branch ")]
        public void fetch()
        {
            //var result_RemoteAdd   = nGit2.remote_Add("origin", repoPath1);//.pathCombine(".git"));
            var fetchCommand = nGit2.Git.Fetch();
            fetchCommand.SetRemote(repoPath1);
            var spec = new RefSpec("refs/heads/master:refs/heads/master");
            fetchCommand.SetRefSpecs(spec);
            fetchCommand.Call();

        }
    }
}
