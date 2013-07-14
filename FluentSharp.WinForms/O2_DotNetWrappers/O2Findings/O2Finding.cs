// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.O2Findings
{
    [Serializable] 
    public class O2Finding : IO2Finding
    {           
        // implementation of interface properties
        public uint actionObject { get; set; }
        public string callerName { get; set; }
        public uint columnNumber { get; set; }
        public byte confidence { get; set; }
        public string context { get; set; }
        public bool exclude { get; set; }
        public string file { get; set; }
        public uint lineNumber { get; set; }
        public string method { get; set; }
        public List<IO2Trace> o2Traces { get; set; }
        public uint ordinal { get; set; }
        public string projectName { get; set; }
        public string propertyIds { get; set; }
        public uint recordId { get; set; }
        public byte severity { get; set; }
        public List<string> text { get; set; }
        public string vulnName { get; set; }
        public string vulnType { get; set; }
     
        // constructors
        public O2Finding()
        {
            callerName = "";
            context = "";
            file = "";
            method = "";
            o2Traces = new List<IO2Trace>();
            projectName = "";
            propertyIds = "";
            text = new List<String>();
            vulnName = "";
            vulnType = "";
        }
        public O2Finding(string vulnName, string vulnType) :this()
        {
            this.vulnName = vulnName;
            this.vulnType = vulnType;
        }
        public O2Finding(string vulnName, string vulnType, string context, string callerName) : this()
        {
            this.vulnName = vulnName;
            this.vulnType = vulnType;
            this.context = context;
            this.callerName = callerName;
        }
        
        // extra properties
        public string Source
        {
            get { return getSafeO2TraceValue(getSource()); }
            set
            {
                var source = getSource();
                if (source != null)
                    source.signature = value;
            }
        }
        public string SourceContext
        {
            get { return getSafeO2TraceContextValue(getSource()); }
            set { setSafeO2TraceContextValue(getSource(), value);}
        }        
        public string KnownSink
        {
            get { return getSafeO2TraceValue(OzasmtUtils.getKnownSink(o2Traces)); }
            set
            {
                var knownSink = OzasmtUtils.getKnownSink(o2Traces);
                if (knownSink != null)
                    knownSink.signature = value;
            }
        }           
        public string Sink
        {
            get { return getSafeO2TraceValue(getSink()); }
            set
            {
                var sink = getSink();
                if (sink != null)
                    sink.signature = value;
            }
        }
        public string SinkContext
        {
            get { return getSafeO2TraceContextValue(getSink()); }
            set { setSafeO2TraceContextValue(getSink(), value); }
        }   
        public string LostSink
        {
            get { return getSafeO2TraceValue(OzasmtUtils.getLostSink(o2Traces)); }
            set
            {
                var lostSinkTrace = OzasmtUtils.getLostSink(o2Traces);
                if (lostSinkTrace!=null)
                    lostSinkTrace.signature = value;                
            }
        }

        public List<string> JoinSinks()
        {
            var joinSinksSignatures = new List<String>();
            foreach(var joinSink in getJoinSinks())
                joinSinksSignatures.Add(joinSink.signature);
            return joinSinksSignatures;
        }

        public List<string> JoinSources()
        {
            var joinSourcesSignatures = new List<String>();
            foreach (var joinSource in getJoinSources())
                joinSourcesSignatures.Add(joinSource.signature);
            return joinSourcesSignatures;
        }

        public string SourceCode
        {
            get
            {
                return Files_WinForms.getLineFromSourceCode(file, lineNumber);
                //return getSafeO2TraceValue(OzasmtUtils.getLostSink(o2Traces));
            }
            set { }
        }
        public string Traces
        {
            get
            {
                var uniqueTraces = getUniqueTraces();
                var result = "";
                foreach (var o2Trace in uniqueTraces)
                    result += " : " + o2Trace.signature;
                return result;
            }
        }
        public string TracesContext
        {
            get
            {
                var uniqueTraces = getUniqueTraces();
                var result = "";
                foreach (var o2Trace in uniqueTraces)
                    result += " : " + o2Trace.context;
                return result;
            }
        }

        // special mappings
        public string _VulnTypeToSourceToSink
        {
            get { return vulnType + "   :   " + Source + "   ->   " + Sink; }
            set { }
        }
        public string _VulnTypeToSource
        {
            get { return vulnType + "   :   " + Source; }
            set { }
        }
        public string _SourceToSink
        {
            get { return Source + "   ->   " + Sink; }
            set { }
        }
        public string _SinkToSource
        {
            get { return Sink + "   <-   "  + Source; }
            set { }
        }
        public string _PathToSource
        {
            get
            {
                var pathToSource = "";
                foreach (var o2Trace in getPathToSource())
                {
                    var traceText = new FilteredSignature(o2Trace.signature).sFunctionName;
                    if (traceText == "")
                        traceText = o2Trace.signature;
                    pathToSource = traceText + " -> " + pathToSource;
                }
                return pathToSource;
            }            
            set { }
        }
        public string _JoinSink
        {
            get
            {
                var result = "";
                foreach (var join in getJoinSinks())
                    result += join + "    ";
                return result;
            }
            set { }
        }
        public string _JoinSource
        {
            get
            {
                var result = "";
                foreach (var join in getJoinSources())
                    result += join + "    ";
                return result; 
            }
            set { }
        }
        public string _JoinLocation
        {
            get
            {
                var result = "";
                foreach (var join in getJoinLocations())
                    result += join + "    ";
                return result;
            }
            set { }
        }

        // methods that return IO2Trace objects
        public IO2Trace getSource()
        {
            return OzasmtUtils.getSource(o2Traces); 
        }
        public List<IO2Trace> getPathToSource()
        {
            return OzasmtUtils.getPathToSource(o2Traces);
        }
        public IO2Trace getKnownSink()
        {
            return OzasmtUtils.getKnownSink(o2Traces); 
        }
        public IO2Trace getLostSink()
        {
            return OzasmtUtils.getLostSink(o2Traces); 
        }
        public IO2Trace getSink()
        {
            return OzasmtUtils.getKnownSink(o2Traces) ?? OzasmtUtils.getLostSink(o2Traces); 
        }
        public List<IO2Trace> getJoinSinks()
        {
            var allO2Traces = OzasmtUtils.getListWithAllTraces(this);
            var results = new List<IO2Trace>();
            foreach (var o2Trace in allO2Traces)
                if (o2Trace.traceType == TraceType.O2JoinSink)
                    results.Add(o2Trace);
            return results;
        }        
        public IO2Trace getJoinSink(string joinSinkSignature)
        {
            foreach (var joinSink in getJoinSinks())
                if (joinSink.signature == joinSinkSignature)
                    return joinSink;
            return null;
        }
        public List<IO2Trace> getJoinSources()
        {
            var allO2Traces = OzasmtUtils.getListWithAllTraces(this);
            var results = new List<IO2Trace>();
            foreach (var o2Trace in allO2Traces)
                if (o2Trace.traceType == TraceType.O2JoinSource)
                    results.Add(o2Trace);
            return results;
        }
        public List<IO2Trace> getJoinLocations()
        {
            var allO2Traces = OzasmtUtils.getListWithAllTraces(this);
            var results = new List<IO2Trace>();
            foreach (var o2Trace in allO2Traces)
                if (o2Trace.traceType == TraceType.O2JoinLocation)
                    results.Add(o2Trace);
            return results;
        }     
        public List<IO2Trace> getUniqueTraces()
        {
            var uniqueTraces = new List<IO2Trace>();
            OzasmtUtils.calculateUniqueListOfO2Traces(o2Traces, uniqueTraces);
            return uniqueTraces;
        }        

        // mist help methods
        private static string getSafeO2TraceValue(IO2Trace o2Trace)
        {
            return (o2Trace != null) ? o2Trace.signature : "";
        }
        private static string getSafeO2TraceContextValue(IO2Trace o2Trace)
        {
            return (o2Trace != null) ? o2Trace.context : "";
        }
        private static void setSafeO2TraceContextValue(IO2Trace o2Trace, string value)
        {
            if (o2Trace != null)
                o2Trace.context = value;
        }
        public override string ToString()
        {
            return vulnName ?? "";  // to deal with '...Attempted to read or write protected memory..' issue 
        }
        public IO2Trace addTrace(string traceSignature)
        {
            return addTrace(traceSignature, TraceType.Type_0);
        }
        public IO2Trace addTrace(string traceSignature, TraceType _traceType)        
        {
			var newTrace = new O2Trace(traceSignature);
			newTrace.traceType = _traceType;
        
            o2Traces.Add(newTrace);
            return newTrace;
        }
        public List<IO2Trace> addTraces(params string[] traceSignatures)
        {
            var tracesToAdd = new List<string>(traceSignatures);
            return addTraces(tracesToAdd);
        }
        public List<IO2Trace> addTraces(List<string> traceSignatures)
        {
            var newTraces = new List<IO2Trace>();
            foreach(var traceSignature in traceSignatures)
            {
                var newTrace = new O2Trace(traceSignature);
                o2Traces.Add(newTrace);
                newTraces.Add(newTrace);
            }

            return newTraces;
        }
        public void addTrace(IO2Trace targetTrace, string traceSignature, TraceType traceType)
        {
            ((O2Trace)targetTrace).addTrace(traceSignature, traceType);
        }
        public void insertTrace(string traceSignature, TraceType traceType)
        {
            var newO2Trace = new O2Trace(traceSignature, traceType);                    
            newO2Trace.childTraces.AddRange(o2Traces);
            o2Traces = new List<IO2Trace>();
            o2Traces.add(newO2Trace);
        }
        
    }
}
