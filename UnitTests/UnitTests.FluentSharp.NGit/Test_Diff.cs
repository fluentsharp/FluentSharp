
using NGit;
using NGit.Api;
using NGit.Diff;
using NGit.Treewalk;
using NUnit.Framework;
using FluentSharp.ExtensionMethods;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    class Test_Diff : Temp_Repo
    {

        [Test(Description = "Gets a string diff of the current repo (i.e. not committed changes")]
        public void diff()
        {
            //Diff when there are no file changes
            var diff1 = nGit.diff();
            Assert.AreEqual("", diff1);

            var fileName        = "test.txt";
            var fileContents_1  = "someText";
            var fileContents_2  = "someText2";
            var expectedSha1_1  = "61447cbfe08f98ece3b0491e9e111bcb3188d79a";
            var expectedSha1_2  = "6b384b15fbeecdd9747f35ff3ce3db4de86c72a4";
            var shortSha1_1     = expectedSha1_1.subString(0,7);
            var shortSha1_2     = expectedSha1_2.subString(0,7);

            nGit.file_Write(fileName, fileContents_1);

            //Diff after writing file and before commit
            var diff2 = nGit.diff();
            Assert.IsTrue(diff2.contains(fileName));
            Assert.IsTrue(diff2.contains(fileContents_1));
            Assert.IsTrue(diff2.contains(shortSha1_1));

            var revCommit1 = nGit.add_and_Commit_using_Status();
            
            //Diff after add and commit
            var diff3 = nGit.diff();
            Assert.AreEqual("", diff3);

            nGit.file_Write(fileName, fileContents_2);

            //Diff after modifiding file
            var diff4 = nGit.diff();
            Assert.IsTrue(diff4.contains(fileName));
            Assert.IsTrue(diff4.contains(fileContents_2));
            Assert.IsTrue(diff4.contains(shortSha1_1));
            Assert.IsTrue(diff4.contains(shortSha1_2));
            
            var revCommit2 = nGit.add_and_Commit_using_Status();

            //Diff of two commits
            var diff5 = nGit.diff(revCommit1, revCommit2);
            Assert.IsNotNull(diff5);
            Assert.IsTrue(diff5.contains(fileName));
            Assert.IsTrue(diff5.contains(fileContents_1));
            Assert.IsTrue(diff5.contains(fileContents_2));
            Assert.IsTrue(diff5.contains(shortSha1_1));
            Assert.IsTrue(diff5.contains(shortSha1_2));
        }

        [Test(Description = "Gets an List of DiffEntry objects (use OutputStream to get a string representation of it)")]
        public void diff_Raw()
        {
            var fileName = "test.txt";
            var fileContents = "someText";
            var expectedSha1 = "61447cbfe08f98ece3b0491e9e111bcb3188d79a";
            var diff_Raw1 = nGit.diff_Raw();
            Assert.IsEmpty(diff_Raw1);
            nGit.file_Write(fileName, fileContents);
            var diff_Raw2 = nGit.diff_Raw();
            Assert.IsNotEmpty(diff_Raw2);
            Assert.AreEqual  (1, diff_Raw2.size());
            var diffEntry = diff_Raw2.first();
            
            Assert.AreEqual("ADD"   , diffEntry.changeType());
            Assert.AreEqual(fileName,  diffEntry.path());
            Assert.AreEqual(expectedSha1   , diffEntry.sha1());

            var revCommit = nGit.add_and_Commit_using_Status();
            var commitFiles = revCommit.commit_Files_IndexedBy_SHA1(nGit);

            Assert.AreEqual(expectedSha1   , commitFiles.keys().first());

            //check null handling
            Assert.IsNull((null as DiffEntry).changeType());
            Assert.IsNull((null as DiffEntry).path());
            Assert.IsNull((null as DiffEntry).sha1());
        }

        [Test(Description = "Gets an List of DiffEntry objects from two commits")]
        public void diff_Commits()
        {
            diff();  // will create two RevCommits
            var revCommits  = nGit.commits();            
            var diffEntries = nGit.diff_Commits(revCommits.first(), revCommits.second());

            Assert.AreEqual  (2, revCommits.size());
            Assert.IsNotEmpty(diffEntries);
            Assert.AreEqual  (1, diffEntries.size());

            //Test Exception
            Assert.IsNull (nGit.diff        (null as string, null as string));
            Assert.IsEmpty(nGit.diff_Commits(null, null));
        }




        [Test]
        public void CommitDiffs()
        {            
            Assert.IsNull(nGit.head());
            nGit.file_Create("testFile.txt", "some Text");
            "head 1 :{0}".info(nGit.head().info());
            nGit.add_and_Commit_using_Status();
            "head 2 :{0}".info(nGit.head().info());
            nGit.file_Write("testFile.txt", "some Text changed");
            nGit.add_and_Commit_using_Status();
            var head3 = nGit.head();
            "head 3 :{0}".info(head3.info());

            var workingTreeIt = new FileTreeIterator(nGit.Repository);

            var indexDiff = new IndexDiff(nGit.Repository, Constants.HEAD, workingTreeIt);
            indexDiff.Diff();
            var result = new Status(indexDiff);
            "RESULT: {0}".info(result);

            /*OutputStream outputStream = "Sharpen.dll".assembly().type("ByteArrayOutputStream").ctor(new object[0]).cast<OutputStream>();

            var diffFormater = new DiffFormatter(outputStream);
            diffFormater.SetRepository(nGit.Repository);
            //diffFormater.Format(refLog.GetNewId(), refLog.GetOldId());
            diffFormater.Format(refLog.GetOldId(), refLog.GetNewId());*/            
        }        
    }
}
