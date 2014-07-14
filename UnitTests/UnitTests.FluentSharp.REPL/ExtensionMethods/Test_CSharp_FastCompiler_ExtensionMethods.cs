using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.REPL.Utils;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp_REPL.ExtensionMethods
{
    [TestFixture]
    public class Test_CSharp_FastCompiler_ExtensionMethods : NUnitTests
    {

        [Test] public void register_GitHub_As_ExternalAssemblerResolver()
        {
            AssemblyResolver.ExternalAssemblerResolver.clear();
            assert_Is_Empty(AssemblyResolver.ExternalAssemblerResolver);

            assert_True(typeof(CSharp_FastCompiler).invoke_Ctor_Static());        //this should call register_GitHub_As_ExternalAssemblerResolver
            
            assert_Is_Not_Empty(AssemblyResolver.ExternalAssemblerResolver);            
        }
    
        [Test] public void compileScriptFile_into_SeparateFolder()
        {
            var scriptFile = Misc_WinForms_Script_Files.PopupWindow_With_LogViewer();

            assert_File_Exists(scriptFile);

            var compiledScript = scriptFile.compileScriptFile_into_SeparateFolder();            

            compiledScript.assert_Not_Null()
                          .assert_File_Exists();   

            var parentFolder        = compiledScript.parentFolder();
            var filesInParentFolder = parentFolder.files();
            filesInParentFolder.fileNames().assert_Size_Is(3)
                                           .assert_Item_Is_Equal(0,"FluentSharp.CoreLib.dll")                                           
                                           .assert_Item_Is_Equal(1,"FluentSharp.WinForms.dll")
                                           .assert_Item_Is_Equal(2,compiledScript.fileName());
;            
            parentFolder.assert_Contains(PublicDI.config.O2TempDir);            
            
            var tmpDll = compiledScript.fileName().inTempDir();
            var tmpPdb = tmpDll.extensionChange("pdb");

            //var tmpCs  = tmpDll.replace("._o2_Script.dll", ".cs");        // the temp file name is added by one, so this doesn't work
            tmpDll.assert_File_Exists();//.assert_File_Deleted();           // can't delete because it is locked
            tmpPdb.assert_File_Exists();//.assert_File_Deleted();           // can't delete because it is locked
                        
            scriptFile         .assert_File_Deleted();
            filesInParentFolder.assert_Files_Deleted();
            parentFolder       .assert_Folder_Deleted();            
        }

    }
}
