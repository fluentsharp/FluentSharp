
using System;
using System.Collections.Generic;
using System.IO;
using FluentSharp.CoreLib;
using NUnit.Framework;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;

namespace UnitTests.FluentSharp_CoreLib
{
    [TestFixture]
    public class Test_IO
    {
        public string TempFile1 { get; set; }
        public string TempFile2 { get; set; }
        [SetUp]
        public void setup()
        {
            TempFile1 = "tempFile".tempFile();
            TempFile1 = "tempFile".tempFile();
            Assert.AreNotEqual(TempFile1,TempFile2);
            Assert.IsFalse(TempFile1.fileExists());
            Assert.IsFalse(TempFile2.fileExists());

        }

        [TearDown]
        public void tearDown()
        {
            TempFile1.file_Delete();
            TempFile2.file_Delete();
            Assert.IsFalse(TempFile1.fileExists());
            Assert.IsFalse(TempFile2.fileExists());
        }


        //IO_ExtensionMethods_FileInfo
        [Test(Description = "Returns true if a file contains the char 0 (usually found on binary files)")]
        public void isBinaryFormat()
        {
            var result_TextFile      = "test".save().isBinaryFormat();
            var result_TextWithChar0 = "aaa\0aaa".saveAs(TempFile1).isBinaryFormat();
            var result_Assembly      = typeof (string).assembly_Location().isBinaryFormat();

            Assert.IsFalse(result_TextFile);
            Assert.IsTrue (result_TextWithChar0);
            Assert.IsTrue (result_Assembly);            
        }

        [Test(Description = "Returns a FileInfo object for the provided file")]
        public void fileInfo()
        {
            var testContent = "testContent".add_RandomLetters(200);
            testContent.saveAs(TempFile1);
            var fileInfo = TempFile1.fileInfo();
            Assert.IsNotNull(fileInfo);
            Assert.IsInstanceOf<FileInfo>(fileInfo);
            Assert.AreEqual(TempFile1,fileInfo.FullName);            
            Assert.AreEqual(testContent.size(),fileInfo.size());            
            Assert.IsTrue  (fileInfo.Exists);

            var nonExistingFile = 50.randomLetters().fileInfo();
            Assert.IsNotNull(nonExistingFile);
            Assert.IsFalse  (nonExistingFile.Exists);
            //test catch
            
            Assert.IsNull((null as string).fileInfo());
        }

        [Test(Description = "Gets the attributes of the current file")]
        public void file_Attributes()
        {
            fileInfo();
            var fileAttributes1  = TempFile1.fileInfo().Attributes;
            var fileAttributes2  = TempFile1.file_Attributes();
            Assert.AreEqual(fileAttributes1, fileAttributes2);

            Assert.AreEqual(default(FileAttributes), (null as string).file_Attributes());
        }

        [Test(Description = "Adds the ReadOnly attribute from a particular file")]
        public void readOnly_Add()
        {
			if(clr.mono())
				"ignoring on mono".assert_Ignore();
            var testContent = "testContent".add_RandomLetters(200);
            testContent.saveAs(TempFile1);
            var fileInfo = TempFile1.fileInfo();
            var attributes = fileInfo.attributes();

            Assert.IsTrue (attributes.str().contains("Archive"));
            Assert.IsTrue (TempFile1.file_Has_Attribute(FileAttributes.Archive));
            Assert.IsFalse(TempFile1.file_Has_Attribute(FileAttributes.ReadOnly));

            fileInfo.readOnly_Add();
            Assert.IsTrue(TempFile1.file_Has_Attribute(FileAttributes.ReadOnly));
            var deleteResult = fileInfo.path().file_Delete();
            Assert.IsFalse(deleteResult);
            fileInfo.readOnly_Remove();
            Assert.IsFalse(TempFile1.file_Has_Attribute(FileAttributes.ReadOnly));
        }

        [Test(Description = "Removes the ReadOnly attribute from a particular file")]
        public void readOnly_Remove()
        {
            readOnly_Add();
        }

        [Test(Description = "Returns true if the provided file has a particular attribute")]
        public void file_Has_Attribute()
        {
            //readOnly_Add();
            Assert.IsFalse((null as string).file_Has_Attribute(FileAttributes.Archive));
        }

        [Test(Description = "Return files from Folder")]
        public void files()
        {
            var tempFolder = "TempFolder".tempDir();
            var file1 = tempFolder.pathCombine("aaa.js");
            var file2 = tempFolder.pathCombine("aaa.cs");
            var file3 = tempFolder.pathCombine("bbb.cs");

            Assert.IsFalse(file1.exists());
            Assert.IsFalse(file2.exists());
            Assert.IsFalse(file3.exists());

            "aaaa".saveAs(file1);
            "bbbb".saveAs(file2);
            "cccc".saveAs(file3);

            Assert.IsTrue(file1.exists());
            Assert.IsTrue(file2.exists());
            Assert.IsTrue(file3.exists());

            var folders = new List<string>() { tempFolder };

            var files = folders.files();
            Assert.IsNotEmpty(files);

            //Assert.AreEqual(files.contains(file1));
            //Assert.AreEqual(files.second(),file3);
            //Assert.AreEqual(files.first(),file2);




        }

        [Test(Description = "Gets the attribues of the provided FileInfo object")]
        public void attributes()
        {
            //readOnly_Add();
            
            var attributes1 = TempFile2.fileInfo().attributes();

            Assert.IsFalse(TempFile2.fileExists());
            Assert.AreEqual(attributes1, default(FileAttributes));
        }

        [Test(Description = "Returns the path of the current FileInfo")]
        public void path()
        {
            //readOnly_Add();
            Assert.IsNull((null as string).fileInfo().path());
        }

        [Test(Description = "Combine two paths (usually a folder and a filename")]
        public void pathCombine()
        {
			var folder1 = clr.mono() ? "/Users/aaa" : @"C:\aaa";
            var file1 = "text.txt";
			var expected1 = clr.mono() ? "/Users/aaa/text.txt" : @"C:\aaa\text.txt";

            Assert.AreEqual(folder1.pathCombine(file1), expected1);

            var largeFile = 255.randomLetters();
            var largePath = folder1.pathCombine(largeFile);
            Assert.Greater(255, largePath.size());

            
			if (clr.not_Mono()) //should not handle bad chars
			{
            	var unsafeString = 40.randomString() + ":*\aaa";
            	Assert.Throws<ArgumentException>(()=> folder1.pathCombine(unsafeString));
			}

            var bigFolder1 = @"C:\aaa".add_RandomLetters(250);

            Assert.Less(250, bigFolder1.size());
            //Assert.Throws<ArgumentException>(()=> folder1.pathCombine(unsafeString));
            Assert.Throws<Exception>(() => bigFolder1.pathCombine(file1));


        }

        [Test(Description = "Removes the readonly attribute from one or more files")]
        public void files_Attribute_ReadOnly_Remove()
        {
            "aaa".saveAs(TempFile1)
                 .file_Attribute_ReadOnly_Add();
            "bbb".saveAs(TempFile2)
                .file_Attribute_ReadOnly_Add();

            var files = new [] {TempFile1, TempFile2}.toList();
            files.files_Attribute_ReadOnly_Remove();
        }


        //IO_ExtensionMethods_Delete_or_Copy
        [Test(Description = "Deletes a file")]
        public void file_Delete()
        {
            "aaaa".saveAs(TempFile1);
            TempFile1.file_Attribute_ReadOnly_Add();
            var deleteResult_ReadOnly = TempFile1.file_Delete();
            TempFile1.file_Attribute_ReadOnly_Remove();
            var deleteResult_NotReadOnly = TempFile1.file_Delete();
            Assert.IsFalse(deleteResult_ReadOnly);
            Assert.IsTrue(deleteResult_NotReadOnly);
        }
    }
}
