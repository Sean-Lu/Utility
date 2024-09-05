using Sean.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sean.Utility;

public class DataTableHelper
{
    public static DataTable Create<T>(string tableName = null, Action<DataColumn> setColumn = null) where T : class
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
    public static DataTable Create<T>(T model, string tableName = null, Action<DataColumn> setColumn = null) where T : class
    {
        if (model == null)
        {
            return null;
        }

        var table = Create<T>(tableName, setColumn);
        AddRow(table, model);
        return table;
    }
    public static DataTable Create<T>(IEnumerable<T> list, string tableName = null, Action<DataColumn> setColumn = null) where T : class
    {
        if (list == null)
        {
            return null;
        }

        var table = Create<T>(tableName, setColumn);
        AddRows(table, list);
        return table;
    }

    public static void AddRow<T>(DataTable table, T item, bool autoCreateColumn = true) where T : class
    {
        if (table == null || item == null)
        {
            return;
        }

        if (item is IDictionary<string, object> dic)
        {
            AddRowFromDictionary(table, dic, autoCreateColumn);
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

    public static void AddRows<T>(DataTable table, IEnumerable<T> items, bool autoCreateColumn = true) where T : class
    {
        if (table == null || items == null || !items.Any())
        {
            return;
        }

        if (items is IEnumerable<IDictionary<string, object>> listDic)
        {
            AddRowsFromDictionary(table, listDic, autoCreateColumn);
            return;
        }

        foreach (var item in items)
        {
            AddRow(table, item, autoCreateColumn);
        }
    }

    private static void AddRowFromDictionary(DataTable table, IDictionary<string, object> dic, bool autoCreateColumn = true)
    {
        if (table == null || dic == null || !dic.Any())
        {
            return;
        }

        var dataRow = table.NewRow();
        foreach (var kv in dic)
        {
            if (!dataRow.Table.Columns.Contains(kv.Key))
            {
                if (!autoCreateColumn)
                {
                    continue;
                }

                if (kv.Value != null)
                {
                    dataRow.Table.Columns.Add(kv.Key, kv.Value.GetType());
                }
                else
                {
                    dataRow.Table.Columns.Add(kv.Key);
                }
            }

            dataRow[kv.Key] = kv.Value ?? DBNull.Value;
        }

        table.Rows.Add(dataRow);
    }

    private static void AddRowsFromDictionary(DataTable table, IEnumerable<IDictionary<string, object>> listDic, bool autoCreateColumn = true)
    {
        if (table == null || listDic == null || !listDic.Any())
        {
            return;
        }

        foreach (var dic in listDic)
        {
            if (dic == null || !dic.Any())
            {
                continue;
            }

            AddRowFromDictionary(table, dic, autoCreateColumn);
        }
    }
}