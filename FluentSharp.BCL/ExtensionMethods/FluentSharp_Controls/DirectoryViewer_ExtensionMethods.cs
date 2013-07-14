using System.Windows.Forms;
using FluentSharp.CoreLib;
using System;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class DirectoryViewer_ExtensionMethods
    {               

        public static DirectoryViewer open(this DirectoryViewer directory, string directoryToOpen)
        {
            directory.openDirectory(directoryToOpen);
            return directory;
        }

        public static DirectoryViewer afterFileSelect(this DirectoryViewer directory, Action<string> callback)
        {
            directory.getTreeView().afterSelect<string>(
                (file) =>
                {
                    if (file.fileExists())
                        callback(file);
                });
            return directory;
        }

        public static DirectoryViewer add_Directory(this Control control)
        {
            return control.add_Directory(PublicDI.config.O2TempDir);
        }

        public static DirectoryViewer add_Directory(this Control control, string startDirectory)
        {
            var directory = control.add_Control<DirectoryViewer>();
            directory.simpleMode_withAddressBar();
            directory._WatchFolder = true;
            directory.openDirectory(startDirectory);
            return directory;
        }

        /*public static DirectoryViewer add_Directory(this Control control, string startDirectory)
        {
            return (DirectoryViewer)control.invokeOnThread(
                () =>
                {
                    var directory = new DirectoryViewer();
                    directory.Dock = DockStyle.Fill;
                    //directory.simpleMode_withAddressBar();
                    //directory.simpleMode();
                    directory._ViewMode = DirectoryViewer.ViewMode.Simple_With_LocationBar;
                    directory.openDirectory(startDirectory);
                    control.Controls.Add(directory);
                    return directory;
                });
        }*/


        public static DirectoryViewer processDroppedObjects(this DirectoryViewer directory, bool value)
        {
            directory.invokeOnThread(() => directory._ProcessDroppedObjects = value);
            return directory;
        }

        public static DirectoryViewer handleDrop(this DirectoryViewer directory, bool value)
        {
            directory.invokeOnThread(() => directory._HandleDrop = value);
            return directory;
        }	        
    }
}
