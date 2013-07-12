using System;
using System.IO;
using FluentSharp.CoreLib;
using NGit;

namespace FluentSharp.Git.Utils
{
    public class GitProgress : TextProgressMonitor
    {
        public Action<string, string, int> onMessage { get; set; }

        public StringWriter FullMessage { get; set; }
        public bool         Cancel      { get; set; }

        public bool         Log_BeginTask { get; set; }
        public bool         Log_Start     { get; set; }
        public bool         Log_Update    { get; set; }
        public bool         Log_EndTask   { get; set; }

        public GitProgress() : this(new StringWriter())
        {
            Log_BeginTask = true;
        }

        public GitProgress(StringWriter stringWriter) : base(stringWriter)
        {
            FullMessage = stringWriter;

            onMessage = showMessageInLogViewer;
        }

        public void showMessageInLogViewer(string type, string taskName, int workCurr)
        {
            FullMessage.WriteLine("[GitProgress] {0} : {1} : {2}".info(type, taskName, workCurr));
        }

        public override void BeginTask(string title, int work)
        {
            if (Log_BeginTask)
                onMessage("BeginTask", title, work);
            base.BeginTask(title, work);
        }

        public override void Start(int totalTasks)
        {
            if (Log_Start)
                onMessage("Start","",totalTasks);
            base.Start(totalTasks);
        }
        public override void Update(int completed)
        {
            if (Log_Update)
                onMessage("Update", "", completed);
            base.Update(completed);
        }

        public override void EndTask()
        {
            if (Log_EndTask)
                onMessage("EndTask", "", -1);
            base.EndTask();
        }
        public override bool IsCancelled()
        {
            return Cancel;
        }
    }
}