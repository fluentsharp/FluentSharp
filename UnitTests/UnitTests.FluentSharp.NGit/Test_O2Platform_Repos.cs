using System;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NUnit.Framework;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Windows;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    class Test_O2Platform_Repos
    {
        public string tempFolder;

        public Test_O2Platform_Repos()
        {
            if (Web.Online.isFalse())
                Assert.Ignore("Skiping because we are offline");

            
        }

        [SetUp]
        public void setup()
        {
            tempFolder = "_API_NGit_O2Platform".tempDir();
            Assert.IsTrue(tempFolder.dirExists());
        }

        [TearDown]
        public void teardown()
        {
            tempFolder.folder_Delete();
            Assert.IsFalse(tempFolder.dirExists());
        }
        [Test]
        public void NGit_O2Platform()
        {
            var repoToClone = "UnitTests_TestRepo";
            var pathToRepo  = tempFolder.pathCombine(repoToClone);
            
            Assert.IsTrue (tempFolder.dirExists());
            Assert.IsEmpty(tempFolder.dirs());
            Assert.IsFalse(pathToRepo.dirExists());
            Assert.IsFalse(pathToRepo.isGitRepository());

            API_NGit_O2Platform ngit_O2 = null;

            Action checkRepo = 
                    ()=>{
                            Assert.IsNotNull(ngit_O2);
                            Assert.IsNull   (ngit_O2.Git);
                            Assert.IsNull   (ngit_O2.Repository);

                            ngit_O2.cloneOrPull(repoToClone);

                            Assert.IsTrue   (pathToRepo.dirExists());
                            Assert.IsTrue   (pathToRepo.isGitRepository());

                            ngit_O2.open(pathToRepo);

                            Assert.IsNotNull(ngit_O2.Git);
                            Assert.IsNotNull(ngit_O2.Repository);

                            ngit_O2.close();
                    };      

            //Test Clone
            ngit_O2 = new API_NGit_O2Platform(tempFolder);
            checkRepo();
            
            //Test Open
            ngit_O2 = new API_NGit_O2Platform(tempFolder);
            checkRepo();

            var result = ngit_O2.delete_Repository_And_Files();
            Assert.IsTrue(result);

            tempFolder.delete_Folder();
            Assert.IsFalse(tempFolder.dirExists());
        }

        [Test]
        public void Clone_Private_Repo_No_Authorization()
        {
            var repoToClone = "UnitTests_TestRepo_Private";
            var pathToRepo = tempFolder.pathCombine(repoToClone);
            Files.deleteFolder(pathToRepo,true);            
            Assert.IsTrue(tempFolder.dirExists());
            Assert.IsEmpty(tempFolder.dirs());
            Assert.IsFalse(pathToRepo.dirExists());
            Assert.IsFalse(pathToRepo.isGitRepository());

            var ngit_O2 = new API_NGit_O2Platform(tempFolder);
            var repositoryUrl = ngit_O2.repositoryUrl(repoToClone);

            Assert.IsNull(ngit_O2.LastException);

            //clone should fail 
            ngit_O2.clone(repositoryUrl, pathToRepo);

            //no git repo should be there (if clone failed
            Assert.IsNotNull(ngit_O2.LastException);
            Assert.IsEmpty(tempFolder.dirs());
            Assert.IsFalse(pathToRepo.dirExists());
            Assert.IsFalse(pathToRepo.isGitRepository());                        
        }
    }
}
