// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces.J2EE
{
    public interface IStrutsConfigXml
    {
        List<IStrutsConfig_FormBean> formBeans { get; set; }
        Dictionary<string, string> globalForwards { get; set; }
        List<IStrutsConfig_Action> actionmappings { get; set; }
        List<IStrutsConfig_PlugIn> plugIns { get; set; }
        Dictionary<string, List<string>> resolvedViews { get; set; }    
    }

    public interface IStrutsConfig_FormBean
    {
        string name { get; set; }
        string type { get; set; }
        string extends { get; set; }
        bool hasValidationMapping { get; set; }
        Dictionary<string, IStrutsConfig_FormBean_Field> fields { get; set; }        
    }

    public interface IStrutsConfig_FormBean_Field
    {
        bool hasValidationMapping { get; set; }
        string name { get; set; }
        string type { get; set; }
        string initial { get; set; }
        string depends { get; set; }        
        Dictionary<string, string> validators { get; set; }
    }

    public interface IStrutsConfig_Action
    {
        string path { get; set; }
        string type { get; set; }
        string name { get; set; }
        string scope { get; set; }
        string input { get; set; }
        string extends { get; set; }
        string parameter { get; set; }
        string unknown { get; set; }
        string validate { get; set; }
        List<IStrutsConfig_Action_Forward> forwards { get; set; }
    }

    public interface IStrutsConfig_Action_Forward
    {
        string name { get; set; }
        string path { get; set; }
        string redirect { get; set; }        
    }

    public interface IStrutsConfig_PlugIn
    {
        string className { get; set; }
        Dictionary<string, string> properties { get; set; }        
    }
}