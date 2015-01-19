using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;

namespace UnitTests.FluentSharp.CoreLib
{
    [TestFixture]
    public class Test_IO_ExtensionMethods_Folders : NUnitTests
    {
        public string tempDir;
        public string temporaryFolderPath = null;          

        [TestFixtureSetUp]
        public void Initialize()
        {
            if (clr.mono())
                "ignoring on mono".assert_Ignore();

            tempDir = PublicDI.config.O2TempDir;
            temporaryFolderPath = "_test_folders_".temp_Dir();            
        }
        [TestFixtureTearDown]
        public void CleanUp()
        {
            Files.delete_Folder_Recursively(temporaryFolderPath);
        }

        [Test]
        public void folderName()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());         
            temporaryFolderPath.folderName().assert_Not_Null();
            randomFilePath.folderName().assert_Is_Null();
            "".folderName().assert_Is_Null();
            (null as string).folderName().assert_Is_Null();
            var folderName = temporaryFolderPath.folderName();
            
            tempDir.path_Combine(folderName).assert_Is(temporaryFolderPath);            
        }

        [Test]
        public void parentFolder()
        {
            var childFolder = temporaryFolderPath.path_Combine("an_child").folder_Create();
            childFolder.parent_Folder().assert_Is_Not_Null();
            childFolder.parent_Folder().assert_Is(temporaryFolderPath);

            "".parent_Folder().assert_Is_Null();
            (null as string).parent_Folder().assert_Is_Null();
            temporaryFolderPath.parent_Folder().assert_Is(tempDir);
        }

        [Test]
        public void parentFolder_Open_in_Explorer()
        {
            var tmpDir = "_open_In_explorer".tempDir();
            tmpDir.parentFolder_Open_in_Explorer().assert_Is_Not_Null();
            var windowTitle = tmpDir.parent_Folder().folderName();
            windowTitle.error();
            var window = windowTitle.win32_Desktop_Window_With_Title();            
            window.assert_Not_Default()
                  .win32_CloseWindow().assert_True();
            
            "".parentFolder_Open_in_Explorer().assert_Is_Null();
            (null as string).parentFolder_Open_in_Explorer().assert_Is_Null();
            10.randomLetters().win32_Desktop_Window_With_Title().assert_Is_Default();
        }

        [Test]
        public void folder()
        {
            temporaryFolderPath.folder("".add_5_RandomLetters()).assert_Is_Null();
            var parentFolder = temporaryFolderPath.parent_Folder();
            var folderName = temporaryFolderPath.folderName();
            parentFolder.folder(folderName).assert_Is_Equal_To(temporaryFolderPath);
            (null as string).folder(folderName).assert_Is_Null();
        }

        [Test]
        public void folders()
        {
            temporaryFolderPath.folders().count().assert_Is_Equal_To(0);
            "".folders().count().assert_Is_Equal_To(0);
            (null as string).folders().count().assert_Is_Equal_To(0);
            temporaryFolderPath.folder_Create_File("test.txt", "Lorem ipsum dolor").parent_Folder().folders().count().assert_Is_Equal_To(0);
            temporaryFolderPath.folder_Delete_Files();
            temporaryFolderPath.folders().count().assert_Is_Equal_To(0);
            temporaryFolderPath.mapPath("test").create_Folder().parent_Folder().folders().count().assert_Is_Equal_To(1);
            temporaryFolderPath.folders()[0].folder_Delete().assert_Is_True();
            
            temporaryFolderPath.folders(true).count().assert_Is_Equal_To(0);
            "".folders(true).count().assert_Is_Equal_To(0);
            (null as string).folders(true).count().assert_Is_Equal_To(0);
            tempDir.folders(true).assert_Not_Empty();            
            temporaryFolderPath.mapPath("test").create_Folder()
                               .mapPath("test2").create_Folder()
                               .parent_Folder().parent_Folder().folders(true).count().assert_Is_Equal_To(2);

        }

        [Test]
        public void createDir()
        {
            (null as string).createDir().assert_Is_Null();
            "".createDir().assert_Is_Null();
            "test_createDir".createDir().assert_Is_Equal_To("test_createDir");
            "test_createDir".delete_Folder().assert_True();
        }

        [Test]
        public void create_Folder()
        {
            (null as string).create_Folder().assert_Is_Null();
            "".create_Folder().assert_Is_Null();
            "test_createDir".create_Folder().assert_Is_Equal_To("test_createDir");
            "test_createDir".delete_Folder().assert_True();
        }
        [Test]
        [UnitTestMethodReference("create_Folder")]
        public void createFolder()
        {
            MethodBase.GetCurrentMethod()
                      .attributes_Custom<UnitTestMethodReferenceAttribute>().first()
                      .MethodToInvoke
                      .invoke_No_Catch(this);
        }

        
        [Test]
        public void directoryName()
        {
            (null as string).directoryName().assert_Is_Empty();
            "".directoryName().assert_Is_Empty();
            temporaryFolderPath.directoryName().assert_Is_Equal_To(temporaryFolderPath.parent_Folder());
            "C:\\".directoryName().assert_Is_Null();
        }

        [Test]
        public void isFolder()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            (null as string).isFolder().assert_Is_Not_True();
            "".isFolder().assert_Is_Not_True();
            temporaryFolderPath.isFolder().assert_Is_True();
            tempDir.isFolder().assert_Is_True();
            randomFilePath.isFolder().assert_Is_Not_True();
        }

        [Test]
        public void folder_Create_File()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            var filename = "text.txt";
            var content = "Lorem ipsum dolor";
            temporaryFolderPath.folder_Create_File(filename, content).file_Exists().assert_Is_True();
            temporaryFolderPath.pathCombine(filename).file_Contents().assert_Is(content);
            "".folder_Create_File(filename, content).assert_Is_Null();
            (null as string).folder_Create_File(filename, content).assert_Is_Null();
            randomFilePath.folder_Create_File(filename, content).assert_Is_Null();
            //cleanup
            temporaryFolderPath.folder_Delete_Files().assert_Is_True();
        }

        [Test]
        public void folder_Delete_Files()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            var filename = "text.txt";
            var content = "Lorem ipsum dolor";

            (null as string).folder_Delete_Files().assert_Is_False();
            "".folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.folder_Create_File(filename, content).fileExists().assert_Is_True();
            temporaryFolderPath.folder_Delete_Files().assert_Is_True();
            randomFilePath.file_Not_Exists().assert_Is_True();
            randomFilePath.folder_Delete_Files().assert_Is_False();
        }

        [Test]
        public void folder_Exists()
        {
            (null as string).folder_Exists().assert_Is_False();
            "".folder_Exists().assert_Is_False();
            temporaryFolderPath.folder_Exists().assert_Is_True();
            var newFolder = temporaryFolderPath.mapPath("newFolder").create_Folder();
            newFolder.folder_Exists().assert_Is_True();
            newFolder.delete_Folder().assert_Is_True();
            tempDir  .folder_Exists().assert_Is_True();
        }

        [Test]
        public void folder_Not_Exists()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            (null as string).folder_Not_Exists().assert_Is_True();
            "".folder_Not_Exists().assert_Is_True();
            temporaryFolderPath.folder_Not_Exists().assert_Is_False();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.pathCombine("something").folder_Not_Exists().assert_Is_True();
            tempDir .folder_Not_Exists().assert_Is_False();
        }

        [Test]
        [UnitTestMethodReference("folder_Exists")]
        public void folderExists()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            UnitTestMethodReferenceAttribute attr = (UnitTestMethodReferenceAttribute)method.GetCustomAttributes(typeof(UnitTestMethodReferenceAttribute), true)[0];
            attr.MethodToInvoke.invoke();
        }

        [Test]
        [UnitTestMethodReference("folder_Exists")]
        public void dirExists()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            UnitTestMethodReferenceAttribute attr = (UnitTestMethodReferenceAttribute)method.GetCustomAttributes(typeof(UnitTestMethodReferenceAttribute), true)[0];
            attr.MethodToInvoke.invoke();
        }

        [Test]
        public void dirs()
        {
            temporaryFolderPath.dirs().count().assert_Is_Equal_To(0);
            "".dirs().count().assert_Is_Equal_To(0);
            (null as string).dirs().count().assert_Is_Equal_To(0);
            temporaryFolderPath.folder_Create_File("test.txt", "Lorem ipsum dolor").parent_Folder().dirs().count().assert_Is_Equal_To(0);
            temporaryFolderPath.folder_Delete_Files();
            temporaryFolderPath.dirs().count().assert_Is_Equal_To(0);
            temporaryFolderPath.mapPath("test").create_Folder().parent_Folder().dirs().count().assert_Is_Equal_To(1);
            temporaryFolderPath.dirs()[0].folder_Delete().assert_Is_True();
        }

        [Test]
        public void directoryInfo()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            "".directoryInfo().assert_Is_Null();
            (null as string).directoryInfo().assert_Is_Null();
            temporaryFolderPath.directoryInfo().assert_Is_Not_Null();
            tempDir.directoryInfo().assert_Is_Not_Null();
            randomFilePath.directoryInfo().assert_Is_Null();
            var directoryInfo = temporaryFolderPath.directoryInfo();
            directoryInfo.Parent.assert_Is_Not_Null();
            directoryInfo.Name.assert_Is_Equal_To(temporaryFolderPath.folder_Name());
            temporaryFolderPath.path_Combine("a.txt").write_To_File("some text");
            directoryInfo.GetFiles().count().assert_Is_Greater(0);
        }

        [Test]
        public void mapPath()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            temporaryFolderPath.mapPath(randomFilePath).assert_Is_Not_Null();
            tempDir.mapPath(randomFilePath).assert_Is_Not_Null();
            "".mapPath(randomFilePath).assert_Is_Null();
            (null as string).mapPath(randomFilePath).assert_Is_Null();
            
            randomFilePath.mapPath(tempDir).assert_Is_Null();
            @"C:\".mapPath(@"\".add_5_RandomLetters()).assert_Is_Not_Null();

            randomFilePath.mapPath(randomFilePath).assert_Is(randomFilePath); // this is a weird behaviour, but it is how it works for the base PathCombine .NET method

        }

        [Test]
        public void temp_Folder()
        {
            var randomFilePath = temporaryFolderPath.path_Combine("_temp_file".add_5_RandomLetters());
            temporaryFolderPath.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            tempDir.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            (null as string).temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
            ("").temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
            "temp".temp_Folder(false).assert_Equal(Path.Combine(PublicDI.config.O2TempDir,"temp")).folder_Delete().assert_Is_True();
            // Combining two paths ( e.g path1 = "C:\temp1" , path2 = "C:\temp2" using Path.Combine will return path2
            "te".temp_Folder(false).dirExists().assert_Is_True().info();
            "te".temp_Folder(false).folder_Delete().assert_Is_True(); 
            randomFilePath.temp_Folder(false).assert_Equal(Path.Combine(PublicDI.config.O2TempDir,randomFilePath));
            randomFilePath.temp_Folder(false).folder_Delete().assert_Is_True();


        }

        [Test]
        [UnitTestMethodReference("temp_Folder")]
        public void temp_Dir()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            UnitTestMethodReferenceAttribute attr = (UnitTestMethodReferenceAttribute)method.GetCustomAttributes(typeof(UnitTestMethodReferenceAttribute), true)[0];
            attr.MethodToInvoke.invoke();
        }

        
    }
}