using FluentSharp;
using FluentSharp.ExtensionMethods;
using NGit;
using NGit.Api;
using NGit.Revwalk;
using NUnit.Framework;
using O2.DotNetWrappers.Windows;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    internal class Test_Init_Clone_Open
    {
        [Test(Description = "Creates a repository in the provided path")]
        public void git_Init()
        {
            var targetFolder = "git_Init".tempDir();

            Assert.IsFalse(targetFolder.isGitRepository());

            var nGit = targetFolder.git_Init();

            Assert.IsTrue(targetFolder.isGitRepository());
            Assert.IsNotNull(nGit);
            Assert.IsNotNull(nGit.Git);
            Assert.IsNotNull(nGit.Repository);

            nGit.write_File("testFile.txt", "Some Contents");
            nGit.add_and_Commit_using_Status();
            //test errors
            var cloneExistingRepo = targetFolder.git_Init();
            Assert.IsNull(cloneExistingRepo);

            var result = nGit.init(targetFolder);
            Assert.IsNull(result);

            result = nGit.init(null);
            Assert.IsNull(result);

            nGit.delete_Repository_And_Files();
        }

        [Test(Description = "Opens a local repository")]
        public void git_Open()
        {
            var tempFolder = "tempRepo".tempDir();
            Assert.IsFalse(tempFolder.isGitRepository());
            tempFolder.git_Init();
            Assert.IsTrue(tempFolder.isGitRepository());
            var nGit = tempFolder.git_Open();
            Assert.IsNotNull(nGit);
            nGit.delete_Repository_And_Files();
        }
       
        [Test(Description = "Deletes the .git repository folder")]
        public void delete_Repository()
        {
            var localRepo = "localRepo".tempDir();
            var gitFolder = localRepo.pathCombine(".git");

            Assert.IsTrue (localRepo.dirExists());            
            Assert.IsFalse(gitFolder.dirExists());
            

            var nGit_Repo = localRepo.git_Init();
            nGit_Repo.file_Create_Random_File();
            nGit_Repo.add_and_Commit_using_Status();

            Assert.IsTrue (gitFolder.dirExists());
            Assert.IsTrue(localRepo.isGitRepository());

            nGit_Repo.delete_Repository();

            Assert.IsFalse(gitFolder.dirExists());
            Assert.IsFalse(localRepo.isGitRepository());
            Files.deleteFolder(localRepo, true);
        }

        [Test(Description = "Deletes the .git repository folder and the checked out files")]
        public void delete_Repository_And_Files()
        {
            var localRepo = "localRepo".tempDir();
            var nGit_Repo = localRepo.git_Init();
            nGit_Repo.file_Create_Random_File();
            nGit_Repo.add_and_Commit_using_Status();

            nGit_Repo.delete_Repository_And_Files();
            
            Assert.IsFalse(localRepo.isGitRepository());
            Assert.IsFalse(localRepo.dirExists());


            //check exceptions
            Assert.IsFalse((null as API_NGit).delete_Repository_And_Files());
            Assert.IsFalse((null as API_NGit).delete_Repository());
            var badNGitObject = new API_NGit { Path_Local_Repository = localRepo };
            Assert.IsFalse(badNGitObject.delete_Repository());
        }

        [Test(Description = "Clones a repository")]
        public void git_Clone()
        {
            //clone Locally
            var localRepo = "localRepo".tempDir();
            var localClone = "localClone".tempDir().pathCombine("Clone");

            Assert.AreNotEqual(localRepo, localClone);
            Assert.IsFalse(localRepo.isGitRepository());
            Assert.IsFalse(localClone.isGitRepository());

            var nGit_Repo = localRepo.git_Init();
            nGit_Repo.file_Create_Random_File();
            nGit_Repo.add_and_Commit_using_Status();

            Assert.IsNotEmpty(nGit_Repo.files());


            var nGit_Clone = localRepo.git_Clone(localClone);

            Assert.IsNotNull(nGit_Clone);
            Assert.IsTrue(localRepo.isGitRepository());
            Assert.IsTrue(localClone.isGitRepository());
            Assert.IsNotEmpty(nGit_Clone.files());

            Assert.AreEqual(nGit_Repo.files().asString(), nGit_Clone.files().asString());

            var result1 = nGit_Repo.delete_Repository_And_Files();
            var result2 = nGit_Clone.delete_Repository_And_Files();

            localClone.parentFolder().error();

            var result3 = Files.deleteFolder(localClone.parentFolder(), true);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
        }        
    }
}
