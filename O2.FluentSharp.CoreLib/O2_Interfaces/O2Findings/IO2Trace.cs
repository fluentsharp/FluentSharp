// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2Trace
    {
        List<IO2Trace> childTraces { get; set; }
        string caller { get; set; }
        string clazz { get; set; }
        uint columnNumber { get; set; }
        string context { get; set; }
        string file { get; set; }
        uint lineNumber { get; set; }
        string method { get; set; }
        uint ordinal { get; set; }
        string signature { get; set; }
        uint taintPropagation { get; set; }
        List<String> text { get; set; }
        TraceType traceType { get; set; }
        uint argument { get; set; }
        uint direction { get; set; }                    
        string ToString();
    }

    [Serializable]
    public class KO2Trace : IO2Trace
    {
        public List<IO2Trace> childTraces { get; set; }
        public string caller { get; set; }
        public string clazz { get; set; }
        public uint columnNumber { get; set; }
        public string context { get; set; }
        public string file { get; set; }
        public uint lineNumber { get; set; }
        public string method { get; set; }
        public uint ordinal { get; set; }
        public string signature { get; set; }
        public uint taintPropagation { get; set; }
        public List<string> text { get; set; }
        public TraceType traceType { get; set; }
        public uint argument { get; set; }
        public uint direction { get; set; }

        public KO2Trace()
        {
            childTraces = new List<IO2Trace>();
            caller = "";
            clazz = "";
            context = "";
            file = "";
            method = "";
            signature = "";
            text = new List<string>();
            traceType = TraceType.Type_0;
        }
    }
}