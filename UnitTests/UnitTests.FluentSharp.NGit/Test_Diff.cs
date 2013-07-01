
using NGit;
using NGit.Api;
using NGit.Treewalk;
using NUnit.Framework;
using FluentSharp.ExtensionMethods;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    class Test_Diff : Temp_Repo
    {
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
