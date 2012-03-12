// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using O2.Interfaces.CIR;

namespace O2.Interfaces.Messages
{
    public enum IM_CirActions 
    {
        setCirData,
        setCirDataAnalysis
        //newData        
    }

    public interface IM_CirAction : IO2Message
    {
        IM_CirActions CirAction { get; set; }
        ICirData CirData { get; set; }
        ICirDataAnalysis CirDataAnalysis { get; set; }
    }
}