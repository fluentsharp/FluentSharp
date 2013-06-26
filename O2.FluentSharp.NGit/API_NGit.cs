// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using FluentSharp.ExtensionMethods;
using NGit.Api;
using NGit;
using NGit.Transport;


namespace FluentSharp
{
    public class API_NGit
    {
        public string               Path_Local_Repository { get; set; }
        public Git                  Git                   { get; set; }        
        public Repository           Repository            { get; set; }
        public GitProgress          LastGitProgress       { get; set; }
        public CredentialsProvider  Credentials           { get; set; }
        public Exception            LastException         { get; set; }           
    }

    public class GitProgress : TextProgressMonitor
    {
        public Action<string, string, int> onMessage { get; set; }
        public StringWriter                FullMessage { get; set; }

        public GitProgress()                             : this(new StringWriter())
        {
        }
        public GitProgress(StringWriter stringWriter)    : base(stringWriter)
        {
            FullMessage = stringWriter;

            onMessage = (type, taskName, workCurr) => "[GitProgress] {0} : {1} : {2}".info(type, taskName, workCurr);
        }

        
        public override void BeginTask(string title, int work)
        {
            onMessage("BeginTask", title, work);
            base.BeginTask(title, work);
        }

        /*   public override void Start(int totalTasks)
        {
            //onMessage("Start","",totalTasks);
            base.Start(totalTasks);
        }*/
        /*    public override void Update(int completed)
        {
            //onMessage("Update", "", completed);
            base.Update(completed);
        }
        public override void EndTask()
        {
            //onMessage("EndTask", "", -1);
            base.EndTask();
        }*/
    }
    
}
