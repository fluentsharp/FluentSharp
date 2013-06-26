using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Misc : Temp_Repo
    {
        [Test(Description = "Returns the NGit repository object")]
        public void repository()
        {
            var repository = nGit.repository();
            Assert.IsNotNull(repository);
            Assert.IsNull((null as API_NGit).repository());
        }
        
        [Test(Description = "Returns the NGIt Git object")]
        public void git()
        {
            var git = nGit.git();
            Assert.IsNotNull(git);
            Assert.IsNull((null as API_NGit).git());
        }
        
        [Test(Description = "Returns the .git folder")]
        public void git_Folder()
        {
            var gitFolder       = nGit.git_Folder();
            var expectedFolder  = nGit.files_Location().pathCombine(".git");
            var headFile        = gitFolder.pathCombine("HEAD");

            Assert.AreEqual(gitFolder, expectedFolder);
            Assert.IsTrue  (headFile.fileExists());
            Assert.AreEqual(headFile.fileContents().trim(), "ref: refs/heads/master");
        }

    }
}
