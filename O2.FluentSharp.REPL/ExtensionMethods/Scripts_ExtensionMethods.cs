using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using O2.DotNetWrappers.ExtensionMethods;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.Views.ASCX.Ascx.MainGUI;

namespace O2.XRules.Database.Utils
{
    public static class Scripts_ExecutionMethods
    {
        public static ascx_Simple_Script_Editor add_ScriptExecution(this Control hostControl)
        {
            return hostControl.add_Script();
        }

        public static ascx_Simple_Script_Editor add_Script(this Control hostControl)
        {
            return hostControl.add<ascx_Simple_Script_Editor>();
        }

        public static ascx_Simple_Script_Editor add_Script(this Control hostControl, bool codeCompleteSupport)
        {
            return (ascx_Simple_Script_Editor)hostControl.invokeOnThread(
                () =>
                {
                    var scriptControl = new ascx_Simple_Script_Editor(codeCompleteSupport);
                    scriptControl.fill();
                    hostControl.add(scriptControl);
                    return scriptControl;
                });
        }

        public static ascx_Simple_Script_Editor set_Command(this ascx_Simple_Script_Editor scriptEditor, string commandText)
        {
            return (ascx_Simple_Script_Editor)scriptEditor.invokeOnThread(
                () =>
                {
                    scriptEditor.commandsToExecute.set_Text(commandText);
                    return scriptEditor;
                });
        }

        public static ascx_Simple_Script_Editor add_DevEnvironment<T>(this Control control)
            where T : Control
        {
            return control.add_DevEnvironment<T>(false);
        }

        public static ascx_Simple_Script_Editor add_DevEnvironment<T>(this Control control, bool includeLogViewer)
            where T : Control
        {
            var tTypeName = typeof(T).name().lowerCaseFirstLetter();
            var groupBoxes = control.add_1x1("Script", tTypeName, false, control.Width / 2);
            var tControl = groupBoxes[1].add<T>();
            if (tControl is TextBox)				// it might make more sent to add this to the control.add<..> method
                (tControl as TextBox).multiLine();
            var script = groupBoxes[0].add_Script(false);
            script.InvocationParameters.add(tTypeName, tControl);
            if (includeLogViewer)
                tControl.insert_Below<ascx_LogViewer>(150);
            return script;
        }

        public static ascx_Simple_Script_Editor execute(this ascx_Simple_Script_Editor scriptEditor, params string[] codesToExecute)
        {
            var codeToExecute = "";
            foreach (var code in codesToExecute)
                codeToExecute += code.line();
            return (ascx_Simple_Script_Editor)scriptEditor.invokeOnThread(
                () =>
                {
                    scriptEditor.commandsToExecute.set_Text(codeToExecute);
                    scriptEditor.onCompileExecuteOnce();
                    return scriptEditor;
                });
        }

        public static ascx_Simple_Script_Editor onCompileExecuteOnce(this ascx_Simple_Script_Editor scriptEditor)
        {
            return (ascx_Simple_Script_Editor)scriptEditor.invokeOnThread(
                () =>
                {
                    scriptEditor.onCompileOK =
                        () =>
                        {
                            scriptEditor.execute();
                            scriptEditor.onCompileOK = null;
                        };
                    return scriptEditor;
                });
        }

        public static ascx_Simple_Script_Editor script_Me(this object objectToScript)
        {
            var objectName = objectToScript.typeName().lowerCaseFirstLetter();
            var topPanel = "PoC - Script the {0} Object".format(objectName).popupWindow(700, 400);
            return objectToScript.script_Me(topPanel);
        }

        public static ascx_Simple_Script_Editor add_Script_Object(this Panel topPanel, object objectToScript)
        {
            return objectToScript.script_Me(topPanel);
        }
        public static ascx_Simple_Script_Editor script_Me(this object objectToScript, Panel topPanel)
        {
            var objectName = objectToScript.typeName().lowerCaseFirstLetter();
            var scriptHost = topPanel.add_Script(false);
            scriptHost.onCompileExecuteOnce();
            scriptHost.InvocationParameters.add(objectName, objectToScript);
            var code =
@"return {0};

//" + @"O2Ref:{1}
//O2" + @"Tag_DontAddExtraO2Files";
            scriptHost.Code = code.format(objectName, objectToScript.type().assemblyLocation()); ;
            return scriptHost;
        }
        //"test".popupWindow().add_Script().InvocationParameters.add("mdbgShell", mdbgShell);        
    }
}
