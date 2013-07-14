// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class KO2MessageQueue : IO2MessageQueue
    {
        public static KO2MessageQueue o2MessageQueue = new KO2MessageQueue();

        private KO2MessageQueue()
        {
          //  onMessages += o2Message => PublicDI.log.i("in KO2MessageQueue: message Received:" + o2Message.messageText);
        }

        public static KO2MessageQueue getO2KernelQueue()
        {
            return o2MessageQueue;
        }

        public event Action<IO2Message> onMessages;

        public Thread sendMessage(string messageText)
        {
            var o2Message = new KO2GenericMessage(messageText);
            return sendMessage(o2Message);
        }

        public Thread sendMessage(IO2Message messageToSend)
        {
            return Callbacks.raiseRegistedCallbacks(onMessages, new object[] { messageToSend });
        }

        public void sendMessageSync(IO2Message messageToSend)
        {
            var messageSent = new AutoResetEvent(false);
            O2Thread.mtaThread((() =>
                                                        {
                                                            var messageThread = sendMessage(messageToSend);
                                                            messageThread.Join();
                                                            messageSent.Set();
                                                        }));
            messageSent.WaitOne();
        }

        //public KO2MessageQueue()
        //{
        //    
        //                  
        //}
    }
}
