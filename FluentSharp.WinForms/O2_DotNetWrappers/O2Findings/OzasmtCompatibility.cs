// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtCompatibility
    {
        public static void makeCompatibleWithOunceV6(IEnumerable<IO2Finding> o2Findings)
        {
            // fix use of non-OSA supported trace types:
            foreach (var o2Finding in o2Findings)
                foreach (var o2Trace in OzasmtUtils.getListWithAllTraces((O2Finding) o2Finding))
                    switch (o2Trace.traceType)
                    {
                        case TraceType.O2Info:
                        case TraceType.O2JoinSink:
                        case TraceType.O2JoinSource:
                            o2Trace.traceType = TraceType.Type_4;
                            break;

                    }
        }
    }
}
