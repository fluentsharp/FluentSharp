using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using O2.Views.ASCX.DataViewers;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using System.Drawing;
using O2.Kernel;

namespace O2.Views.ASCX.ExtensionMethods
{
    public static class ascx_TableList_ExtensionMethods
    {
        #region add

        public static ascx_TableList add_TableList(this Control control)
        {
            return control.add_TableList("");
        }

        public static ascx_TableList add_TableList<T>(this Control control, IEnumerable<T> collection)
        {
            return control.add_TableList("", collection);
        }

        public static ascx_TableList add_TableList<T>(this Control control, string title, IEnumerable<T> collection)
        {
            return control.add_TableList(title).show<T>(collection);
        }

        public static ascx_TableList add_TableList(this Control control, string tableTitle)
        {
            return (ascx_TableList)control.invokeOnThread(
                                        () =>
                                        {
                                            var tableList = new ascx_TableList();
                                            tableList._Title = tableTitle;
                                            tableList.Dock = DockStyle.Fill;
                                            control.Controls.Add(tableList);
                                            return tableList;
                                        });
        }

        #endregion

        #region misc
        public static ascx_TableList title(this ascx_TableList tableList, string title)
        {
            tableList.invokeOnThread(() => tableList._Title = title);
            return tableList;

        }

        public static ascx_TableList show(this ascx_TableList tableList, object targetObject)
        {
            if (tableList.notNull() && targetObject.notNull())
            {
                tableList.clearTable();
                tableList.title("{0}".format(targetObject.typeFullName()));
                tableList.add_Columns("name", "value");
                foreach (var property in PublicDI.reflection.getProperties(targetObject))
                    tableList.add_Row(property.Name, targetObject.prop(property.Name).str());
                tableList.makeColumnWidthMatchCellWidth();
            }
            return tableList;
        }

        public static ascx_TableList show<T>(this ascx_TableList tableList, IEnumerable<T> collection, params string[] columnsToShow)
        {
            tableList.setDataTable(collection.dataTable(columnsToShow));
            return tableList;
        }

        public static ascx_TableList clearTable(this ascx_TableList tableList)
        {
            var listViewControl = tableList.getListViewControl();
            listViewControl.invokeOnThread(() => listViewControl.Clear());

            return tableList;
        }

        public static ListView listView(this ascx_TableList tableList)
        {
            return tableList.getListViewControl();
        }

        public static ListViewItem backColor(this ListViewItem listViewItem, Color color)
        {
            return (ListViewItem)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    listViewItem.BackColor = color;
                    return listViewItem;
                });
        }

        public static ListViewItem textColor(this ListViewItem listViewItem, Color color)
        {
            return (ListViewItem)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    listViewItem.ForeColor = color;
                    return listViewItem;
                });
        }

        public static ascx_TableList resizeToMatchContents(this ascx_TableList tableList)
        {
            return tableList.resize();
        }

        public static ascx_TableList resize(this ascx_TableList tableList)
        {
            return (ascx_TableList)tableList.invokeOnThread(
                () =>
                {
                    tableList.listView().AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    return tableList;
                });
        }

        public static void add_Tab_IfListHasData<T>(this TabControl tabControl, string tabName, List<T> list)
        {
            if (list.Count > 0)
                tabControl.add_Tab(tabName).add_TableList(list);
        }
        #endregion

        #region Column(s)

        public static ascx_TableList add_Columns(this ascx_TableList tableList, List<string> columnNames)
        {
            return (ascx_TableList)tableList.invokeOnThread(
                () =>
                {
                    ListView listView = tableList.getListViewControl();
                    listView.Columns.Clear();
                    listView.AllowColumnReorder = true;
                    foreach (var columnName in columnNames)
                        listView.Columns.Add(columnName);
                    return tableList;
                });

        }

        public static ascx_TableList add_Column(this ascx_TableList tableList, string columnName)
        {
            var listViewControl = tableList.getListViewControl();
            listViewControl.invokeOnThread(() => listViewControl.Columns.Add(columnName));

            return tableList;
        }

        public static ascx_TableList add_Columns(this ascx_TableList tableList, params string[] columnsName)
        {
            tableList.add_Columns(columnsName.toList());
            return tableList;
        }

        #endregion

        #region Row(s)

        public static ascx_TableList add_Row(this ascx_TableList tableList, List<string> rowData)
        {
            return (ascx_TableList)tableList.invokeOnThread(
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
                    return tableList;
                });
        }

        public static ascx_TableList add_Row(this ascx_TableList tableList, params string[] cellValues)
        {
            tableList.add_Row(cellValues.toList());
            return tableList;
        }

        public static ListViewItem row(this ascx_TableList tableList, int rowIndex)
        {
            var rows = tableList.rows();
            if (rowIndex < rows.size())
                return rows[rowIndex];
            return null;
        }

        public static List<ListViewItem> rows(this ascx_TableList tableList)
        {
            return tableList.items();
        }

        public static ListViewItem lastRow(this ascx_TableList tableList)
        {
            return (ListViewItem)tableList.invokeOnThread(
                () =>
                {
                    var listView = tableList.listView();
                    if (listView.Items.size() > 0)
                        return listView.Items[listView.Items.size() - 1];
                    return null;
                });
        }


        #endregion

        #region ListViewItem

        public static List<ListViewItem> items(this ascx_TableList tableList)
        {
            return (List<ListViewItem>)tableList.invokeOnThread(
                () =>
                {
                    var items = new List<ListViewItem>();
                    foreach (ListViewItem item in tableList.getListViewControl().Items)
                        items.Add(item);
                    return items;
                });
        }

        public static List<ListViewItem.ListViewSubItem> items(this ListViewItem listViewItem)
        {
            return (List<ListViewItem.ListViewSubItem>)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    var items = new List<ListViewItem.ListViewSubItem>();
                    foreach (ListViewItem.ListViewSubItem subItem in listViewItem.SubItems)
                        items.Add(subItem);
                    return items;
                });
        }

        #endregion

        #region values

        public static List<String> values(this ListViewItem listViewItem)
        {
            return (List<String>)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    return (from item in listViewItem.items()
                            select item.Text).toList();
                });
        }

        public static List<List<String>> values(this ascx_TableList tableList)
        {
            return (List<List<String>>)tableList.invokeOnThread(
                () =>
                {
                    var values = new List<List<String>>();
                    foreach (var row in tableList.rows())
                        values.Add(row.values());
                    return values;
                });
        }

        #endregion

        #region events
        public static ascx_TableList afterSelect(this ascx_TableList tableList, Action<List<ListViewItem>> callback)
        {
            tableList.getListViewControl().SelectedIndexChanged +=
                (sender, e) =>
                {
                    if (callback.notNull())
                    {
                        var selectedItems = new List<ListViewItem>();
                        foreach (ListViewItem item in tableList.getListViewControl().SelectedItems)
                            selectedItems.Add(item);
                        callback(selectedItems);
                    }
                };
            return tableList;
        }
        #endregion
    }
}
