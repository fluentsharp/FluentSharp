using System;
using System.Drawing;
using System.Collections.Generic;
using Merlin;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Views.ASCX.CoreControls;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;

namespace O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods
{
    public static class EX_O2_Ascx
    {
        public static ascx_Directory add_Directory(this IStep step)
        {
            return step.add_Directory(PublicDI.config.O2TempDir);
        }

        public static ascx_Directory add_Directory(this IStep step, string startDirectory)
        {
           	var directory = new ascx_Directory();
			directory.Dock = DockStyle.Fill;
			directory._HideFiles = true;
			directory._ViewMode = ascx_Directory.ViewMode.Simple_With_LocationBar;
			directory.AllowDrop = false;
			
            directory.openDirectory(startDirectory);
            directory.refreshDirectoryView();
            directory._WatchFolder = true;
            step.add_Control(directory);
            return directory;
        }


        public static IStep add_Directory(this List<IStep> steps, string stepName)
        {
            return steps.add_Directory(stepName, PublicDI.config.O2TempDir);
        }

        public static IStep add_Directory(this List<IStep> steps, string stepName, string startDirectory)
        {

            var newStep = steps.add_Panel(stepName);// new TemplateStep(new Panel(), 0, stepName);

            var directory = newStep.add_Directory(startDirectory);

            newStep.OnComponentLoad += step => directory.refreshDirectoryView();

            steps.Add(newStep);
            return newStep;
        }

        public static IStep add_Message(this List<IStep> steps, string stepTitle)
        {
            return steps.add_Message(stepTitle, "");
        }

        public static IStep add_Message(this List<IStep> steps, string stepTitle, Func<string> messageToAdd)
        {
            //var message = messageToAdd();
            var initialMessage = "initial message: ";
            var newStep = Ex_Windows_Forms.createStepWith_TextBox(stepTitle, initialMessage);
            newStep.OnComponentAction =
                (step) =>
                {
                    step.set_Text(messageToAdd());
                };
            steps.Add(newStep);
            return newStep;
        }

        public static IStep add_Message(this List<IStep> steps, string stepTitle, string message)
        {
            var newStep = Ex_Windows_Forms.createStepWith_TextBox(stepTitle, message);
            steps.Add(newStep);
            return newStep;
        }

        public static IStep add_Action(this List<IStep> steps, string stepTitle, Action<IStep> action)
        {
            var textBox = Ex_Windows_Forms.create_TextBox("");
            var newStep = Ex_Windows_Forms.createStepWith_TextBox(stepTitle, textBox);

            //newStep.NextHandler = ()=>  action(textBox);
            newStep.OnComponentAction = action;
            steps.Add(newStep);
            return newStep;
        }


        public static IStep add_Control(this List<IStep> steps, Type controlType, string stepTitle, string stepSubTitle)
        {
            return add_Control(steps, controlType, stepTitle, stepSubTitle, null);
        }

        public static IStep add_Control(this List<IStep> steps, Type controlType, string stepTitle, string stepSubTitle, Action<IStep> onComponentLoad)
        {
            //control.AllowDrop = false;
            var newStep = new TemplateStep(controlType);//, 10, stepTitle);
            newStep.Title = stepTitle;
            newStep.Subtitle = stepSubTitle;
            newStep.OnComponentAction = onComponentLoad;
            steps.Add(newStep);
            return newStep;
        }

        public static IStep add_Control(this List<IStep> steps, Control control, string stepTitle, string stepSubTitle)
        {
            return add_Control(steps, control, stepTitle, stepSubTitle, null);
        }

        public static IStep add_Control(this List<IStep> steps, Control control, string stepTitle, string stepSubTitle, Action<IStep> onComponentLoad)
        {
            control.AllowDrop = false;
            var newStep = new TemplateStep(control, 10, stepTitle);
			newStep.OnComponentAction = onComponentLoad;
			newStep.Subtitle = stepSubTitle;
            steps.Add(newStep);
            return newStep;
        }

        public static Control add_Control(this IStep step, Type controlType)
        {
            if (step.UI != null)
            {
                return (Control)step.UI.invokeOnThread(
                    () =>
                    {
                        var control = (Control)PublicDI.reflection.createObjectUsingDefaultConstructor(controlType);
                        if (control != null)
                            step.add_Control(control);
                        return control;
                    });
            }
            PublicDI.log.error("was null");
            return null;
        }

        public static Control add_Control(this IStep step, Control control)
        {
            if (step.UI == null)
                return null;
            return (Control) step.UI.invokeOnThread(
                                 () =>
                                     {
                                         step.UI.Controls.Add(control);
                                         control.BringToFront();
                                         return control;
                                     });
        }

        public static IStep add_SelectFile(this List<IStep> steps, string stepTitle, string defaultFolder, Action<string> setResult, Action<TextBox, Button> callbackWithControls)
        {
            return steps.add_SelectFile(stepTitle, defaultFolder, -1, -1, -1, setResult, callbackWithControls);
        }

        public static IStep add_SelectFile(this List<IStep> steps, string stepTitle, string defaultFolder, int top, int textBoxLeft, int textBoxWidth, Action<string> setResult, Action<TextBox, Button> callbackWithControls)
        {
            // textbox
            var textBox = new TextBox();
            textBox.TextChanged += (sender, e) => setResult(textBox.Text);
            textBox.Text = defaultFolder;
            if (top > -1)
                textBox.Top = top;
            if (textBoxLeft > -1)
                textBox.Left = textBoxLeft;
            textBox.Width = (textBoxWidth > -1) ? textBoxWidth : 90;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            textBox.AllowDrop = true;
            textBox.DragDrop += (sender, e) => textBox.set_Text(Dnd.tryToGetFileOrDirectoryFromDroppedObject(e));
            textBox.DragEnter += (sender, e) => e.Effect = DragDropEffects.Copy;

            // button
            var button = new Button();
            if (top > -1)
                button.Top = top;
            button.Top -= 2;
            button.Text = "Select File";
            button.Width += 20;
            button.Left = textBox.Width + textBox.Left + 2;
            button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            button.Click += (sender, e) =>
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = defaultFolder;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = openFileDialog.FileName;
                    openFileDialog.Dispose();
                }
            };

            // panel
            var step = steps.add_Panel("stepTitle");
            //var panel = new FlowLayoutPanel();
            //panel.Dock = DockStyle.Fill;// AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            step.add_Control(textBox);
            step.add_Control(button);
            callbackWithControls(textBox, button);
            //var newStep = new TemplateStep(panel, 10, stepTitle);
            //steps.Add(newStep);
            return step;
        }  

        public static IStep add_SelectFolder(this List<IStep> steps, string stepTitle, string defaultFolder, Action<string> setResult)
        {
            // textbox
            var textBox = new TextBox();
            textBox.TextChanged += (sender, e) =>
            {
                setResult(textBox.Text);
                PublicDI.log.info("in TextChanged");
            };
            textBox.Text = defaultFolder;
            textBox.Width = 400;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            textBox.AllowDrop = true;
            textBox.DragDrop += (sender, e) => textBox.set_Text(Dnd.tryToGetFileOrDirectoryFromDroppedObject(e));
            textBox.DragEnter += (sender, e) => e.Effect = DragDropEffects.Copy;

            // button
            var button = new Button();
            button.Text = "Select Folder";
            button.Width += 20;
            button.Click += (sender, e) =>
            {
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath = defaultFolder;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = folderBrowserDialog.SelectedPath;
                    folderBrowserDialog.Dispose();
                }
            };

            // panel
            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;// AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            panel.Controls.Add(textBox);
            panel.Controls.Add(button);

            var newStep = new TemplateStep(panel, 10, stepTitle);
            steps.Add(newStep);
            return newStep;
        }

        public static IStep add_FlowLayoutPanel(this List<IStep> steps, string stepTitle, string stepSubTitle)
        {
            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            var textBox = new TextBox();
            textBox.Text = "asdas";
            panel.Controls.Add(textBox);
            return steps.add_Control(panel, stepTitle, stepSubTitle);
        }

        /*public static IStep add_WebBrowserAndTextBox(this List<IStep> steps, string stepTitle, string stepSubTitle, Action<IStep> onComponentLoad)
        {
            var hostPanel = new Panel();
            var splitControl = hostPanel.add_SplitContainer(
                                false, 		//setOrientationToHorizontal
                                true,		// setDockStyleoFill
                                true);		// setBorderStyleTo3D)
            splitControl.Panel1.add_GroupBox("Webpage");
            splitControl.Panel2.add_GroupBox("Analysis");

            splitControl.SplitterDistance = 100;
            return steps.add_Control(hostPanel, stepTitle, stepSubTitle, onComponentLoad);
        }*/

        public static Label add_Label(this IStep step, string text)
        {
            return step.add_Label(text, -1, -1);
        }

        public static Label add_Label(this IStep step, string text, int top)
        {
            return step.add_Label(text, top, -1);
        }

        public static Label add_Label(this IStep step, string text, int top, int left)
        {            
            if (step.UI == null)
                return null;
            return (Label) step.UI.invokeOnThread(
                               () =>
                                   {
                                       var label = new Label();
                                       label.AutoSize = true;
                                       label.Text = text;
                                       if (top > -1)
                                           label.Top = top;
                                       if (left > -1)
                                           label.Left = left;
                                       step.UI.Controls.Add(label);
                                       label.AutoSize = true;
                                       label.BringToFront();
                                       return label;
                                   });
        }

        public static LinkLabel add_Link(this IStep step, string text, int top, int left, MethodInvoker onClick)
        {
            var link = new LinkLabel();
            link.AutoSize = true;
            link.Text = text;
            link.Top = top;
            link.Left = left;
            link.LinkClicked += (sender, e) => { if (onClick != null) onClick(); };
            step.add_Control(link);
            return link;
        }

        public static TextBox add_DropArea(this IStep step, int left, int top, int width, int height, Action<string> onDroppedFile)
        {
            var colorWhenNotReadyToDrop = Color.LightCoral;
            var colorWhenReadyToDrop = Color.LightGreen;
            var dropArea = new TextBox();
            dropArea.Multiline = true;
            dropArea.BackColor = colorWhenNotReadyToDrop;
            dropArea.Left = left;
            dropArea.Top = top;
            dropArea.Width = width;
            dropArea.Height = height;
            dropArea.AllowDrop = true;

            dropArea.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            dropArea.DragDrop += (sender, e) =>
            {
                var fileOrDirectory = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                dropArea.set_Text(fileOrDirectory);
                onDroppedFile(fileOrDirectory);
            };
            dropArea.DragEnter += (sender, e) =>
            {
                e.Effect = DragDropEffects.Copy;
                dropArea.BackColor = colorWhenReadyToDrop;
            };
            dropArea.DragLeave += (sender, e) => dropArea.BackColor = colorWhenNotReadyToDrop;

            step.add_Control(dropArea);

            return dropArea;
        }

        public static TextBox add_TextBox(this IStep step)
        {
            if (step.UI == null)
                return null;
            var textBox = new TextBox();
            textBox.DeselectAll();
            step.UI.Controls.Add(textBox);
            textBox.BringToFront();
            return textBox;
        }

        public static TextBox add_TextBox(this IStep step, string originalText, int top)
        {
            return add_TextBox(step, originalText, top, -1, -1);
        }

        public static TextBox add_TextBox(this IStep step, string originalText, int top, int left)
        {
            return add_TextBox(step, originalText, top, left, -1);
        }

        public static TextBox add_TextBox(this IStep step, string originalText, int top, int left, int width)
        {
            var textBox = step.add_TextBox();
            if (textBox != null)
            {
                textBox.Text = originalText;
                textBox.DeselectAll();
                //textBox.Height = height;       	
                textBox.Top = top;
                if (left > -1)
                    textBox.Left = left;
                if (width > -1)
                    textBox.Width = width;
            }
            return textBox;

        }

        public static Label append_Label(this Control targetControl, string originalText)
        {
            return targetControl.append_Label(originalText, false);
        }

        public static Label append_Label(this Control targetControl, string originalText, bool appendBelow)
        {
            var newLabel = new Label();
            newLabel.Text = originalText;
            targetControl.append_Control(newLabel, appendBelow);
            return newLabel;
        }

        public static TextBox append_TextBox(this Control targetControl, string originalText)
        {
            return targetControl.append_TextBox(originalText, null, false);
        }

        public static TextBox append_TextBox(this Control targetControl, string originalText, Action<string> onTextChange)
        {
            return targetControl.append_TextBox(originalText, onTextChange, false);
        }

        public static TextBox append_TextBox(this Control targetControl, string originalText, Action<string> onTextChange, bool appendBelow)
        {

            var newTextBox = new TextBox();
            newTextBox.Text = originalText;
            if (onTextChange != null)
                newTextBox.TextChanged += (sender, e) => onTextChange(newTextBox.Text);
            targetControl.append_Control(newTextBox, appendBelow);
            return newTextBox;
        }

        public static void append_Control(this Control controlToAppend, Control newControl)
        {
            controlToAppend.append_Control(newControl, false);
        }

        public static void append_Control(this Control controlToAppend, Control newControl, bool appendBelow)
        {
            if (controlToAppend.Parent != null && newControl != null)
            {
                if (appendBelow)
                {
                    newControl.Left = controlToAppend.Left;
                    newControl.Top = controlToAppend.Top + controlToAppend.Height;
                }
                else
                {
                    newControl.Left = controlToAppend.Left + controlToAppend.Width;
                    newControl.Top = controlToAppend.Top;
                }
                controlToAppend.Parent.Controls.Add(newControl);
                controlToAppend.BringToFront();
            }
            else
                PublicDI.log.error("in append_Control, controlToAppend.Parent == null or newControl == null");
        }

        public static IStep add_Panel(this List<IStep> steps, string stepTitle)
        {
            return steps.add_Panel(stepTitle, "", null);
        }
        public static IStep add_Panel(this List<IStep> steps, string stepTitle, string stepSubTitle, Action<IStep> onComponentLoad)
        {
            Panel panel = new Panel();
            var newStep = new TemplateStep(panel, stepTitle);

            newStep.Subtitle = stepSubTitle;
            newStep.OnComponentAction = onComponentLoad;
            steps.Add(newStep);
            return newStep;
        }

        public static void clear(this IStep step)
        {
            if (step.UI != null)
            {
                PublicDI.log.info("clearing");
                step.UI.invokeOnThread(
                    () =>
                    {
                        step.UI.Controls.Clear();
                        return;
                    });
            }
            else
                PublicDI.log.error("was null");
        }

    }
}
