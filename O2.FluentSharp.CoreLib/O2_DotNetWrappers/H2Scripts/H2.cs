using System;
using System.Collections.Generic;


namespace FluentSharp.CoreLib.API
{
    public class H2
    {
        public string SourceCode { get; set; }        
        //public List<string> ReferencedAssemblies { get; set; }        
        //public Dictionary<string, object> InvocationParameters { get; set; }
   
        public H2()
        {            
            SourceCode = "";            
            //ReferencedAssemblies = new List<string>();
            //InvocationParameters = new Dictionary<string, object>();
        }

        public H2(string sourceCode) : this()
        {
            SourceCode = sourceCode;
        }

        public static H2 load(string path)
        {
            if (path.fileExists().isFalse())
                return null;
            if (path.isBinaryFormat())
                return null;
            
            //we can't use .fileContents since it has native support for the h2 file format
            var fileContents = path.fileContents_AsByteArray().ascii().trim();
            if(fileContents.starts("<?xml "))                       
                return fileContents.deserialize<H2>(false);
            return new H2(fileContents);


        }        

        public bool save(string path)
        {
            //return this.serialize(path);
            return this.SourceCode.saveAs(path)
                                  .fileExists();
        }        
    }
}
