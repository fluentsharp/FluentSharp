// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class TaskThread : ITaskThread
    {
        private ITaskControl taskControl;
        //public TaskStatus taskstatus = TaskStatus.Ready;
        private readonly ITask task;

        public TaskThread(ITask _task)
        {
            task = _task;
            task.taskStatus = TaskStatus.Ready;
        }

        public void setTaskControl(ITaskControl taskControl)
        {
            if (taskControl != null)
            {
                this.taskControl = taskControl;
                this.taskControl.setStartLinkVisibleStatus(true);
            }
        }
       

        public void start()
        {
            try
            {                
                task.taskStatus = TaskStatus.Executing;
                if (taskControl != null)
                {
                    taskControl.setProgressBarValue(0);
                    taskControl.startExecutionTimeCounterThread();
                }
                task.taskStatus = task.execute() ? TaskStatus.Completed_Ok : TaskStatus.Completed_Failed;
                if (taskControl != null)                
                    taskControl.setProgressBarValue(taskControl.getProgressBarMaximum());
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
            }
        }


        public void wait(double secondsToWait)
        {
            task.taskStatus = TaskStatus.Waiting;
            Thread.Sleep((int) (1000*secondsToWait));
        }


        public ITask getTask()
        {
            return task;
        }


        public bool isTaskCompleted()
        {
            return (task.taskStatus == TaskStatus.Completed_Ok || task.taskStatus == TaskStatus.Completed_Failed);
        }        
    }
}
