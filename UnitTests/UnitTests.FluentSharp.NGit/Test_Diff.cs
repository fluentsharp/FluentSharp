
using FluentSharp;
using NGit;
using NGit.Api;
using NGit.Treewalk;
using NUnit.Framework;
using FluentSharp.ExtensionMethods;

namespace UnitTests.FluentSharp.NGit
{
    [TestFixture]
    class Test_Diff
    {
        [Test]
        public void CommitDiffs()
        {
            var NGitApi = new API_NGit();

            var tempRepo = "_tempRepo".tempDir();
            NGitApi.init(tempRepo);
            Assert.IsNull(NGitApi.head());
            NGitApi.create_File("testFile.txt", "some Text");
            "head 1 :{0}".info(NGitApi.head().info());
            NGitApi.add_and_Commit_using_Status();
            "head 2 :{0}".info(NGitApi.head().info());
            NGitApi.write_File("testFile.txt", "some Text changed");
            NGitApi.add_and_Commit_using_Status();
            var head3 = NGitApi.head();
            "head 3 :{0}".info(head3.info());

            WorkingTreeIterator workingTreeIt = new FileTreeIterator(NGitApi.Repository);

            var indexDiff = new IndexDiff(NGitApi.Repository, Constants.HEAD, workingTreeIt);
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
