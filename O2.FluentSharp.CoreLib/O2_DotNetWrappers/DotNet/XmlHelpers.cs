// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;



namespace FluentSharp.CoreLib.API
{
    public class XmlHelpers
    {
        public static string getRootElementText(string fileToProcess)
        {
            // Need to do some more benchmarking but I think the fastest way to get this is to use StreamReader
            try
            {
                TextReader textReader = new StreamReader(fileToProcess);
                var fileLine = textReader.ReadLine();
                if (fileLine.index("<?xml ") > -1) // check if the first line has <?xml
                {
                    var nextLine = textReader.ReadLine();
                    if (nextLine.index("<!--") > -1)              // check if there is a comment)
                    {
                        while (nextLine.index("-->") == -1)
                            // and if so, loop until we find the end of it                        
                            nextLine = textReader.ReadLine();
                        return textReader.ReadLine();
                    }
                    textReader.Close();
                    return nextLine; // if not return the 2nd line
                }

                PublicDI.log.error("in getRootElementText, the file provided did not have <?xml on the first line: {0}   -    {1}", fileToProcess, fileLine);
            }
            catch (Exception ex)
            {
                   PublicDI.log.ex(ex);
            }
            return "";                     
        }
    }
}
