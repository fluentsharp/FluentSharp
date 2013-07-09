using System;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using NGit;
using NGit.Revwalk;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit
{
    [TestFixture]
    public class Test_Commit : Temp_Repo
    {
        [Test(Description = "Makes a commit with the files currently staged")]
        public void commit()
        {
            Assert.IsEmpty(nGit.commits_SHA1());

            var revCommit  = nGit.add_and_Commit_Random_File();
            var firstCommit = nGit.commits_SHA1().first();

            Assert.IsNotEmpty(nGit.commits_SHA1());
            Assert.AreEqual(nGit.commits().size(),1);

            var author = revCommit.GetAuthorIdent();
            Assert.AreEqual(NGit_Consts.DEFAULT_COMMIT_NAME , author.GetName());
            Assert.AreEqual(NGit_Consts.DEFAULT_COMMIT_EMAIL, author.GetEmailAddress());

            var committer = revCommit.GetCommitterIdent();
            Assert.AreEqual(NGit_Consts.DEFAULT_COMMIT_NAME , committer.GetName());
            Assert.AreEqual(NGit_Consts.DEFAULT_COMMIT_EMAIL, committer.GetEmailAddress());

            var commitedFiles = revCommit.commit_Files(nGit);
            var repoFiles = nGit.files();

            Assert.AreEqual(1, repoFiles.size());
            Assert.AreEqual(commitedFiles.size(), repoFiles.size());
            Assert.AreEqual(commitedFiles.first(), repoFiles.first());            
        }

        [Test(Description = "Gets the list of files from the provided revCommit")]
        public void commit_Files()
        {
            //create two test files:
            // one on the repository root
            // another one on a sub folder
            var file1 = "file1_".add_RandomLetters();
            var dir2 = "dir_".add_RandomLetters();
            var file2 = dir2 + "// " +  "file2_".add_RandomLetters();            
            var fullPath1 = nGit.file_FullPath(file1);
            var fullPath2 = nGit.file_FullPath(file2);            
            var fileContents1 = 300.randomLetters();
            var fileContents2 = 300.randomLetters();

            Assert.IsFalse(file1.fileExists());
            Assert.IsFalse(file2.fileExists());

            fullPath2.parentFolder().createDir();
            nGit.file_Write(file1, fileContents1);
            nGit.file_Write(file2, fileContents2);

            Assert.IsTrue(fullPath1.fileExists());
            Assert.IsTrue(fullPath2.fileExists());
            Assert.AreEqual(fileContents1,fullPath1.fileContents());
            Assert.AreEqual(fileContents2, fullPath2.fileContents());

            //check that commit_Files returns both files
            var revCommit = nGit.add_and_Commit_using_Status();
            var commitFiles = revCommit.commit_Files(nGit);
            Assert.AreEqual(2, commitFiles.size());            
            Assert.AreEqual(file2.fixDoubleForwardSlash(), commitFiles.first());
            Assert.AreEqual(file1, commitFiles.second());          
        }

        [Test(Description = "Gets the fill path list of files from the provided revCommit")]
        public void commit_Files_FullPath()
        {
            var revCommit1          = nGit.add_and_Commit_Random_File();
            var fileAdded           = revCommit1.commit_Files(nGit);
            var fileAdded_FullPath  = revCommit1.commit_Files_FullPath(nGit);

            Assert.AreEqual(fileAdded.size(), 1);
            Assert.AreEqual(fileAdded.size(), fileAdded_FullPath.size());
            Assert.AreEqual(fileAdded_FullPath.first(), nGit.file_FullPath(fileAdded.first()));
            Assert.IsTrue  (fileAdded_FullPath.first().fileExists());
        }

        [Test(Description = "Gets the commits files indexed by the file SHA1")]
        public void commit_Files_IndexedBy_SHA1()
        {
            var file1 = "aaa.text";
            var file2 = "bbb.text";
            var contents1 = "12345";
            var contents2 = "abcdef";
            var sha1 = "bd41cba781d8349272bf3eb92568285b411c027c";
            var sha2 = "d96dc95707c20a371b14928ee42071f00e00b645";
            nGit.file_Create(file1, contents1);
            nGit.file_Create(file2, contents2);
            var recCommit = nGit.add_and_Commit_using_Status();
            var commitFiles = recCommit.commit_Files(nGit);
            Assert.AreEqual(2, commitFiles.size());
            Assert.AreEqual(commitFiles.first() , file1);
            Assert.AreEqual(commitFiles.second(), file2);
            var filesBySha1 = recCommit.commit_Files_IndexedBy_SHA1(nGit);
            Assert.AreEqual(2, filesBySha1.size());
            Assert.IsTrue(filesBySha1.keys().contains(sha1));
            Assert.IsTrue(filesBySha1.keys().contains(sha2));
            Assert.AreEqual(filesBySha1[sha1], file1);
            Assert.AreEqual(filesBySha1[sha2], file2);
            // filesBySha1.script_Me("files").waitForClose();

        }

        [Test(Description = "Returns the SAH1 value of the current commit (same as RevCommitName)")]
        public void sha1()
        {            
            new Test_RefLogs(this).sha1();
        }

        [Test(Description = "Gets a commit's author")]
        public void author()
        {
            var revCommit = nGit.add_and_Commit_Random_File();
            var author = revCommit.author();
            var committer = revCommit.author();
            
            Assert.AreEqual(author.name()               , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(author.email()              , NGit_Consts.DEFAULT_COMMIT_EMAIL);
            Assert.AreEqual(committer.name()            , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(committer.email()           , NGit_Consts.DEFAULT_COMMIT_EMAIL);

            Assert.AreEqual(revCommit.author_Name()     , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(revCommit.author_Email()    , NGit_Consts.DEFAULT_COMMIT_EMAIL);
            Assert.AreEqual(revCommit.committer_Name()  , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(revCommit.committer_Email() , NGit_Consts.DEFAULT_COMMIT_EMAIL);

            //check null values
            Assert.IsNull((null as RevCommit).author());
            Assert.IsNull((null as RevCommit).committer());            
        }

        [Test(Description = "Gets a commit's committer")]
        public void committer()
        {
            author();
        }

        [Test(Description = "Gets the DateTime of the commit (from the Committer value)")]
        public void when()
        {
            var now = DateTime.Now;
            var revCommit = nGit.add_and_Commit_Random_File();
            var when1 = revCommit.when();
            var when2 = revCommit.committer().when();

            Assert.AreNotEqual(when1, default(DateTime));
            Assert.AreEqual   (when1, when2);
            Assert.AreEqual   (when1.ToLongDateString(), now.ToLongDateString());

            //Check nulls
            Assert.IsNull  ((null as PersonIdent).name());
            Assert.IsNull  ((null as PersonIdent).email());
            Assert.AreEqual((null as PersonIdent).when(), default(DateTime));
        }

        [Test]
        public void Commit_Specific_Author_or_Committer_via_Global_Setting()
        {                        
            var name1 = 10.randomLetters();
            var email1 = 10.randomLetters();
            var name2 = 10.randomLetters();
            var email2 = 10.randomLetters();

            Assert.AreNotEqual(name1,name2);
            Assert.AreNotEqual(email1, email2);

            //custom author, default commmitter
            var person1 = name1.personIdent(email1);
            nGit.Author = person1;

            var revCommit1 = nGit.add_and_Commit_Random_File();

            var author1 = revCommit1.GetAuthorIdent();
            Assert.AreEqual(author1.GetName(), name1);
            Assert.AreEqual(author1.GetEmailAddress(), email1);
            var committer1 = revCommit1.GetCommitterIdent();
            Assert.AreEqual(committer1.GetName(), NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual( committer1.GetEmailAddress(), NGit_Consts.DEFAULT_COMMIT_EMAIL);

            //custom author, custom commmitter

            var person2 = name2.personIdent(email2);
            nGit.Committer = person2;

            var revCommit = nGit.add_and_Commit_Random_File();

            var author = revCommit.GetAuthorIdent();
            Assert.AreEqual(author.GetName(), name1);
            Assert.AreEqual(author.GetEmailAddress(), email1);
            var committer = revCommit.GetCommitterIdent();
            Assert.AreEqual(committer.GetName(), name2);
            Assert.AreEqual(committer.GetEmailAddress(), email2);
        }

        [Test]
        public void Commit_Specific_Author_or_Committer_via_Commit_Setting()
        {
            //test data
            var name1 = 10.randomLetters();
            var email1 = 10.randomLetters();
            var message1 = 10.randomLetters();
            var name2A = 10.randomLetters();
            var email2A = 10.randomLetters();
            var name2B = 10.randomLetters();
            var email2B = 10.randomLetters();
            var message2 = 10.randomLetters();            

            //create commits
            nGit.file_Create_Random_File();
            var revCommit1 = nGit.commit(message1, name1, email1);
            nGit.file_Create_Random_File();
            var revCommit2 = nGit.commit(message2, name2A.personIdent(email2A), name2B.personIdent(email2B));
            var revCommit3 = nGit.add_and_Commit_Random_File();
            
            //get commit values
            var author1     = revCommit1.GetAuthorIdent();
            var committer1  = revCommit1.GetCommitterIdent();
            var author2     = revCommit2.GetAuthorIdent();
            var committer2  = revCommit2.GetCommitterIdent();
            var author3     = revCommit3.GetAuthorIdent();
            var committer3  = revCommit3.GetCommitterIdent();

            //check values

            Assert.AreEqual(author1   .GetName()        , name1);
            Assert.AreEqual(author1   .GetEmailAddress(), email1);            
            Assert.AreEqual(committer1.GetName()        , name1);
            Assert.AreEqual(committer1.GetEmailAddress(), email1);
            Assert.AreEqual(revCommit1.GetShortMessage(), message1);

            Assert.AreEqual(author2   .GetName()        , name2A);
            Assert.AreEqual(author2   .GetEmailAddress(), email2A);
            Assert.AreEqual(committer2.GetName()        , name2B);
            Assert.AreEqual(committer2.GetEmailAddress(), email2B);
            Assert.AreEqual(revCommit2.GetShortMessage(), message2);

            Assert.AreEqual(author3.GetName()           , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(author3.GetEmailAddress()   , NGit_Consts.DEFAULT_COMMIT_EMAIL);
            Assert.AreEqual(committer3.GetName()        , NGit_Consts.DEFAULT_COMMIT_NAME);
            Assert.AreEqual(committer3.GetEmailAddress(), NGit_Consts.DEFAULT_COMMIT_EMAIL);
        }
    }
}
