using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSharp.CoreLib;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.NUnit
{
    [TestFixture]
    public class NUnitTests_ExtensionMethods_IO
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
    }
}
