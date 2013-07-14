// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class BTask : ITask
    {
        private TaskStatus _taskStatus;

        #region ITask Members

        public string taskName { get; set; }
        public Type sourceType { get; set; }
        public Type resultsType { get; set; }
        public object sourceObject { get; set; }
        public object resultsObject { get; set; }
        //public virtual ascx_Task taskControl { get; set; }
        public virtual ITaskControl taskControl { get; set; }

        public virtual bool execute()
        {
            PublicDI.log.error("in BTask execute method was invoked and it should had been implemented by current Task");
            return false;
        }

        public void setProgressBarMaximum(int value)
        {
            if (taskControl != null)
                taskControl.setProgressBarMaximum(value);
        }

        public void setProgressBarValue(int value)
        {
            if (taskControl != null)
                taskControl.setProgressBarValue(value);
        }

        public void incProgressBarValue()
        {
            if (taskControl != null)
                taskControl.incProgressBarValue();
        }

        public string getTaskName()
        {
            return taskName ?? GetType().ToString();
        }

        public virtual bool readyToStart()
        {
            return true;
        }


        public TaskStatus taskStatus
        {
            set
            {
                _taskStatus = value;
                // set taskStatus on the host control
                //            if (taskControl != null)
                //                taskControl.setTaskStatus(value);
                // invoke events
                if (onTaskStatusChange != null)
                    onTaskStatusChange(this, value);
                if (taskStatus == TaskStatus.Completed_Ok || taskStatus == TaskStatus.Completed_Failed)
                    if (onTaskExecutionCompletion != null)
                        onTaskExecutionCompletion(resultsObject);
            }
            get { return _taskStatus; }
        }

        public event TaskEvents.TaskEvent_StatusChanged onTaskStatusChange;

        public event TaskEvents.TaskEvent_ResultsObject onTaskExecutionCompletion;

        #endregion
    }
}
