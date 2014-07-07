using System;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.REPL;
using FluentSharp.WinForms;

namespace FluentSharp.MsBuild
{
    public class Package_O2_Script_into_separate_Folder
    {
        public static Panel Main(bool startHidden = false, string targetScript = null)
        {
            //O2ConfigSettings.O2Version = "Package_O2_Script_v1";
            //O2Setup.extractEmbededConfigZips(); 

            //var topPanel = panel.clear().add_Panel(); 
            var topPanel = "Util - Compile H2, O2 or CS Script into separate Folder".popupWindow(800,300, startHidden)
													                .insert_LogViewer(); 
          
            var browser = topPanel.add_WebBrowser_Control() 
					                .add_NavigationBar();
            var dropZone = topPanel.insert_Left(200)
					                .add_Button("Drop script file here \n\n to package it into seperate folder").fill()
					                .font_bold();
	 	 		    
            var lastScriptFile = "";					  
            var compiledScript = ""; 
	
            Action<string> packageScript =  
	            (scriptFile)=>{		   
					            lastScriptFile = scriptFile;
					            dropZone.green(); 
					
					            O2Thread.mtaThread(   
						            ()=>{
								            var pathToAssemblies = "";
								            var projectFile = "";
								            if (scriptFile.package_Script(ref compiledScript, ref pathToAssemblies, ref projectFile).valid())
								            {
								 	            browser.open(pathToAssemblies);
									            dropZone.azure();
								            }
								            else
								            {
								 	            var logFile = projectFile + ".log";
									            "LogFile: {0}".error(logFile); 
									            browser.set_Text(logFile.fileContents().replace("".line(), "<br>"));
									            dropZone.pink();
								            }
							            });  
				            };  
            dropZone.parent().insert_Below(20).add_Link("Execute", ()=> compiledScript.startProcess())
								                .append_Link("Open Folder", ()=> compiledScript.directoryName().startProcess())
								                .append_Link("Edit Target", ()=> lastScriptFile.local().showInCodeEditor());
            //								  .append_Link("Clear Cache", ()=> { CompileEngine.CachedCompiledAssemblies.Clear(); "Compilation Cache Cleared".info();});
            dropZone.onDrop(packageScript);
            dropZone.onClick(()=>packageScript(lastScriptFile));

            dropZone.add_MenuItem_with_TestScripts(packageScript);

            if (targetScript.valid())
                packageScript(targetScript);
            //return packageScript;
            return topPanel;
        }
    }
}
