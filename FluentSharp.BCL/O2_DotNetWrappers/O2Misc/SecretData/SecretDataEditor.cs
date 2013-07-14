using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Utils;

namespace FluentSharp.WinForms.Controls
{
    public class SecretDataEditor
    {    
    	public DirectoryViewer directory;
    	public DataGridView dataGridView;
		public string selectedFile;
		public ToolStripStatusLabel statusLabel;
		
		public string showGui()
		{
			var userDataDirectory = PublicDI.config.UserData.createDir();
			return showGui(userDataDirectory);
		}
		
        public string showGui(string userDataDirectory)
        {
            var panel = O2Gui.open<Panel>("Secret Data Files", 750, 200);
            
            var controls = panel.add_1x1("Folder with secret data files", "SecretData", true, 230);
            directory = controls[0].add_Directory(userDataDirectory);
            if (userDataDirectory.files().size() == 0)
                new SecretData().serialize(userDataDirectory.pathCombine("SecretData.xml"));

			directory.parent().insert_Below<Panel>(30)					 
					 .add_Link("Create new Secret's file", 5, 0, createNewSecretsFile)
					 .append_Link("Save Loaded File", saveLoadedSecretsFile);
			
	        dataGridView = controls[1].add_DataGridView();
            dataGridView.AllowUserToAddRows = true;
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.add_Columns(typeof(Credential));
            var contextMenu = dataGridView.add_ContextMenu();
            contextMenu.add_MenuItem("Save", saveLoadedSecretsFile);
            contextMenu.add_MenuItem("Create new File", createNewSecretsFile);

            directory.afterFileSelect(loadFile);                        
            statusLabel = panel.parentForm().add_StatusStrip();
            
            statusMessage("Select Secrets file to load from TreeView on the left");
            return "done";
        }
        
        public void statusMessage(string message)
        {        
        	statusLabel.set_Text(message);
        }
        
        public void createNewSecretsFile()
        {
        	var secretData = new SecretData();
        	var fileName = "What is the new file name".askUser();
        	if (fileName.valid())
        	{        	
	        	if (fileName.extension(".xml").isFalse())
	        		fileName+=".xml";
	            selectedFile = directory.getCurrentDirectory().pathCombine(fileName);            
	            secretData.serialize(selectedFile);
	            statusMessage("Created new File: {0}".format(selectedFile));
	        }   
        }
        
        public void saveLoadedSecretsFile()
        {
        	if (selectedFile.valid().isFalse())
        	{
        		statusMessage("Error: no file loaded");
        		return;
        	}
        	dataGridView.enabled(false);
        	var secretData = new SecretData();
            foreach (var row in dataGridView.rows())
                if ((row[0] as string).valid())
                    secretData.Credentials.createTypeAndAddToList(
					row[0],
					row[1],
					row[2],
					row[3],
					row[4]);
            secretData.serialize(selectedFile);
            dataGridView.enabled(true);
            statusMessage("Saved to file: {0}".format(selectedFile));
        }
		
		
		public void loadFile(string fileToLoad)
		{
			selectedFile = fileToLoad;
			statusMessage("Loaded from File: {0}".format(selectedFile));			  
            dataGridView.remove_Rows();
            try
            {            		
                var secretData = selectedFile.deserialize<SecretData>();
                dataGridView.add_Rows(secretData.Credentials);               
            }
            catch (Exception ex)
            {
            	ex.log("in loadFile");
            	statusMessage("Error loading select file: {0}".format(fileToLoad));
            }
		}               		
    }

    public static class SecretDataEditor_ExtensionMethods
    {
        public static string show_SecretDataEditor(this string userDataDirectory)
		{
			return new SecretDataEditor().showGui(userDataDirectory);
		}
    }
}