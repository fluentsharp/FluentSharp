using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentSharp.AST;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.APIs;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Ast.ExtensionMethods
{
    [TestFixture]
    public class Test_Ast_CSharp_ExtensionMethods_Code_Generation
    {
        [Test] public void mapNuGetReferences()
        {
            var targetPackage = "FluentSharp.HtmlAgilityPack";
            var nuGet = new API_NuGet("_temp_Packages_Folder".temp_Dir());

            new API_NuGet().setup().NuGet_Exe.file_CopyToFolder(nuGet.Packages_Folder);         // do this so that the nuget.exe file is not downloaded every time this unit test runs
            nuGet.NuGet_Exe.assert_File_Exists();
            nuGet.path_Package(targetPackage).assert_Null();
            
            nuGet.path_Package(targetPackage);
            var compilerOptions = new CSharp_FastCompiler_CompilerOptions();
            compilerOptions.ReferencedAssemblies.assert_Not_Empty();
            compilerOptions.NuGet_References.add(targetPackage);

            compilerOptions.mapNuGetReferences(nuGet);
            
            nuGet.path_Package(targetPackage).assert_Not_Null();
            compilerOptions.ReferencedAssemblies.contains(targetPackage);
            Files.delete_Folder_Recursively(nuGet.Packages_Folder)
                 .assert_True();            
        }
        [Test] public void mapOptionsDefinedInsideComments()
        {
            var compilerOptions = new CSharp_FastCompiler_CompilerOptions();

            var comments = new List<string>();
                        

            compilerOptions.mapOptionsDefinedInsideComments(comments)
                           .toXml().assert_Equal(new CSharp_FastCompiler_CompilerOptions().toXml(), "Empty comments should make no changes to compilerOptions");
            comments.add("This is a random content");

            compilerOptions.mapOptionsDefinedInsideComments(comments)
                           .toXml().assert_Equal(new CSharp_FastCompiler_CompilerOptions().toXml(), "Non relevant comments should make no changes to compilerOptions");

            //O2Tag_OnlyAddReferencedAssemblies
            compilerOptions.onlyAddReferencedAssemblies.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("O2Tag_OnlyAddReferencedAssemblies"))
                           .onlyAddReferencedAssemblies.assert_True();
            
            //generateDebugSymbols
            compilerOptions.generateDebugSymbols.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("generateDebugSymbols"))
                           .generateDebugSymbols.assert_True();

            //SetInvocationParametersToDynamic
            compilerOptions.ResolveInvocationParametersType.assert_True();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("SetInvocationParametersToDynamic"))
                           .ResolveInvocationParametersType.assert_False();

            //DontSetInvocationParametersToDynamic
            compilerOptions.ResolveInvocationParametersType.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("DontSetInvocationParametersToDynamic"))
                           .ResolveInvocationParametersType.assert_True();

            //StaThread
            compilerOptions.ExecuteInStaThread.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("StaThread"))
                           .ExecuteInStaThread.assert_True();

            //MtaThread
            compilerOptions.ExecuteInMtaThread.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("MtaThread"))
                           .ExecuteInMtaThread.assert_True();

            //WorkOffline
            compilerOptions.WorkOffline.assert_False();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("WorkOffline"))
                           .WorkOffline.assert_True();

            //using
            compilerOptions.Extra_Using_Statements.assert_Empty();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("using ABC"))
                           .Extra_Using_Statements.assert_Not_Empty()
                                                  .first().assert_Is("ABC");
            //O2Ref
            compilerOptions.ReferencedAssemblies.assert_Not_Contains("ABC");
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("O2Ref:ABC"))
                           .ReferencedAssemblies.assert_Contains("ABC");

            //O2Download
            compilerOptions.FilesToDownload.assert_Empty();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("O2Download:ABC"))
                           .FilesToDownload.assert_Not_Empty()
                                                  .first().assert_Is("ABC");

            //O2File
            compilerOptions.ExtraSourceCodeFilesToCompile.assert_Empty();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("O2File:ABC"))
                           .ExtraSourceCodeFilesToCompile.assert_Not_Empty()
                                                         .first().assert_Is("ABC");

            
            //O2Dir

            var tempDir = "_DirToInclude".temp_Dir();
            var file    = tempDir.folder_Create_File("tempFile.cs", "some code");
            compilerOptions.ExtraSourceCodeFilesToCompile.assert_Not_Contains(file);
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("O2Dir:".append(tempDir)))
                           .ExtraSourceCodeFilesToCompile.assert_Contains(file);
            
            tempDir.assert_Folder_Deleted();

            //NuGet
            compilerOptions.NuGet_References.assert_Empty();
            compilerOptions.mapOptionsDefinedInsideComments(comments.add("NuGet:ABC"))
                           .NuGet_References.assert_Not_Empty()
                                            .first().assert_Is("ABC");
        }
    }
}
