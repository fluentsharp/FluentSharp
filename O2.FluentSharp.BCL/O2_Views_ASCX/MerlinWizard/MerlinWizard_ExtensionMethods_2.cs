using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using O2.Kernel;
using O2.Views.ASCX.CoreControls;
//O2Tag_AddReferenceFile:Merlin.dll
using Merlin;


namespace O2.Views.ASCX.MerlinWizard
{
    public static class Ascx_ExtensionMethods_2
    {
    	public static IStep createStepWithTextBox(string stepTitle, string message)
    	{
			var textBox = new TextBox();
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.ReadOnly = true;
            textBox.Text = message;
            textBox.Select(0, 0);	
            var newStep = new TemplateStep(textBox, 10, stepTitle);
            return newStep;
    	}
    	public static IStep add_Message(this List<IStep> steps, string stepTitle,string message)
    	{
            var newStep = createStepWithTextBox(stepTitle,message);
            steps.Add(newStep);
            return newStep;
    	}
    	
    	public static IStep add_Action(this List<IStep> steps, string stepTitle,Action<TextBox> action)
    	{
    		var newStep = createStepWithTextBox(stepTitle, "something is happening");
            steps.Add(newStep);
            return newStep;
    	}
    }
}
