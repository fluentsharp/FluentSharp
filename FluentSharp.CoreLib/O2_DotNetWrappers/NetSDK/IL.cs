// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;




namespace FluentSharp.CoreLib.API
{
    public class IL
    {
        public static string pathToExecutableIlAsm = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\ilasm.exe";

        public static List<String> possibleILdasmLocations = new List<string>();
                                                           
		static IL()
		{
			possibleILdasmLocations.add(@"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\Ildasm.exe")
								   .add(@"C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\ildasm.exe")
                                   .add(PublicDI.config.CurrentExecutableDirectory + "\\Ildasm.exe");                                                           
		}
		
        public static string getILAsmExe()
        {
            if (File.Exists(pathToExecutableIlAsm))
                return pathToExecutableIlAsm;
            return "";
        }

        public static string getILDasmExe()
        {
            foreach (var possibleLocation in possibleILdasmLocations)
                if (File.Exists(possibleLocation))
                    return possibleLocation;
            return "";
        }

        public static string createILforAssembly(string targetAssembly)
        {
            var targetDirectory = targetAssembly.directoryName();
            var ilFileToCreate = targetDirectory.pathCombine(targetAssembly.fileName_WithoutExtension() + ".il");
            var arguments = string.Format("\"{0}\" /OUT=\"{1}\"", targetAssembly, ilFileToCreate);
            var process = Processes.startProcessAsConsoleApplication(getILDasmExe(), arguments);
            process.WaitForExit();
            if (File.Exists(ilFileToCreate))
                return ilFileToCreate;
            return "";
        }

        public static string createExeFromIL(string ilFile, bool createDll, bool createPDB)
        {            
            var fileToCreate = ilFile.Replace(".il", (createDll) ? ".dll" : ".exe");
            var arguments = string.Format("\"{0}\" /Output=\"{1}\" {2} /QUIET", ilFile, fileToCreate,
                ((createDll) ? "/DLL" : "/EXE") + ((createPDB) ? " /PDB" : ""));
            var process = Processes.startProcessAsConsoleApplication(getILAsmExe(), arguments);
            process.WaitForExit();
            if (File.Exists(fileToCreate))
                return fileToCreate;
            return "";
        }

        /// <summary>
        /// Create Pdb for targetExe (if there is not a PDB in there, there original exe file will be replaced)
        /// </summary>
        /// <param name="targetExe"></param>
        /// <param name="overrideExistingPdd"></param>
        /// <returns></returns>
        public static string createPdb(string targetExe, bool overrideExistingPdd)
        {
            var pdbFileNameInExeFolder = Path.ChangeExtension(targetExe, ".pdb");
            if (File.Exists(pdbFileNameInExeFolder))
                return pdbFileNameInExeFolder;
            var ilFileInTempFolder = createILforAssembly(targetExe);
            if (File.Exists(ilFileInTempFolder))
            {
                File.Delete(targetExe);
                var newExeInTempFolder = createExeFromIL(ilFileInTempFolder, (Path.GetExtension(targetExe) == ".dll"), true /*createPDB*/);
                if (File.Exists(newExeInTempFolder))
                {
                    var pdbInTempFolder = Path.ChangeExtension(newExeInTempFolder, ".pdb");
                    if (File.Exists(pdbInTempFolder))
                    {
                        /*var targetExeFolder = Path.GetDirectoryName(targetExe);
                        Files.Copy(pdbInTempFolder, targetExeFolder);                   // copy pdb
                        Files.Copy(ilFileInTempFolder, targetExeFolder);                // copy il
                        Files.Copy(newExeInTempFolder, targetExeFolder);                // copy exe      */                  
                        return pdbFileNameInExeFolder;
                    }
                }
            }
            return "";
        }
    }
}
