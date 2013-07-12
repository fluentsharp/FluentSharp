// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IO2Rule
    {
        O2RuleType RuleType { get; set; }
        string DbId { get; set; }
        string Severity { get; set; }
        string Signature { get; set; }
        string VulnType { get; set; }
        string Param { get; set; }
        string Return { get; set; }
        string FromArgs { get; set; }
        string ToArgs { get; set; }
        string Comments { get; set; }
        bool FromDb { get; set; }
        bool Tagged { get; set; }       // to be used on drag and drop of functions
    }
}