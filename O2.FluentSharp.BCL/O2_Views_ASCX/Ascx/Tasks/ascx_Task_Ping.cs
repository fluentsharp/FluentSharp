using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.Network;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
//O2File:ascx_SimpleTaskGui.cs

namespace O2.XRules.Database._Rules.APIs.Tasks
{
    public class ascx_Task_Ping : ascx_SimpleTaskGui
    {
        public ascx_Task_Ping()
        {
            this.TaskType = "Ping";
            this.TaskName = "...";
            this.TaskFunction = () =>
            {
                var ping = new Ping();
                return ping.ping(TaskName);
            };
        }

        public ascx_Task_Ping start(string target)
        {
            this.clear();
            this.TaskName = target;
            this.buildGui();
            this.start();
            return this;
        }

    }
}
