using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.NUnit
{
    [TestFixture]
    public class Test_NUnitTests_ExtensionMethods_IO
    {
        [Test] public void assert_Folder_Empty()
        { 
            var tempFolder = "a_Folder".temp_Folder();
            tempFolder.assert_Folder_Exists()
                      .assert_Folder_Empty()
                      .folder_Create_File("fileName.txt", "some contents")          // returns the file created
                        .assert_File_Exists()
                        .parentFolder()                                             // back to tempFolder
                            .assert_Are_Equal(folder=>folder.files().size(),1)
                            .assert_Folder_Not_Empty()
                            .assert_Is_True(folder=>folder.folder_Delete_Files())   // delete created file
                            .assert_Folder_Empty()
                            .assert_Folder_Deleted();                               // delete folder
         
            tempFolder.assert_Folder_Not_Exists();
        }
        
        [Test] public void assert_Dir_Exists()
        {            
            var dir = PublicDI.config.O2TempDir;
            dir.assert_Dir_Exists();            
        }
        [Test] public void assert_Dir_Not_Exists()
        {            
            var dir = PublicDI.config.O2TempDir.pathCombine("".add_RandomLetters(10));
            dir.assert_Dir_Not_Exists ();            
        }
        [Test] public void assert_File_Exists()
        {            
            var file1 = "aaa".tempFile();
            var file2 = "aaa";

            file1.assert_File_Not_Exists();
            file2.assert_File_Not_Exists();

            "123".saveAs(file1);

            file1.assert_File_Exists();
        }
        [Test] public void assert_Folder_Exists()
        {            
            var folder = PublicDI.config.O2TempDir;
            folder.assert_Folder_Exists();      
        }
        [Test] public void assert_Folder_Not_Exists()
        {            
            var folder = PublicDI.config.O2TempDir.pathCombine("".add_RandomLetters(10));
            folder.assert_Folder_Not_Exists ();       
        }        
    }
}
