// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ITask
    {
        string taskName { get; set; }
        object sourceObject { get; set; }
        Type sourceType { get; set; }
        object resultsObject { get; }
        Type resultsType { get; set; }
        ITaskControl taskControl { get; set; }
        TaskStatus taskStatus { get; set; }

        bool execute();
        //bool executeSync();
        //void start();
        //void pause();
        //void resume();
        //void stop();

        bool readyToStart();

        void setProgressBarValue(int value);
        void setProgressBarMaximum(int value);
        void incProgressBarValue();
        string getTaskName();

        event TaskEvents.TaskEvent_StatusChanged onTaskStatusChange;
        event TaskEvents.TaskEvent_ResultsObject onTaskExecutionCompletion;
    }
}