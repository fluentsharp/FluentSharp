using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.ExtensionMethods;
using NGit.Revwalk;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Status : Temp_Repo
    {
        [Test(Description = "Gets string with lots of detais about the status of the current repo (not committed changes)")]
        public void status()
        {
            var file1 = "TestFile.txt";
            var content1 =  "aaaaa";
            var untrackedMessage = "untracked: TestFile.txt\n";

            var status1 = nGit.status();
            nGit.file_Write(file1, content1);

            var status2 = nGit.status();
            var revCommit = nGit.commit_using_Status();
            
            Assert.AreEqual(""              , status1);
            Assert.AreEqual(untrackedMessage, status2);
            Assert.AreEqual(untrackedMessage, revCommit.message());

            //test nulls
            Assert.IsNull((null as RevCommit).message());
        }
    }
}
