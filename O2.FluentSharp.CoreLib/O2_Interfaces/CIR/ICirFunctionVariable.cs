// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirFunctionVariable
    {
        String defSymbol { get; set; }
        int iGuaranteedInitBeforeUsed { get; set; }
        String refSymbol { get; set; }
        String sName { get; set; }
        String sPrintableType { get; set; }
        String sSymbolDef { get; set; }
        String sSymbolRef { get; set; }
        String sUniqueID { get; set; }
    }
}