using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NUnit.Framework;
using O2.DotNetWrappers.Network;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    class Test_O2Platform_Repos
    {        
        [Test]
        public void NGit_O2Platform()
        {
            if (Web.Online.isFalse())
                Assert.Ignore("Skiping because we are offline");
            var tempFolder = "_API_NGit_O2Platform".tempDir();
            
            var repoToClone = "UnitTests_TestRepo";
            var pathToRepo  = tempFolder.pathCombine(repoToClone);
            
            Assert.IsTrue (tempFolder.dirExists());
            Assert.IsEmpty(tempFolder.dirs());
            Assert.IsFalse(pathToRepo.dirExists());
            Assert.IsFalse(pathToRepo.isGitRepository());


            API_NGit_O2Platform ngitO2Platform = null;

            Action checkRepo = 
                    ()=>{
                            Assert.IsNotNull(ngitO2Platform);
                            Assert.IsNull   (ngitO2Platform.Git);
                            Assert.IsNull   (ngitO2Platform.Repository);

                            ngitO2Platform  .cloneOrPull(repoToClone);

                            Assert.IsTrue   (pathToRepo.dirExists());
                            Assert.IsTrue   (pathToRepo.isGitRepository());

                            ngitO2Platform.open(pathToRepo);
                            
                            Assert.IsNotNull(ngitO2Platform.Git);
                            Assert.IsNotNull(ngitO2Platform.Repository);
                        };

            //Test Clone
            ngitO2Platform = new API_NGit_O2Platform(tempFolder);
            checkRepo();

            //Test Open
            ngitO2Platform = new API_NGit_O2Platform(tempFolder);
            checkRepo();
        }
    }
}
