// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    public class KO2Message : IO2Message
    {
        public Guid messageGUID { get; set; }
        public string messageText { get; set; }
        public object returnData { get; set; }        

        public KO2Message()
        {
            messageGUID = new Guid();
            messageText = "KO2Message";            
        }

    }
}
