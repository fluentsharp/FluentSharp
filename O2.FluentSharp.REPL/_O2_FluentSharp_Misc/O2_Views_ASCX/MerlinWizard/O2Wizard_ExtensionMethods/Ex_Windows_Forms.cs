using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Merlin;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel.CodeUtils;


namespace O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods
{
    public static class Ex_Windows_Forms
    {
        public static TextBox create_TextBox(string message)
        {
            var textBox = new TextBox();
            textBox.Multiline = true;
            textBox.WordWrap = false;
            textBox.ScrollBars = ScrollBars.Both;
            textBox.ReadOnly = true;
            textBox.Text = message;
            textBox.Select(0, 0);
            return textBox;
        }

        public static TemplateStep createStepWith_TextBox(string stepTitle, string message)
        {
            var textBox = create_TextBox(message);
            return createStepWith_TextBox(stepTitle, textBox);
        }

        public static TemplateStep createStepWith_TextBox(string stepTitle, TextBox textBox)
        {
            var newStep = new TemplateStep(textBox, 10, stepTitle);
            return newStep;
        }

        public static void set_Text(this IStep step, string message)
        {
            if (step.FirstControl != null)
                step.Controller.wizardForm.invokeOnThread(
                    () =>
                        {                            
                            step.FirstControl.Text = message;
                            //step.FirstControl.invoke("DeselectAll");                               
                        });            
        }

        public static void append_Text(this IStep step, string messageFormat, params Object[] variables)
        {
            step.append_Text(string.Format(messageFormat, variables));
        }

        public static void append_Line(this IStep step)
        {
            step.append_Text(Environment.NewLine);
        }

        public static void append_Line(this IStep step, string message, bool extraLineAfter)
        {
            step.append_Line(message, extraLineAfter, false);
        }

        public static void append_Line(this IStep step, string message, bool extraLineAfter, bool extraLineBefore)
        {
            if (extraLineBefore)
                step.append_Line();
            step.append_Text(message + Environment.NewLine);
            if (extraLineAfter)
                step.append_Line();
        }
        
        public static void append_Line(this IStep step, string messageFormat, params Object[] variables)
        {
            step.append_Text(string.Format(messageFormat + Environment.NewLine, variables));
        }

        public static void append_Line(this IStep step, string message)
        {
            step.append_Text(message + Environment.NewLine);
        }

        public static void append_Text(this IStep step, string message)
        {
            if (step.FirstControl != null)
                step.Controller.wizardForm.invokeOnThread(
                    () => step.FirstControl.Text += message);
            else if (step.UI != null)
                if (step.UI.Controls.Count > 0)
                    if (step.UI.Controls[0] is TextBox)
                    {
                        var targetTextBox = (TextBox)step.UI.Controls[0];
                        targetTextBox.append_Text(message);
                        
                        /*targetTextBox.invokeOnThread(
                            () =>
                                {                                    
                                    targetTextBox.Text += message;                                    
                                });                        */
                    }                        
        }        

        public static void add_CheckBox(this IStep step, string checkBoxText, bool initialCheckedValue, Action<bool> onChange)
        {
            step.UI.invokeOnThread(
                () =>
                {
                    var checkBox = new CheckBox();
                    checkBox.Text = checkBoxText;
                    checkBox.AutoSize = true;
                    checkBox.Checked = initialCheckedValue;
                    if (onChange != null)
                        checkBox.CheckedChanged += (sender, e) => onChange(checkBox.Checked);
                    // at the moment this assumes that the control[0] is a contained of (for example) type FlowLayoutPanel
                    step.UI.Controls[0].Controls.Add(checkBox);
                });
        }

        public static IStep add_WebBrowser(this List<IStep> steps, string stepTitle, string stepSubTitle)
        {
            return steps.add_WebBrowser(stepTitle, stepSubTitle, null, null);
        }
        public static IStep add_WebBrowser(this List<IStep> steps, string stepTitle, string stepSubTitle, string defaultUrl)
        {
            return steps.add_WebBrowser(stepTitle, stepSubTitle, defaultUrl, null);
        }

        public static IStep add_WebBrowser(this List<IStep> steps, string stepTitle, string stepSubTitle, string defaultUrl, Action<IStep> OnComponentAction)
        {

            //control.AllowDrop = false;
            var newStep = new TemplateStep(new Panel(), 10, stepTitle);
            newStep.Subtitle = stepSubTitle;
            newStep.OnComponentLoad =
                (step) =>
                {
                    var webBrowser = new WebBrowser();
                    step.UI = webBrowser;
                    if (false == string.IsNullOrEmpty(defaultUrl))
                        webBrowser.Navigate(defaultUrl);
                };
            newStep.OnComponentAction = OnComponentAction;
            steps.Add(newStep);
            return newStep;
        }
    }
}
