// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{
    public class SourceCodeMappingsUtils  
    {     
        public static SourceCodeMappings sourceCodeMappings;
        public static string sourceCodeMappingFileName;

        static SourceCodeMappingsUtils()
        {
            sourceCodeMappingFileName = "SourceCodeMappingsFile.xml";			// must be set before the call to getSourceCodeMappings();
            sourceCodeMappings = SourceCodeMappingsUtils.getSourceCodeMappings();
        }
        public static void loadSourceCodeMappings(DataGridView targetDataGridView)
        {
            targetDataGridView.invokeOnThread(
                () =>
                    {
                        targetDataGridView.Columns.Clear();
                        O2Forms.addToDataGridView_Column(targetDataGridView, "replaceThisString", -1);
                        O2Forms.addToDataGridView_Column(targetDataGridView, "withThisString", -1);
                        var scmSourceCodeMappings = getSourceCodeMappings();
                        foreach (SourceCodeMappingsMapping mMapping in scmSourceCodeMappings.Mapping)
                        {
                            O2Forms.addToDataGridView_Row(targetDataGridView, null,
                                                          new object[]
                                                              {mMapping.replaceThisString, mMapping.withThisString});
                        }
                    });
        }
        public static String resolveCommonPath(String sPath1, String sPath2)
        {
            String sCommonPath = "";
            String[] asSplittedPath1 = sPath1.Split('\\');
            String[] asSplittedPath2 = sPath2.Split('\\');
            int iPath1Pos = asSplittedPath1.Length;
            int iPath2Pos = asSplittedPath2.Length;
            while (iPath1Pos > 0 && iPath2Pos > 0)
            {
                //iPath1Pos--;
                //iPath2Pos--;
                if (asSplittedPath1[--iPath1Pos] == asSplittedPath2[--iPath2Pos])
                    sCommonPath = Path.Combine(asSplittedPath1[iPath1Pos], sCommonPath);
                else
                    break;
            }
            return sCommonPath;
        }

        public static String askUserToFindFileOnLocalDisk(String sFileToFind)
        {
            if (sFileToFind != "")
            {
                PublicDI.log.info("Asking the user to resolve the file reference: {0}", sFileToFind);
                String sFilename = Path.GetFileName(sFileToFind);
                var opdOpenFileDialog = new OpenFileDialog();
                opdOpenFileDialog.Filter = (sFilename + "|" + sFilename);
                DialogResult drDialogResult = opdOpenFileDialog.ShowDialog();
                if (drDialogResult == DialogResult.OK)
                    return opdOpenFileDialog.FileName;
            }
            return "";
        }

        public static String getSourceCodeMappingsFile()
        {
            return Path.Combine(PublicDI.config.O2TempDir, SourceCodeMappingsUtils.sourceCodeMappingFileName);
        }
        
        public static SourceCodeMappings getSourceCodeMappings()
        {
			try
			{
				if (File.Exists(getSourceCodeMappingsFile()))
					return
						(SourceCodeMappings)Serialize.getDeSerializedObjectFromXmlFile(getSourceCodeMappingsFile(), typeof(SourceCodeMappings));
			}
			catch (Exception ex)
			{
				"[getSourceCodeMappings] {0}".error(ex.Message);
			}

            var scmSourceCodeMappings = new SourceCodeMappings();
            scmSourceCodeMappings.Mapping = new SourceCodeMappingsMapping[] {};
            return scmSourceCodeMappings;
        }

        public static void saveSourceCodeMappings(SourceCodeMappings scmSourceCodeMappings)
        {
            Serialize.createSerializedXmlFileFromObject(scmSourceCodeMappings, getSourceCodeMappingsFile(), null);
        }

        public static SourceCodeMappings getSourceCodeMappingsFromDataGridView(DataGridView dgvDataGridView)
        {
            if (dgvDataGridView.Columns.Count != 2)
            {
                PublicDI.log.error(
                    "in getSourceCodeMappingsFromDataGridView: invalid DataGridView : dgvDataGridView.Columns.Count != 2 ");
                return null;
            }

            var lmMappings = new List<SourceCodeMappingsMapping>();
            foreach (DataGridViewRow rRow in dgvDataGridView.Rows)
            {
                if (rRow.Cells[0].Value != null && rRow.Cells[1].Value != null)
                {
                    var mMapping = new SourceCodeMappingsMapping();
					mMapping.withThisString = rRow.Cells[1].Value.ToString();
					mMapping.replaceThisString = rRow.Cells[0].Value.ToString();
                    lmMappings.Add(mMapping);
                }
            }
            var scmSourceCodeMappings = new SourceCodeMappings();
			scmSourceCodeMappings.Mapping = lmMappings.ToArray();
            return scmSourceCodeMappings;

        }

        public static string mapFile(string fileToMap,Control hostControl)
        {
            if (hostControl != null)
                return (string) hostControl.invokeOnThread(() =>
                                                               {
                                                                   if (fileToMap != null && false == File.Exists(fileToMap))
                                                                   {
                                                                       var resolvedFileMapping =
                                                                           new resolvedFileMapping(fileToMap);
                                                                       if (resolvedFileMapping.resolveFileMapping())
                                                                           return resolvedFileMapping.sMappedFile;
                                                                       else
                                                                           PublicDI.log.error(
                                                                               "in SourceCodeMappingsUtils.mapFile, could not map file: {0}",
                                                                               fileToMap);
                                                                   }
                                                                   return fileToMap;
                                                               });
            return mapFile(fileToMap);
        }

        public static string mapFile(string fileToMap)
        {
            if (File.Exists(fileToMap))
                return fileToMap;
            var resolvedFileMapping = new resolvedFileMapping(fileToMap);
            if (resolvedFileMapping.tryToResolveUsingCurrentSourceCodeMappings())
                return resolvedFileMapping.sMappedFile;
            PublicDI.log.error("in SourceCodeMappingsUtils.mapFile, could not map file: {0}", fileToMap);
            return fileToMap;
        }

        #region Nested type: resolvedFileMapping

        public class resolvedFileMapping
        {            
            public String sCommonPath;
            public String sFix_PathToFind;
            public String sFix_PathToReplace;
            public String sMappedFile;
            public String sOriginalFile;

            public resolvedFileMapping(String sFileToMap)
            {
                sOriginalFile = sFileToMap;
            }

            public bool resolveFileMapping()
            {
                if (sOriginalFile == null)
                    return false;
                if (tryToResolveUsingCurrentSourceCodeMappings())
                    return true;
                sMappedFile = askUserToFindFileOnLocalDisk(sOriginalFile);
                if (sMappedFile != "")
                {
                    sCommonPath = resolveCommonPath(sOriginalFile, sMappedFile);
                    if (sCommonPath == "")
                        return false;
                    sFix_PathToFind = sOriginalFile.Replace(sCommonPath, "");
                    sFix_PathToReplace = sMappedFile.Replace(sCommonPath, "");
                    addMappingToCurrentListAndSaveIt(sFix_PathToFind, sFix_PathToReplace);
                    return true;
                }                
                return false;
            }

            public void addMappingToCurrentListAndSaveIt(string pathToFind, string pathToReplace)
            {
                var mappings = new List<SourceCodeMappingsMapping>(SourceCodeMappingsUtils.sourceCodeMappings.Mapping);

				var sourceCodeMappings = new SourceCodeMappingsMapping();
				sourceCodeMappings.withThisString = pathToReplace;
				sourceCodeMappings.replaceThisString = pathToFind;
				
                mappings.Add(sourceCodeMappings);
                SourceCodeMappingsUtils.sourceCodeMappings.Mapping = mappings.ToArray();
                saveSourceCodeMappings(SourceCodeMappingsUtils.sourceCodeMappings);
            }

            public bool tryToResolveUsingCurrentSourceCodeMappings()
            {
                if (SourceCodeMappingsUtils.sourceCodeMappings != null)
                    foreach (var sourceCodeMapping in SourceCodeMappingsUtils.sourceCodeMappings.Mapping)
                    {
                        var mappedFile = (sourceCodeMapping.replaceThisString != "") ?
                            sOriginalFile.Replace(sourceCodeMapping.replaceThisString, sourceCodeMapping.withThisString) :
                            Path.Combine(sourceCodeMapping.withThisString,sOriginalFile);
                        if (File.Exists(mappedFile))
                        {
                            sMappedFile = mappedFile;
                            return true;
                        }
                    }
                return false;
            }
        }

        #endregion



        
    }
}
