// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_Task : UserControl, ITaskControl
    {
        public bool configVisible;
        public int executionTimeSeconds;
        public bool onCompletionRemoteTaskFromParentControl = true;
        public string taskName;
        public ITaskThread taskThread;

        public ascx_Task(ITaskThread _taskThread)
        {
            InitializeComponent();
            try
            {
                taskThread = _taskThread;
                taskThread.setTaskControl(this);
                setTaskName(taskThread.getTask().getTaskName());
                setConfigVisibleStatus(false);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in ascx_Task()");
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public string _TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;
                setTaskGroupBoxText(taskName);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowConfig
        {
            get { return configVisible; }
            set { setConfigVisibleStatus(value); }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public int _ProgressBarMaximum
        {
            get { return taskProgressBar.Maximum; }
            set { taskProgressBar.Maximum = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public int _ProgressBarValue
        {
            get { return taskProgressBar.Value; }
            set { taskProgressBar.Value = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _startTask
        {
            get { return false; }
            set { if (value) startTask(); }
        }

        public void setConfigVisibleStatus(bool value)
        {
            configVisible = value;
            Height = configVisible ? 150 : 50;
        }

        public void setTaskGroupBoxText(string value)
        {
            if (taskGroupBox.InvokeRequired)
                taskGroupBox.Invoke(new EventHandler(delegate { setTaskGroupBoxText(value); }));
            else
                taskGroupBox.Text = value;
        }

        private void llConfig_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            setConfigVisibleStatus(!configVisible);
        }

        private void llStartStop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            startTask();
        }

        /*public Control getControl()
        {
            throw new System.NotImplementedException();
        }*/

        public void startTask()
        {
            if (taskThread != null)
                new Thread(taskThread.start).Start();
        }

        public void setResulsObject(object value)
        {
            if (lbResulsObject.InvokeRequired)
                lbResulsObject.Invoke(new EventHandler(delegate { setResulsObject(value); }));
            else
                lbResulsObject.Visible = true;
        }

        /*     private void lbResulsObject_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop("Results Object", DragDropEffects.Copy);
        }

        private void lbSourceObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        */
   

        public void setProgressBarMaximum(int value)
        {
            if (taskProgressBar.InvokeRequired)
                taskProgressBar.Invoke(new EventHandler((sender, e) => setProgressBarMaximum(value)));
            else
                taskProgressBar.Value = value;
        }

        public int getProgressBarMaximum()
        {
            //  if (taskProgressBar.InvokeRequired)
            //      taskProgressBar.Invoke(new EventHandler((sender, e) => getProgressBarMaximum()));
            //  else
            return taskProgressBar.Maximum;
            //  return 0;
        }

        public void setProgressBarValue(int value)
        {
            if (taskProgressBar.InvokeRequired)
                taskProgressBar.Invoke(new EventHandler((sender, e) => setProgressBarValue(value)));
            else
                taskProgressBar.Value = value;
        }

        public void setTaskStatus(TaskStatus taskStatus)
        {
            if (lbTaskStatus.InvokeRequired)
                lbTaskStatus.Invoke(new EventHandler((sender, e) => setTaskStatus(taskStatus)));
            else
            {
                lbTaskStatus.Text = taskStatus.ToString();
                switch (taskStatus)
                {
                    case TaskStatus.Completed_Ok:
                        setResultText("OK", Color.Green);
                        setProgressBarValue(taskProgressBar.Maximum);
                        break;
                    case TaskStatus.Completed_Failed:
                        setResultText("Fail", Color.Red);
                        break;
                    default:
                        setResultText("...", Color.Orange);
                        break;
                }
            }
        }

        public void incProgressBarValue()
        {
            setProgressBarMaximum(taskProgressBar.Value + 1);
        }

        public void setStartLinkVisibleStatus(bool value)
        {
            llStartStop.Visible = value;
        }

        public void setResultText(string resultText, Color color)
        {
            if (lbResultText.InvokeRequired)
                lbResultText.Invoke(new EventHandler((sender, e) => setResultText(resultText, color)));
            else
            {
                lbResultText.Text = resultText;
                lbResultText.ForeColor = color;
                Application.DoEvents();
            }
        }

        public void removeTaskFromParentControl(int secondsToWait)
        {
            if (Parent != null)
            {
                if (Parent.InvokeRequired)
                    Parent.Invoke(new EventHandler((sender, e) => removeTaskFromParentControl(secondsToWait)));
                else
                {
                    Thread.Sleep(1000*1);
                    Control parentControl = O2Forms.findParentThatHostsControl(this);
                    if (parentControl != null)
                        parentControl.Controls.Remove(this);
                }
            }
        }

        public void setTaskName(string name)
        {
            if (taskGroupBox.InvokeRequired)
                taskGroupBox.Invoke(new EventHandler((sender, e) => setTaskName(name)));
            else
                taskGroupBox.Text = name;
        }


        public void incExecutionSecondsCounter()
        {
            if (lbExecutionSecondsCounter.InvokeRequired)
                lbExecutionSecondsCounter.Invoke(new EventHandler((sender, e) => incExecutionSecondsCounter()));
            else
                lbExecutionSecondsCounter.Text = executionTimeSeconds++ + " s";
        }

        private void executionTimeCounterThread()
        {
            while (false == taskThread.isTaskCompleted())
            {
                Thread.Sleep(1000);
                incExecutionSecondsCounter();
                //   incProgressBarValue();
            }
            if (onCompletionRemoteTaskFromParentControl)
                removeTaskFromParentControl(1);
        }

        public void startExecutionTimeCounterThread()
        {
            new Thread(executionTimeCounterThread).Start();
        }
    }
}
