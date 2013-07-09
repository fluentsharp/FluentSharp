// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Xml.Serialization;

namespace FluentSharp.CoreLib.API
{    
    public class SourceCodeMappings
    {        
        [XmlElement("Mapping")] public SourceCodeMappingsMapping[] Mapping { get; set; }
    }

    public class SourceCodeMappingsMapping
    {        
        [XmlAttribute] public string replaceThisString { get; set; }        
        [XmlAttribute] public string withThisString { get; set; }
    }
}
