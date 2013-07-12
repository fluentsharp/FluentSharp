using System;
using FluentSharp.BCL;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.REPL.Utils
{
    public class O2Scripts
    {
        public static string getZipWithLocalO2Scripts()
        {
            var tempDir = "_TempScriptsFolder".tempDir();
            var localScriptsFolder = PublicDI.config.LocalScriptsFolder;
            var tempScriptsFolder = tempDir.pathCombine("O2.Platform.Scripts");
            var zipFile = tempDir.pathCombine("O2.Platform.Scripts.zip");


            "[getZipWithLocalO2Scripts] Step 1: Copying files".debug();
            Files.copyFolder(localScriptsFolder, tempDir, true, false, ".git");

            "[getZipWithLocalO2Scripts] Step 2: calculating Hashes".debug();
            var files = tempScriptsFolder.files(true);
            var items = new Items();
            foreach (var file in files)
            {
                var hash = file.fileContents_AsByteArray().hash();
                items.add(file.remove(tempScriptsFolder + "\\"), hash.str());
            }
            var hashesFile = tempScriptsFolder.pathCombine("ScriptHashes-{0}.xml".format(DateTime.Now.safeFileName()));
            items.saveAs(hashesFile);

            "[getZipWithLocalO2Scripts] Step 3: Creating Zip".debug();
            tempScriptsFolder.zip_Folder(zipFile);

            return zipFile;
        }
        public static void downloadO2Scripts()
        {
            downloadO2Scripts((message)=>message.info());
        }
        public static void downloadO2Scripts(Action<string> statusMessage)
        {
            statusMessage("Downloading zip with latest rules");
			var url = "https://github.com/o2platform/O2.Platform.Scripts/zipball/master";
			var zipFile = url.download();
						
			statusMessage("Unziping files into temp folder");
			var targetFolder = @"_Downloaded_O2Scripts".tempDir(false).fullPath();
            if (targetFolder.size() > 120)
            {
                "[downloadO2Scripts] targetFolder path was more than 120 chars: {0}".error(targetFolder);
                targetFolder = System.IO.Path.GetTempPath().pathCombine(targetFolder.fileName());
                "[downloadO2Scripts] set targetFolder to: {0}".info(targetFolder);
            }
			zipFile.unzip_File(targetFolder);
						
			statusMessage("Copying files into scripts folder");
			var baseDir = targetFolder.folders().first();            
			var scriptsFolder = PublicDI.config.LocalScriptsFolder;
			var filesCopied = 0;
			foreach(var file in baseDir.files(true))
			{
				var targetFile = file.replace(baseDir,scriptsFolder);
				targetFile.parentFolder().createDir();
				file.file_Copy(targetFile);
				filesCopied++;	
			}
			"copied: {0}".info(filesCopied);
			CompileEngine.clearLocalScriptFileMappings();
			statusMessage("Done");		
        }
    }
}
