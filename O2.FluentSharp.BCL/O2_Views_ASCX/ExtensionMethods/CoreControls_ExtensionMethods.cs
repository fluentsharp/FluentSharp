using System.Collections.Generic;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.Views.ASCX.CoreControls;
using O2.Views.ASCX.DataViewers;
using System;

namespace O2.Views.ASCX.ExtensionMethods
{
    public static class CoreControls_ExtensionMethods
    {
        #region ascx_Directory        

        public static ascx_Directory open(this ascx_Directory directory, string directoryToOpen)
        {
            directory.openDirectory(directoryToOpen);
            return directory;
        }

        public static ascx_Directory afterFileSelect(this ascx_Directory directory, Action<string> callback)
        {
            directory.getTreeView().afterSelect<string>(
                (file) =>
                {
                    if (file.fileExists())
                        callback(file);
                });
            return directory;
        }

        public static ascx_Directory add_Directory(this Control control)
        {
            return control.add_Directory(PublicDI.config.O2TempDir);
        }

        public static ascx_Directory add_Directory(this Control control, string startDirectory)
        {
            var directory = control.add_Control<ascx_Directory>();
            directory.simpleMode_withAddressBar();
            directory._WatchFolder = true;
            directory.openDirectory(startDirectory);
            return directory;
        }

        /*public static ascx_Directory add_Directory(this Control control, string startDirectory)
        {
            return (ascx_Directory)control.invokeOnThread(
                () =>
                {
                    var directory = new ascx_Directory();
                    directory.Dock = DockStyle.Fill;
                    //directory.simpleMode_withAddressBar();
                    //directory.simpleMode();
                    directory._ViewMode = ascx_Directory.ViewMode.Simple_With_LocationBar;
                    directory.openDirectory(startDirectory);
                    control.Controls.Add(directory);
                    return directory;
                });
        }*/

        public static ascx_Directory processDroppedObjects(this ascx_Directory directory, bool value)
        {
            directory.invokeOnThread(() => directory._ProcessDroppedObjects = value);
            return directory;
        }

        public static ascx_Directory handleDrop(this ascx_Directory directory, bool value)
        {
            directory.invokeOnThread(() => directory._HandleDrop = value);
            return directory;
        }	

        #endregion

        #region ascx_TableList

                
                
        



     

        /*public static ascx_TableList add_TableList<T>(this Control control, List<T> contents)
        {
            return control.add_TableList("", contents);
        }*/

        /*public static ascx_TableList add_TableList<T>(this Control control, string name, List<T> contents)
        {
            var tableList = control.add_TableList(name);
            var dataTable = CreateDataTable.from_List<T>(contents);
            tableList.setDataTable(dataTable);

            //var ascx_TableList = 
            //return ascx_TableList;
            return null;
        }*/

        

        #endregion
    }
}
