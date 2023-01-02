using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sean.Utility.Extensions
{
    public static class DataTableExtensions
    {
        public static List<Dictionary<string, object>> ToDictionary(this DataTable table)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    foreach (DataColumn dc in table.Columns)
                    {
                        dic[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    list.Add(dic);
                }
            }
            return list;
        }

        public static DataTable ToDataTable(this List<Dictionary<string, object>> dic)
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
