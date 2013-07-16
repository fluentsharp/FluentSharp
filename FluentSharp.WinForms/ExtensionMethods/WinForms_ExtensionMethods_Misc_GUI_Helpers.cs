using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Misc_GUI_Helpers
    {           
        public static Action<string> insert_FolderViewer_Simple(this Control targetPanel, Action<string> openFile) 	
        {
            var startFolder = "".tempDir().parentFolder();
            return targetPanel.insert_FolderViewer_Simple(openFile, startFolder);            
        }
        public static Action<string> insert_FolderViewer_Simple(this Control targetPanel, Action<string> openFile, string startFolder) 	
        {
            Action<string> openFolder = null;

            targetPanel.insert_FolderViewer_Simple(openFile, ref openFolder);
            openFolder(startFolder);
            return openFolder;
        }
        public static TreeView       insert_FolderViewer_Simple(this Control targetPanel, Action<string> openFile, ref Action<string> openFolder)
        {
            return targetPanel.insert_Left()
                                 .add_FolderViewer_Simple(openFile, ref openFolder, false);
        }    	
        public static TreeView       add_FolderViewer_Simple<T>(this T targetPanel, Action<string> openFile, ref Action<string> openFolderRef, bool addFileMenu)  		where T : Control    		
        {
            Action<TreeNode, string> mapFolder = 
                (treeNode, path)=>{	
                                    "mapping folder: {0}".info(path);
                                    treeNode.treeView().beginUpdate();													
                                    var folders = path.folders().sort();						
                                    foreach(var folder in folders)
                                        treeNode.add_Node(folder.fileName(), folder, folder.files().size() >0 || folder.folders().size()>0)
                                                .color(Color.Peru);
                                    var files = path.files();
                                    treeNode.add_Nodes(files, (file)=> file.fileName(), (file)=>file, (file)=>false,(file)=>Color.DarkBlue);
                                    treeNode.treeView().endUpdate();
                    };
            
            
            var treeView = targetPanel.add_TreeView();
            treeView.beforeExpand<string>((treeNode, path) => mapFolder(treeNode,path));	
            
            
            var textBox = treeView.insert_Above(20).add_TextBox().fill();
            var currentFolder = "";
            
            Action<string> openFolder = 
                (path)=>{	
                            if (path.notValid())
                                path = targetPanel.askUserForDirectory(textBox.get_Text());
                            if (path.isFile())
                                path = path.parentFolder();
                            currentFolder = path;
                            treeView.clear();
                            if (currentFolder.parentFolder().valid())
                                treeView.add_Node("..", "..");
                            if (path.dirExists())				
                                mapFolder(treeView.rootNode(), path);
                            else 
                                "Provided directory name doesn't exist: {0}".error(path);
                            textBox.set_Text(path);					
                        };
                        
            treeView.afterSelect<string>(
                (file)=>{		
                            if (file.fileExists())
                            {
                                var fileSize = file.fileInfo().size();
                                var mBytes = fileSize /  (1024 * 1024);
                                if (mBytes >  10)
                                    if ("Are you sure you want to open a file with {0} MBytes".format(mBytes).askUserQuestion().isFalse())
                                        return;
                                "opening file '{0}' with size {1}".info(file, mBytes);
                                openFile(file);
                                
                            }
                        });			
                
            treeView.onDoubleClick<string>(
                (file)=>{
                            if (file == "..")
                                openFolder(currentFolder.parentFolder());				
                        });
            textBox.onEnter(openFolder);			
            textBox.onDrop(openFolder);
            treeView.onDrop(openFolder);
            treeView.onDrag<string>();
            if (addFileMenu)
            {
                targetPanel.mainMenu().clear()
                       .add_Menu("File")
                       .add_MenuItem("open", ()=> openFolder(""));
            }
            openFolderRef = openFolder;
            
            return treeView;
        }    	        
        public static TextBox        add_Notepad(this Control topPanel)
        {
            Action<string> openFile = null;
            Func<string> currentFile = null;
            return topPanel.add_Notepad(ref openFile, ref currentFile, true);
        }    	
        public static TextBox        add_Notepad(this Control topPanel, ref Action<string> openFileRef, ref Func<string> currentFileRef, bool addMenus)
        {
            var textArea = topPanel.add_TextBox(true)
                                   .wordWrap(false)
                                   .noMaxLength();
            var currentFile = "";
            
            Action<string> openFile =
                (file)=>{ 		
                            "Opening File: {0}".info(file);
                            textArea.set_Text((currentFile = file).fileContents()
                                                                  .fix_CRLF());
                        };
            
            Action<string> saveFile =
                (file)=>{
                            if(file == "")
                                file = topPanel.askUserForFileToSave(currentFile.parentFolder());
                            if (file.notValid())
                                return;				
                            "Contents Saved to: {0}".info(file);
                            textArea.get_Text()
                                    .saveAs(currentFile = file);
                        };			
            
            if(addMenus)
            {
                topPanel.add_MainMenu()  
                        .add_Menu("File")
                            .add("New"		, ()=> openFile(""))
                            .add("Open"		, ()=> openFile(topPanel.askUserForFileToOpen(currentFile.parentFolder())))
                            .add("Save"		, ()=> saveFile(currentFile))
                            .add("Save As"	, ()=> saveFile(""))
                            
                        .add_Menu("Edit")
                            .add("Undo"		, ()=> textArea.Undo()	).add_Separator()
                            .add("Cut"		, ()=> textArea.Cut()	)
                            .add("Copy"		, ()=> textArea.Copy()	)
                            .add("Paste"	, ()=> textArea.Paste()	).add_Separator()
                            .add("TextArea Object Properties", 	 ()=> textArea.showInfo());
                "Welcome to O2's Notepad :)".debug();
            }
            textArea.onDrop(openFile);
            
            openFileRef = openFile;
            currentFileRef = ()=> currentFile;
            
            return textArea;
        }
        public static Panel          popupWith_Diff_ListWith<T, T1>(this List<T> listA, List<T1> listB)
        {
            var topPanel = "List compare".popupWindow();
            topPanel.add_GroupBox("List A: {0}".format(typeof(T))).add_TreeView().add_Nodes(listA).sort().parent()
                    .insert_Right("List B: {0}".format(typeof(T1))).add_TreeView().add_Nodes(listB).sort();
            return topPanel;
        }
    }
}
