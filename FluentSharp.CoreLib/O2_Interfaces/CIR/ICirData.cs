// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirData
    {        
        Dictionary<string, ICirClass> dClasses_bySignature { get; set; }        
        Dictionary<string, ICirFunction> dFunctions_bySignature { get; set; }


        bool bStoreControlFlowBlockRawDataInsideCirDataFile { get; set; }
        bool bVerbose { get; set; }
        Dictionary<string, ICirFunction> dTemp_Functions_bySymbolDef { get; set; }

        //Dictionary<string, ICirFunction> dFunctions_bySymbolDef { get; set; }       // temp dictionary
        //Dictionary<string, ICirClass> dClasses_bySymbolDef { get; set; }
        Dictionary<string, string> dSymbols { get; set; }
        List<string> lFiles { get; set; }
        string sDbId { get; set; }

        ICirClass getClass(string cirClassToFind);
        ICirClass getClass(string cirClassToFind, bool exactMatch, bool createOnNotMatch);
        ICirClass addClass(string newClassSignature);
        void remapXRefs();
    }
}