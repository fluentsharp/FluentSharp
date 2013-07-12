using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using FluentSharp.BCL.Utils;
using FluentSharp.CoreLib;

namespace FluentSharp.BCL
{
    public static class Data_ExtensionMethods
    {
        public static DataTable add_Column(this DataTable dataTable, string columnTitle)
        {
            return dataTable.add_Columns(columnTitle);
        }

        public static DataTable add_Columns(this DataTable dataTable, params string[] columnsTitle)
        {
            columnsTitle.forEach<string>((title) => dataTable.Columns.Add(title));
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
                collection.toList().forEach<string>((item) => dataTable.newRow(item));
            }
            else
            {
                var columnsToShowList = columnsToShow.ToList();
                typeof(T).properties().forEach<PropertyInfo>(
                    (propertyInfo) =>
                    {
                        if (columnsToShow == null || columnsToShow.size() == 0 || columnsToShowList.Contains(propertyInfo.Name))
                            dataTable.add_Column(propertyInfo.Name);
                    });

                foreach (var item in collection)
                {
                    var row = dataTable.newRow();
                    dataTable.Columns.toList().forEach<DataColumn>(
                        (dataColumn) =>{
                                            row[dataColumn.ColumnName] = item.property(dataColumn.ColumnName);
                                        });
                }
            }
            return dataTable;
        }
    }
}
