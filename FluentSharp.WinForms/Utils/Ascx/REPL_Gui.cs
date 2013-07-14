using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms.Controls
{
    public class REPL_Gui
    {
        public Panel TopPanel { get; set; }
        public Panel Code_Panel { get; set; }
        public Panel Output_Panel { get; set; }
        public Button Execute_Button { get; set; }
        public RichTextBox Output_View_RichTextBox { get; set; }
        public Panel Output_View_Object { get; set; }
        public Action Execute { get; set; }
        public Thread ExecutionThread { get; set; }

        public Action On_ExecuteCode { get; set; }

        public REPL_Gui buildGui(Control targetControl)
        {
            try
            {
                TopPanel = targetControl.clear().add_Panel();

                Code_Panel = TopPanel.insert_Left("Code");

                Output_Panel = TopPanel.add_GroupBox("Invoke and Result")
                                       .add_GroupBox("Output").add_Panel();
                Execute_Button = Output_Panel.parent().insert_Above(60).add_Button("Execute").fill();
                Execute_Button.insert_Below(20).add_Link("stop execution", () => this.stopCurrentExecution());
                Output_View_RichTextBox = Output_Panel.add_RichTextBox();
                Output_View_Object = Output_Panel.add_Panel();
                //set actions

                Execute_Button.onClick(
                    () =>
                        {
                            try
                            {
                                ExecutionThread = O2Thread.mtaThread(() => On_ExecuteCode.invoke());
                            }
                            catch (Exception ex)
                            {
                                ex.log();
                            }
                        });
            }
            catch (Exception ex)
            {
                ex.log("[REPL_Gui] in buildGui");
            }
            return this;
        }
    }
}

namespace FluentSharp.WinForms
{
    public static class REPL_Gui_ExtensionMethods_CreateGuis
    {
        public static REPL_Gui add_REPL_Gui(this Control targetControl)
        {
            return new REPL_Gui().buildGui(targetControl);
        }
        public static REPL_Gui add_TextBased_REPL_Gui(this Control targetControl)
        {
            var replGui = targetControl.add_REPL_Gui();    
            var codeText = replGui.Code_Panel.add_TextArea().allowTabs();   
            Action execute = 
	            ()=>{ 
			            var compilationOk = false;   
			            var code = codeText.get_Text();
			            var result = code.compileAndExecuteCodeSnippet(
								            (okMsg)=>   { replGui.showOutput(okMsg); compilationOk = true;},
								            (failMsg)=> { replGui.showErrorMessage(failMsg); });
			            if(compilationOk)
				            replGui.showOutput(result); 			            
		            }; 		
            replGui.On_ExecuteCode = execute;  
            codeText.set_Text("return \"Hello World\";");
            replGui.Execute_Button.click();
            return replGui;
        }
    }
    public static class REPL_Gui_ExtensionMethods_Helpers
    {
        public static REPL_Gui stopCurrentExecution(this REPL_Gui replGui)
		{
		  	if (replGui.ExecutionThread.notNull() && replGui.ExecutionThread.IsAlive)
            {
                "ExecutionThread is alive, so stopping it".info();
                replGui.ExecutionThread.Abort();
                replGui.Output_View_RichTextBox.textColor(Color.Red).set_Text("...current thread stopped...");
            }
            return replGui; 
		}
		
		public static REPL_Gui showErrorMessage(this REPL_Gui replGui, string msg)
		{
			replGui.Output_View_Object.visible(false);
			replGui.Output_View_RichTextBox.visible(true).textColor(Color.Red)
                    		   							 .set_Text(msg);	
			return replGui;
		}
		public static REPL_Gui showOutput(this REPL_Gui replGui, object result)
        {
        	var richTextBox = replGui.Output_View_RichTextBox;
        	var panel = replGui.Output_View_Object;
            richTextBox.visible(false);
            panel.visible(false).clear();            

            if (result == null)
                result = "[null value]";

            switch (result.typeName())
            {
                case "Boolean":
                case "String":
                case "Int64":
                case "Int32":
                case "Int16":
                case "Byte":
                    richTextBox.visible(true).textColor(Color.Black)
                    		   				 .set_Text(result.str());
                    break;
                case "Bitmap":
                    panel.visible(true).add_PictureBox()
                    				  .load((Bitmap)result);
                    break;
                default:
                    panel.visible(true).add_Control<ctrl_ShowInfo>()
                    	               .show(result);                    
                    break;
            }
            return replGui;
		}
	}
}
