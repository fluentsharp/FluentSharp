using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSharp.CoreLib.API;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Roslyn.Compilers;
using System.Reflection;
using System.IO;
using FluentSharp.CoreLib;
using System.Threading;

namespace FluentSharp.FluentRoslyn
{
    public static class API_Roslyn_ExtensionMethods_Compilation
    {
		public static Compilation compiler(this SyntaxTree tree, string assemblyName)
		{
			return tree.compiler(assemblyName, true);
		}

        public static Compilation compiler(this SyntaxTree tree, string assemblyName, bool compileIntoDll)
        {
            var compilationOptions = new CompilationOptions(compileIntoDll ? OutputKind.DynamicallyLinkedLibrary
                                                                           : OutputKind.ConsoleApplication);
            return Compilation.Create(assemblyName, compilationOptions,null,null,null,null)
                             .add_Reference("mscorlib")
                              .AddSyntaxTrees(tree);
        }

        /*        public static Compilation compiler(this SyntaxTree tree, string assemblyName, bool compileIntoDll = true)
        {
            return tree.compiler(assemblyName, compileIntoDll 
                                    ? OutputKind.DynamicallyLinkedLibrary
                                    : OutputKind.ConsoleApplication);            
        }

        public static Compilation compiler(this SyntaxTree tree, string assemblyName, OutputKind outputKind)
        {
            var compilationOptions = new CompilationOptions(outputKind);
            return Compilation.Create(assemblyName, compilationOptions)
                             .add_Reference("mscorlib")
                              .AddSyntaxTrees(tree);
        }*/

        public static T add_Reference<T>(this T compilation, string assemblyName)
            where T : CommonCompilation
        {
            return (T)compilation.AddReferences(assemblyName.assemblyReference());
        }

        public static AssemblyFileReference assemblyReference(this string assemblyName)
        {
            return new AssemblyFileReference(assemblyName.assembly_Location(),null,false);
        }
        
        public static List<Roslyn.Compilers.Common.CommonDiagnostic> errors(this CommonCompilation compilation)
        {
            return compilation.GetDiagnostics(default(CancellationToken)).errors();
        }

        public static List<CommonDiagnostic> errors(this CommonEmitResult emitResult)
        {
            return emitResult.Diagnostics.errors();
        }

        public static string errors_Details(this CommonCompilation compilation)
        {
            return compilation.errors().asString();
        }

        public static string asString(this List<CommonDiagnostic> diagnostics)        
        {
            var details = new StringBuilder();
            foreach (var error in diagnostics)
                details.AppendLine(error.str());
            return details.str();
        }

        public static List<CommonDiagnostic> errors(this IEnumerable<CommonDiagnostic> diagnostics)
        {
            return (from diagnostic in diagnostics
                    where diagnostic.Info.Severity == DiagnosticSeverity.Error
                    select diagnostic).toList();
        }

        public static Assembly create_Assembly<T>(this T compilation)
            where T : CommonCompilation
        {
            var ilStream = new MemoryStream();

			var result = compilation.Emit(ilStream, null, null, null, default(CancellationToken), null, null, null);
            if (result.Success)
            {
                "create_Assembly was ok".info();
                return Assembly.Load(ilStream.ToArray());
            }
            else
            {
                result.errors().asString().info();
            }
            "create_Assembly failed".error();
            return null;
        }
        
        public static Assembly create_Assembly_IntoDisk(this CommonCompilation compilation)
        {
            var fileToCreate = PublicDI.config.O2TempDir.pathCombine("_dynamicDll_{0}.dll".format(5.randomLetters()));
            return compilation.create_Assembly(fileToCreate);
        }
        public static Assembly create_Assembly(this CommonCompilation compilation, string fileToCreate)            
        {
            fileToCreate.deleteIfExists();
            if (fileToCreate.fileExists())
            {
                "[create_Assembly] targetExe could not be deleted".error();
                return null;
            }
            fileToCreate.directoryName().createDir();
            var pdbFilename = "{0}.pdb".format(fileToCreate);

            CommonEmitResult emitResult = null;            
            //compilation.
            using (var ilStream = new FileStream(fileToCreate, FileMode.OpenOrCreate))
            using (var pdbStream = new FileStream(pdbFilename, FileMode.OpenOrCreate))
            {
				emitResult = compilation.Emit(ilStream, pdbFilename, pdbStream, null,default(CancellationToken),null,null,null);
                if (emitResult.Success && fileToCreate.fileExists())
                {
                    "[create_Assembly] created assembly: {0}".info(fileToCreate);
                }
                else
                {
                    "[create_Assembly] could not create assembly: {0}".error(fileToCreate);
                    "[create_Assembly] Compilation errors: {1} {0}".error(emitResult.Diagnostics.errors().asString());
                }
            }            
            if (emitResult.Success)
                return Assembly.LoadFrom(fileToCreate);
            return null;
        }


        /*
		
var errors =  compilation.GetDiagnostics();
if (errors.Count() ==0)
{	
    var ilStream = new MemoryStream();
    compilation.Emit(ilStream);
    var assembly = Assembly.Load(ilStream.ToArray()); 	
    assembly.EntryPoint.invokeStatic();    
            
    return assembly.typeFullName();
}*/

    }
}
