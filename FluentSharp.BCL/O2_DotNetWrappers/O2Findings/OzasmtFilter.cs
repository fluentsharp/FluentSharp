// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtFilter
    {
        public static List<IO2Finding> getFindingsWithSink(List<IO2Finding> findings, string regExToFind)
        {
            var results = new List<IO2Finding>();
            foreach (IO2Finding o2Finding in findings)
            {
                IO2Trace sink = OzasmtUtils.getKnownSink(o2Finding.o2Traces);
                if (sink != null && sink.signature != "" && RegEx.findStringInString(sink.signature, regExToFind))
                    results.Add(o2Finding);
            }
            return results;
        }
    }
}
