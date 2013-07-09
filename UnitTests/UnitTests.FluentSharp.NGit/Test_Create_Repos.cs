using System.IO;
using FluentSharp;
using FluentSharp.CoreLib;
using NUnit.Framework;
using O2.DotNetWrappers.Windows;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Create_Repos// : Temp_Repo
    {            
                       
        [Test] public void Create_Repo_Add_Files_Check_Head()
        {
            var nGit = new API_NGit();
            var tempRepo = "_tempRepo".tempDir(true);
            "TestRepo is: {0}".info(tempRepo);
            //Creating a local temp Repo
            Assert.IsFalse(tempRepo.isGitRepository() , "Should not be a repo");
            nGit.init(tempRepo);                        
            Assert.IsTrue(tempRepo.isGitRepository(), "Should be a repo");            
            Assert.IsNull(nGit.head());

            //Adding a file (using method 1)
            nGit.file_Create("testFile.txt", "some Text");
            nGit.add_and_Commit_using_Status();
            var head1 = nGit.head();
            Assert.IsNotNull(head1);
                        
            //Adding another file (using method 2)
            nGit.file_Create("testFile2.txt", "some Text");
            nGit.add("testFile2.txt");
            nGit.commit("Adding Another file");
            
            //making sure the head has changed
            var head2 = nGit.head();
            Assert.AreNotEqual(head1,head2);

            nGit.delete_Repository_And_Files();
        }

        [Test]
        public void Create_and_Delete_Repo_On_Disk()
        {
            // Delete repo with no commit
            var tempRepo1 = "_tempRepo".tempDir(true);
            tempRepo1.git_Init();

            Assert.IsTrue(tempRepo1.dirExists());
            Assert.IsTrue(tempRepo1.isGitRepository());

            Files.deleteFolder(tempRepo1, true);

            Assert.IsFalse(tempRepo1.dirExists());

            // Delete repo with commit
            var tempRepo2 = "_tempRepo".tempDir(true);
            var repo = tempRepo2.git_Init();

            repo.file_Write("a file.txt", "some content in the file");
            
            repo.add_and_Commit_using_Status();
            
            //need to remove the read-only attribute (before deleting)
            foreach (var file in tempRepo2.files(true))
                file.file_Attribute_ReadOnly_Remove();
            
            var deleteResult = Files.deleteFolder(tempRepo2, true);

            Assert.IsTrue(deleteResult);
            Assert.IsFalse(tempRepo2.dirExists());
        }

    }
}
