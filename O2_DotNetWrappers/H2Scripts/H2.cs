using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.ExtensionMethods;

namespace O2.DotNetWrappers.H2Scripts
{
    public class H2
    {
        public string SourceCode { get; set; }        
        public List<string> ReferencedAssemblies { get; set; }
        private Dictionary<string, object> InvocationParameters { get; set; }
   
        public H2()
        {            
            SourceCode = "";            
            ReferencedAssemblies = new List<string>();
            InvocationParameters = new Dictionary<string, object>();
        }

        public H2(string sourceCode) : this()
        {
            SourceCode = sourceCode;
        }

        public static H2 load(string path)
        {
            return path.deserialize<H2>();            
        }        

        public bool save(string path)
        {
            return this.serialize(path);
        }        
    }
}
