using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_DataGridView
    {        
        public static DataGridView add_DataGridView(this Control control, params int[] position)
        {
            return control.invokeOnThread(() =>
                {
                    var dataGridView = control.add_Control<DataGridView>(position);
                    dataGridView.AllowUserToAddRows = false;
                    dataGridView.AllowUserToDeleteRows = false;
                    dataGridView.RowHeadersVisible = false;
                    dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    return dataGridView;
                });
        }
        public static DataGridView columnWidth(this DataGridView dataGridView, int id, int width)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    if (width > -1)
                        dataGridView.Columns[id].Width = width;
                    else
                        dataGridView.Columns[id].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    return dataGridView;
                });
        }
        public static int add_Column(this DataGridView dataGridView, string title)
        {
            return dataGridView.add_Column(title, -1);
        }
        public static int add_Column(this DataGridView dataGridView, string title, int width)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    int id = dataGridView.Columns.Add(title, title);
                    dataGridView.columnWidth(id, width);
                    return id;
                });
        }
        public static int add_Column_Link(this DataGridView dataGridView, string title)
        {
            return dataGridView.add_Column_Link(title, -1, false);
        }
        public static int add_Column_Link(this DataGridView dataGridView, string title, bool useColumnTextForLinkValue)
        {
            return dataGridView.add_Column_Link(title, -1, useColumnTextForLinkValue);
        }
        public static int add_Column_Link(this DataGridView dataGridView, string title, int width, bool useColumnTextForLinkValue)
        {
            return dataGridView.invokeOnThread(() =>
                {                    
					var links = new DataGridViewLinkColumn();
					links.TrackVisitedState = true;
					links.LinkColor = Color.Blue;
					links.LinkBehavior = LinkBehavior.SystemDefault;
					links.ActiveLinkColor = Color.White;
					links.DataPropertyName = title;
					links.HeaderText = title;

                    if (useColumnTextForLinkValue)
                    {
                        links.UseColumnTextForLinkValue = true;
                        links.Text = title;
                    }
                    //links.VisitedLinkColor = Color.Blue;	
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                    var id = dataGridView.Columns.Add(links);
                    dataGridView.columnWidth(id, width);
                    return id;
                });
        }
        public static DataGridView add_Columns(this DataGridView dataGridView, Type type)
        {
            type.properties().ForEach(property => dataGridView.add_Column(property.Name));
            return dataGridView;
        }
        public static DataGridView add_Columns(this DataGridView dataGridView, List<string> columns)
        {
            return dataGridView.add_Columns(columns.ToArray());
        }
        public static DataGridView add_Columns(this DataGridView dataGridView, params string[] columns)
        {
            columns.toList().forEach<string>(column => dataGridView.add_Column(column));
            return dataGridView;
        }
        public static int add_Row(this DataGridView dataGridView, params object[] cells)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    int id = dataGridView.Rows.Add(cells);                    
                    return id;
                });
        }
        public static void add_Rows<T>(this DataGridView dataGridView, List<T> collection)
        {
            collection.ForEach(
                item =>
                    {
                        var values = new List<object>();
                        foreach (var property in item.type().properties())
                            values.Add(item.prop(property.Name));
                        dataGridView.add_Row(values.ToArray());
                    });
        }
        public static void set_Value(this DataGridView dataGridView, int row, int column, object value)
        {
            dataGridView.invokeOnThread(
                () =>
                {
                    dataGridView.Rows[row].Cells[column].Value = value;
                });
        }    
        public static DataGridViewRow get_Row(this DataGridView dataGridView, int rowId)
        {
            return dataGridView.invokeOnThread(() => dataGridView.Rows[rowId]);
        }
        public static object value(this DataGridView dataGridView, int rowId, int columnId)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    try
                    {
                        var data = dataGridView.Rows[rowId].Cells[columnId].Value;
                        if (data != null)
                            return data;
                    }
                    catch (Exception ex)
                    {
                        PublicDI.log.ex(ex, "in DataGridView.value");
                    }
                    return "";			// default to returning "" if data is null
                });
        }
        public static DataGridView onClick(this DataGridView dataGridView, Action<int, int> cellClicked)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.CellContentClick += (sender, e) => cellClicked(e.RowIndex, e.ColumnIndex);
                    return dataGridView;
                });
        }
        public static DataGridView remove_Row(this DataGridView dataGridView, DataGridViewRow row)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Rows.Remove(row);
                    return dataGridView;
                });
        }
        public static DataGridView remove_Column(this DataGridView dataGridView, DataGridViewColumn column)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Columns.Remove(column);
                    return dataGridView;
                });
        }
        public static DataGridView remove_Rows(this DataGridView dataGridView)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Rows.Clear();
                    return dataGridView;
                });
        }
        public static DataGridView remove_Columns(this DataGridView dataGridView)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Columns.Clear();
                    return dataGridView;
                });
        }
        public static List<List<object>> rows(this DataGridView dataGridView)
        {
            return dataGridView.invokeOnThread(
                () => (from DataGridViewRow row in dataGridView.Rows 
                       select (from DataGridViewCell cell in row.Cells 
                               select cell.Value ?? "")
                               .ToList())
                               .ToList());
        }
        public static DataGridView noSelection(this DataGridView dataGridView)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.SelectionChanged += (sender, e) => dataGridView.ClearSelection();
                    return dataGridView;
                });
        }
        public static DataGridView column_backColor(this DataGridView dataGridView, int columnId, Color color)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Columns[columnId].DefaultCellStyle.BackColor = color;
                    return dataGridView;
                });
        }
        public static DataGridView showFileStrings(this DataGridView dataGridView, string file, bool ignoreCharZeroAfterValidChar, int minimumStringSize)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Columns.Clear();
                    dataGridView.add_Column("string");

                    foreach (var _string in file.contentsAsBytes().strings_From_Bytes(ignoreCharZeroAfterValidChar, minimumStringSize))
                        dataGridView.add_Row(_string);
                    return dataGridView;
                });
        }
        public static DataGridView showFileContents(this DataGridView dataGridView, string file, Func<byte, string> encoding)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    showFileContents(dataGridView, file, 16, encoding);
                    return dataGridView;
                });
        }
        public static DataGridView showFileContents(this DataGridView dataGridView, string file, int splitPoint, Func<byte, string> encoding)
        {
            return dataGridView.invokeOnThread(() =>
                {
                    dataGridView.Columns.Clear();
                    dataGridView.add_Column("");
                    for (byte b = 0; b < splitPoint; b++)
                        dataGridView.add_Column(b.hex());
                    dataGridView.column_backColor(0, Color.LightGray);
                    if (!file.fileExists())
                    {
                        PublicDI.log.error("provided file to load doesn't exists :{0}", file);
                        return dataGridView;
                    }
                    var bytes = file.contentsAsBytes();
                    var row = new List<string>();
                    var rowId = 0;
                    row.add(rowId.hex());
                    // ReSharper disable CoVariantArrayConversion
                    foreach (var value in bytes)
                    {
                        row.add(encoding(value));
                        if (row.Count == splitPoint)
                        {                            
                            dataGridView.add_Row(row.ToArray());                            
                            row.Clear();
                            row.add((rowId++).hex());
                        }
                    }                    
                    dataGridView.add_Row(row.ToArray());
                    // ReSharper restore CoVariantArrayConversion
                    //dataGridView.add_Row(row);
                    return dataGridView;
                });
        }
        public static DataGridView allowNewRows(this DataGridView dataGridView)
        {
            return dataGridView.allowNewRows(true);
        }
        public static DataGridView allowNewRows(this DataGridView dataGridView, bool value)
        {
            dataGridView.invokeOnThread(() => dataGridView.AllowUserToAddRows = value);
            return dataGridView;
        }
        public static DataGridView allowRowsDeletion(this DataGridView dataGridView)
        {
            return dataGridView.allowRowsDeletion(true);
        }
        public static DataGridView allowRowsDeletion(this DataGridView dataGridView, bool value)
        {
            dataGridView.invokeOnThread(() => dataGridView.AllowUserToDeleteRows = value);
            return dataGridView;
        }
        public static DataGridView allowColumnResize(this DataGridView dataGridView)
        {
            return dataGridView.allowColumnResize(true);
        }
        public static DataGridView allowColumnResize(this DataGridView dataGridView, bool value)
        {
            dataGridView.invokeOnThread(() => dataGridView.AllowUserToResizeColumns = value);
            return dataGridView;
        }
        public static DataGridView allowColumnOrder(this DataGridView dataGridView)
        {
            return dataGridView.allowColumnOrder(true);
        }
        public static DataGridView allowColumnOrder(this DataGridView dataGridView, bool value)
        {
            dataGridView.invokeOnThread(() => dataGridView.AllowUserToOrderColumns = value);
            return dataGridView;
        }	
		public static DataGridView dataSource(this DataGridView dataGridView, System.Data.DataTable dataTable)
		{
			dataGridView.invokeOnThread(
				()=>{
						dataGridView.DataSource = dataTable;
					});
			return dataGridView;
		}		
		public static DataGridView ignoreDataErrors(this DataGridView dataGridView)
		{
			return dataGridView.ignoreDataErrors(false);
		}		
		public static DataGridView ignoreDataErrors(this DataGridView dataGridView, bool showErrorInLog)
		{
			dataGridView.invokeOnThread(
				()=>{
						dataGridView.DataError+= 
								(sender,e) => { 
													if (showErrorInLog)
														" dataGridView error: {0}".error(e.Context);
											  };
					});
			return dataGridView;		
		}				
		public static DataGridView onDoubleClick<T>(this DataGridView dataGridView, Action<T> callback)
		{
			dataGridView.onDoubleClick(
				(dataGridViewRow)=>{
										if (dataGridViewRow.Tag.notNull() && dataGridViewRow.Tag is T)
											callback((T)dataGridViewRow.Tag);
								   });
			return dataGridView;
		}								
		public static DataGridView onDoubleClick(this DataGridView dataGridView, Action<DataGridViewRow> callback)
		{
			dataGridView.invokeOnThread(
				()=>{
						dataGridView.DoubleClick+= 
							(sender,e)=>{
											if (dataGridView.SelectedRows.size() == 1)
											{
												var selectedRow = dataGridView.SelectedRows[0];
												callback(selectedRow);
											}
										 };
					});
			return dataGridView;		
		}		
		public static DataGridView afterSelect<T>(this DataGridView dataGridView, Action<T> callback)
		{
			dataGridView.afterSelect(
				(dataGridViewRow)=>{
										if (dataGridViewRow.Tag.notNull() && dataGridViewRow.Tag is T)
											callback((T)dataGridViewRow.Tag);
								   });
			dataGridView.onDoubleClick(callback);					   
			return dataGridView;
		}		
		public static DataGridView afterSelect(this DataGridView dataGridView, Action<DataGridViewRow> callback)
		{
			dataGridView.invokeOnThread(
				()=>{
						dataGridView.SelectionChanged+= 
							(sender,e)=>{
											if (dataGridView.SelectedRows.size() == 1)
											{
												var selectedRow = dataGridView.SelectedRows[0];
												callback(selectedRow);
											}
										 };
					});
			return dataGridView;		
		}		
		public static DataGridView row_Height(this DataGridView dataGridView, int value)
		{
			dataGridView.invokeOnThread(()=>dataGridView.RowTemplate.Height = value);
			return dataGridView;
		}		
		public static List<string> values(this DataGridViewRow dataViewGridRow)
		{
			return dataViewGridRow.DataGridView.invokeOnThread(
				()=>{
						var values = new List<string>();
						foreach(var cell in dataViewGridRow.Cells)
							values.add(cell.property("Value").str());
						return values;
					});			
		}				
		public static DataGridView show(this DataGridView dataGridView, object _object)
		{
			dataGridView.Tag = _object;	
			dataGridView.remove_Columns();
			if(_object is IEnumerable)
			{
				var list = (_object as IEnumerable);
			    var enumerable = list as object[] ?? list.Cast<object>().ToArray();
			    var first = enumerable.first();  
				var names = first.type().properties().names(); 
				dataGridView.add_Columns(names);
				foreach(var item in enumerable)
				{
					var rowId = dataGridView.add_Row(item.propertyValues().ToArray()); 
					dataGridView.get_Row(rowId).Tag = item;										
				}
			}
			else
			{
				var names = _object.type().properties().names(); 
				dataGridView.add_Column("Property name",150); 
				dataGridView.add_Column("Property value");
				foreach(var name in names)
					dataGridView.add_Row(name, _object.prop(name));												
			}		
			return dataGridView;
		 }

    }
}
