// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Threading;

namespace FluentSharp.CoreLib.API
{
    public class FolderWatcher
    {
        #region Delegates

        public delegate void CallbackOnFolderWatchEvent(FolderWatcher folderWatcher);

        #endregion

        public CallbackOnFolderWatchEvent callbackOnFolderWatchEvent;

        public bool enabled;
        public string file = "";
        public string fileChanged = "";
        public string fileCreated = "";
        public string fileDeleted = "";
        public string folderWatched = "";

        public FileSystemWatcher fileSystemWatcher = null;

        public FolderWatcher(string folderToWatch)
        {
            folderWatched = folderToWatch;
            startFolderWatcher();
        }

        public void disable()
        {
            enabled = false;
            fileSystemWatcher.Changed -= fileSystemWatcher_onChange;
            fileSystemWatcher.Created -= fileSystemWatcher_onCreate;
            fileSystemWatcher.Deleted -= fileSystemWatcher_onDelete;
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
            fileSystemWatcher = null;            
        }

        public FolderWatcher(string folderToWatch, CallbackOnFolderWatchEvent callback) : this(folderToWatch)
        {
            callbackOnFolderWatchEvent += callback;
        }

        public override string ToString()
        {
            return folderWatched  ?? "";  // to deal with '...Attempted to read or write protected memory..' issue ;
        }
        
        private void startFolderWatcher()
        {
            try
            {
                if (Directory.Exists(folderWatched))
                {
                    fileSystemWatcher = new FileSystemWatcher(folderWatched);
                    fileSystemWatcher.Changed += fileSystemWatcher_onChange;
                    fileSystemWatcher.Created += fileSystemWatcher_onCreate;
                    fileSystemWatcher.Deleted += fileSystemWatcher_onDelete;
                    fileSystemWatcher.EnableRaisingEvents = true;
                    enabled = true;
                }
                else
                    PublicDI.log.error("Directory to watch does not exist: {0}", folderWatched);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in startFolderWatcher :{0}", ex.Message);
            }
        }

        private void fileSystemWatcher_onDelete(object sender, FileSystemEventArgs e)
        {
            if (enabled && fileDeleted != e.FullPath)
            {
                fileDeleted = e.FullPath;
                file = fileDeleted;
                raiseCallback();
            }
        }

        private void fileSystemWatcher_onCreate(object sender, FileSystemEventArgs e)
        {
            if (enabled && fileCreated != e.FullPath)
            {
                fileCreated = e.FullPath;                
                file = fileCreated;
                checkIfFileCanBeOpened(file, 5);
                raiseCallback();
            }
        }

        // this is tend to triger more than once
        private void fileSystemWatcher_onChange(object sender, FileSystemEventArgs e)
        {
            if (enabled && fileChanged != e.FullPath)
            {
                fileChanged = e.FullPath;
                file = fileChanged;
                checkIfFileCanBeOpened(file, 5);
                raiseCallback();
            }
        }

        /*private void processFile(string fileToProcess)
        {
            file = fileToProcess;
            loadfilesInAffectedFolder();
        }*/

        /* private void loadfilesInAffectedFolder()
        {
            filesInAffectedFolder = new List<string>();
            foreach (string fileInFolder in Files.getFilesFromDir_returnFullPath(folderWatched))
                filesInAffectedFolder.Add(fileInFolder);
        }*/

        private void raiseCallback()
        {
            //var fileInfo = new FileInfo(this.file);            
            Callbacks.raiseRegistedCallbacks(callbackOnFolderWatchEvent, new object[] { this });
        }
        /// <summary>
        /// This function tries to check if the file is already in a state the can be opened
        /// since we get the filewatch events (from windows) very early on and in some cases the file might
        /// still be 'dirty' or in used (by whoever created it)
        /// </summary>
        /// <param name="fileToTest"></param>
        /// <param name="numberOfAttempts"></param>
        /// <returns></returns>
        public bool checkIfFileCanBeOpened(string fileToTest, int numberOfAttempts)
        {
            while (numberOfAttempts-- > 0)
                try
                {
                    if (File.Exists(fileToTest)) // if the file doesn't exist anymore also return true
                        using (FileStream fileStream = File.OpenRead(fileToTest))
                        {
                            fileStream.Close();
                        }
                    //     PublicDI.log.info("in checkIfFileCanBeOpened {0}, fileOpen ok , {1}", numberOfAttempts, fileToTest);
                    return true;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            PublicDI.log.info("in checkIfFileCanBeOpened, could not open file , {0}", fileToTest);
            return false;
        }
    }
}
