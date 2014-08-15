using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using FluentSharp.CoreLib;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class Data_ExtensionMethods
    {
        public static DataTable add_Column(this DataTable dataTable, string columnTitle)
        {
            return dataTable.add_Columns(columnTitle);
        }

        public static DataTable add_Columns(this DataTable dataTable, params string[] columnsTitle)
        {
            columnsTitle.forEach(title => dataTable.Columns.Add(title));
            return dataTable;
        }


        public static DataRow newRow(this DataTable dataTable, params object[] items)
        {
            var newRow = dataTable.NewRow();
            if (items != null)
                newRow.ItemArray = items;
            dataTable.Rows.Add(newRow);
            return newRow;
        }


        public static DataTable dataTable<T>(this IEnumerable<T> collection, params string[] columnsToShow)
        {
            if (columnsToShow.empty())
                return CreateDataTable.from_List(collection.toList());
            var dataTable = new DataTable();            
            if (typeof(T) == typeof(String))			// we need to handle string seperately since it is also an IEnumerable
            {
                dataTable.add_Column("String");
                collection.toList<string>().forEach(value => dataTable.newRow(value));
            }
            else
            {
                var columnsToShowList = columnsToShow.ToList();
                typeof(T).properties().forEach(
                    (propertyInfo) =>
                    {
                        if (columnsToShow == null || columnsToShow.size() == 0 || columnsToShowList.Contains(propertyInfo.Name))
                            dataTable.add_Column(propertyInfo.Name);
                    });

                foreach (var item in collection)
                {
                    var row        = dataTable.newRow();
                    var pinnedItem = item;
                    dataTable.Columns.toList<DataColumn>().forEach(
                        (dataColumn) =>{
                                            row[dataColumn.ColumnName] = pinnedItem.property(dataColumn.ColumnName);
                                        });
                }
            }
            return dataTable;
        }
    }
}
