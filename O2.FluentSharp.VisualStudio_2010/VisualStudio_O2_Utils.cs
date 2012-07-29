using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.XRules.Database.Utils;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.FluentSharp
{
    public class VisualStudio_O2_Utils
    {
        public static ascx_Simple_Script_Editor open_ScriptEditor()
        {                     
            CompileEngine.DefaultReferencedAssemblies
                            .add_OnlyNewItems("Microsoft.VisualStudio.Shell.10.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.8.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.9.0.dll",
                                              "Microsoft.VisualStudio.Shell.Interop.10.0.dll",
                                              "Microsoft.VisualStudio.OLE.Interop.dll",
                                              "Microsoft.VisualStudio.CommandBars.dll",
                                              "EnvDTE.dll",
                                              "EnvDTE80.dll",
                                              "O2_FluentSharp_VisualStudio_2010.dll");
            CompileEngine.DefaultUsingStatements.add_OnlyNewItems("O2.FluentSharp");

            var scriptEditor = new VisualStudio_2010().script_Me("visualStudio");

            scriptEditor.Code += ("//O2File:" + "ExtensionMethods/VisualStudio_2010_ExtensionMethods.cs").lineBeforeAndAfter();

//            var scriptEditor = open.scriptEditor();
            
//            scriptEditor.inspector.Code = @"return VisualStudio_2010.Package;";

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
            return scriptEditor;            
        }
    }
}
