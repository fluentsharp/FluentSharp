// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.WinForms.Interfaces;

//O2File:KO2Message.cs

namespace FluentSharp.WinForms.Utils
{
    public class KO2GenericMessage : KO2Message, IO2Message
    {                
        public string messageSugestedTarget { get; set; }
        public IO2Message inResponseToMessage { get; set; }
        public List<object> messageData { get; set; }
        public bool handled { get; set; }

        public KO2GenericMessage()
        {                   
            messageText = "";
            messageSugestedTarget = "";
            inResponseToMessage = null;
            messageData = new List<object>();
            handled = false;
        }

        public KO2GenericMessage(string _messageText) : this ()
        {
            messageText = _messageText;
        }
        public KO2GenericMessage(string _messageText, List<object> _messageData)
            : this()
        {
            messageText = _messageText;            
            messageData = _messageData;
        }
    }
}
