// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2Assessment
    {                
        List<IO2Finding> o2Findings { get; set; }
        string name { get; set; }

        string save(IO2AssessmentSave o2AssessmentSave);
        bool save(IO2AssessmentSave o2AssessmentSave, string sPathToSaveAssessment);
        bool saveAsO2Format(string targetFile);
    }
}