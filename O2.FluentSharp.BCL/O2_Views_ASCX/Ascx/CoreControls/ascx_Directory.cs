// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.Kernel.CodeUtils;
using O2.Views.ASCX;

namespace O2.Views.ASCX.CoreControls
{
    public partial class ascx_Directory : UserControl
    {
        #region Delegates and Events

        public delegate void dDirectoryEvent(String sValue);

        public event dDirectoryEvent eDirectoryEvent_Click;
        public event dDirectoryEvent eDirectoryEvent_DoubleClick;
        public event Callbacks.dMethod_String _onTreeViewDrop;
        public event FolderWatcher.CallbackOnFolderWatchEvent _onFileWatchEvent;
        public event Callbacks.dMethod_String _onDirectoryRefresh;
        public event Callbacks.dMethod_String _onDirectoryClick;
        public event Callbacks.dMethod_String _onDirectoryDoubleClick;



        #endregion

        #region ViewMode enum

        public enum ViewMode
        {
            Simple,
            Simple_With_LocationBar,
            Advanced
        }

        #endregion

        private FolderWatcher folderWatcher;
        //public String fileFilter = "*.*";
        private ViewMode viewMode = ViewMode.Advanced;
        public List<string> listOfExtensionsToShowToolTipContents = new List<string> { ".cs" , ".o2", ".h2" };

        public ascx_Directory()
        {
            InitializeComponent();
            _ProcessDroppedObjects = true;
            _ShowFileSize = false;
            _ShowLinkToUpperFolder = true;
            _FileFilter = "*.*";
            if (DI.config!=null)
                openDirectory(DI.config.O2TempDir); // if not specified one, open the temp directory

			//tvDirectory.showToolTip();

            tvDirectory.onDrag<string>();
        }

        public ascx_Directory(String sDirectoryToOpen) : this()
        {
            InitializeComponent();
            openDirectory(sDirectoryToOpen);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public ViewMode _ViewMode
        {
            set
            {
                this.invokeOnThread(
                    () =>
                        {
                            viewMode = value;
                            switch (value)
                            {
                                case ViewMode.Simple:
                                    scAddressAndRest.Panel1Collapsed = true;
                                    scViewerAndSettings.Panel2Collapsed = true;
                                    break;
                                case ViewMode.Simple_With_LocationBar:
                                    scAddressAndRest.Panel1Collapsed = false;
                                    scViewerAndSettings.Panel2Collapsed = true;
                                    break;
                                case ViewMode.Advanced:
                                    scAddressAndRest.Panel1Collapsed = false;
                                    scViewerAndSettings.Panel2Collapsed = false;
                                    break;
                            }
                        });
            }
            get { return viewMode; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _HandleDrop 
        {
            get { return tvDirectory.AllowDrop; }
            set { tvDirectory.AllowDrop = value; } 
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowFileContentsOnTopTip { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public string _FileFilter { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ProcessDroppedObjects { get; set; }

        /*    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _autoOpenFileOnClick { get; set; }*/

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowFileSize { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _ShowLinkToUpperFolder { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _HideFiles{ get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        public bool _WatchFolder
        {
            set
            {
                // setShowLinkToUpperFolder(
                // {
                // }
                //)
                if (cbWatchFolder.InvokeRequired)
                    cbWatchFolder.Invoke(new EventHandler(delegate { _WatchFolder = value; }));
                else
                {
                    cbWatchFolder.Checked = value;
                    if (value)
                        setupFolderWatched();
                    else if (folderWatcher != null)
                        folderWatcher.enabled = false;
                }
            }
            get { return cbWatchFolder.Checked; }
        }

        
        public void openDirectory(String sDirectoryToOpen, String fileFilter)
        {
            _FileFilter = fileFilter;
            openDirectory(sDirectoryToOpen);
        }

        public void setDirectory(String sDirectoryToOpen)
        {
            openDirectory(sDirectoryToOpen);
        }

        public void openDirectory(String sDirectoryToOpen)
        {
            try
            {
                if (sDirectoryToOpen.valid())
                    if (tbCurrentDirectoryName.okThread(delegate { openDirectory(sDirectoryToOpen); }))
                    {
                        if (sDirectoryToOpen.dirExists())
                        {
                            //Files.checkIfDirectoryExistsAndCreateIfNot(sDirectoryToOpen);
                            tbCurrentDirectoryName.Text = Path.GetFullPath(sDirectoryToOpen);
                            setupFolderWatched();
                            refreshDirectoryView();
                        }
                    }
            }
            catch (Exception ex)
            {
                DI.log.error("in openDirectory: {0}", ex.Message);
            }
        }

        private void tvDirectory_DoubleClick(object sender, EventArgs e)
        {
            if (tvDirectory.SelectedNode != null && tvDirectory.SelectedNode.Tag != null)
            {
                var sSelectedNodeTag = (String) tvDirectory.SelectedNode.Tag;

                if (eDirectoryEvent_DoubleClick != null)
                    eDirectoryEvent_DoubleClick(sSelectedNodeTag);

                Callbacks.raiseRegistedCallbacks(_onDirectoryDoubleClick, new[] { sSelectedNodeTag });

                if (Directory.Exists(sSelectedNodeTag))
                {
                    tbCurrentDirectoryName.Text = sSelectedNodeTag;
                    openDirectory(tbCurrentDirectoryName.Text);

                    // Bug hack: fix the missing upper directory link problem
                    if (tvDirectory.Nodes.Count == 1 && tvDirectory.Nodes[0].Text == "..")
                    {
                        tvDirectory.Nodes[0].Text = "..";
                    }                                        
                }
            }            
        }

        private void btDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                deleteSelectedFileOrFolder();
                //refreshDirectoryView();
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in btDeleteFile_Click");
            }
        }


        private void deleteSelectedFileOrFolder()
        {
            if (tvDirectory.SelectedNode != null  && tvDirectory.SelectedNode.Text != ".." &&
                tvDirectory.SelectedNode.Tag != null && tvDirectory.SelectedNode.Tag is string)            
                deleteFileOrFolder((String)tvDirectory.SelectedNode.Tag);            
        }

        public bool deleteFileOrFolder(string fileOrFolderToDelete)
        {
            if (File.Exists(fileOrFolderToDelete))
            {
                if (DialogResult.Yes ==
                    MessageBox.Show("Do you want to delete the file " + (String)tvDirectory.SelectedNode.Tag,
                                    "Confirm Delete", MessageBoxButtons.YesNo))                
                    return Files.deleteFile(fileOrFolderToDelete);
            }
            else if (Directory.Exists(fileOrFolderToDelete))
                if (DialogResult.Yes ==
                    MessageBox.Show(
                        "Do you want to delete this Directory (all files and folders will be recursively deleted): \n\n" +
                        fileOrFolderToDelete, "Confirm Delete", MessageBoxButtons.YesNo))
                {
                    Files.deleteFolder(fileOrFolderToDelete, true /* deleteRecursively*/);                        
                    return true;
                }
            return false;
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            refreshDirectoryView();
        }

        public void refreshDirectoryView()
        {
            if (tvDirectory.okThread(delegate { refreshDirectoryView(); }))
            {
                O2Forms.loadTreeViewWithDirectoriesAndFiles(tvDirectory, tbCurrentDirectoryName.Text, _FileFilter,
                                                            tbCurrentDirectoryName, _ShowFileSize, _HideFiles);
                if (false == _ShowLinkToUpperFolder && tvDirectory.Nodes.Count > 0 && tvDirectory.Nodes[0].Text == "..")
                    tvDirectory.Nodes.RemoveAt(0);

                Callbacks.raiseRegistedCallbacks(_onDirectoryRefresh, new[] {tbCurrentDirectoryName.Text});
            }
        }

        private void btCreateDirectory_Click(object sender, EventArgs e)
        {
            if (tbCurrentDirectoryName.Text == "")
                DI.log.error("in btCreateFile_Click, tbCurrentDirectoryName.Text == \"\"");
            else
            {
                String sNewDirectoryName = Path.Combine(tbCurrentDirectoryName.Text, tbNewDirectoryName.Text);
                Directory.CreateDirectory(sNewDirectoryName);
                if (Directory.Exists(sNewDirectoryName))
                {
                    DI.log.debug("Directory created: {0}", sNewDirectoryName);
                    refreshDirectoryView();
                }
                else
                    DI.log.error("Directory not created : {0}", sNewDirectoryName);
            }
        }

        private void btCreateFile_Click(object sender, EventArgs e)
        {
            if (tbCurrentDirectoryName.Text == "")
                DI.log.error("in btCreateFile_Click, tbCurrentDirectoryName.Text == \"\"");
            else
            {
                if (tbNewFileName.Text != "")
                {
                    String sNewFileName = Path.Combine(tbCurrentDirectoryName.Text, tbNewFileName.Text);
                    FileStream fsNewFile = File.Create(sNewFileName);
                    fsNewFile.Close();
                    if (File.Exists(sNewFileName))
                    {
                        DI.log.debug("File created: {0}", sNewFileName);
                        refreshDirectoryView();
                    }
                    else
                        DI.log.error("File not created : {0}", sNewFileName);
                }
                else
                    DI.log.error("New file name not provided");
            }
        }

        private void tvDirectory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvDirectory.SelectedNode.Tag == null)       // if the Tag is not set
                tvDirectory.SelectedNode = null;            // don't let this node be selected
            else   
            {
                var sSelectedNodeTag = (String) tvDirectory.SelectedNode.Tag;
                if (eDirectoryEvent_Click != null)                
                    eDirectoryEvent_Click(sSelectedNodeTag);
                if (sSelectedNodeTag != null)
                {
                    renameFileToolStripMenuItem.Enabled = File.Exists(sSelectedNodeTag);
                    deleteFileToolStripMenuItem.Enabled = File.Exists(sSelectedNodeTag);
                    deleteFolderToolStripMenuItem.Enabled = (tvDirectory.SelectedNode.Text != ".." && Directory.Exists(sSelectedNodeTag));
                    if (File.Exists(sSelectedNodeTag))
                    {
                 
                        tbRenameSelectedItem.Text = sSelectedNodeTag;
                        tbRenameSelectedItem.Tag = sSelectedNodeTag;
                        tbRenameSelectedItem.TextBox.Width = sSelectedNodeTag.Length * 7;
                        tbRenameSelectedItem.Width = sSelectedNodeTag.Length * 7;
                    }                    
                }

                Callbacks.raiseRegistedCallbacks(_onDirectoryClick, new[] { sSelectedNodeTag });

                setupFolderWatched();
            }
            ///Bug: Missing TreeNode on TreeView with 1 Node
            /// no idea why but when there is only one TreeNode, it is set with the ForeColor of 0;0;0 (i.e. white)
            /// the short term fix is to make it black here
            if (e.Node.ForeColor.IsEmpty) // == System.Drawing.Color.White)
                e.Node.ForeColor = System.Drawing.Color.Black;
        }

        private void tvDirectory_Click(object sender, EventArgs e)
        {
        }

        private void tbCurrentDirectoryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
                openDirectory(tbCurrentDirectoryName.Text);
        }

        public void hideFileAndDirectoryCreationControls()
        {
            btCreateDirectory.Visible = false;
            btCreateFile.Visible = false;
            tbNewDirectoryName.Visible = false;
            tbNewFileName.Visible = false;
        }

        public void simpleMode()
        {
            _ViewMode = ViewMode.Simple;

            /*this.invokeOnThread(
                () =>
                    {
                        tvDirectory.Dock = DockStyle.Fill;
                        tvDirectory.BringToFront();
                    });*/
            
        }

        public ascx_Directory simpleMode_withAddressBar()
        {
            _ViewMode = ViewMode.Simple_With_LocationBar;
            return this;
            /*
            this.invokeOnThread(
                () =>
                    {
                        tbCurrentDirectoryName.Top = 0;
                        tbCurrentDirectoryName.BringToFront();

                        tvDirectory.Top = 20;
                        tvDirectory.Height = Height - 20;
                        tvDirectory.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right |
                                             AnchorStyles.Top;
                        tvDirectory.BringToFront();
                    });
             */ 
        }

        public String getCurrentDirectory()
        {
            return tbCurrentDirectoryName.get_Text();            
        }

        public String getSelectedItem()
        {
            return tvDirectory.SelectedNode.Name;
        }

        public String getSelectedItem_FullPath()
        {
            if (tbCurrentDirectoryName.Text != null && tvDirectory.SelectedNode.Name != null)
                return Path.Combine(tbCurrentDirectoryName.Text, tvDirectory.SelectedNode.Name);
            return "";
        }

        private void ascx_Directory_Load(object sender, EventArgs e)
        {
            this.onClosed(
                ()=>{
                        if (folderWatcher != null)
                            folderWatcher.disable();
                    });
        }

        /*   private void llMode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }*/


        public void setupFolderWatched()
        {
            if (folderWatcher != null)
            {
                if (folderWatcher.folderWatched == getCurrentDirectory())
                    return;
                else
                    folderWatcher.disable();
            }
                                            
            if (cbWatchFolder.Checked)
                folderWatcher = new FolderWatcher(getCurrentDirectory(), folderChangesCallback);
        }

        private void folderChangesCallback(FolderWatcher callback)
        {
            Callbacks.raiseRegistedCallbacks(_onFileWatchEvent, new[] {callback});

            if (callback.folderWatched == getCurrentDirectory())    // if there is no parent form it means this is not being used any more
            {
                if (this.parentForm().isNull())
                {
                    folderWatcher.enabled = false;
                    "Directory Viewer not used anymore, so disabling it".info();
                }
                else
                    refreshDirectoryView();
            }
        }

        private void cbMonitorFolder_CheckedChanged(object sender, EventArgs e)
        {
            _WatchFolder = cbWatchFolder.Checked;
        }

        public void setShowLinkToUpperFolder(bool value)
        {
            _ShowLinkToUpperFolder = value;
        }

        public void setMoveOnDrag(bool value)
        {
            if (cbMoveOnDrag.InvokeRequired)
                cbMoveOnDrag.Invoke(new EventHandler(delegate { setMoveOnDrag(value); }));
            else
                cbMoveOnDrag.Checked = value;
        }

        private void btSelectDirectory_Click(object sender, EventArgs e)
        {
            tbCurrentDirectoryName.Text = tbCurrentDirectoryName.Text != "..."
                                              ? O2Forms.askUserForDirectory(DI.config.CurrentExecutableDirectory)
                                              : O2Forms.askUserForDirectory(tbCurrentDirectoryName.Text);

            refreshDirectoryView();
        }


/*        private void tvDirectory_ItemDrag(object sender, ItemDragEventArgs e)
        {
            tvDirectory.invokeOnThread(
                () =>
                {
                    tvDirectory.SelectedNode = (TreeNode)e.Item;
                    var dragData = tvDirectory.SelectedNode.Tag.ToString();

                    //Note: see if there is a way to make this work since I have had a couple race condition with treeview Drag& Drops
                    //O2Thread.mtaThread(
                    //    () =>
                    //    {
                            if (DragDropEffects.Copy == DoDragDrop(dragData, DragDropEffects.Copy))
                                if (cbMoveOnDrag.Checked)
                                    Files.deleteFile(tvDirectory.SelectedNode.Tag.ToString());
                        //});
                });
            

        }*/

        private void tvDirectory_DragEnter(object sender, DragEventArgs e)
        {
            string fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e, false /*downloadIfHttp */);
            if (File.Exists(fileOrFolder))
            {
                if (Path.GetDirectoryName(fileOrFolder) == getCurrentDirectory())
                    return;
            }
            else if (fileOrFolder == getCurrentDirectory())
                return;
            e.Effect = DragDropEffects.Copy;
        }

        private void tvDirectory_DragDrop(object sender, DragEventArgs e)
        {
            if (_onTreeViewDrop != null)
                _onTreeViewDrop(Dnd.tryToGetFileOrDirectoryFromDroppedObject(e));
            if (_ProcessDroppedObjects)
            {
                string fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
                if (File.Exists(fileOrFolder) && Path.GetDirectoryName(fileOrFolder) != getCurrentDirectory())
                {
                    var fileName = "";
                    if (Path.GetDirectoryName(fileOrFolder) + '\\' == DI.config.O2TempDir)
                        fileName = Files.MoveFile(fileOrFolder, getCurrentDirectory());
                    else
                        fileName = Files.Copy(fileOrFolder, getCurrentDirectory());
                    if (fileName != "")
                        _onDirectoryDoubleClick(fileName);
                }
                else if (Directory.Exists(fileOrFolder) && fileOrFolder != getCurrentDirectory())
                    Files.copyFolder(fileOrFolder, getCurrentDirectory());
            }
        }

        private void tvDirectory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btDeleteFile_Click(null, null);
        }

        public List<string> getFiles()
        {
            return Files.getFilesFromDir_returnFullPath(getCurrentDirectory(),_FileFilter);
        }

        public TreeView getTreeView()
        {
            return tvDirectory;
        }

        public void setFileFilter(string fileFilter)
        {
            _FileFilter = fileFilter;
            refreshDirectoryView();
        }

        public void setDisableMove(bool value)
        {
            this.invokeOnThread(() => this.Enabled = value);
        }

        private void miCreateDirectory_Click(object sender, EventArgs e)
        {
            if (miNewDirectoryName.Text != null)
            {
                var targetDirectory = Path.Combine(getCurrentDirectory(), miNewDirectoryName.Text);
                Directory.CreateDirectory(targetDirectory);
            }
        }

        private void miNewDirectoryName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                miCreateDirectory_Click(null, null);               
                directoryMenu.Close();                
            }
        }

        private void tbRenameSelectedItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender is ToolStripControlHost && ((ToolStripControlHost)sender).Tag!=null)
                {
                    string originalFile = (string)((ToolStripControlHost)sender).Tag;
                    string targetfile = (string)((ToolStripControlHost)sender).Text;
                    if (originalFile != targetfile && File.Exists(originalFile))
                    {
                        Files.MoveFile(originalFile, targetfile);
                    }
                }
                directoryMenu.Close();
            }
        }

        private void tvDirectory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var currentNode = tvDirectory.GetNodeAt(e.Location);
                if (currentNode != null)
                    tvDirectory.SelectedNode = currentNode;
                allowDragAndDropToolStripMenuItem.Enabled = !tvDirectory.AllowDrop;
            }
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedFileOrFolder();
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedFileOrFolder();
        }

        private void tvDirectory_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void tvDirectory_MouseHover(object sender, EventArgs e)
        {
            
        }

       /* private void tvDirectory_MouseMove(object sender, MouseEventArgs e)
        {
            var currentNode = tvDirectory.GetNodeAt(e.Location);
            if (currentNode != null)
            {
                setNodeTagToFileContents(currentNode);
                //DI.log.info(currentNode.Text);
            }
        }*/

/*        private void setNodeTagToFileContents(TreeNode currentNode)
        {			
            if (_ShowFileContentsOnTopTip &&  currentNode.Tag != null && currentNode.ToolTipText == "") 
            {
                var currentNodeFile = currentNode.Tag.ToString();
                if (File.Exists(currentNodeFile.ToString()))
                {
                    var fileExtension = Path.GetExtension(currentNodeFile);
                    if (listOfExtensionsToShowToolTipContents.Contains(fileExtension))
                    {
                        var fileContents = Files.getFileContents(currentNodeFile);
                        currentNode.ToolTipText = fileContents;

                        / *currentNode.ToolTipText = (fileContents.Length < 2000)
                                ? fileContents
                                : fileContents.Substring(0, 2000);* /
                    }                    
                }
            }
        }
*/		
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshDirectoryView();
        }

        private void allowDragAndDropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvDirectory.AllowDrop = true;            
        }
    }
}
