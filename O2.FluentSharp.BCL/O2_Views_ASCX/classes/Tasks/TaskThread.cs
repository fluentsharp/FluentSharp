// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using O2.Interfaces.Tasks;
using O2.Views.ASCX;

namespace O2.Views.ASCX.classes.Tasks
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

        public void setTaskControl(ITaskControl _taskControl)
        {
            if (_taskControl != null)
            {
                taskControl = _taskControl;
                taskControl.setStartLinkVisibleStatus(true);
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
                task.taskStatus = task.execute() ? TaskStatus.Completed_OK : TaskStatus.Completed_Failed;
                if (taskControl != null)                
                    taskControl.setProgressBarValue(taskControl.getProgressBarMaximum());
            }
            catch (Exception ex)
            {
                DI.log.ex(ex);
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
            return (task.taskStatus == TaskStatus.Completed_OK || task.taskStatus == TaskStatus.Completed_Failed);
        }        
    }
}
