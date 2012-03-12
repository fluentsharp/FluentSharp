// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using O2.Interfaces.O2Findings;

namespace O2.Interfaces.CIR
{
    public interface ICirTraces
    {
        List<IO2Finding> IsSink { get; set; }
        List<IO2Finding> IsSource { get; set; }
    }
}