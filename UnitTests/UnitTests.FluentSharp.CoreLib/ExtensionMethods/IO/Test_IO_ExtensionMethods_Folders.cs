using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.O2_DotNetWrappers.Windows;
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
        private string temporaryFolderPath = null;
        private string randomFilePath = null;
        private string driveLetter = "C";
        private string rootDrive = null;

        [TestFixtureSetUp]
        public void Initialize()
        {
            if (clr.mono())
                "ignoring on mono".assert_Ignore();
            rootDrive = @"{0}:\".format(driveLetter);
            if (temporaryFolderPath.notValid())
            {
                var randomPath = Path.GetRandomFileName().Replace(".", "");
                temporaryFolderPath = string.Format(@"{0}{1}", rootDrive, randomPath);
            }

            if (temporaryFolderPath.folder_Not_Exists())
            {
                // It needs to create directory with specific rights in order to able to delete it after test execution
                DirectorySecurity securityRules = new DirectorySecurity();
                securityRules.AddAccessRule(new FileSystemAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
                Directory.CreateDirectory(temporaryFolderPath, securityRules);
            }

            if (randomFilePath.notValid())
            {
                randomFilePath = Path.GetRandomFileName();
            }
        }
        [TestFixtureTearDown]
        public void CleanUp()
        {
            Files.delete_Folder_Recursively(temporaryFolderPath);
        }

        [Test]
        public void folderName()
        {
            temporaryFolderPath.folderName().assert_Not_Null();
            randomFilePath.folderName().assert_Is_Null();
            "".folderName().assert_Is_Null();
            (null as string).folderName().assert_Is_Null();
            var folderName = temporaryFolderPath.folderName();
            rootDrive.pathCombine(folderName).folderName().assert_Is(folderName);
        }

        [Test]
        public void parentFolder()
        {
            temporaryFolderPath.parent_Folder().assert_Is_Not_Null();
            randomFilePath.parent_Folder().assert_Is_Empty();
            "".parent_Folder().assert_Is_Null();
            (null as string).parent_Folder().assert_Is_Null();
            temporaryFolderPath.parent_Folder().assert_Is(rootDrive);
        }

        [Test]
        public void parentFolder_Open_in_Explorer()
        {
            temporaryFolderPath.parentFolder_Open_in_Explorer().assert_Is_Not_Null();
            var openWindowsWithText = Window.FindWindowsWithText(driveLetter + ":"); 
            if(!openWindowsWithText.empty())
            {
                openWindowsWithText.forEach(x => Window.CloseWindow(x));
            }
            "".parentFolder_Open_in_Explorer().assert_Is_Null();
            (null as string).parentFolder_Open_in_Explorer().assert_Is_Null();
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
            rootDrive.folders(true).count().assert_Is_Not_Equal_To(0);
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
            MethodBase method = MethodBase.GetCurrentMethod();
            UnitTestMethodReferenceAttribute attr = (UnitTestMethodReferenceAttribute)method.GetCustomAttributes(typeof(UnitTestMethodReferenceAttribute), true)[0];
            attr.MethodToInvoke.invoke();
        }

        
        [Test]
        public void directoryName()
        {
            (null as string).directoryName().assert_Is_Empty();
            "".directoryName().assert_Is_Empty();
            temporaryFolderPath.directoryName().assert_Is_Equal_To(temporaryFolderPath.parent_Folder());
            rootDrive.directoryName().assert_Is_Null();
        }

        [Test]
        public void isFolder()
        {
            (null as string).isFolder().assert_Is_Not_True();
            "".isFolder().assert_Is_Not_True();
            temporaryFolderPath.isFolder().assert_Is_True();
            rootDrive.isFolder().assert_Is_True();
            randomFilePath.isFolder().assert_Is_Not_True();
        }

        [Test]
        public void folder_Create_File()
        {
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
            rootDrive.folder_Exists().assert_Is_True();
        }

        [Test]
        public void folder_Not_Exists()
        {
            (null as string).folder_Not_Exists().assert_Is_True();
            "".folder_Not_Exists().assert_Is_True();
            temporaryFolderPath.folder_Not_Exists().assert_Is_False();
            randomFilePath.folder_Delete_Files().assert_Is_False();
            temporaryFolderPath.pathCombine("something").folder_Not_Exists().assert_Is_True();
            rootDrive.folder_Not_Exists().assert_Is_False();
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
            "".directoryInfo().assert_Is_Null();
            (null as string).directoryInfo().assert_Is_Null();
            temporaryFolderPath.directoryInfo().assert_Is_Not_Null();
            rootDrive.directoryInfo().assert_Is_Not_Null();
            randomFilePath.directoryInfo().assert_Is_Null();
            var directoryInfo = rootDrive.directoryInfo();
            directoryInfo.Parent.assert_Is_Null();
            directoryInfo.Name.assert_Is_Equal_To(@"{0}:\".format(driveLetter));
            directoryInfo.GetFiles().count().assert_Is_Greater(0);
        }

        [Test]
        public void mapPath()
        {
            temporaryFolderPath.mapPath(randomFilePath).assert_Is_Not_Null();
            driveLetter.mapPath(randomFilePath).assert_Is_Not_Null();
            "".mapPath(randomFilePath).assert_Is_Null();
            (null as string).mapPath(randomFilePath).assert_Is_Null();
            randomFilePath.mapPath(randomFilePath).assert_Is_Null();
            randomFilePath.mapPath(driveLetter).assert_Is_Null();
            @"C:\".mapPath(@"\".add_5_RandomLetters()).assert_Is_Not_Null();

        }

        [Test]
        public void temp_Folder()
        {
            temporaryFolderPath.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            rootDrive.temp_Folder().assert_Is_Not_Null().folder_Delete().assert_Is_True();
            (null as string).temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
            ("").temp_Folder().assert_Equal(PublicDI.config.O2TempDir);
            "temp".temp_Folder(false).assert_Equal(Path.Combine(PublicDI.config.O2TempDir,"temp")).folder_Delete().assert_Is_True();
            // Combining two paths ( e.g path1 = "C:\temp1" , path2 = "C:\temp2" using Path.Combine will return path2
            @"C:\te".temp_Folder(false).dirExists().assert_Is_True();
            @"C:\te".folder_Delete().assert_Is_True();
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