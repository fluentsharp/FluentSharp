using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    class Test_RevWalk : Temp_Repo
    {
        [Test(Description = "Returns a RevWalk object for the current Repository")]
        public void revWalk()
        {
            Assert.IsNotNull(nGit.revWalk());
            Assert.IsNull   ((null as API_NGit).revWalk());
        }

        [Test(Description = "Returns a list of RevObjects from the current revWalk")]
        public void objects()
        {
            Assert.IsEmpty   (nGit.revWalk().objects()  , "Default revWalk should not have any objects");

            nGit.add_and_Commit_Random_File();

            var revWalk = nGit.revCommits_Raw();
            var commits = revWalk.size();            
            var objects = revWalk.objects();

            Assert.AreEqual  (1, commits                , "There should a 1 commit in this repo");
            Assert.IsNotEmpty(objects);
            Assert.AreEqual  (2, objects.size()         , "There should be two objects (a tree and a commit)");
        }

        [Test(Description = "Returns an Object from its SHA1")]
        public void object_Get()
        {
            nGit.add_and_Commit_Random_File();
            var commits_SHA1 = nGit.commits_SHA1();
            var revCommits = nGit.revCommits_Raw();
            Assert.IsNotEmpty(commits_SHA1);
            foreach (var commit_SHA1 in commits_SHA1)
            {
                var ok_ObjectId  = revCommits.object_Get(commit_SHA1);
                var bad_ObjectId = revCommits.object_Get(commit_SHA1+"1");
                Assert.IsNotNull(ok_ObjectId);
                Assert.IsNull(bad_ObjectId);

            }
        }
    }
}
