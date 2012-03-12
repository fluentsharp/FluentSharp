using System.Collections.Generic;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Kernel;
using O2.Views.ASCX.CoreControls;
using O2.Views.ASCX.DataViewers;

namespace O2.Views.ASCX.classes
{
    public static class Ascx_ExtensionMethods
    {


        #region ascx_Directory

        public static ascx_Directory add_Directory(this Control control)
        {
            return control.add_Directory(PublicDI.config.O2TempDir);
        }
        public static ascx_Directory add_Directory(this Control control, string startDirectory)
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
        }
        #endregion

        #region ascx_TableList

        public static ascx_TableList add_TableList(this Control control)
        {
            return control.add_TableList("");
        }
        public static ascx_TableList add_TableList(this Control control, string tableTitle)
        {
            return (ascx_TableList) control.invokeOnThread(
                                        () =>
                                            {
                                                var tableList = new ascx_TableList();
                                                tableList._Title = tableTitle;
                                                tableList.Dock = DockStyle.Fill;
                                                control.Controls.Add(tableList);
                                                return tableList;
                                            });
        }

        public static void add_Columns(this ascx_TableList tableList, List<string> columnNames)
        {
            tableList.invokeOnThread(
                () =>
                    {
                        ListView listView = tableList.getListViewControl();
                        listView.Columns.Clear();
                        listView.AllowColumnReorder = true;
                        foreach (var columnName in columnNames)
                            listView.Columns.Add(columnName);
                    });

        }

        public static void add_Row(this ascx_TableList tableList, List<string> rowData)
        {
            tableList.invokeOnThread(
                () =>
                    {
                        if (rowData.Count > 0)
                        {
                            var listView = tableList.getListViewControl();
                            var listViewItem = new ListViewItem();
                            listViewItem.Text = rowData[0]; // hack because SubItems starts adding on the 2nd Column :(
                            rowData.RemoveAt(0);
                            listViewItem.SubItems.AddRange(rowData.ToArray());
                            listView.Items.Add(listViewItem);

                        }
                    });
        }

        public static ascx_TableList add_TableList<T>(this Control control, List<T> contents)
        {
            return control.add_TableList("", contents);
        }

        public static ascx_TableList add_TableList<T>(this Control control, string name, List<T> contents)
        {
            var tableList = control.add_TableList(name);
            var dataTable = CreateDataTable.from_List<T>(contents);
            tableList.setDataTable(dataTable);

            //var ascx_TableList = 
            //return ascx_TableList;
            return null;
        }

        public static void add_Tab_IfListHasData<T>(this TabControl tabControl, string tabName, List<T> list)
        {
            if (list.Count > 0)
                tabControl.add_Tab(tabName).add_TableList(list);
        }

        #endregion
    }
}
