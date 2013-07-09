// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
/*  //This shouldn't be here, specially with such dependency on xsd.exe
 
using System;
using System.Diagnostics;
using System.IO;



namespace O2.DotNetWrappers.DotNet
{
    public class DevUtils
    {
        const string sLocationOfXsdExecutable1 = @"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd\xsd.exe";
        const string sLocationOfXsdExecutable2 = @"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\xsd.exe";

        public static Process createCSharpFileFromXsd(String pathToXsd)
        {
            return createCSharpFileFromXsd(pathToXsd, "O2.Core.XSD");
        }

        public static Process createCSharpFileFromXsd(String pathToXsd, string targetNamespace)
        {
            if (false == File.Exists(pathToXsd))
                PublicDI.log.error("in createCSharpFileFromXsd: could not file XSD file to convert:{0}", pathToXsd);
            else
            {                
                String xsdExecutable = "";

                if (File.Exists(sLocationOfXsdExecutable1))
                    xsdExecutable = sLocationOfXsdExecutable1;
                else if (File.Exists(sLocationOfXsdExecutable2))
                    xsdExecutable = sLocationOfXsdExecutable2;
                if (xsdExecutable == "")
                    PublicDI.log.error("Could not find xsd.exe on this computer");
                else
                {
                    String sOutputDir = Path.GetDirectoryName(pathToXsd);
                    String sArguments = String.Format("\"{0}\" /c /n:{1} /out:\"{2}\"", pathToXsd, targetNamespace, sOutputDir);
                    return Processes.startProcess(xsdExecutable, sArguments);
                }
            }
            return null;
        }

        public static void createXSDFileFromClass(String sPathToAssemblyToProcess, String sTypeToConvert,
                                                  String sFolderToSaveXSD)
        {
            if (sFolderToSaveXSD == "")
                sFolderToSaveXSD = PublicDI.config.O2TempDir;
            if (false == File.Exists(sPathToAssemblyToProcess))
                PublicDI.log.error("in createXSDFileFromClass: could not file Assembly file to process:{0}",
                             sPathToAssemblyToProcess);
            else
            {
                var sLocationOfXsdExecutable1 = @"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\xsd\xsd.exe";
                var sLocationOfXsdExecutable2 = @"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\xsd.exe";
                var XsdExecutable = "";

                if (File.Exists(sLocationOfXsdExecutable1))
                    XsdExecutable = sLocationOfXsdExecutable1;
                else if (File.Exists(sLocationOfXsdExecutable2))
                    XsdExecutable = sLocationOfXsdExecutable2;
                if (XsdExecutable == "")
                    PublicDI.log.error("Could not find xsd.exe on this computer");
                else
                {
                    //String sOutputDir = Path.GetDirectoryName(sPathToXsd);
                    String sArguments = String.Format("\"{0}\" /type:\"{1}\" /out:\"{2}\"", sPathToAssemblyToProcess,
                                                      sTypeToConvert, sFolderToSaveXSD);
                    Processes.startProcessAsConsoleApplication(XsdExecutable, sArguments);
                }
            }
        }
    }
}
*/