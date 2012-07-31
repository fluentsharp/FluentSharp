using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.XRules.Database.Utils;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.FluentSharp.REPL;
using O2.Views.ASCX.Ascx.MainGUI;

namespace O2.FluentSharp
{
    public class VisualStudio_O2_Utils
    {
        public static void installO2Scripts_IfDoesntExist()
        {
            if (PublicDI.config.LocalScriptsFolder.dirExists().isFalse())
            {
                O2Scripts.downloadO2Scripts();
/*                "CatNet.cs".local()
                           .compile()
                           .type("CatNet")
                           .ctor();*/
            }
        
        }
         
        public static void compileAndExecuteScript(string scriptFile, string type, string method)
        {
            addVisualStudioReferencesForCompilation();
            O2Thread.mtaThread(() =>
            {
                if (waitForDTEObject())
                {
                    "[createO2PlatformMenu] Got DTE object".info();
                    var file =scriptFile.pathCombine_With_ExecutingAssembly_Folder();
                    if (file.fileExists().isFalse())
                        "[createO2PlatformMenu] could not find script with O2 Platform menu: {0}".error(file);
                    else
                    {
                        "[createO2PlatformMenu] compiling {0}".info(file);
                        var assembly = file.compile();
                        if (assembly.notNull())
                        {
                            "[createO2PlatformMenu] executing {0}.{1} method".info(type,method);
                            assembly.type(type)
                            .method(method)
                            .invoke();
                        }
                        else
                        {
                            "[createO2PlatformMenu] failed to compile file: {0}".error(file);
                            "[createO2PlatformMenu] opening an o2 Script editor to help debugging the issue".info();
                            file.script_Me("file");
                        }
                    }
                }
            });
        }

        public static void createO2PlatformMenu()
        {
            compileAndExecuteScript(@"VS_Scripts\O2_Menus_In_VisualStudio.cs", "O2_Menus_In_VisualStudio" ,"buildMenus");
        }

        public static void createO2PlatformDockWindow()
        {
            compileAndExecuteScript(@"VS_Scripts\O2_Menus_In_VisualStudio.cs", "O2_Menus_In_VisualStudio", "createDocWindow");
        }

        public static bool waitForDTEObject()
        {
            var maxWaitLoops = 10;
            while (maxWaitLoops-- > 0)
            {
                if (VisualStudio_2010.DTE2.notNull())
                    return true;
                "[waitForDTEObject] waiting 500 ms".info();
                500.sleep();
            }
            "[waitForDTEObject] failed to get DTE object after {0} attempts".error(maxWaitLoops);
            return false;
        }

        public static void addVisualStudioReferencesForCompilation()
        {
            CompileEngine.DefaultReferencedAssemblies
                            .add_OnlyNewItems(//needed for VS scripting
                                              "O2_FluentSharp_VisualStudio_2010.dll",
                                              "Microsoft.VisualStudio.Shell.10.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.8.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.9.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.10.0.dll",
                                              "Microsoft.VisualStudio.Shell.UI.Internal.dll",
                                              "Microsoft.VisualStudio.OLE.Interop.dll",
                                              "Microsoft.VisualStudio.CommandBars.dll",
                                              "EnvDTE.dll",
                                              "EnvDTE80.dll",                                              
                                              //needed for WPF manipulation
                                              "WindowsFormsIntegration.dll",
                                              "Microsoft.VisualStudio.Text.UI.dll",
                                              "Microsoft.VisualStudio.Text.UI.Wpf.dll",
                                              "Microsoft.VisualStudio.CoreUtility.dll",
                                              "Microsoft.VisualStudio.Platform.VSEditor.dll",
                                              "Microsoft.VisualStudio.Text.Data.dll",
                                              "O2_FluentSharp_WPF.dll",
                                              //needed for WPF
                                              "PresentationFramework.dll",
                                              "PresentationCore.dll",
                                              "WindowsBase.dll",
                                              "System.Xaml.dll"
                                              );
            CompileEngine.DefaultUsingStatements
                            .add_OnlyNewItems("O2.FluentSharp",
                                              "O2.FluentSharp.Packages",
                                              "WPF_Media = System.Windows.Media",
                                              "WPF_Controls = System.Windows.Controls");
        }

        public static ascx_LogViewer open_LogViewer()
        {
            return open.logViewer();  
        }
        public static ascx_Simple_Script_Editor open_ScriptEditor()
        {
            addVisualStudioReferencesForCompilation();

            var scriptEditor = new VisualStudio_2010().script_Me("visualStudio");

            scriptEditor.Code += ("//O2File:" + "ExtensionMethods/VisualStudio_2010_ExtensionMethods.cs").lineBeforeAndAfter();

//            var scriptEditor = open.scriptEditor();
            
//            scriptEditor.inspector.Code = @"return VisualStudio_2010.Package;";

            return scriptEditor;            
        }
    }

/*var defaultCode =
"//testing autoupdate
var ErrorList = VisualStudio_2010.ErrorListProvider;

ErrorList.clear();
ErrorList.add_Error(""You can Errors like this"");
ErrorList.add_Warning(""You can Warnings like this"");
ErrorList.add_Message(""You can Messages like this"");
""This is another error"".add_Error();
""This is another Warning"".add_Warning();
""This is anpther Message"".add_Message();

return VisualStudio_2010.Package;
//O2File:ExtensionMethods/VisualStudio_2010_ExtensionMethods.cs";
scriptEditor.wait(2000);
scriptEditor.inspector.set_Script(defaultCode);
CompileEngine.clearCompilationCache();            
scriptEditor.inspector.compile();
 * */
}
