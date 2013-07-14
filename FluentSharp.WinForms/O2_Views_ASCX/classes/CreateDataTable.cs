// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Data;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms.Utils
{  	
    public class CreateDataTable
    {
    	// create DataTable from a generic list (by using it's properties)
    	public static DataTable from_List<T>(List<T> data)
    	{
    		var dataTable = new DataTable();
            try
            {
                //foreach(var property in typeof(T).properties())
                foreach (var property in typeof (T).properties())
                    dataTable.Columns.Add(property.Name);
                foreach (var item in data)
                {
                    var rowContents = new List<object>();
                    foreach (var property in typeof (T).properties())
                        rowContents.Add(item.prop(property.Name));

                    dataTable.Rows.Add(rowContents.ToArray());
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in from_List: {0}", ex.Message);
            }
    	    removeEmptyColumns(dataTable);
    		return dataTable;
    	}
        
    	
    	// create DataTable from a generic list (by using it's fields)
    	public static DataTable from_List_using_Fields<T>(List<T> data)
    	{    		
    		var dataTable = new DataTable();    		
    		foreach(var field in typeof(T).fieldInfos())      		
    			dataTable.Columns.Add(field.Name);    		    		
    		foreach(var item in data)
    		{
    			var rowContents = new List<object>();
    			foreach(var field in typeof(T).fieldInfos())  
    				rowContents.Add(item.field(field.Name));    			
                dataTable.Rows.Add(rowContents.ToArray());
    		}    		
    		return dataTable;
    	}
    	
        public static DataTable fromDictionary_StringString(Dictionary<string, string> dictionary, string column1Title, string column2Title)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(column1Title);
            dataTable.Columns.Add(column2Title);
            if (dictionary != null)
                foreach (var item in dictionary)
                    dataTable.Rows.Add(new[] { item.Key, item.Value });
            return dataTable;
        }


        private static void removeEmptyColumns(DataTable dataTable)
        {           
            // calculate columnsWithData
            var columnsWithData = new List<DataColumn>();
            foreach(DataRow row in dataTable.Rows)                            
                foreach(DataColumn column in dataTable.Columns)                                    
                    {
                        var cellValue = row[column];
                        if (cellValue != null && cellValue.ToString() != "")
                        {
                            if (false == columnsWithData.Contains(column))
                                columnsWithData.Add(column);

                            if (cellValue.ToString() == "System.__ComObject")
                                row[column] = PublicDI.reflection.getComObjectTypeName(cellValue);
                            //break;
                        }
                    }
            PublicDI.log.info("There are {0} columns with data", columnsWithData.Count);
            // calculate the columnsToDelete
            var columnsToDelete =  new List<DataColumn>();
            foreach(DataColumn column in dataTable.Columns)
                if(false == columnsWithData.Contains(column))
                    columnsToDelete.Add(column);
            // delete them 
            foreach(DataColumn column in columnsToDelete)
                dataTable.Columns.Remove(column);                    
            
        }
    }
}
