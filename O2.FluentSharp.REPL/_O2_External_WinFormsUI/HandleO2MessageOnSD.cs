// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Windows.Forms;
using O2.DotNetWrappers.O2Misc;
using O2.External.SharpDevelop.Ascx;
using O2.External.WinFormsUI.Forms;
using O2.External.WinFormsUI.O2Environment;
using O2.Interfaces.Messages;
using O2.Interfaces.Views;
using O2.Kernel.CodeUtils;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.External.SharpDevelop
{
    public class HandleO2MessageOnSD
    {
        public static bool autoTryToFixSourceCodeFileReferences = true;

        public static void o2MessageHelper_Handle_IM_FileOrFolderSelected(IO2Message o2Message)
        {
            o2MessageHelper_Handle_IM_FileOrFolderSelected(o2Message, null);
        }
        
        public static void o2MessageHelper_Handle_IM_FileOrFolderSelected(IO2Message o2Message, string parentControl)
        {
            // open file in ascx_SourceCodeEditor
            if (o2Message is IM_FileOrFolderSelected)
            {
                var fileOrFolderSelectedMessage = ((IM_FileOrFolderSelected) o2Message);
                //var fileSelected = .pathToFileOrFolder;
                fileOrFolderSelectedMessage.pathToFileOrFolder = tryToResolveFileLocation(fileOrFolderSelectedMessage.pathToFileOrFolder, O2AscxGUI.getGuiWithDockPanelAsControl());
                if (File.Exists(fileOrFolderSelectedMessage.pathToFileOrFolder))
                {
                    // var filename = Path.GetFileName(fileOrFolderSelectedMessage.pathToFileOrFolder);

                    var ascx_ScriptControl = getScriptEditor(fileOrFolderSelectedMessage.pathToFileOrFolder);
                    O2Messages.getAscx(ascx_ScriptControl,
                                       guiControl =>
                                           {
                                               if (guiControl != null && guiControl is ascx_SourceCodeEditor)
                                               {
                                                   var sourceCodeEditor = (ascx_SourceCodeEditor)guiControl;
                                                   switch (fileOrFolderSelectedMessage.messageText)
                                                   {
                                                       case "KM_Show_Selected_Text":
                                                           loadFileAndSelectText(sourceCodeEditor, fileOrFolderSelectedMessage);   
                                                           break;
                                                       default:
                                                           loadFileAndSelectLine(sourceCodeEditor, fileOrFolderSelectedMessage);   
                                                           break;
                                                   }

                                                   if (parentControl != null)
                                                       O2DockUtils.setDockContentState(parentControl,
                                                                                       O2DockState.DockLeft);
                                               }
                                           });
                }
            }
        }

        private static void loadFileAndSelectText(ascx_SourceCodeEditor sourceCodeEditor, IM_FileOrFolderSelected fileOrFolderSelectedMessage)
        {
            
            sourceCodeEditor.loadSourceCodeFile(fileOrFolderSelectedMessage.pathToFileOrFolder);
            sourceCodeEditor.setSelectedText(fileOrFolderSelectedMessage.lineNumber,
                                             fileOrFolderSelectedMessage.columnNumber,
                                             fileOrFolderSelectedMessage.showAsError);            
        }

        private static void loadFileAndSelectLine(ascx_SourceCodeEditor sourceCodeEditor, IM_FileOrFolderSelected fileOrFolderSelectedMessage)
        {
            sourceCodeEditor.loadSourceCodeFile(fileOrFolderSelectedMessage.pathToFileOrFolder);
            sourceCodeEditor.setSelectedLineNumber(fileOrFolderSelectedMessage.lineNumber);
        }

        private static string getScriptEditor(string filename)
        {
            var ascxSourceCodeEditor = getScriptEditorControlName(filename);
            if (false == O2AscxGUI.isAscxLoaded(ascxSourceCodeEditor))
                O2AscxGUI.openAscx(typeof(ascx_SourceCodeEditor), O2DockState.Document, ascxSourceCodeEditor);
            return ascxSourceCodeEditor;
        }

        public static void setSelectedLineNumber(string filename, int lineNumber)
        {
            if (false == String.IsNullOrEmpty(filename))
            {
                var scriptEditor = getScriptEditor(filename);
                O2AscxGUI.invokeOnAscxControl(scriptEditor, "setSelectedLineNumber", new object[] { filename, lineNumber });
            }
        }

        public static string getScriptEditorControlName(string fileName)
        {
            return "Script editor: " + fileName;
        }


        public static void setO2MessageFileEventListener()
        {
            setO2MessageFileEventListener(null);
        }

        public static void setO2MessageFileEventListener(string staticViewerControlName)
        {
            KO2MessageQueue.getO2KernelQueue().onMessages +=
                o2Message => o2MessageHelper_Handle_IM_FileOrFolderSelected(o2Message, staticViewerControlName); 
        }

        public static string tryToResolveFileLocation(string fileToMap, Control hostControl)
        {            
            if (autoTryToFixSourceCodeFileReferences && false == File.Exists(fileToMap) && false == Directory.Exists(fileToMap))
            {
                return SourceCodeMappingsUtils.mapFile(fileToMap, hostControl);
            }
            return fileToMap;
        }
    }
}
