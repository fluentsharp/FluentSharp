// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.O2Findings
{
    [Serializable]
    public class O2Trace : KO2Trace
    {       

        public O2Trace()
        {
         
        }

        public O2Trace(String clazz) : this()
        {
            this.clazz = clazz;
            signature = clazz; // also make the signature default to this value
        }

        public O2Trace(String clazz, TraceType traceType) : this(clazz)
        {
            this.traceType = traceType;
        }

        public O2Trace(String clazz, String method) : this(clazz)
        {
            this.method = method;
        }

        public O2Trace(String clazz, String method, TraceType traceType) : this(clazz, method)
        {
            this.traceType = traceType;
        }

       
        public override string ToString()
        {
            return signature  ?? "";  // to deal with '...Attempted to read or write protected memory..' issue ;
        }

        public string SourceCode
        {
            get
            {
                if (file == "" || lineNumber == 0)
                    return "";
                return Files_WinForms.getLineFromSourceCode(file, lineNumber);                
            }
            set { }
        }

        public IO2Trace addTrace_IfNotEmpty(string prefix, string signature)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(signature))
                return null;
            return addTrace(prefix + signature);
        }

        public IO2Trace addTrace(string traceSignature)
        {
            return addTrace(traceSignature, TraceType.Type_0);
        }

        public IO2Trace addTrace(string traceSignature, TraceType _traceType)
        {            
            var newTrace = new O2Trace(traceSignature);
			newTrace.traceType = _traceType;
            childTraces.Add(newTrace);
            return newTrace;
        }

        public List<IO2Trace> addTraces(params string[] traceSignatures)
        {
            return addTraces("", TraceType.Type_0, traceSignatures);
        }

        public List<IO2Trace> addTraces(string prefixText, TraceType _traceType, params string[] traceSignatures)
        {            
            var newTraces = new List<IO2Trace>();
            foreach (var traceSignature in traceSignatures)
            {
                var newTrace = new O2Trace(prefixText + traceSignature);
                newTrace.traceType = _traceType;
                childTraces.Add(newTrace);
                newTraces.Add(newTrace);
            }

            return newTraces;
        }

        public List<IO2Trace> AddTraces(List<string> traceSignatures)
        {
            return addTraces(traceSignatures.ToArray());
        }
    }
}
