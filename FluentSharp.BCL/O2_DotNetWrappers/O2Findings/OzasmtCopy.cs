// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtCopy
    {
        public static IO2Finding createCopy(IO2Finding o2Finding)        
        {
            return createCopy(o2Finding, true);
        }

        public static IO2Finding createCopy(IO2Finding o2Finding, bool processChildTraces)
        {
            var newO2Finding = new O2Finding();
			newO2Finding.vulnType = o2Finding.vulnType;
			newO2Finding.vulnName = o2Finding.vulnName;
			newO2Finding.text = createCopy(o2Finding.text);
			newO2Finding.severity = o2Finding.severity;
			newO2Finding.recordId = o2Finding.recordId;
			newO2Finding.propertyIds = o2Finding.propertyIds;
			newO2Finding.projectName = o2Finding.projectName;
			newO2Finding.ordinal = o2Finding.ordinal;
			newO2Finding.lineNumber = o2Finding.lineNumber;
			newO2Finding.file = o2Finding.file;
			newO2Finding.exclude = o2Finding.exclude;
			newO2Finding.confidence = o2Finding.confidence;
			newO2Finding.columnNumber = o2Finding.actionObject;
			newO2Finding.context = o2Finding.context;
			newO2Finding.callerName = o2Finding.callerName;
			newO2Finding.actionObject = o2Finding.actionObject;
			
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

			var newO2Trace = new O2Trace();
			newO2Trace.text = createCopy(o2Trace.text);
			newO2Trace.traceType = o2Trace.traceType;
			newO2Trace.taintPropagation = o2Trace.taintPropagation;
			newO2Trace.signature = o2Trace.signature;
			newO2Trace.ordinal = o2Trace.ordinal;
			newO2Trace.lineNumber = o2Trace.lineNumber;
			newO2Trace.method = o2Trace.method;
			newO2Trace.file = o2Trace.file;
			newO2Trace.context = o2Trace.context;
			newO2Trace.columnNumber = o2Trace.columnNumber;
			newO2Trace.clazz = o2Trace.clazz;
			
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
