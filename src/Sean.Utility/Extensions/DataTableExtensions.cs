using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sean.Utility.Extensions
{
    public static class DataTableExtensions
    {
        public static IList<IDictionary<string, object>> ToDictionary(this DataTable table)
        {
            var result = new List<IDictionary<string, object>>();
            if (table != null && table.Rows.Count > 0)
            {
                var listColumnName = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    listColumnName.Add(column.ColumnName);
                }

                foreach (DataRow row in table.Rows)
                {
                    var dic = new Dictionary<string, object>();
                    listColumnName.ForEach(c =>
                    {
                        var value = row[c];
                        if (row[c] == DBNull.Value)
                        {
                            value = null;
                        }
                        dic[c] = value;
                    });
                    result.Add(dic);
                }
            }
            return result;
        }

        public static DataTable ToDataTable(this IList<IDictionary<string, object>> dic)
        {
            var table = new DataTable();
            if (dic != null && dic.Count > 0)
            {
                dic.First().ForEach(c =>
                {
                    table.Columns.Add(c.Key);
                });
                dic.ForEach(c =>
                {
                    var row = table.NewRow();
                    c.ForEach(o =>
                    {
                        row[o.Key] = o.Value;
                    });
                    table.Rows.Add(row);
                });
            }
            return table;
        }
    }
}
