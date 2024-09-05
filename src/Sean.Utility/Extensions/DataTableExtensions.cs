using System.Collections.Generic;
using System.Data;

namespace Sean.Utility.Extensions
{
    public static class DataTableExtensions
    {
        public static List<Dictionary<string, object>> ToDictionary(this DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow dr in table.Rows)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (DataColumn dc in table.Columns)
                    {
                        dic[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    list.Add(dic);
                }
            }
            return list;
        }

        public static DataTable ToDataTable(this IEnumerable<IDictionary<string, object>> listDic)
        {
            if (listDic == null)
            {
                return null;
            }

            var table = new DataTable();
            DataTableHelper.AddRows(table, listDic);
            return table;
        }
    }
}
