using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sean.Utility.Extensions;

namespace Sean.Utility
{
    public class DataTable<T> where T : class
    {
        public static DataTable Create(string tableName = null, Action<DataColumn> setColumn = null)
        {
            var dataTable = new DataTable(tableName ?? typeof(T).Name);
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                var propertyType = propertyInfo.PropertyType.IsNullableType() ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
                var column = dataTable.Columns.Add(propertyInfo.Name, propertyType ?? propertyInfo.PropertyType);
                setColumn?.Invoke(column);
            }
            return dataTable;
        }
        public static DataTable Create(T model, string tableName = null, Action<DataColumn> setColumn = null)
        {
            if (model == null)
            {
                return null;
            }

            var table = Create(tableName, setColumn);
            AddItem(table, model);
            return table;
        }
        public static DataTable Create(IEnumerable<T> list, string tableName = null, Action<DataColumn> setColumn = null)
        {
            if (list == null)
            {
                return null;
            }

            var table = Create(tableName, setColumn);
            AddItems(table, list);
            return table;
        }

        public static void AddItem(DataTable table, T item, bool autoCreateColumn = true)
        {
            if (table == null || item == null)
            {
                return;
            }

            var propertyInfos = typeof(T).GetProperties();
            var dataRow = table.NewRow();
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyValue = propertyInfo.GetValue(item, null);

                if (!dataRow.Table.Columns.Contains(propertyInfo.Name))
                {
                    if (!autoCreateColumn)
                    {
                        continue;
                    }

                    if (propertyValue != null)
                    {
                        dataRow.Table.Columns.Add(propertyInfo.Name, propertyValue.GetType());
                    }
                    else
                    {
                        dataRow.Table.Columns.Add(propertyInfo.Name);
                    }
                }

                dataRow[propertyInfo.Name] = propertyValue ?? DBNull.Value;
            }

            table.Rows.Add(dataRow);
        }

        public static void AddItems(DataTable table, IEnumerable<T> items, bool autoCreateColumn = true)
        {
            if (table == null || items == null || !items.Any())
            {
                return;
            }

            foreach (var item in items)
            {
                AddItem(table, item, autoCreateColumn);
            }
        }
    }
}
