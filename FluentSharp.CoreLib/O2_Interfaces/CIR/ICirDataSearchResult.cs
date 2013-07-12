// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface ICirDataSearchResult
    {
        ICirDataAnalysis fcdAnalysis { get; set; }
        List<ICirClass> lccTargetCirClasses { get; set; }
        List<string> lsResult_CallsMade { get; set; }
        List<string> lsResult_CallsMadeToExternalMethods { get; set; }
        List<string> lsResult_CallsMadeToExternalMethods_DontHaveDbMapping { get; set; }
        List<string> lsResult_CallsMadeToExternalMethods_HaveDbMapping { get; set; }
        List<string> lsResult_Classes { get; set; }
        List<string> lsResult_Classes_WithControlFlowGraphs { get; set; }
        List<string> lsResult_Functions { get; set; }
        List<string> lsResult_Functions_DontHaveDbMapping { get; set; }
        List<string> lsResult_Functions_HaveDbMapping { get; set; }
        List<string> lsResult_Functions_WithControlFlowGraphs { get; set; }
        void clearResultVars();       
    }
}