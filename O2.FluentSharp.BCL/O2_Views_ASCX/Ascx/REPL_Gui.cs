using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using System.Drawing;
using O2.Views.ASCX.DataViewers;

namespace O2.XRules.Database.Utils
{
	public class REPL_Gui
	{
		public Panel 		TopPanel				{ get; set; }
		public Panel 		Code_Panel 				{ get; set; }
		public Panel 		Output_Panel 			{ get; set; }
		public Button		Execute_Button			{ get; set; }
		public RichTextBox 	Output_View_RichTextBox { get; set; }
		public Panel 		Output_View_Object 		{ get; set; }
		public Action 		Execute					{ get; set; }
		public Thread 		ExecutionThread			{ get; set; }
		
		public Action		On_ExecuteCode			{ get; set; }		
		
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
				Output_View_Object		= Output_Panel.add_Panel();
				//set actions
				
				Execute_Button.onClick(
					()=>{
							try
							{
								ExecutionThread = O2Thread.mtaThread(()=> On_ExecuteCode.invoke());
							}
							catch(Exception ex)
							{
								ex.log(); 
							}
						});
			}
			catch(Exception ex)
			{
				ex.log("[REPL_Gui] in buildGui");
			}
			return this;
		}	
	}
			
	public static class REPL_Gui_ExtensionMethods
	{
		public static REPL_Gui add_REPL_Gui(this Control targetControl)
		{
			return new REPL_Gui().buildGui(targetControl);
		}
		
		
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
                    panel.visible(true).add_Control<ascx_ShowInfo>()
                    	               .show(result);                    
                    break;
            }
            return replGui;
		}
	}
}
