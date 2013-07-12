// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.CoreLib.API
{
    public class O2Rule : IO2Rule
    {
        public O2RuleType RuleType { get; set; }
        public string DbId { get; set; }
        public string Severity { get; set; }
        public string Signature { get; set; }
        public string VulnType { get; set; }
        public string Param { get; set; }
        public string Return { get; set; }
        public string FromArgs { get; set; }
        public string ToArgs { get; set; }
        public string Comments { get; set; }
        public bool FromDb { get; set; }
        public bool Tagged { get; set; }
        public bool ModifiedInDB { get; set; }


        public O2Rule()
        {
            RuleType = O2RuleType.NotMapped;
            DbId = "0";
            Severity = "Medium";
            Signature = "";
            VulnType = "";
            Param = "";
            Return = "";
            FromArgs = "";
            ToArgs = "";
            Comments = "";
            FromDb = false;
            Tagged = false;
        }

        public O2Rule(string vulnType, string signature, string dbId)
            : this()
        {            
            VulnType = vulnType;
            Signature = signature;
            DbId = dbId;
        }

        public O2Rule(O2RuleType ruleType, string vulnType, string signature, string dbId) 
            : this(vulnType, signature, dbId)
        {
            RuleType = ruleType;            
        }

        public O2Rule(O2RuleType ruleType, string vulnType, string signature, string dbId, bool tagged)
            : this(ruleType, vulnType, signature, dbId)
        {
            Tagged = tagged;
        }
    }
}
