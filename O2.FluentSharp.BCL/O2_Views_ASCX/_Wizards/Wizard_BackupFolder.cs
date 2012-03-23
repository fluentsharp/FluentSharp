// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using O2.Interfaces.O2Core;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Windows;
using O2.DotNetWrappers.Network;
using O2.Views.ASCX;
using O2.Views.ASCX.CoreControls;
using O2.Views.ASCX.classes;
using O2.Views.ASCX.MerlinWizard;
using O2.DotNetWrappers.Zip;
// extra references and the namespaces they import
//O2Ref:nunit.framework.dll
using O2.Views.ASCX.MerlinWizard.O2Wizard_ExtensionMethods;
using Merlin;
using MerlinStepLibrary;
using System.Threading;

namespace O2.Views.ASCX._Wizards
{	
    [O2Wizard]
	public class Wizard_BackupFolder
	{
		private static IO2Log log = PublicDI.log;

        [StartWizard]
        public Thread runWizard(string startFolder)
		{
			var targetFolder = Path.Combine(PublicDI.config.O2TempDir,"..\\_o2_Backups");
			Files.checkIfDirectoryExistsAndCreateIfNot(targetFolder);
            return runWizard(startFolder, targetFolder);
        }

        [StartWizard]
        public Thread runWizard(string startFolder, string targetFolder)
        {
            var o2Wizard = new O2Wizard("Backup folder: " + startFolder);
            
            //var steps = new List<IStep>();
            o2Wizard.Steps.add_Directory("Choose Directory To Backup", startFolder);
            o2Wizard.Steps.add_Directory("Choose Directory To Store Zip file", targetFolder);
            o2Wizard.Steps.add_Action("Confirm backup action", confirmBackupAction);
            o2Wizard.Steps.add_Action("Backing up files", executeTask);
            //steps.add_Message("All OK", "This is a message and all is OK");
            //steps.add_Message("Problem", "Something went wrong");

            //return steps.startWizard("Backup folder: " + startFolder);            
            o2Wizard.start();
            return null;
        }
		
		public void confirmBackupAction(IStep step)
		{			
			O2Thread.mtaThread(
			()=> {
				step.setText("");				
				var sourceDirectory = step.getPathFromStep(0);
				var targetDirectory = step.getPathFromStep(1);
				var targetFile = calculateTargetFileName(sourceDirectory, targetDirectory);
				step.append_Text("You are about to create a backup of the folder: {1}{1}\t{0} {1}{1} ", sourceDirectory, Environment.NewLine);
				step.append_Text("into the file: {1}{1}\t{0}{1}{1}", targetFile, Environment.NewLine);
				step.append_Text("Do you want to processed");
			});
		}
		
		
		public void executeTask(IStep step)
		{			
			O2Thread.mtaThread(
			()=> {
			
				var sourceDirectory = step.getPathFromStep(0);
				var targetDirectory = step.getPathFromStep(1);
				var targetFile = calculateTargetFileName(sourceDirectory, targetDirectory);
								
				step.allowNext(false);
				step.allowBack(false);						
				step.append_Line(" .... creating zip file ....");
				
				new zipUtils().zipFolder(sourceDirectory, targetFile);
				if (File.Exists(targetFile))
					step.append_Line("File Created: {0}", targetFile);	
				else
					step.append_Line("There was a problem creating the file: {0}", targetFile);	
				step.append_Line("all done");
				step.allowNext(true);
				step.allowBack(true);									
			});		
		}
		
		public string calculateTargetFileName(string sourceDirectory, string targetDirectory)
		{
			var filename = string.Format("O2 Backup for ({0}) done on ({1}).zip",sourceDirectory, DateTime.Now.ToString());
			filename  = Files.getSafeFileNameString(filename);
			return Path.Combine(targetDirectory, filename + ".zip");
		}						
	}		
}
