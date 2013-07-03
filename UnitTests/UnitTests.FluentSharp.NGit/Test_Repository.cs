using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp;
using FluentSharp.ExtensionMethods;
using NGit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Repository : Temp_Repo
    {
        [Test(Description = "Creates a branch")]
        public void branche_Create()
        {   
            nGit.add_and_Commit_Random_File();

            var branchName      = "branch".add_RandomLetters();
            var branches_Before = nGit.branches();
            var newBranch       = nGit.branch_Create(branchName);
            var branches_After  = nGit.branches();

            Assert.IsNotNull(newBranch);
            Assert.AreEqual(branches_Before.size(),1);
            Assert.AreEqual(branches_After.size(),2);
            Assert.AreEqual(branches_After.first(),branchName);
            Assert.AreEqual(newBranch,branchName);
        }

        [Test(Description = "Returns string list of all refs")]
        public void branches()
        {
            var branchesRaw1 = nGit.branches_Raw();
            var branches1    = nGit.branches();

            Assert.IsEmpty(branchesRaw1);
            Assert.IsEmpty(branches1);

            nGit.add_and_Commit_Random_File();

            var branchesRaw2 = nGit.branches_Raw();
            var branches2    = nGit.branches();

            Assert.IsNotEmpty(branchesRaw2);
            Assert.IsNotEmpty(branches2);
            Assert.AreEqual  (branches2.size() ,1);            
            Assert.AreEqual  (branches2.first(), "master");            
            Assert.AreEqual  (branchesRaw2.first().GetName(), "refs/heads/master");
            Assert.IsEmpty   ((null as API_NGit).branches_Raw());
        }
        [Test(Description = "Returns the SHA1 of the current repo HEAD")]
        public void head()
        {
            var head_BeforeCommit = nGit.head();
            var revCommit = nGit.add_and_Commit_Random_File();
            var head_AfterCommit = nGit.head();
            Assert.IsNull   (head_BeforeCommit);
            Assert.IsNotNull(head_AfterCommit);
            Assert.AreEqual (revCommit.sha1(), head_AfterCommit);
            Assert.IsNull   ((null as API_NGit).head());
        }

        [Test(Description = "Retuns a true is the the provided folder is a Git repository")]
        public void isGitRepository()
        {
            Assert.IsTrue(this.nGit.files_Location().isGitRepository());
            Assert.IsTrue(this.nGit.Path_Local_Repository.isGitRepository());
            Assert.IsTrue(this.repoPath.isGitRepository());

            var tempDir = "another_test".tempDir();

            Assert.IsFalse(tempDir.isGitRepository());
            var tempGitRepo = tempDir.git_Init();
            Assert.IsTrue(tempDir.isGitRepository());
            tempGitRepo.delete_Repository_And_Files();
        }

        [Test(Description = "Returns string list of all refs")]
        public void refs()
        {
            var refsRaw1 = nGit.refs_Raw();
            var refs1    = nGit.refs();

            Assert.IsEmpty(refsRaw1);
            Assert.IsEmpty(refs1);

            nGit.add_and_Commit_Random_File();

            var refsRaw2 = nGit.refs_Raw();
            var refs2    = nGit.refs();

            Assert.IsNotEmpty(refsRaw2);
            Assert.IsNotEmpty(refs2);
            Assert.AreEqual  (refs2.size()  ,2);
            Assert.AreEqual  (refs2.first() , "HEAD");
            Assert.AreEqual  (refs2.second(), "refs/heads/master");
            Assert.AreEqual  (refsRaw2.first().GetName(), "HEAD");
            Assert.AreEqual  (refsRaw2.second().GetName(), "refs/heads/master");
            Assert.IsEmpty   ((null as API_NGit).refs_Raw());
        }        

        [Test(Description = "Returns the NGit repository object")]
        public void repository()
        {
            var repository = nGit.repository();
            Assert.IsNotNull(repository);
            Assert.IsNull((null as API_NGit).repository());
        }

        [Test(Description = "Sets the Global Credential to use on Pushes and Pulls")]
        public void use_Credential()
        {
            var username = "a name".add_RandomLetters();
            var password = "a password".add_RandomLetters();
            nGit.use_Credential(username, password);
            Assert.AreEqual(username, nGit.Credentials.field<string>("username")); 
            Assert.AreEqual(password, nGit.Credentials.field<char[]>("password").ascii());
        } 

    }
}
