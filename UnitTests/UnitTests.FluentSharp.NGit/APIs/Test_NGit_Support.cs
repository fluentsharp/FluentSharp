using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Git
{
    [TestFixture]
    public class Test_API_NGit
    {        
        public API_NGit NGitApi  { get; set; }

        public Test_API_NGit()
        {            
            NGitApi = new API_NGit();
        }


        //Workflows
        [Test] public void CreateRepoUsingNGit()
        {
            var testRepo2 = "_tempRepo".tempDir(true);
            testRepo2.info();
            "IsGitrepo: {0}".info(testRepo2.isGitRepository());
            Assert.IsFalse(testRepo2.isGitRepository() , "Should not be a repo");
            var initCommand = NGit.Api.Git.Init();
            initCommand.SetDirectory(testRepo2);            
            initCommand.Call();
            "IsGitrepo: {0}".info(testRepo2.isGitRepository());
            Assert.IsTrue(testRepo2.isGitRepository() , "Should be a repo");
            deleteRepo(testRepo2);
            Assert.IsFalse(testRepo2.isGitRepository());
            Assert.IsFalse(testRepo2.dirExists());
        }
        [Test] public void CreateLocalTestRepo()
        {
            var tempRepo = "_tempRepo".tempDir(true);
            "TestRepo is: {0}".info(tempRepo);
            //NGitApi.script_Me().waitForClose();
            //Creating a local temp Repo
            Assert.IsFalse(tempRepo.isGitRepository() , "Should not be a repo");
            NGitApi.init(tempRepo);                        
            Assert.IsTrue(tempRepo.isGitRepository(), "Should be a repo");            
            Assert.IsNull(NGitApi.head());

            //Adding a file (using method 1)
            NGitApi.file_Create("testFile.txt", "some Text");
            NGitApi.add_and_Commit_using_Status();
            var head1 = NGitApi.head();
            Assert.IsNotNull(head1);
                        
            //Adding another file (using method 2)
            NGitApi.file_Create("testFile2.txt", "some Text");
            NGitApi.add("testFile2.txt");
            NGitApi.commit("Adding Another file");
            
            //making sure the head has changed
            var head2 = NGitApi.head();
            Assert.AreNotEqual(head1,head2);        
            deleteRepo(tempRepo);
            Assert.IsFalse(tempRepo.isGitRepository());
            Assert.IsFalse(tempRepo.dirExists());
        }

        //helper method

        public void deleteRepo(string targetRepo)
        {
            var files = targetRepo.files(true);
            files.files_Attribute_ReadOnly_Remove(); // need to do this or the repo cannot be deleted            
            Assert.IsTrue (targetRepo.isGitRepository() , "Should be a repo");
            Assert.IsTrue (targetRepo.dirExists());
            Assert.IsTrue (Files.deleteFolder(targetRepo,true));            
            Assert.IsFalse(targetRepo.dirExists());
        }
        /*
        [Test]
        public void CommitDiffs()
        {
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

             WorkingTreeIterator workingTreeIt  = new FileTreeIterator(NGitApi.Repository);

            var indexDiff = new IndexDiff(NGitApi.Repository, Constants.HEAD, workingTreeIt);
            indexDiff.Diff();
		    var result = new Status(indexDiff);
            "RESULT: {0}".info(result);

            OutputStream outputStream = "Sharpen.dll".assembly().type("ByteArrayOutputStream").ctor(new object[0]).cast<OutputStream>();

            var diffFormater = new DiffFormatter(outputStream);
            diffFormater.SetRepository(nGit.Repository);
            //diffFormater.Format(refLog.GetNewId(), refLog.GetOldId());
            diffFormater.Format(refLog.GetOldId(), refLog.GetNewId());
             
        }* */
    }
}
