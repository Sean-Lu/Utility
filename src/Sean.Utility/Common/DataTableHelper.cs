using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sean.Utility.Common
{
    public class DataTableHelper
    {
        public static DataTable Create<T>(string tableName = null, Action<DataColumn> setColumn = null) where T : class
        {
            return DataTable<T>.Create(tableName, setColumn);
        }
        public static DataTable Create<T>(T model, string tableName = null, Action<DataColumn> setColumn = null) where T : class
        {
            return DataTable<T>.Create(model, tableName, setColumn);
        }
        public static DataTable Create<T>(IEnumerable<T> list, string tableName = null, Action<DataColumn> setColumn = null) where T : class
        {
            return DataTable<T>.Create(list, tableName, setColumn);
        }

        public static void AddItem<T>(DataTable table, T item, bool autoCreateColumn = true) where T : class
        {
            if (table == null || item == null)
            {
                return;
            }

            if (item is IDictionary<string, object> dic)
            {
                AddItemFromDictionary(table, dic, autoCreateColumn);
                return;
            }

            DataTable<T>.AddItem(table, item, autoCreateColumn);
        }

        public static void AddItems<T>(DataTable table, IEnumerable<T> items, bool autoCreateColumn = true) where T : class
        {
            if (table == null || items == null || !items.Any())
            {
                return;
            }

            if (items is IEnumerable<IDictionary<string, object>> listDic)
            {
                AddItemsFromDictionary(table, listDic, autoCreateColumn);
                return;
            }

            DataTable<T>.AddItems(table, items, autoCreateColumn);
        }

        private static void AddItemFromDictionary(DataTable table, IDictionary<string, object> dic, bool autoCreateColumn = true)
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

        private static void AddItemsFromDictionary(DataTable table, IEnumerable<IDictionary<string, object>> listDic, bool autoCreateColumn = true)
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

                AddItemFromDictionary(table, dic, autoCreateColumn);
            }
        }
    }
}
