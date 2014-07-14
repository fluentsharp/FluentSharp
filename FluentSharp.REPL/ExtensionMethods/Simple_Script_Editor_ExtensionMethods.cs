using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Controls;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class Simple_Script_Editor_ExtensionMethods
    {
        public static CSharp_FastCompiler csharpCompiler(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            return simpleScriptEditor.notNull() ? simpleScriptEditor.CSharpCompiler : null;                
        }
        public static ascx_Simple_Script_Editor csharpCompiler(this ascx_Simple_Script_Editor simpleScriptEditor, CSharp_FastCompiler value)
        {
            if (simpleScriptEditor.notNull())
                simpleScriptEditor.CSharpCompiler = value;
            return simpleScriptEditor;
        }
        public static ascx_Simple_Script_Editor compile_WaitFor_CompilationComplete(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            simpleScriptEditor.compile();
            simpleScriptEditor.csharpCompiler().waitForCompilationComplete();            
            return simpleScriptEditor;
        }

        public static ascx_Simple_Script_Editor execute_WaitFor_ExecutionComplete(this ascx_Simple_Script_Editor simpleScriptEditor, int maxWaitSeconds = 20)
        {
            simpleScriptEditor.execute();
            return simpleScriptEditor.waitFor_ExecutionComplete(maxWaitSeconds);
        }
        public static ascx_Simple_Script_Editor waitFor_ExecutionComplete(this ascx_Simple_Script_Editor simpleScriptEditor, int maxWaitSeconds = 20)
        {
            var executionThread = simpleScriptEditor.ExecutionThread;
            if (executionThread.isNull())                                   // if the execution thread is not alive            
                for(int i=0 ; i < maxWaitSeconds ; i++)
                {
                    500.sleep();                                            // wait 500ms for a maximum of maxWaitSeconds times (default is 10sec)
                    executionThread = simpleScriptEditor.ExecutionThread;   // try again
                    if(executionThread.notNull())
                        break;
                }
            
            if(executionThread.notNull() && executionThread.IsAlive)
                if (executionThread.Join(20 * 1000).isFalse())                    
                    "[ascx_Simple_Script_Editor][waitFor_ExecutionComplete] the execution lasted more than {0} seconds".error(maxWaitSeconds);
            return simpleScriptEditor;
        }
        

        //the one below seems to aggresive and not optiomal
        /*public static ascx_Simple_Script_Editor wait_For_OnExecute_Once(this ascx_Simple_Script_Editor simpleScriptEditor)
        {            
            var previousOnExecute = simpleScriptEditor.onExecute;
            var sync = false.autoResetEvent();
            simpleScriptEditor.onExecute = result =>
                {
                    sync.set();
                };             
            sync.waitOne();
            simpleScriptEditor.onExecute = previousOnExecute;
            return simpleScriptEditor;
        }*/
        public static object executionResult (this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            return simpleScriptEditor.executionResult<object>();
        }
        public static T executionResult <T>(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            if (simpleScriptEditor.notNull() && simpleScriptEditor.LastExecutionResult.notNull())
                if(simpleScriptEditor.LastExecutionResult is T)
                    return (T)simpleScriptEditor.LastExecutionResult;
            return default(T);
        }
        public static Dictionary<string, object> invocationParameters(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            return simpleScriptEditor.notNull() ? simpleScriptEditor.InvocationParameters : null;
        }

        public static T invocationParameter<T>(this ascx_Simple_Script_Editor simpleScriptEditor)
        {
            foreach(var invocationParameter in simpleScriptEditor.invocationParameters())
                if (invocationParameter.Value is T)
                    return (T)invocationParameter.Value;
            return default(T);
        }
        

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
