// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtSearch
    {
        public static IO2Trace findO2TraceSignature(List<IO2Trace> o2Traces, string signatureToFind)
        {
            foreach (var o2Trace in o2Traces)
            {
                var result = findO2TraceSignature(o2Trace, signatureToFind);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static IO2Trace findO2TraceSignature(IO2Trace o2Trace, string signatureToFind)
        {
            if (o2Trace != null)
            {
                if (o2Trace.signature == signatureToFind)
                    return o2Trace;
                if (o2Trace.childTraces != null)
                    foreach (IO2Trace o2ChildTrace in o2Trace.childTraces)
                    {
                        IO2Trace match = findO2TraceSignature(o2ChildTrace, signatureToFind);
                        if (match != null)
                            return match;
                    }
            }
            return null;
        }

        public static bool isO2TraceAChildTraceOfO2Trace(IO2Trace rootO2Trace, IO2Trace o2TraceToFind)
        {
            if (rootO2Trace != null)
                foreach (IO2Trace o2ChildTrace in rootO2Trace.childTraces)
                {
                    if (o2ChildTrace == o2TraceToFind)
                        return true;
                    bool match = isO2TraceAChildTraceOfO2Trace(o2ChildTrace, o2TraceToFind);
                    if (match)
                        return true;
                }
            return false;
        }


        public static IEnumerable<string> getUniqueStringListOf_LostSinks(List<IO2Finding> o2Findings)
        {
            return (from O2Finding o2Finding in o2Findings where o2Finding.LostSink != "" 
                    select o2Finding.getLostSink().signature).Distinct();            
        }

        public static IEnumerable<IO2Trace> getUniqueIO2TraceListOf_LostSinks(List<IO2Finding> o2Findings)
        {                        
            var lostSinksTraces = new Dictionary<string, IO2Trace>();
            foreach(O2Finding o2Finding in o2Findings)
            {
                var lostSink = o2Finding.getLostSink();
                if (lostSink != null && lostSinksTraces.ContainsKey(lostSink.signature))
                    lostSinksTraces.Add(lostSink.signature, lostSink);                
            }
            return lostSinksTraces.Values;
        }


        public static Dictionary<string,List<O2Finding>> getDictionaryWithJoinSinks(List<IO2Finding> strutsFindings)
        {
            var joinSinksDictionary = new Dictionary<string, List<O2Finding>>();
            foreach(O2Finding o2Finding in strutsFindings)
            {                
                foreach(var joinSink in o2Finding.getJoinSinks())
                {
                    if (false == joinSinksDictionary.ContainsKey(joinSink.signature))
                        joinSinksDictionary.Add(joinSink.signature, new List<O2Finding>() );
                    joinSinksDictionary[joinSink.signature].Add(o2Finding);
                }
            }
            return joinSinksDictionary;
        }
    }
}
