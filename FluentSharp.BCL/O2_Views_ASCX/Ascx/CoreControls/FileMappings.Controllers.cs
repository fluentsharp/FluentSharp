// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class FileMappings
    {
        private bool runOnLoad = true;
        private const int fileIcon = 1;
        private const int folderIcon = 0;
        public Callbacks.dMethod_String eventAfterSelect;
        public Callbacks.dMethod_String eventDoubleClick;
        public List<String> extensionsFilter = new List<string>();
        public List<String> loadedFiles = new List<string>();
        public Dictionary<String, List<String>> mappingsOfLoadedFiles = new Dictionary<string, List<string>>();

        private const string textFiles = " .txt .settings .xml .xsl .xsd .xstl .js ,html .htm .xhtml";
        private const string extensionFilter_AllFiles = ".*";
        private const string extensionFilter_JavaFiles = ".java .jsp .jsp .properties .jspf .shtml .tld" + textFiles;
        private const string extensionFilter_DotNetFiles = ".cs .vb .aspx .asp .application" + textFiles;

        public void onLoad()
        {
            if (DesignMode == false && runOnLoad)
            {
                setExtensionsToShow(extensionFilter_AllFiles);
                runOnLoad = false;
            }
        }

        public FileMappings()
        {
            InitializeComponent();
        }

        public TreeView getProjectTreeView()
        {
            return tvFileMappings;
        }

        public void loadFilesFromFolder(string folderToLoad, string viewFilter)
        {
            addFolder(folderToLoad, viewFilter);
        }

        public void addFolder(string folderToProcess, string viewFilter)
        {
            this.invokeOnThread(
                () =>
                    {
                        progressBarLoadFiles.Maximum = 10;
                        progressBarLoadFiles.Value = 0;
                        lbStatus.Text = "Adding Dropped folder";
                    });
            try
            {
                if (Directory.Exists(folderToProcess))
                {
                    List<string> lsFiles = null; 
                    var recursiveFileSearchCompleted = false;
                    O2Thread.mtaThread(
                        () =>
                            {
                                lsFiles = Files.getListOfAllFilesFromDirectory(
                                    folderToProcess, cbRecursiveLoadForFolders.Checked,
                                    // calback invoked when search is completed
                                    filesInFolder =>
                                        {
                                            recursiveFileSearchCompleted = true;
                                            PublicDI.log.info("There where {0} files discovered in folder",
                                                        filesInFolder.Count);
                                            foreach (string file in filesInFolder)
                                                addFile(file, false, viewFilter);
                                            this.invokeOnThread(() => showMappingsOnTreeView(viewFilter));
                                        });
                            });                    
                    // start new thread to move the progress bar every second
                    O2Thread.mtaThread(
                        () =>
                            {
                                while (false == recursiveFileSearchCompleted)
                                {
                                    this.invokeOnThread(
                                        () =>
                                            {
                                                if (progressBarLoadFiles.Value ==
                                                    progressBarLoadFiles.Maximum)
                                                    progressBarLoadFiles.Value = 0;
                                                else
                                                    progressBarLoadFiles.Value++;
                                                if (lsFiles != null)
                                                    lbNumberOfFilesLoaded.Text = lsFiles.Count.ToString();
                                                Processes.Sleep(1000);
                                            });
                                }

                            });
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in addFolder:{0}", ex.Message);
            }
        }

        public void addFiles(IEnumerable<string> filesToAdd)
        {
            addFiles(filesToAdd, "");
        }

        public void addFiles(IEnumerable<string> filesToAdd, string viewFilter)
        {
            hideDropHelpInfo();
            foreach(var fileToAdd in filesToAdd)
                addFile(fileToAdd, false, viewFilter);
            showMappingsOnTreeView(viewFilter);
        }

        public void addFile(string fileToAdd, string viewFilter)
        {
            addFile(fileToAdd, true, viewFilter);
        }

        public void addFile(string fileToAdd, bool refreshMappingsView, string viewFilter)
        {
            var extension = Path.GetExtension(fileToAdd).ToLower();
            if (extensionsFilter.Count == 0 || extensionsFilter.Contains(extensionFilter_AllFiles) || extensionsFilter.Contains(extension))
            {
                if (false == mappingsOfLoadedFiles.ContainsKey(extension))
                    mappingsOfLoadedFiles.Add(extension, new List<string>());
                if (false == mappingsOfLoadedFiles[extension].Contains(fileToAdd))
                    mappingsOfLoadedFiles[extension].Add(fileToAdd);
                if (refreshMappingsView)
                    showMappingsOnTreeView(viewFilter);
            }
            //            tvProjectFiles.Nodes.Add(Path.GetFileName(fileName), fileName, "Explorer_File.ico");
        }

        public void showMappingsOnTreeView(string viewFilter)
        {
            O2Thread.mtaThread(
                () =>
                    {
                        double totalFilesSizeInMb = 0;
                        this.invokeOnThread(
                            () =>
                                {
                                    loadedFiles.Clear();
                                    lbSelectedFile.Text = "";                                    
                                    //var nodesWithMappings = new TreeView().Nodes;                                    
                                    progressBarLoadFiles.Maximum = getNumberOfFilesLoaded(mappingsOfLoadedFiles);
                                    lbNumberOfFilesLoaded.Text = "0";
                                    progressBarLoadFiles.Value = 0;
                                    lbStatus.Text = "Mapping Files";
                                });
                        var tempTreeNodeCollection = new List<TreeNode>();
                        foreach (string extension in mappingsOfLoadedFiles.Keys)
                        {
                            // create new extension node as a folder
                            var extensionNode = new TreeNode(extension, folderIcon, folderIcon);
                            // set its color depending on the items on the extensionsFilter list                            
                            // make the node contains reference to its files 
                            extensionNode.Tag = mappingsOfLoadedFiles[extension];
                            foreach (string file in mappingsOfLoadedFiles[extension])
                                if (File.Exists(file))
                                {
                                    if (viewFilter == "" || RegEx.findStringInString(file, viewFilter))
                                    {
                                        var fileSize = (cbShowFileSizes.Checked)
                                                           ? "  :  {0:#,###,###.0} kb".format(Files_WinForms.getFileSize(file) / (double)1024)
                                                           : "";
                                        var nodeText = Path.GetFileName(file) + fileSize;
                                        var newNode = O2Forms.newTreeNode(nodeText, file, fileIcon, file);
                                        newNode.ToolTipText = file;
                                        extensionNode.Nodes.Add(newNode);
                                        loadedFiles.Add(file);
                                    }
                                }
                            if (extensionNode.Nodes.Count > 0)
                            {
                                double filesSizeInMb = Files_WinForms.getFilesSize(mappingsOfLoadedFiles[extension]) /
                                                       (double) (1024*1024);
                                totalFilesSizeInMb += filesSizeInMb;

                                var filesSizeText = (cbShowFileSizes.Checked)
                                                        ? "  :  {0:#,###,###.0} Mb".format(filesSizeInMb)
                                                        : "";

                                extensionNode.Text = "{0}  ({1} files) {2}".format(extension,
                                                                   mappingsOfLoadedFiles[extension].Count, filesSizeText);
                                //tvFileMappings.Nodes.Add(extensionNode);                            
                                tempTreeNodeCollection.Add(extensionNode);
                            }
                            this.invokeOnThread(
                                () =>
                                    {
                                        progressBarLoadFiles.Value += mappingsOfLoadedFiles[extension].Count;
                                        lbNumberOfFilesLoaded.Text = progressBarLoadFiles.Value.ToString();
                                    });
                        }
                        this.invokeOnThread(() => lbStatus.Text = "Populate TreeView");
                        O2Thread.mtaThread(
                            () => this.invokeOnThread(
                                      () =>
                                          {
                                              //foreach(TreeNode node in tempTreeNodeCollection)                                    
                                              tvFileMappings.Nodes.Clear();
                                              tvFileMappings.Sorted = true;
                                              tvFileMappings.Visible = false;
                                              tvFileMappings.Nodes.AddRange(tempTreeNodeCollection.ToArray());
                                              applyColorsToRootNodes();
                                              tvFileMappings.Visible = true;
                                              lbSelectedFile.Text = "Size:  {0:#,###,###.0} Mb".format(totalFilesSizeInMb);
                                              lbStatus.Text = "Load Complete";

                                              setLbNumberOfFilesSelectedText();
                                          }));
                    });
        }

        private static int getNumberOfFilesLoaded(Dictionary<string, List<string>> mappingsOfLoadedFiles)
        {
            var numberOfFilesLoaded = 0;
            foreach (var key in mappingsOfLoadedFiles.Keys)
                numberOfFilesLoaded += mappingsOfLoadedFiles[key].Count;
            return numberOfFilesLoaded;
        }

        private void applyColorsToRootNodes()
        {
            this.invokeOnThread(
                () =>
                    {
                        foreach (TreeNode node in tvFileMappings.Nodes)
                        {
                            var splittedNodeText = node.Text.Split(new string[] {"  "},
                                                                   StringSplitOptions.RemoveEmptyEntries);
                            
                                node.ForeColor = (splittedNodeText.Length > 1) 
                                    ? getNodeColorForExtension(splittedNodeText[0]) :
                                      getNodeColorForExtension("");
                        }
                    });
        }

        private Color getNodeColorForExtension(string extensionName)
        {
            if (extensionsFilter.Contains(extensionFilter_AllFiles) || (extensionsFilter.Contains(extensionName)))
                return Color.Black;
            return Color.LightGray;                           
        }


        public void clearMappings()
        {
            mappingsOfLoadedFiles = new Dictionary<string, List<string>>();
        }

        public void expandTree()
        {
            tvFileMappings.ExpandAll();
        }

        public void setFolderSearchRecursiveMode(bool mode)
        {
            cbRecursiveLoadForFolders.Checked = mode;
        }

        private void setExtensionsToShow_internal(string extentionsToShow)
        {
            extentionsToShow = extentionsToShow.Replace(",", " ");      // remove * and , since we don't use them in the filter but they are obvious choises for users to enter as part of their filter
            extensionsFilter = new List<string>();
            if (extentionsToShow != "")
            {
                string[] extensions = extentionsToShow.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                extensionsFilter.AddRange(extensions);
                // add the trailing dot to the extention if the user didn't provide one
                for (int i = 0; i < extensionsFilter.Count; i++)
                {
                    if (extensionsFilter[i][0] != '.')
                        extensionsFilter[i] = "." + extensionsFilter[i];
                }
            }
            setLbNumberOfFilesSelectedText();
        }
        public void setExtensionsToShow(string extentionsToShow)
        {
            this.invokeOnThread(
             () =>
             {
                 tbExtensionsToShow.Text = extentionsToShow;
                 applyColorsToRootNodes();                 
             });
        }

        private void setLbNumberOfFilesSelectedText()
        {
            this.invokeOnThread(()=> lbNumberOfFilesSelected.Text = getFilesThatMatchCurrentExtensionFilter().Count().ToString());
        }

        public void setExtensionsToShow_DotNetFiles()
        {
            setExtensionsToShow(extensionFilter_DotNetFiles);
        }

        public void setExtensionsToShow_JavaFiles()
        {
            setExtensionsToShow(extensionFilter_JavaFiles);
        }
        
        public List<String> getLoadedFiles()
        {
            return loadedFiles;
        }

        public List<string> getFilesThatMatchCurrentExtensionFilter()
        {
            var files = new List<string>();
            foreach (string extension in mappingsOfLoadedFiles.Keys)
                if (extensionsFilter.Count == 0 || extensionsFilter.Contains(extensionFilter_AllFiles)  || extensionsFilter.Contains(extension))
                    files.AddRange(mappingsOfLoadedFiles[extension]);
            return files;                        
        }

        private static bool deleteFilesMappedToFromSelectedNodeTag(TreeView targetTreeView)
        {
            if (targetTreeView.SelectedNode != null)
            {                
                var filesToDelete = new List<string>();

                if (targetTreeView.SelectedNode.Tag is string)               
                    filesToDelete.Add((string) targetTreeView.SelectedNode.Tag);                
                else if (targetTreeView.SelectedNode.Tag is List<string>)                
                    filesToDelete.AddRange((List<string>) targetTreeView.SelectedNode.Tag);

                if (filesToDelete.Count > 0)                
                    return Files_WinForms.deleteFiles(filesToDelete, true /*askUserForConfirmation*/);                                    
            }
            return false;
        }

        private void hideDropHelpInfo()
        {
            this.invokeOnThread(() => lbDropHelpInfo.Visible = false);
        }

        private void handleDrop(DragEventArgs e)
        {
            O2Thread.mtaThread(
                () =>
                    {
                        string itemDroped = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                        if (itemDroped != "")
                        {

                            if (File.Exists(itemDroped))
                                addFile(itemDroped, true, tbViewFilter.Text);
                            else
                            {
                                if (cbOnDropClearLoadedFiles.Checked)
                                    clearMappings();
                                addFolder(itemDroped, tbViewFilter.Text);
                            }

                            hideDropHelpInfo();
                        }
                    });
        }
    }
}
