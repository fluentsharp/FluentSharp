// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2Finding
    {
        uint actionObject { get; set; }
        string callerName { get; set; }
        uint columnNumber { get; set; }
        byte confidence { get; set; }
        string context { get; set; }
        bool exclude { get; set; }
        String file { get; set; }
        uint lineNumber { get; set; }
        string method { get; set; }
        List<IO2Trace> o2Traces { get; set; }
        uint ordinal { get; set; }
        string projectName { get; set; }
        string propertyIds { get; set; }
        uint recordId { get; set; }
        byte severity { get; set; }
        List<String> text { get; set; }
        string vulnName { get; set; }
        string vulnType { get; set; }
        string ToString();
    }
}