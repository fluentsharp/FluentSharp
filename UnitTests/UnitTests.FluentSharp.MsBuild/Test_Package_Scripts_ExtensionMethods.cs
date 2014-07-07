using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.MsBuild;
using FluentSharp.NUnit;
using FluentSharp.REPL;
using FluentSharp.WinForms;
using NUnit.Framework;

namespace UnitTests.FluentSharp.MsBuild
{
    [TestFixture]
    public class Test_Package_Scripts_ExtensionMethods : NUnitTests
    {
        //public API_Create_Exe apiCreateExe;

        public string scriptFile;        
        [SetUp]
        public void setup()
        {
            //apiCreateExe = new API_Create_Exe();
            scriptFile = Misc_WinForms_Script_Files.PopupWindow_With_LogViewer()
                                                   .assert_File_Exists();                                          

            var sourceFile = Misc_WinForms_Script_Files.PopupWindow_With_LogViewer();
			sourceFile.assert_File_Exists();							
        }

        [TearDown]
        public void tearDown()
        {
            
        }
        
        [Test]
        public void package_Script()
        {            
            var pathToAssemblies = "";
            var projectFile = "";
            var compiledScript = "";            

            Action<List<string>> beforeAddingReferences = 
	            (referencesToAdd)=> {  
							            "[in beforeAddingReferences] assembly references: {0}".debug(referencesToAdd.asString());													
					 	            };
            Action<List<string>> beforeEmbedingFiles = 
	            (filesToEmbed)=>{ 						
						            "[in beforeEmbedingFiles] assembly references: {0}".debug(filesToEmbed.asString());								
					            };					

            var createdExe = scriptFile.package_Script(ref compiledScript, ref pathToAssemblies, ref projectFile, beforeAddingReferences, beforeEmbedingFiles);
            
            
            

            createdExe.assert_Not_Null()
                      .assert_File_Exists();            
            
            var rootFolder        = createdExe.parentFolder().parentFolder()     .assert_Folder_Exists();
            var exe_In_RootFolder = rootFolder.pathCombine(createdExe.fileName()).assert_File_Exists();
            var buildFiles_In_RootFolder = rootFolder.pathCombine("_BuildFiles") .assert_Folder_Exists();

            Files.delete_Folder_Recursively(buildFiles_In_RootFolder).assert_True();
            
            exe_In_RootFolder.assert_File_Deleted();                        
            
            exe_In_RootFolder.parentFolder().assert_Folder_Deleted();



            //this test is a variation of the script below was at O2.Platform.Scripts\3rdParty\Microsoft\MSBuild\PoC - Scriping Package workflow.h2

            /*
            var pathToAssemblies = "";
            var projectFile = "";
            var compiledScript = "";
            var scriptFile = "Util - LogViewer.h2"; 

            Action<List<string>> beforeAddingReferences = 
	            (referencesToAdd)=> {  
							            "[in beforeAddingReferences] assembly references: {0}".debug(referencesToAdd.asString());													
					 	            };
            Action<List<string>> beforeEmbedingFiles = 
	            (filesToEmbed)=>{ 						
						            "[in beforeEmbedingFiles] assembly references: {0}".debug(filesToEmbed.asString());								
					            };					

            var createdExe = scriptFile.package_Script(ref compiledScript, ref pathToAssemblies, ref projectFile, beforeAddingReferences, beforeEmbedingFiles);

            "scriptFile: {0}".info(scriptFile);
            "pathToAssemblies: {0}".info(pathToAssemblies);
            "projectFile: {0}".info(projectFile);
            "compiledScript: {0}".info(compiledScript);
            "createdExe: {0}".info(createdExe);
            pathToAssemblies.startProcess();*/
        }
    }
}
