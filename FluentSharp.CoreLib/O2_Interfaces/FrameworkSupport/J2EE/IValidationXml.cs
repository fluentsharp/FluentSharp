// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces.J2EE
{
    public interface IValidationXml
    {
        List<IValidation_Form> forms { get; set; }     
    }

    public interface IValidation_Form
    {
        string name { get; set; }
        Dictionary<string,IValidation_Form_Field> fields { get; set; }        
    }

    public interface IValidation_Form_Field
    {
        string property { get; set; }
        string depends { get; set; }
        Dictionary<string, string> vars { get; set; }
    }
}