// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using O2.Interfaces.O2Findings;

namespace O2.DotNetWrappers.O2Findings
{
    public class OzasmtCopy
    {
        public static IO2Finding createCopy(IO2Finding o2Finding)        
        {
            return createCopy(o2Finding, true);
        }

        public static IO2Finding createCopy(IO2Finding o2Finding, bool processChildTraces)
        {
            var newO2Finding = new O2Finding
                                   {
                                       actionObject = o2Finding.actionObject,
                                       callerName = o2Finding.callerName,
                                       context = o2Finding.context,
                                       columnNumber = o2Finding.actionObject,
                                       confidence = o2Finding.confidence,
                                       exclude = o2Finding.exclude,
                                       file = o2Finding.file,
                                       lineNumber = o2Finding.lineNumber,
                                       ordinal = o2Finding.ordinal,
                                       projectName = o2Finding.projectName,
                                       propertyIds = o2Finding.propertyIds,
                                       recordId = o2Finding.recordId,
                                       severity = o2Finding.severity,
                                       text = createCopy(o2Finding.text),
                                       vulnName = o2Finding.vulnName,
                                       vulnType = o2Finding.vulnType
                                   };
            if (processChildTraces)
                newO2Finding.o2Traces = createCopy(o2Finding.o2Traces);

            return newO2Finding;
        }

        public static IO2Trace createCopy(IO2Trace o2Trace)
        {
            return createCopy(o2Trace, true);
        }

        public static IO2Trace createCopy(IO2Trace o2Trace, bool processChildTraces)
        {
            if (o2Trace == null)
                return null;

            var newO2Trace = new O2Trace
                                 {
                                     clazz = o2Trace.clazz,
                                     columnNumber = o2Trace.columnNumber,
                                     context = o2Trace.context,
                                     file = o2Trace.file,
                                     method = o2Trace.method,
                                     lineNumber = o2Trace.lineNumber,
                                     ordinal = o2Trace.ordinal,
                                     signature = o2Trace.signature,
                                     taintPropagation = o2Trace.taintPropagation,
                                     traceType = o2Trace.traceType,
                                     text = createCopy(o2Trace.text)
                                 };
            if (processChildTraces)
                newO2Trace.childTraces = createCopy(o2Trace.childTraces);
            return newO2Trace;
        }

        public static List<IO2Trace> createCopy(List<IO2Trace> childTraces)
        {
            //if (childTraces == null)
            //    return null;
            var newChildTraces = new List<IO2Trace>();
            foreach (O2Trace o2Trace in childTraces)
                newChildTraces.Add(createCopy(o2Trace));
            return newChildTraces;
        }

        public static List<String> createCopy(List<String> text)
        {
            if (text == null)
                return null;
            var newText = new List<String>();
            foreach (string line in text)
                newText.Add(line);
            return newText;
        }
        
    }
}
