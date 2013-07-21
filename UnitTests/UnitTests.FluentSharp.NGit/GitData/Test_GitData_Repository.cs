using System;
using FluentSharp.CoreLib;
using FluentSharp.Git;
using FluentSharp.Git.APIs;
using FluentSharp.Git.Utils;
using FluentSharp.REPL;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_NGit.GitData
{

    [TestFixture]
    public class Test_GitData_Repository : Temp_Clone_O2_Platform_Scripts
    {
        /*[Test]
        public void Tes_gitData_Repository() 
        {
            var path = @"E:\O2\_Source_Code\FluentSharp_Fork.WatiN";
            path = @"E:\O2\O2.Platform.Scripts";
            path = @"E:\TeamMentor\TM_Dev Repos\TM_DEV";
//            path = @"E:\TeamMentor\TM_Clients\Master - Site_www.teammentor.net\Library_Data\XmlDatabase\TM_Libraries_WWW\.Net 2.0";
            var nGit = path.git_Open();
            var start = DateTime.Now;
            var gitDataRepository = nGit.gitData_Repository(true);
            gitDataRepository.script_Me_WaitForClose();
            return;
            var time1 = (DateTime.Now - start).str();
            var xml = gitDataRepository.toXml();
            var time2 = (DateTime.Now - start).str();
            xml.size().mBytesStr().info();
            time1.info();
            time2.info();
            "size: {0}, took: {1} | {2} ".info(xml.size().mBytesStr(), time1, time2);
            xml.saveWithExtension(".txt").startProcess();
        }*/
        [Test]
        public void gitData_Repository()
        {
            //typeof(API_NGit).assembly().Location.script_Me("location").waitForClose();
            //new API_NGit().script_Me().waitForClose();
            //return;
            var start= DateTime.Now;
            var gitData_Repository  = nGit.gitData_Repository();

            "Mapping took: {0}".info(start.duration_To_Now());
            //var currentBranch       = gitData_Repository.CurrentBranch;
            var firstFile           = gitData_Repository.files().first();
            var firstCommit         = gitData_Repository.commits().first();

            Assert.IsNotNull  (gitData_Repository);
            Assert.IsNotNull  (gitData_Repository.Config.LocalPath);
            Assert.IsTrue     (gitData_Repository.Config.LocalPath.dirExists());

            //Current Branch
            Assert.IsNotNull  (gitData_Repository);
            Assert.IsNotNull  (gitData_Repository.CurrentBranch);
            Assert.AreNotEqual(gitData_Repository.HEAD, NGit_Consts.EMPTY_SHA1);
            Assert.IsNotEmpty (gitData_Repository.Commits);
            Assert.IsNotEmpty (gitData_Repository.Files);

            
            //Files
            Assert.IsNotNull  (firstFile);
//            Assert.IsTrue     (firstFile.FilePath.valid());
//            Assert.AreEqual   (firstFile.FilePath, ".gitignore");
//            Assert.AreNotEqual(firstFile.Sha1    , NGit_Consts.EMPTY_SHA1);

            //Commits 
            Assert.IsNotNull  (firstCommit);            
            Assert.IsNotNull  (firstCommit.Author);            
            Assert.IsNotNull  (firstCommit.Committer);
            Assert.IsNotNull  (firstCommit.Message);
            Assert.IsNotNull  (firstCommit.Sha1);
            Assert.IsNotNull  (firstCommit.When);
            Assert.IsNotNull  (firstCommit.Tree);

            Assert.IsEmpty    (firstCommit.Tree); //by default don't map commit files

            
            //Test null handing
            Assert.IsNull   ((null as API_NGit).gitData_Repository());
            Assert.IsNull   (new API_NGit().gitData_Repository());            
        }

        [Test]
        public void gitData_Repository_Via_Path()
        {
            var gitData = repoPath.gitData_Repository();
            Assert.IsNotNull(gitData);            
            Assert.IsNotEmpty(gitData.Commits);
        }
        [Test]
        public void gitData_Files()
        {
            var files = nGit.gitData_Files();
            var filesChecked = 0;
            foreach(var file in files.take(50))
            {
            //var firstFile = files.first(); 
                var objectLoader = nGit.open_Object(file.Sha1);
                var bytes = objectLoader.bytes();
                var ascii = bytes.ascii();
                Assert.IsNotNull(bytes);
                Assert.IsNotEmpty(bytes);
                Assert.IsTrue(ascii.valid());
                var fullPath = nGit.file_FullPath(file.FilePath);
                Assert.IsTrue(fullPath.fileExists());
                if (file.FilePath.extension(".h2"))
                {
                    Assert.AreEqual(ascii.fix_CRLF(), nGit.file_FullPath(file.FilePath).contents(false).fix_CRLF());
                    filesChecked++;
                }
            }
            Assert.Less(10,filesChecked);
            //Test null handing
            Assert.IsEmpty((null as API_NGit).gitData_Files());
            Assert.IsEmpty(nGit.gitData_Files(-1,null));
        }

        [Test]
        public void gitData_Braches()
        {
            
            var branches_Via_NGit   = nGit.branches();
            var gitData_Repository  = nGit.gitData_Repository();
            var branches            = gitData_Repository.branches();

            Assert.Less    (2,branches_Via_NGit.size());
            Assert.AreEqual(branches_Via_NGit.size(), branches.size());
            Assert.AreEqual(branches_Via_NGit.first(), branches.first().Name.simple_BranchName());            
        }
        [Test]
        public void gitData_File_Commits()
        {                        
            var file = @"3rdParty/Clojure/API_Clojure.cs";
            var fileCommits = file.file_Commits(nGit);
            
            Assert.IsNotEmpty (fileCommits);
            Assert.AreEqual   (3, fileCommits.size());
            Assert.AreNotEqual(fileCommits[0].FileContents, fileCommits[1].FileContents);
            Assert.AreNotEqual(fileCommits[0].FileContents, fileCommits[2].FileContents);            
            return; 
            /*           var files = nGit.files();
            foreach(var file in files)
            {
                "mapping file: {0}".info(file);
                var firstFile = files.item(12);
                var fileCommits = firstFile.file_Commits(nGit);
                Assert.IsNotEmpty(fileCommits);
                if (fileCommits.size() > 1)
                {
                    fileCommits.showInfo();
                    "continue".alert();
                    return;
                }
            }*/
            Assert.Fail();
        }

        [Test]
        public void Max_CommitsToShow_Max_FilesToShow()
        {
            var gitData_Repository1  = nGit.gitData_Repository();
            var commits1             = gitData_Repository1.Commits;
            var commit1_Size         = commits1.size();
            var maxFilesToShow       = 100.random();
            var gitData_Repository2  = nGit.gitData_Repository();
            Assert.Less  (0, commit1_Size);
        }


        [Test]
        public void Open_GitData_In_REPL_And_Code_Viewer()
        {
            var gitData_Repository  = nGit.gitData_Repository(false);
            gitData_Repository.Config.Max_CommitsToShow = 5;
            gitData_Repository.Config.Max_FilesToShow = 40;
            gitData_Repository.loadData();
            gitData_Repository.map_File_Commits();
            var topPanel = "Git Data".popupWindow();
            var codeViewer = topPanel.add_SourceCodeViewer();
            
            var toXml = gitData_Repository.toXml();

            codeViewer.set_Text(toXml, ".xml");
            gitData_Repository.script_Me(topPanel.insert_Below())
                              .add_InvocationParameter("codeViewer",codeViewer)
                              .add_InvocationParameter("nGit",nGit)
                              .code_Append("//using FluentSharp.Git");

            topPanel.parentForm().close();
            //var toXml = gitData_Repository.json_Serialize();
            //"Size: {0}".info(toXml.Length.kBytesStr());
            /*
            //gitData_Repository.script_Me_WaitForClose();
            Assert.IsTrue(toXml.valid());
            toXml.showInCodeViewer().waitForClose();*/
        }
    }
    public class Test_GitData_Ctors
    {
        [Test]
        public void GitData_Repository_Ctor()
        {
            var gitRepository = new GitData_Repository();
            var config        = gitRepository.Config;
            Assert.IsNotNull (gitRepository);
            Assert.IsNotNull (config);
            Assert.IsNull    (config.LocalPath);            
            Assert.Greater   (0,config.Max_CommitsToShow);        
            Assert.Greater   (0,config.Max_FilesToShow);            
            Assert.IsNotNull(gitRepository.Files);
            Assert.IsEmpty  (gitRepository.Files);
            Assert.IsNotNull(gitRepository.HEAD);
            Assert.AreEqual (gitRepository.HEAD, NGit_Consts.EMPTY_SHA1);
            Assert.IsNotNull(gitRepository.Commits);
            Assert.IsEmpty  (gitRepository.Commits);
        }
        [Test]
        public void GitData_Branch_Ctor()
        {
            var gitBranch = new GitData_Branch();
            Assert.IsNotNull(gitBranch);        
        }
        [Test]
        public void GitData_Commit_Ctor()
        {
            var gitCommit = new GitData_Commit();
            Assert.IsNotNull(gitCommit);
            Assert.IsNotNull(gitCommit.Tree);  
            Assert.IsEmpty  (gitCommit.Tree);  
        }
    }
    
}
