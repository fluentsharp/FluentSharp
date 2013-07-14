// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.Interfaces;

namespace FluentSharp.WinForms.Utils
{
    class KM_CirAction : KO2Message, IM_CirAction
    {
        public IM_CirActions CirAction { get; set; }
        public ICirData CirData { get; set; }
        public ICirDataAnalysis CirDataAnalysis { get; set; }

        public static KM_CirAction setCirData(ICirData cirData)
        {
			var kmCirAction = new KM_CirAction();
			kmCirAction.CirData = cirData;
			kmCirAction.CirAction = IM_CirActions.setCirData;
            return kmCirAction;
        }

        public static KM_CirAction setCirDataAnalysis(ICirDataAnalysis cirDataAnalysis)
        {
            var kmCirAction = new KM_CirAction();
			kmCirAction.CirDataAnalysis = cirDataAnalysis;
			kmCirAction.CirAction = IM_CirActions.setCirDataAnalysis;
            return kmCirAction;
        }
    }
}
