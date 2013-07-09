// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
namespace FluentSharp.CoreLib.Interfaces
{
    public interface ITaskThread
    {
        void setTaskControl(ITaskControl taskControl);
        void start();
        void wait(double secondsToWait);
        ITask getTask();
        bool isTaskCompleted();
    }
}