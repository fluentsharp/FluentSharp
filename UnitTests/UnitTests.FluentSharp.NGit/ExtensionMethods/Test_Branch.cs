using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NGit.Api.Errors;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit.ExtensionMethods
{
    [TestFixture]
    public class Test_Branch : Temp_Repo
    {
        [Test(Description = "Returns string list of all branches")]
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
        [Test(Description = "Creates a branch")]
        public void branch_Create()
        {
            var newBranch = "testBranch".add_RandomLetters(10);
            nGit.branch_Create(newBranch);
            Assert.IsInstanceOf<RefNotFoundException>(KO2Log.Last_Exception);

            nGit.add_and_Commit_Random_File();
            var result   = nGit.branch_Create(newBranch);
            var branches = nGit.branches();
            Assert.IsTrue(result);            
            Assert.AreEqual(branches.size(),2);         
            Assert.AreEqual(branches.first(),"master");
            Assert.AreEqual(branches.last(),newBranch);

            Assert.IsFalse(nGit              .branch_Create(null));
            Assert.IsFalse((null as API_NGit).branch_Create(newBranch));
        }

        [Test(Description = "Creates a branch")]
        public void branch_Delete()
        {
            var newBranch = "testBranch".add_RandomLetters(10);            
            nGit.add_and_Commit_Random_File();
            nGit.branch_Create(newBranch);
            
            var branches_Before = nGit.branches();            
            Assert.AreEqual(branches_Before.size(),2);
            Assert.AreEqual(branches_Before.last(),newBranch);

            var result          = nGit.branch_Delete(newBranch);
            var branches_After = nGit.branches();            

            Assert.IsTrue(result);
            Assert.AreEqual(branches_After.size(),1);
            Assert.AreEqual(branches_After.last(),"master");
            
            Assert.IsFalse(nGit              .branch_Delete("Aaa"));
            Assert.IsFalse(nGit              .branch_Delete(null));
            Assert.IsFalse((null as API_NGit).branch_Delete(newBranch));
        }

        [Test(Description = "Renames a branch")]
        public void branch_Rename()
        {
            var newBranch     = "testBranch".add_RandomLetters(10);            
            var renamedBranch = "testBranch".add_RandomLetters(10);            
            nGit.add_and_Commit_Random_File();
            nGit.branch_Create(newBranch);
            var result = nGit.branch_Rename(newBranch, renamedBranch);
            var branches = nGit.branches();

            Assert.AreNotEqual(newBranch, renamedBranch);
            Assert.IsTrue     (result);
            Assert.AreEqual   (branches.size() ,2);
            Assert.AreEqual   (branches.first(),"master");
            Assert.AreEqual   (branches.second(),renamedBranch);

            Assert.IsFalse(nGit              .branch_Rename(null,null));
            Assert.IsFalse((null as API_NGit).branch_Rename(newBranch,renamedBranch));
        }
        [Test(Description = "Gets the current branch")]
        public void branch_Current()
        {
            nGit.add_and_Commit_Random_File();
            var currentBranch = nGit.branch_Current();
            Assert.AreEqual(currentBranch,"master");
            Assert.IsNull  ((null as API_NGit).branch_Current());
        }
        [Test(Description = "Checkout (i.e switches into) a particular branch")]
        public void branch_Checkout()
        {
            var newBranch     = "testBranch".add_RandomLetters(10);
            nGit.add_and_Commit_Random_File();
            nGit.branch_Create(newBranch);

            Assert.AreEqual(nGit.branch_Current(),"master");
            var result1 = nGit.branch_Checkout(newBranch);

            Assert.AreEqual(nGit.branch_Current(),newBranch);
            var result2 = nGit.checkout("master");

            Assert.AreEqual(nGit.branch_Current(),"master");
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);

            Assert.IsFalse(nGit              .branch_Checkout(null));
            Assert.IsFalse((null as API_NGit).branch_Checkout(newBranch));
        }
    }
}
