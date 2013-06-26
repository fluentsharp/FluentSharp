using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NUnit.Framework;
using O2.DotNetWrappers.Windows;

namespace UnitTests.FluentSharp_NGit
{
    public class Temp_Repo
    {
        public API_NGit nGit;
        public string repoPath;

        [SetUp]
        public void SetUp()
        {            
            repoPath = "_tempRepo".tempDir();
            Assert.IsFalse(repoPath.isGitRepository(), " repoPath should not be a repo");
            Assert.IsEmpty(repoPath.files()          , " repoPath should be empty");
            Assert.IsEmpty(repoPath.dirs()           , " repoPath should be empty");
            nGit = repoPath.git_Init();
        }

        [TearDown]
        public void TearDown()
        {
            Assert.IsTrue(repoPath.dirExists());
            nGit.close()
                .delete_Repository_And_Files();            
            Assert.IsFalse(repoPath.dirExists());
        }

        [Test]
        public void CreateRepoUsingNGit()
        {
            Assert.IsTrue(repoPath.isGitRepository(), "Should be a repo");
            Assert.IsNotNull(nGit);
            Assert.AreEqual(nGit.files_Location(), repoPath);
        }

    }
}
    