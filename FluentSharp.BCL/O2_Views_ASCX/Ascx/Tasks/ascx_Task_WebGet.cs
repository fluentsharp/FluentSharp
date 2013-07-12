using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.BCL.Controls
{
    public class ascx_Task_WebGet : ascx_SimpleTaskGui
    {

        public ascx_Task_WebGet()
        {
            this.TaskType = "Web (GET request)";
            this.TaskName = "...";
            this.TaskFunction = () =>
            {
                var response = new Web().getUrlContents(TaskName);
                messageLabel.set_Text("response size: {0}".format(response.size()));
                return response != "";
            };
        }

        public ascx_Task_WebGet start(string target)
        {
            this.clear();
            this.TaskName = target.uri().str();
            this.buildGui();
            this.start();
            return this;
        }
    }
}
