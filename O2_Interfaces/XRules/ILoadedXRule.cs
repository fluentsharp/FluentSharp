// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace O2.Interfaces.XRules
{
    public interface ILoadedXRule
    {
        IXRule XRule { get; set; }
        string Source { get; set; }
        Dictionary<XRuleAttribute, MethodInfo> methods { get; set; }
    }
}