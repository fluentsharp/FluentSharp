using System;
using System.Windows.Forms;
using FluentSharp.BCL;
using FluentSharp.BCL.Controls;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Controls;

namespace FluentSharp.REPL
{
    public static class Scripts_ExecutionMethods
    {
        public static ascx_Simple_Script_Editor add_ScriptExecution(this Control hostControl)
        {
            return hostControl.add_Script();
        }
        public static ascx_Simple_Script_Editor insert_Script(this Control control)
        {
            return control.insert_Below().add_Script();
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
        public static ascx_Simple_Script_Editor add_Script_With_FolderViewer(this Control hostControl, string startFolder)
        {
            var script = hostControl.add_Script();
            Action<string> openFile = (file) => script.openFile(file);            
            hostControl.insert_FolderViewer_Simple(openFile, startFolder);
            return script;
        }
        public static ascx_Simple_Script_Editor set_Code(this ascx_Simple_Script_Editor scriptEditor, string code)
        {
            return scriptEditor.set_Command(code);
        }
        public static ascx_Simple_Script_Editor set_Command(this ascx_Simple_Script_Editor scriptEditor, string commandText)
        {
            return (ascx_Simple_Script_Editor)scriptEditor.invokeOnThread(
                () =>
                {
                    scriptEditor.Code = commandText;
                    return scriptEditor;
                });
        }
        public static ascx_Simple_Script_Editor add_DevEnvironment<T>(this Control control) where T : Control
        {
            return control.add_DevEnvironment<T>(false);
        }
        public static ascx_Simple_Script_Editor add_DevEnvironment<T>(this Control control, bool includeLogViewer) where T : Control
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
        public static ascx_Simple_Script_Editor script_Me_WaitForClose(this object objectToScript)
        {
            return objectToScript.script_Me().waitForClose();
        }
        public static ascx_Simple_Script_Editor script_Me(this object objectToScript)
        {
            var objectName = objectToScript.typeName().lowerCaseFirstLetter();
            return objectToScript.script_Me(objectName);
        }
        public static ascx_Simple_Script_Editor script_Me(this object objectToScript, string objectName)
        {
            var topPanel = "PoC - Script the {0} Object".format(objectName).popupWindow(700, 400).insert_LogViewer();
            return objectToScript.script_Me(objectName,topPanel);
        }
        public static ascx_Simple_Script_Editor add_Script_Object(this Panel topPanel, object objectToScript)
        {
            return objectToScript.script_Me(topPanel);
        }
        public static ascx_Simple_Script_Editor add_Script_Me(this Panel panel, object _object)
        {
            return _object.script_Me(panel.clear());
        }
        public static ascx_Simple_Script_Editor add_Script_Me(this Panel panel, object targetObject, string varName)
        {
            return targetObject.script_Me(varName, panel);
        }
        public static ascx_Simple_Script_Editor insert_Right_Script_Me(this Control control, object _object)
        {
            return control.insert_Right().add_Script_Me(_object);
        }
        public static ascx_Simple_Script_Editor insert_Below_Script_Me(this Control control, object _object)
        {
            return control.insert_Below().add_Script_Me(_object);
        }
        public static ascx_Simple_Script_Editor script_Me(this object objectToScript, Panel topPanel)
        {
            var objectName = objectToScript.typeName().lowerCaseFirstLetter();
            return objectToScript.script_Me(objectName, topPanel);
        }
        public static ascx_Simple_Script_Editor script_Me(this object objectToScript, string objectName, Panel topPanel)
        {
            var scriptHost = topPanel.add_Script(true);             //enable autocomplete by default
            scriptHost.onCompileExecuteOnce();
            scriptHost.InvocationParameters.add(objectName, objectToScript);
            var code =
@"return {0};

//" + @"O2Ref:{1}";
            scriptHost.Code = code.format(objectName, objectToScript.type().assemblyLocation().fileName()); ;
            if (objectToScript.isNull())
                "[script_Me] provided objectToScript was null".error();
            return scriptHost;
        }
        public static ascx_Simple_Script_Editor add_InvocationParameter(this ascx_Simple_Script_Editor scriptEditor, string parameterName, object parameterObject)
		{
			scriptEditor.InvocationParameters.add(parameterName,parameterObject);
			return scriptEditor;
		}
		public static ascx_Simple_Script_Editor code_Insert(this ascx_Simple_Script_Editor scriptEditor, string textToInsert)
		{
			scriptEditor.Code = textToInsert.line() + scriptEditor.Code;
			return scriptEditor;
		}
		public static ascx_Simple_Script_Editor code_Append(this ascx_Simple_Script_Editor scriptEditor, string textToInsert)
		{
			scriptEditor.Code = scriptEditor.Code.line() +  textToInsert;
			return scriptEditor;
		}
        public static SourceCodeEditor          csharp_Colors(this SourceCodeEditor codeEditor)
        {
            return codeEditor.set_ColorsForCSharp();
        }
        public static ascx_Simple_Script_Editor executeOnCompile(this ascx_Simple_Script_Editor simpleEditor)
        {
            simpleEditor.ExecuteOnCompile = true;
            return simpleEditor;
        }
        public static ascx_Simple_Script_Editor makeInvocationParametersTypeGeneric(this ascx_Simple_Script_Editor simpleEditor)
        {
            simpleEditor.csharpCompiler.ResolveInvocationParametersType = false;
            return simpleEditor;
        }

        
        //"test".popupWindow().add_Script().InvocationParameters.add("mdbgShell", mdbgShell);        
    }
}
