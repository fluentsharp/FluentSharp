// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;

namespace FluentSharp.WinForms.Interfaces
{
    public interface IO2MessageQueue
    {
        event Action<IO2Message> onMessages;

        Thread sendMessage(string messageText);
        Thread sendMessage(IO2Message messageToSend);

        void sendMessageSync(IO2Message messageToSend);
    }
}