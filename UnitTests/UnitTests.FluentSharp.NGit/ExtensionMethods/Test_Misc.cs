using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Git
{
    [TestFixture]
    public class Test_Misc : Temp_Repo
    {
        [Test(Description = "Changes backslash \\ into a forward slash")]
        public void changeBackSlashWithForwardSlash()
        {
            var test1   = @"C:\\aa\\bb\\c.txt";
            var test2   = @"\\aa\\bb\\c.txt";
            var test3   = @"aa\\bb\\c.txt";
            var test4   = @"bb\\c.txt";
            var result1 = @"C:/aa/bb/c.txt";
            var result2 = @"/aa/bb/c.txt";
            var result3 = @"aa/bb/c.txt";
            var result4 = @"bb/c.txt";

            Assert.AreEqual(result1, test1.changeBackslashWithForwardSlash());
            Assert.AreEqual(result2, test2.changeBackslashWithForwardSlash());
            Assert.AreEqual(result3, test3.changeBackslashWithForwardSlash());
            Assert.AreEqual(result4, test4.changeBackslashWithForwardSlash());
        }

        [Test(Description = "Changes double forward slash //  into a single slash /")]
        public void fixDoubleForwardSlash()
        {
            var test1 = @"C://aa//bb//c.txt";
            var test2 = @"//aa//bb//c.txt";
            var test3 = @"aa//bb//c.txt";
            var test4 = @"bb//c.txt";
            var result1 = @"C:/aa/bb/c.txt";
            var result2 = @"/aa/bb/c.txt";
            var result3 = @"aa/bb/c.txt";
            var result4 = @"bb/c.txt";

            Assert.AreEqual(result1, test1.fixDoubleForwardSlash());
            Assert.AreEqual(result2, test2.fixDoubleForwardSlash());
            Assert.AreEqual(result3, test3.fixDoubleForwardSlash());
            Assert.AreEqual(result4, test4.fixDoubleForwardSlash());
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
       
         [Test(Description = "Returns true id objectId is not null or not 0000000000")]
        public void valid()
         {
             var objectId1 = "6b384b15fbeecdd9747f35ff3ce3db4de86c72a4".objectId();             
             var objectId2 = NGit_Consts.EMPTY_SHA1.objectId();
             var objectId3 = (null as string).objectId();
             var objectId4 = "6b384b15fbeecdd9747f35ff3ce3db4de86c72--".objectId();
             var objectId5 = "6b384b15fbeecdd9747f35ff3ce3db4de86c72a4111".objectId();

             Assert.NotNull(objectId1);
             Assert.NotNull(objectId2);
             Assert.NotNull(objectId3);
             Assert.IsNull (objectId4);
             Assert.IsNull (objectId5);

             Assert.IsTrue(objectId1.valid());
             Assert.IsFalse(objectId2.valid());
             Assert.IsFalse(objectId3.valid());
             Assert.IsFalse(objectId4.valid());
             Assert.IsFalse(objectId5.valid());
         }
    }
}
