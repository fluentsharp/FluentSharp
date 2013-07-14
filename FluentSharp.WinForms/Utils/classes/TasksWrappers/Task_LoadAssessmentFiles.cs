// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;
using FluentSharp.WinForms.O2Findings;

namespace FluentSharp.WinForms.Utils
{
    public class Task_LoadAssessmentFiles : BTask
    {
        private readonly IO2AssessmentLoad o2AssessmentLoad;

        public Task_LoadAssessmentFiles(IO2AssessmentLoad _o2AssessmentLoad, string ozasmtFile)
            : this(_o2AssessmentLoad, new List<string>(new[] { ozasmtFile }))
        {
            o2AssessmentLoad = _o2AssessmentLoad;            
        }

        public Task_LoadAssessmentFiles(IO2AssessmentLoad _o2AssessmentLoad, List<string> ozasmtFiles)
        {
            o2AssessmentLoad = _o2AssessmentLoad;
            sourceType = typeof (List<string>);
            resultsType = typeof (O2Assessment);
            sourceObject = ozasmtFiles;
            taskName = "Load Asssessment File";
        }

        public override bool execute()
        {
            if (sourceObject == null)
                PublicDI.log.error("source object was null");
            else
                if (sourceObject.GetType() != sourceType)
                    PublicDI.log.error("source object type was not List<string> is was " + sourceObject.GetType().FullName);
                else
                {
                    var filesToProcess = (List<string>) sourceObject;

                    setProgressBarValue(filesToProcess.Count);

                    var o2Assessment = new O2Assessment();
                    foreach (string file in filesToProcess)
                    {
                        PublicDI.log.info("Importing file {0}", file);
                        if (false == o2AssessmentLoad.importFile(file, o2Assessment))
                            return false;
                        PublicDI.log.info("There are {0} Findings loaded ", o2Assessment.o2Findings.Count);
                        incProgressBarValue();
                    }
                    resultsObject = o2Assessment;
                    return true;
                }
            return false;
        }
    }
}
