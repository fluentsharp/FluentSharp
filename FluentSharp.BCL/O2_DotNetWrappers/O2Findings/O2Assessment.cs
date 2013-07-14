// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    [Serializable]
    public class O2Assessment : IO2Assessment
    {        
        //public IO2AssessmentSave o2AssessmentSave { get; set; }
        public string name { get; set; }
        
       // public string lastOzasmtImportFile { get; set; }
       // public long lastOzasmtImportFileSize { get; set; }
       // public TimeSpan lastOzasmtImportTimeSpan { get; set; }
        public bool lastOzasmtImportWasItSucessfull { get; set; }     
        public List<IO2Finding> o2Findings{get;set;}

        public O2Assessment()
        {            
            o2Findings = new List<IO2Finding>();                        
        }

        public O2Assessment(IO2AssessmentLoad o2AssessmentLoad, string sPathToAssessmentToOpen) 
            : this()
        {
            o2AssessmentLoad.importFile(sPathToAssessmentToOpen, this);
        }

        public O2Assessment(List<IO2Finding> _o2Findings)
        {
            o2Findings = _o2Findings;
        }

        public virtual string save(IO2AssessmentSave o2AssessmentSave)
        {
            return o2AssessmentSave.save(o2Findings);
        }

        public virtual bool save(IO2AssessmentSave o2AssessmentSave, string sPathToSaveAssessment)
        {
            return o2AssessmentSave.save(name, o2Findings, sPathToSaveAssessment);            
        }


        public bool saveAsO2Format(string targetFile)
        {
            if (Path.GetExtension(targetFile) != PublicDI.config.O2FindingsFileExtension)
                targetFile += PublicDI.config.O2FindingsFileExtension;
            var result = Serialize.createSerializedBinaryFileFromObject(this, targetFile);
            if (result)
                PublicDI.log.info("Serialized Assessment into : {0}", targetFile);
            else
                PublicDI.log.error("There was a problem serializing Struts Mapping object saved to: {0}", targetFile);
            return result;
        }
    }
}
