// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Drawing;
using System.Windows.Forms;
using FluentSharp.WinForms;
using FluentSharp.CoreLib;

namespace FluentSharp.REPL.Controls
{
    public class ascx_FolderView : Control
    {   
        public string RootFolder { get; set; }
    	
        public TreeView FolderView { get; set; }    	    	
        public ascx_SourceCodeViewer CodeViewer { get; set; }
    	
        public string 	Title_FolderView 	{ get; set; }
        public string 	Title_CodeEditor 	{ get; set; }
        public int		SplitterDistance	{ get; set; }
    	
        public ascx_FolderView() : this(true)
        {
		
        }
        public ascx_FolderView(bool buildGuiOnCtor)
        {
            this.Width = 300;
            this.Height = 300;    		
            Title_FolderView = "Folder and Files";
            Title_CodeEditor = "File Contents";
            SplitterDistance = 150;
            if (buildGuiOnCtor)
                buildGui();
        }
 
 	
 
        public ascx_FolderView buildGui()
        {
            var topPanel = this.clear().add_Panel();
            CodeViewer = topPanel.title(Title_CodeEditor).add_SourceCodeViewer();
            FolderView = topPanel.insert_Left(SplitterDistance, Title_FolderView).add_TreeView();
			
            FolderView.afterSelect<string>(
                (fileOrFolder)=>{
                                    if (fileOrFolder.fileExists())
                                        CodeViewer.open(fileOrFolder);
                });										
			
            FolderView.beforeExpand<string>((treeNode, path) => loadFolder(treeNode,path));
			
            FolderView.onDrop((fileOrfolder) => {
                                                    FolderView.clear();
                                                    if (fileOrfolder.dirExists())
                                                        loadFolder(FolderView.rootNode(),fileOrfolder); 
            });						
            FolderView.add_ContextMenu()
                      .add_MenuItem("Refresh",true, ()=> refresh())
                      .add_MenuItem("Open in Windows Explorer", 
                                    ()=> FolderView.selected().get_Tag().str().startProcess() );
						
            CodeViewer.set_Text("....select file on the left to view its contents here...");			 
            return this;
        }		
		
        public ascx_FolderView refresh()
        {
            this.loadFolder(RootFolder);
            return this;
        }
		
        public ascx_FolderView reloadSelectdNode()
        {
            var treeNode = this.FolderView.selected();
            if (treeNode.notNull())
                loadFolder(treeNode,(string)treeNode.Tag);
            return this;
        }
		
        public ascx_FolderView loadFolder(TreeNode treeNode, string path)
        {
            "Loading Folder: {0}".info(path);
            if (path.valid())
            {
                FolderView.beginUpdate();
                path = Environment.ExpandEnvironmentVariables(path);  // in case there are Environment variables like %SystemDrive%
                //"There are {0} files {0}".info(path.files().size());
                //"There are {0} folders {0}".info(path.folder.folders().size());
                var folders = path.folders().sort();
                foreach(var folder in folders)
                    treeNode.add_Node(folder.fileName(), folder, folder.files().size() >0 || folder.folders().size()>0)
                            .color(Color.DarkOrange);
                var files = path.files();
                treeNode.add_Nodes(files, (file)=> file.fileName(), (file)=>file, (file)=>false,(file)=>Color.DarkBlue);
                FolderView.endUpdate();
            }
            return this;
        }
		
        public ascx_FolderView loadFolder(string path)
        {			
            if (path.notNull())
                RootFolder = Environment.ExpandEnvironmentVariables(path);
            FolderView.clear();
            return loadFolder(FolderView.rootNode(),RootFolder);
        }
		
    }
}