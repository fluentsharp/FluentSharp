// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.DotNetWrappers.SearchApi;
//O2File:SearchEngine.cs
//O2File:SearchUtils.cs

namespace O2.XRules.Database.Utils
{
    public class test_ascx_SimpleFileSearch : ContainerControl
    {       		
		public static void launchGui()
		{
			var simpleSearch = O2Gui.open<ascx_SimpleFileSearch>("Util - Simple File Search", 500,400);			
			var localScriptsFolder = PublicDI.config.LocalScriptsFolder;
			var filesToShow = localScriptsFolder.files(true,"*.cs","*.h2","*.o2");
			simpleSearch.loadFiles(localScriptsFolder, filesToShow); 
		}
	}

    public class ascx_SimpleFileSearch : ContainerControl
    {       		
		public SearchEngine searchEngine;
		
		public TextBox Path { get;set; }		
		public Panel leftPanel;
		public ascx_SourceCodeEditor sourceCode;
		public DataGridView dataGridView;
		public TreeView treeView;
		public TextBox textSearch_TextBox;
		

        public ascx_SimpleFileSearch()
    	{
    		this.Width = 300;
    		this.Height = 300;
    		buildGui();
    	}
 
    	public void buildGui()
    	{
    		var topPanel = this.add_Panel();
    		Path = topPanel.insert_Above<TextBox>(20);
			sourceCode = topPanel.add_SourceCodeEditor();
			dataGridView = sourceCode.insert_Above<Panel>(100).add_DataGridView();
			leftPanel = topPanel.insert_Left<Panel>(300);									
			
			Path.onEnter(loadFiles);
			Path.onDrop(
				(fileOrFolder)=>{
									Path.set_Text(fileOrFolder);
									loadFiles(fileOrFolder);
								}); 	   	   	   	   
			dataGridView.SelectionChanged+= 
				(sender,e) => {
						if (dataGridView.SelectedRows.size() == 1)
						{
							var selectedRow = dataGridView.SelectedRows[0]; 
							var filePath = selectedRow.Cells[0].Value.str();
							var filename = selectedRow.Cells[1].Value.str();
							var lineNumber = selectedRow.Cells[2].Value.str();
							"opening up source code: {0}".info(filePath);
							sourceCode.open(filePath.pathCombine(filename));  
							sourceCode.gotoLine(lineNumber.toInt() + 1);
							dataGridView.focus();
						}
				  };
								
		}
		
		public void loadFiles(string filesPath)
		{			
			if (filesPath.dirExists())
				loadFiles(filesPath, filesPath.files(true));
		}
		
		public void loadFiles(string filesPath, List<string> filesToLoad)
		{						
			sourceCode.set_Text("Loading files from: {0}".format(filesPath));
			Path.set_Text(filesPath);
			var filesContent = new Dictionary<string,string>();
			var nonBinaryFiles = new List<string>();
			foreach(var file in filesToLoad) 
				if (file.isBinaryFormat().isFalse()) 
					nonBinaryFiles.add(file);
			
			foreach(var file in nonBinaryFiles) 
					filesContent.add(file.remove(filesPath),file.contents());	
					
			searchEngine = new SearchEngine();
			searchEngine.loadFiles(nonBinaryFiles); 			
			
			//searchEngine.fixH2FileLoadingIssue();
			
			leftPanel.clear();
			treeView = leftPanel.add_TreeViewWithFilter(filesContent); 
			treeView.afterSelect<string>(
				(fileContents)=>{					
									sourceCode.open(filesPath + treeView.selected().get_Text());
								});						
			sourceCode.colorCodeForExtension(treeView.selected().str());
			sourceCode.set_Text("");
			textSearch_TextBox = leftPanel.controls<TextBox>(true)[1];
			textSearch_TextBox.onEnter(
				(text)=>{
					var searchResults = searchEngine.searchFor(text);
                    SearchUtils.loadInDataGridView_textSearchResults(searchResults, dataGridView);  
				});
			
			
		}		
	}
 
}
