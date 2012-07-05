using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using O2.DotNetWrappers.DotNet;

namespace O2.XRules.Database._Rules.APIs.Tasks
{
    public class ascx_SimpleTaskGui : Control
    {
        public string TaskType { get; set; }
        public string TaskName { get; set; }

        public bool TaskEnabled { get; set; }

        public int StartWidth { get; set; }
        public int StartHeight { get; set; }
        public int StatusBoxWidth { get; set; }
        public int SleepPeriod { get; set; }
        public int TaskCount { get; set; }
        public bool ExecuteOnlyOnce { get; set; }

        public GroupBox groupBox;
        public LinkLabel startLinkLabel;
        public LinkLabel stopLinkLabel;
        public Label messageLabel;
        public Panel failPanel;
        public Panel okPanel;
        public Thread taskThread;

        public Func<bool> TaskFunction;

        public ascx_SimpleTaskGui()
        {
            TaskName = "";
            StartWidth = 250;
            StartHeight = 60;
            StatusBoxWidth = 20;
            SleepPeriod = 2000;
            TaskEnabled = true;
            TaskCount = 0;
            ExecuteOnlyOnce = false;
            //buildGui();

        }

        public void buildGui(string taskType, string taskName, Func<bool> taskFunction)
        {
            TaskType = taskType;
            TaskName = taskName;
            TaskFunction = taskFunction;
            buildGui();
        }

        public void buildGui()
        {
            this.fill(false);
            this.width(StartWidth);
            this.height(StartHeight);

            groupBox = this.add_GroupBox(TaskType);
            var label = groupBox.add_Control<Label>();
            var linksPanel = label.insert_Below<Panel>();
            failPanel = label.insert_Right<Panel>(StatusBoxWidth).backColor(Color.Red);
            okPanel = label.insert_Right<Panel>(StatusBoxWidth).backColor(Color.Green);


            startLinkLabel = linksPanel.add_Link("start", 0, 0, start).enabled(true);
            stopLinkLabel = linksPanel.add_Link("stop", 0, 30, stop).enabled(false);
            messageLabel = linksPanel.add_Label("", 0, 60).visible(false);

            groupBox.set_Text(TaskType);
            label.set_Text(TaskName);
        }


        public void start()
        {
            TaskEnabled = true;
            if (TaskFunction == null)
            {
                "in ascx_SimpleTaskGui, TaskFunction not set".error();
                return;
            }
            startLinkLabel.enabled(false);
            stopLinkLabel.enabled(true);
            taskThread = O2Thread.mtaThread(executionThread);
            this.focus();
        }


        public void stop()
        {
            O2Thread.mtaThread(
            () =>
            {
                TaskEnabled = false;
                taskThread.Join(SleepPeriod);	// give it time to finish    			
                if (taskThread.ThreadState != ThreadState.Stopped)
                {
                    "Thread was still alive, so aborting it".error();
                    taskThread.Abort();
                }
                startLinkLabel.enabled(true);
                stopLinkLabel.enabled(false);
                messageLabel.visible(false);
                this.focus();
            });
        }



        public void executionThread()
        {
            try
            {
                while (TaskEnabled)
                {
                    messageLabel.visible(false);
                    resetOkFailPanelColors();

                    100.sleep();    			    			// to have a small blink on the Gui

                    if (TaskFunction())
                        onOk();
                    else
                        onFail();
                    if (ExecuteOnlyOnce)
                        break;
                    groupBox.set_Text("{0}: waiting {1}s ({2})".format(TaskType, SleepPeriod / 1000, TaskCount++));
                    messageLabel.visible(true);
                    SleepPeriod.sleep();
                }
                "executionThread completed".debug();
                stop();
            }
            catch (Exception ex)
            {
                ex.log("in ascx_SimpleTaskGui.executionThread");
            }
        }

        public void resetOkFailPanelColors()
        {
            failPanel.backColor("Control");
            okPanel.backColor("Control");
        }

        public void onOk()
        {
            okPanel.backColor(Color.Green);
        }

        public void onFail()
        {
            failPanel.backColor(Color.Red);
        }
    }
}
