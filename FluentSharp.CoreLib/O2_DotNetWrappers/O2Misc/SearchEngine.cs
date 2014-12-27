// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Windows.Forms;
using FluentSharp.CoreLib.API;


namespace FluentSharp.CoreLib.API
{
    public class SearchEngine
    {
        //public Dictionary<String, String> dLines;                
        public Dictionary<String, List<String>> dLoadedFilesCache ; // <FileName, Lines from FileContents>
        //public Dictionary<String, String> dWords;


        public SearchEngine()
        {
            dLoadedFilesCache = new Dictionary<String, List<String>>();
        }

        public int NumberOfFiles
        {
            get { return dLoadedFilesCache.Keys.Count; }
        }

        public int NumberOfLines
        {
            get
            {                
                return (from List<string> lines in dLoadedFilesCache.Values select lines.Count).Sum();                
            }
        }

        public void clearLoadedFiles()
        {
            dLoadedFilesCache = new Dictionary<String, List<String>>();
            //dLines = new Dictionary<string, string>();
            //dWords = new Dictionary<string, string>();
        }
       
        public void loadFiles(List<String> lFilesToLoad)
        {
            loadFiles(lFilesToLoad, null);
        }

        public void loadFiles(List<String> lFilesToLoad, Action<int> onPartialLoad)
        {
            int iFilesProcessed = 0;
            foreach (String sFileToLoad in lFilesToLoad)
            {
                loadFile(sFileToLoad);
                if (iFilesProcessed++ % 100 == 0)
                {
                    PublicDI.log.info("Processed files: {0} /{1}", iFilesProcessed, lFilesToLoad.Count);
                    if (onPartialLoad != null)
                        onPartialLoad(iFilesProcessed);
                }
            }

            // some file stats

            PublicDI.log.debug("Number of Files Currently Loaded: {0}", dLoadedFilesCache.Keys.Count);
            int iLinesOfCode = 0;
            //int iChars = 0;
            foreach (var lsLines in dLoadedFilesCache.Values)
                iLinesOfCode += lsLines.Count;
            PublicDI.log.debug("Number of Lines of Code Currently Loaded: {0}", iLinesOfCode);
        }

        public void loadFiles(String sTargetDir, String sExtension, bool bRecursive)
        {
            loadAllFilesFromDirectoryThatMatchExtension(sTargetDir, sExtension, bRecursive);
        }

        public void loadAllFilesFromDirectoryThatMatchExtension(String sTargetDir, String sExtension, bool bRecursive)
        {
            if (false == Directory.Exists(sTargetDir))
            {
                PublicDI.log.debug("Directory provide does not exist: {0}", sTargetDir);
                return;
            }
            var lFilesToLoad = new List<string>();
            Files.getListOfAllFilesFromDirectory(lFilesToLoad, sTargetDir, bRecursive, sExtension, false);
            PublicDI.log.debug("{0} files found, loading them", lFilesToLoad.Count);
            loadFiles(lFilesToLoad);
        }

        public bool loadFile(String sFileToLoad)//, bool bTryToFilesFromAssesmentRun)
        {
            try
            {

                if (false == dLoadedFilesCache.ContainsKey(sFileToLoad))
                {
                    if (false == File.Exists(sFileToLoad))
                        PublicDI.log.error("Submited item is not a file: {0}", sFileToLoad);
                    else
                        //if (false == bTryToFilesFromAssesmentRun)  // || false == tryToLoadAllFilesFromAssessmentRunFile(sFileToLoad, dLoadedFilesCache))
                    {
                        String sFileContents = Files.getFileContents(sFileToLoad);
                        if (sFileContents.IndexOf('\x00') > -1)
                        {
                            PublicDI.log.error("Skipping load of file since it has at least one char 0 (zero) on it: {0}", sFileToLoad);
                        }
                        else
                        {
                            // fix the cases where \n is used instead of \r\n
                            if (sFileContents.IndexOf("\r\n") == -1)
                                sFileContents = sFileContents.Replace("\n", "\r\n");
                            //String[] sSplit = sFileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            dLoadedFilesCache.Add(sFileToLoad,
                                                  new List<String>(sFileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.None)));
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In loadFile, error while processing file: {0} : {1}", sFileToLoad, ex.Message);
            }
            return false;

            // public void updateListOfLoadedFiles()
        }

        public void clearLoadedFileCache()
        {
            dLoadedFilesCache.Clear();
        }
       
        public List<TextSearchResult> searchFor(String sRegExToSearch)
        {
            return executeSearch(sRegExToSearch);
        }

        public List<TextSearchResult> executeSearch(String sRegExToSearch)
        {
            return executeSearch(new List<String>(new[] {sRegExToSearch}));
        }

        public List<TextSearchResult> executeSearch(List<String> lsRegExToSearch)
        {
            var lreSearchRegEx = new List<Regex>();
            try
            {
                foreach (String sSearchCriteria in lsRegExToSearch)
                    lreSearchRegEx.Add(RegEx.createRegEx(sSearchCriteria));
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
            }
            
            return executeSearch(lreSearchRegEx);
        }

        public List<TextSearchResult> executeSearch(List<Regex> lreRegExToSearch)
        {
            // O2Timer tO2Timer = new O2Timer("text Search").start();
            var tsrSearchResults = new List<TextSearchResult>();
            try
            {
                //var sbResult = new StringBuilder();
                foreach (Regex rRegEx in lreRegExToSearch)
                {
                    foreach (String sFileToSearch in dLoadedFilesCache.Keys)
                    {
                        for (int iLine = 0; iLine < dLoadedFilesCache[sFileToSearch].Count; iLine++)
                        {
                            Match mMatch = rRegEx.Match(dLoadedFilesCache[sFileToSearch][iLine]);
                            if (mMatch.Success)
                                tsrSearchResults.Add(new TextSearchResult(rRegEx, mMatch.Value,
                                                                          dLoadedFilesCache[sFileToSearch][iLine],
                                                                          sFileToSearch, iLine, mMatch.Index,
                                                                          mMatch.Length));
                        }
                        //List<String> lsMatchedLines = regEx.execRegExOnText_getLines(rRegEx, dLoadedFilesCache[sFileToSearch]);
                        //foreach (String sMatchLine in lsMatchedLines)
                        //    tsrSearchResults.Add(new textSearchResult(rRegEx,sMatchLine,sFileToSearch)); 
                    }
                }
                PublicDI.log.info("Number of search matches: {0}", tsrSearchResults.Count);
                //tO2Timer.stop();
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex);
            }
            return tsrSearchResults;
        }

        public Dictionary<String, List<String>> getLoadedFilesCache()
        {
            return dLoadedFilesCache;
        }
    }

        public class TextSearchResult
    {
        public int iLength;
        public int iLineNumber;
        public int iPosition;
        public Regex rRegExUsed;
        public String sFile;
        public String sMatchLine;
        public String sMatchText;

        public TextSearchResult(Regex rRegExUsed, String sMatchText, String sMatchLine, String sFile,
                                int iLineNumber, int iPosition, int iLength)
        {
            this.rRegExUsed = rRegExUsed;
            this.sMatchText = sMatchText;
            this.sMatchLine = sMatchLine;
            this.sFile = sFile;
            this.iLineNumber = iLineNumber;
            this.iPosition = iPosition;
            this.iLength = iLength;
        }
    }
}
