// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Controls
{
    public partial class ascx_O2InstallAndTempDirectories : UserControl
    {
        public ascx_O2InstallAndTempDirectories()
        {
            InitializeComponent();
        }

        private void btChangeTempDir_Click(object sender, EventArgs e)
        {
            PublicDI.config.O2TempDir = tbCurrentO2TempDir.Text;
            PublicDI.config.O2TempDir = tbCurrentO2TempDir.Text;
            directoryWithO2TempDir.setDirectory(PublicDI.config.O2TempDir);
        }

        private void btDeleteTempFolderContents_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes ==
                MessageBox.Show(
                    "Are you sure you want to delete the entire contents of the folder " + PublicDI.config.O2TempDir + " ?",
                    "Confirm O2 Temp Folder deletion (after deletion, an empty folder will be created)",
                    MessageBoxButtons.YesNo))
            {
                O2Thread.mtaThread(
                    () =>
                        {
                            Thread_Invoke_ExtensionMethods.invokeOnThread((Control)this, (Func<object>)(() => lbMessage_DeletingTempFolder.Visible = true));
                            Files.deleteFolder(PublicDI.config.O2TempDir,true);
                            Files.checkIfDirectoryExistsAndCreateIfNot(PublicDI.config.O2TempDir, true);
                            Thread_Invoke_ExtensionMethods.invokeOnThread((Control)this, (Func<object>)(() => lbMessage_O2TempFolderContentsDeleted.Visible = true));
                        }
                    );
            }
        }

        private void ascx_O2InstallAndTempDirectories_Load(object sender, EventArgs e)
        {
            lbMessage_DeletingTempFolder.Visible = false;
            lbMessage_O2TempFolderContentsDeleted.Visible = false;
            if (PublicDI.config.O2TempDir != PublicDI.config.O2TempDir)
                //PublicDI.log.showMessageBox("Something wrong with the config files since PublicDI.config.O2TempDir != PublicDI.config.O2TempDir");
                PublicDI.log.error("Something wrong with the config files since PublicDI.config.O2TempDir != PublicDI.config.O2TempDir");
            directoryWithO2TempDir.setDirectory(tbCurrentO2TempDir.Text = PublicDI.config.O2TempDir);
            directoryWithO2Install.setDirectory(tbCurrentO2InstallDirectory.Text = PublicDI.config.CurrentExecutableDirectory);            
        }
        
        

        
    }
}