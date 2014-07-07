using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Controls;

namespace FluentSharp.REPL.Controls
{
    public static class ascx_Simple_Script_Editor_ExtensionMethods
    {
        public static Panel packageCurrentScriptAsStandAloneExe(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            if ("FluentSharp.MsBuild".assembly().isNull())
            {
                "[packageCurrentScriptAsStandAloneExe] could not invoke because FluentSharp.MsBuild assembly is not avaiable".error();
                return null;
            }
            var h2File = simpleScriptEditor.currentSourceCodeFilePath();
            if (h2File.valid())
                simpleScriptEditor.saveScript();
            else
                h2File = simpleScriptEditor.Code.h2_File();
            

            var assembly = "FluentSharp.MsBuild".assembly();
            var type     = assembly.type("Package_O2_Script_into_separate_Folder");

            var topPanel = type.invokeStatic("Main", /*startHidden*/ false, /*targetScript*/ h2File);

            return topPanel.cast<Panel>();
            
            
            //var packageScript = (Action<string>)"Util - Package O2 Script into separate Folder.h2".executeFirstMethod();
            //packageScript(h2File);
        }
    }
}
