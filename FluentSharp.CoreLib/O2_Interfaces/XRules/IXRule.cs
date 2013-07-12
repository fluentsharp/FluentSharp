// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IXRule
    {
        string Name { get; set; }
        string Description { get; set; }        
    }

    public class KXRule : IXRule
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public KXRule()
        {
            Name = "";
            Description = "O2 XRule";            
        }
                
        public override string ToString()
        {
            return Name;
        }        
    }
}