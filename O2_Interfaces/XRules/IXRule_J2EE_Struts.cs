// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using O2.Interfaces.FrameworkSupport.J2EE;
using O2.Interfaces.O2Findings;

namespace O2.Interfaces.XRules
{
    public interface IXRule_J2EE_Struts
    {
        List<IO2Finding> execute(IStrutsConfigXml strutsConfigXml);
    }
}