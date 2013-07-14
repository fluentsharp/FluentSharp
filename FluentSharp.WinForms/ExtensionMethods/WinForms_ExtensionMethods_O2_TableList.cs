using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Controls;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_O2_TableList
    {
        //add
        public static ctrl_TableList add_TableList(this Control control)
        {
            return control.add_TableList("");
        }
        public static ctrl_TableList add_TableList<T>(this Control control, IEnumerable<T> collection)
        {
            return control.add_TableList("", collection);
        }
        public static ctrl_TableList add_TableList<T>(this Control control, string title, IEnumerable<T> collection)
        {
            return control.add_TableList(title).show<T>(collection);
        }
        public static ctrl_TableList add_TableList(this Control control, string tableTitle)
        {
            return control.invokeOnThread(() => {
                                                    var tableList = new ctrl_TableList();
                                                    tableList._Title = tableTitle;
                                                    tableList.Dock = DockStyle.Fill;
                                                    control.Controls.Add(tableList);
                                                    return tableList;
                                                });
        }        

        //misc
        public static ctrl_TableList title(this ctrl_TableList tableList, string title)
        {
            tableList.invokeOnThread(() => tableList._Title = title);
            return tableList;

        }
        public static ctrl_TableList show(this ctrl_TableList tableList, object targetObject)
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
        public static ctrl_TableList show<T>(this ctrl_TableList tableList, IEnumerable<T> collection, params string[] columnsToShow)
        {
            tableList.setDataTable(collection.dataTable(columnsToShow));
            return tableList;
        }
        public static ctrl_TableList clearTable(this ctrl_TableList tableList)
        {
            var listViewControl = tableList.getListViewControl();
            listViewControl.invokeOnThread(() => listViewControl.Clear());

            return tableList;
        }
        public static ListView listView(this ctrl_TableList tableList)
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
        public static ctrl_TableList resizeToMatchContents(this ctrl_TableList tableList)
        {
            return tableList.resize();
        }
        public static ctrl_TableList resize(this ctrl_TableList tableList)
        {
            return (ctrl_TableList)tableList.invokeOnThread(
                () =>
                {
                    tableList.listView().AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    return tableList;
                });
        }
        public static ctrl_TableList add_Tab_IfListHasData<T>(this TabControl tabControl, string tabName, List<T> list)
        {
            if (list.Count > 0)
                return tabControl.add_Tab(tabName).add_TableList(list);
            return null;
        }        

        //Column(s)
        public static ctrl_TableList add_Columns(this ctrl_TableList tableList, List<string> columnNames)
        {
            return (ctrl_TableList)tableList.invokeOnThread(
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
        public static ctrl_TableList add_Column(this ctrl_TableList tableList, string columnName)
        {
            var listViewControl = tableList.getListViewControl();
            listViewControl.invokeOnThread(() => listViewControl.Columns.Add(columnName));

            return tableList;
        }
        public static ctrl_TableList add_Columns(this ctrl_TableList tableList, params string[] columnsName)
        {
            tableList.add_Columns(columnsName.toList());
            return tableList;
        }

        //Row(s)
        public static ctrl_TableList add_Row(this ctrl_TableList tableList, List<string> rowData)
        {
            return (ctrl_TableList)tableList.invokeOnThread(
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
        public static ctrl_TableList add_Row(this ctrl_TableList tableList, params string[] cellValues)
        {
            tableList.add_Row(cellValues.toList());
            return tableList;
        }
        public static ListViewItem row(this ctrl_TableList tableList, int rowIndex)
        {
            var rows = tableList.rows();
            if (rowIndex < rows.size())
                return rows[rowIndex];
            return null;
        }
        public static List<ListViewItem> rows(this ctrl_TableList tableList)
        {
            return tableList.items();
        }
        public static ListViewItem lastRow(this ctrl_TableList tableList)
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

        // ListViewItem
        public static List<ListViewItem> items(this ctrl_TableList tableList)
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

        //values
        public static List<string> values(this ListViewItem listViewItem)
        {
            return (List<string>)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    return (from item in listViewItem.items()
                            select item.Text).toList();
                });
        }
        public static List<List<string>> values(this ctrl_TableList tableList)
        {
            return (List<List<string>>)tableList.invokeOnThread(
                () =>
                {
                    var values = new List<List<string>>();
                    foreach (var row in tableList.rows())
                        values.Add(row.values());
                    return values;
                });
        }

        //events
        public static ctrl_TableList afterSelect(this ctrl_TableList tableList, Action<List<ListViewItem>> callback)
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


        //TO MAP to groups above

        public static ctrl_TableList setWidthToContent(this ctrl_TableList tableList)
		{
			tableList.makeColumnWidthMatchCellWidth();
			tableList.Resize+=(e,s)=> {	 tableList.makeColumnWidthMatchCellWidth();};
			tableList.getListViewControl().ColumnClick+=(e,s)=> { tableList.makeColumnWidthMatchCellWidth();};
			return tableList;
		}		
		public static ctrl_TableList show_In_ListView<T>(this IEnumerable<T> data)
		{
			return data.show_In_ListView("View data in List Viewer", 600,400);
		}		
		public static ctrl_TableList show_In_ListView<T>(this IEnumerable<T> data, string title, int width, int height)
		{
			return O2Gui.open<Panel>(title, width, height).add_TableList().show(data);
		}		
		public static ctrl_TableList columnsWidthToMatchControlSize(this ctrl_TableList tableList)
		{		
			tableList.parent().widthAdd(1);		// this trick forces it (need to find how to invoke it directly
			return tableList;
		}		
		public static ctrl_TableList onDoubleClick_get_Row(this ctrl_TableList tableList,  Action<ListViewItem> callback)
		{
			tableList.invokeOnThread(
				()=>{
						tableList.listView().DoubleClick+= (sender,e)
							=>{
									var selectedRow =tableList.selected();
									if (selectedRow.notNull())
										O2Thread.mtaThread(()=> callback(selectedRow));
							  };	
					});
			return tableList;					
		}		
		public static ctrl_TableList onDoubleClick<T>(this ctrl_TableList tableList,  Action<T> callback)
		{
			tableList.invokeOnThread(
				()=>{
						tableList.listView().DoubleClick+= (sender,e)
							=>{
									var selectedRows = tableList.listView().SelectedItems;
									if (selectedRows.size()==1)
									{
									 	var selectedRow = selectedRows[0]; 
									 	if (selectedRow.Tag.notNull() && selectedRow.Tag is T)
									 		O2Thread.mtaThread(()=> callback((T)selectedRow.Tag) );;
									}
								};
					});
			return tableList;
		}		
		public static ctrl_TableList onDoubleClick_ShowTagObject<T>(this ctrl_TableList tableList)
		{
			return tableList.onDoubleClick<T>((t)=> t.showInfo());
		}				
		public static ctrl_TableList afterSelect<T>(this ctrl_TableList tableList,  Action<T> callback)
		{
			tableList.afterSelect(
				(selectedRows)=>{			
									if (selectedRows.size()==1)
									{
									 	var selectedRow = selectedRows[0]; 
									 	if (selectedRow.Tag.notNull() && selectedRow.Tag is T)
									 		O2Thread.mtaThread(()=> callback((T)selectedRow.Tag));
									}
								});
			return tableList;
		}		
		public static ctrl_TableList afterSelects<T>(this ctrl_TableList tableList,  Action<List<T>> callback)			
		{
			tableList.afterSelect(
				(selectedRows)=>{	
									var tags = new List<T>();
									foreach(var selectedRow in selectedRows)
									{									 	
									 	if (selectedRow.Tag.notNull() && selectedRow.Tag is T)
									 		tags.add((T)selectedRow.Tag);
									}
									if (tags.size() > 0)
										O2Thread.mtaThread(()=> callback(tags));
								});
			return tableList;
		}				
		public static ctrl_TableList afterSelect_get_Cell(this ctrl_TableList tableList, int rowNumber, Action<string> callback)
		{
			tableList.afterSelect(
				(selectedRows)=>{			
						if (selectedRows.size()==1)
						{
						 	var selectedRow = selectedRows[0]; 
						 	var values = selectedRow.values();
						 	if (values.size() > rowNumber)
						 		O2Thread.mtaThread(()=> callback(values[rowNumber]));
						}
					});
			return tableList;
		}						
		public static ctrl_TableList afterSelect_set_Cell(this ctrl_TableList tableList, int rowNumber, TextBox textBox)
		{
			return tableList.afterSelect_get_Cell(rowNumber,(value)=> textBox.set_Text(value));			
		}		
		public static ctrl_TableList afterSelect_get_Row(this ctrl_TableList tableList, Action<ListViewItem> callback)
		{
			tableList.afterSelect(
				(selectedRows)=>{			
						if (selectedRows.size()==1)
						 	O2Thread.mtaThread(()=> callback(selectedRows[0]));
					});
			return tableList;
		}		
		public static ctrl_TableList afterSelect_get_RowIndex(this ctrl_TableList tableList, Action<int> callback)
		{
			tableList.afterSelect(
				(selectedRows)=>{			
						if (selectedRows.size()==1)
						 	O2Thread.mtaThread(()=> callback(selectedRows[0].Index));
					});
			return tableList;
		}		
		public static ctrl_TableList afterSelect<T>(this ctrl_TableList tableList, List<T> items, Action<T> callback)
		{
			tableList.afterSelect(
				(selectedRows)=>{			
						if (selectedRows.size()==1)
						{
							var index = selectedRows[0].Index;							
							if (index < items.size())
						 		O2Thread.mtaThread(()=> callback(items[index]));
						}
					});
			return tableList;
		}		
		public static ctrl_TableList selectFirst(this ctrl_TableList tableList)
		{
			return (ctrl_TableList)tableList.invokeOnThread(
				()=>{
						var listView = tableList.getListViewControl();
						listView.SelectedIndices.Clear();
						listView.SelectedIndices.Add(0);
						return tableList;
					});
		}		
		public static ListViewItem selected(this ctrl_TableList tableList)
		{
			return (ListViewItem)tableList.invokeOnThread(
				()=>{
						if (tableList.listView().SelectedItems.Count >0)
							return tableList.listView().SelectedItems[0];
						return null;
					});
		}		
		public static object tag(this ctrl_TableList tableList)
		{
			return (object)tableList.invokeOnThread(
				()=>{
						var selectedItem = tableList.selected();
						if (selectedItem.notNull())
							return selectedItem.Tag;							
						return null;
					});
		}
		public static T tag<T>(this ctrl_TableList tableList)
		{
			return tableList.selected<T>();
		}		
		public static T selected<T>(this ctrl_TableList tableList)
		{
			var tag = tableList.tag();
			if (tag.notNull() && tag is T)
				return (T)tag;
			return default(T);
		}				
		public static ctrl_TableList clearRows(this ctrl_TableList tableList)
		{
			return (ctrl_TableList)tableList.invokeOnThread(
				()=>{
						tableList.getListViewControl().Items.Clear();
						return tableList;
					});
		}						
		public static ctrl_TableList set_ColumnAutoSizeForLastColumn(this ctrl_TableList tableList)
		{
			return (ctrl_TableList)tableList.invokeOnThread(
				()=>{
						var listView = tableList.getListViewControl();
						if (listView.Columns.size()>0)
							listView.Columns[listView.Columns.size() -1].Width = -2;
						return tableList;
					});
		}
		public static ctrl_TableList column_Width(this ctrl_TableList tableList, int columnNumber, int width)
		{
			return tableList.set_ColumnWidth(columnNumber, width);
		}		
		public static ctrl_TableList set_ColumnWidth(this ctrl_TableList tableList, int columnNumber, int width)
		{
			return (ctrl_TableList)tableList.invokeOnThread(
				()=>{
						var listView = tableList.getListViewControl();
						//if (listView.Columns.size()>columnNumber)
							tableList.getListViewControl().Columns[columnNumber].Width = width;
						return tableList;
					});
		}		
		public static ctrl_TableList columns_Width(this ctrl_TableList tableList, params int[] widths)
		{
			return tableList.set_ColumnsWidth(widths)
							.set_ColumnsWidth(widths);    // BUG: there some extra event that gets fired on the fist column which reverts it to the original size
													  	// the current solution is to invoke set_ColumnWidth twice 	
		}
		public static ctrl_TableList set_ColumnsWidth(this ctrl_TableList tableList, params int[] widths)
		{
			return tableList.set_ColumnsWidth(true, widths);
		}		
		public static ctrl_TableList set_ColumnsWidth(this ctrl_TableList tableList,bool lastColumnShouldAutoSize,  params int[] widths)
		{
			return (ctrl_TableList)tableList.invokeOnThread(
				()=>{
						var listView = tableList.getListViewControl();
						for(var i = 0 ; i < widths.Length ; i++)
							listView.Columns[i].Width = widths[i];
						if (lastColumnShouldAutoSize)	
							tableList.set_ColumnAutoSizeForLastColumn();
						return tableList.set_ColumnAutoSizeForLastColumn();
					});
		}		
		//very similar to the code in add_Row (prob with that one is that it doens't return the new ListViewItem object
		public static ctrl_TableList add_Row<T>(this ctrl_TableList tableList, T _objectToAdd, Func<List<string>> getRowData)
		{		
			 return (ctrl_TableList)tableList.invokeOnThread(
                () =>{
                		var rowData = getRowData();
						if (rowData.Count > 0)
                    	{	                		
	                		var listView = tableList.getListViewControl();
	                        var listViewItem = new ListViewItem();
	                        listViewItem.Text = rowData[0]; // hack because SubItems starts adding on the 2nd Column :(
	                        rowData.RemoveAt(0);
	                        listViewItem.SubItems.AddRange(rowData.ToArray());	                        
	                        listView.Items.Add(listViewItem);                        
	                        listViewItem.Tag = _objectToAdd; // only difference with main add_Row
	                    }
                    	return tableList;
					});
		}		
		public static ListViewItem add_ListViewItem(this ctrl_TableList tableList, params string[] rowData)
		{
			return tableList.add_ListViewItem(rowData.toList());
		}		
		public static ListViewItem add_ListViewItem(this ctrl_TableList tableList, List<string> rowData)
        {
            return (ListViewItem)tableList.invokeOnThread(
                ()=>{
	                	var listView = tableList.getListViewControl();
	                    var listViewItem = new ListViewItem();
	                    if (rowData.Count > 0)
	                    {	                        
	                        listViewItem.Text = rowData[0]; // hack because SubItems starts adding on the 2nd Column :(
	                        rowData.RemoveAt(0);
	                        listViewItem.SubItems.AddRange(rowData.ToArray());
	                        listView.Items.Add(listViewItem);                        
	                    }
	                    return listViewItem;
	                });
        }

        //Sorting
        public static ctrl_TableList ascending(this ctrl_TableList tableList)
        {
            return tableList.sort(SortOrder.Ascending);
        }

        public static ctrl_TableList descending(this ctrl_TableList tableList)
        {
            return tableList.sort(SortOrder.Descending);
        }
        public static ctrl_TableList sort(this ctrl_TableList tableList)
        {
            return tableList.sort(SortOrder.Ascending);
        }

        public static ctrl_TableList sort(this ctrl_TableList tableList, SortOrder sortOrder)
        {
            return (ctrl_TableList)tableList.invokeOnThread(
                () =>
                {
                    tableList.listView().Sorting = sortOrder;
                    return tableList;
                });
        }
        
        //Colors        
        public static ListViewItem foreColor(this ListViewItem listViewItem, Color color)			
		{
			return (ListViewItem)listViewItem.ListView.invokeOnThread(
                () =>
                {
                    listViewItem.ForeColor = color;
                    return listViewItem;
                });
		}		
		public static ListViewItem foreColor(this ListViewItem listViewItem, bool selector, Color color_True, Color color_False )
		{
			return listViewItem.foreColor( (selector) 
												? color_True 
												: color_False);			
		}		
        public static ListViewItem white(this ListViewItem listViewItem)			
		{
			return listViewItem.foreColor(Color.White);
		}		
        public static ListViewItem orange(this ListViewItem listViewItem)			
		{
			return listViewItem.foreColor(Color.Orange);
		}
		public static ListViewItem red(this ListViewItem listViewItem)			
		{
			return listViewItem.foreColor(Color.Red);
		}				
		public static ListViewItem green(this ListViewItem listViewItem)			
		{
			return listViewItem.foreColor(Color.DarkGreen);
		}
		public static ListViewItem blue(this ListViewItem listViewItem)			
		{
			return listViewItem.foreColor(Color.Blue);
		}		
		public static ListViewItem white_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.White);
		}		
        public static ListViewItem pink_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.LightPink);
		}
		public static ListViewItem red_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.Red);
		}		
		public static ListViewItem azure_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.Azure	);
		}		
		public static ListViewItem green_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.LightGreen);
		}
		public static ListViewItem blue_bc(this ListViewItem listViewItem)			
		{
			return listViewItem.backColor(Color.LightBlue);
		}
    }
}
