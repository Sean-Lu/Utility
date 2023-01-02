using System.Collections.Generic;
using System.Data;

namespace Sean.Utility.Extensions
{
    public static class DataSetExtensions
    {
        public static Dictionary<string, List<Dictionary<string, object>>> ToDictionary(this DataSet ds)
        {
            var result = new Dictionary<string, List<Dictionary<string, object>>>();
            if (ds != null && ds.Tables.Count > 0)
            {
                for (var i = 0; i < ds.Tables.Count; i++)
                {
                    var table = ds.Tables[i];
                    var tableName = !string.IsNullOrWhiteSpace(table.TableName) ? table.TableName : $"Table{i + 1}";
                    var dic = table.ToDictionary();
                    result.Add(tableName, dic);
                }
            }
            return result;
        }

        public static DataSet ToDataSet(this Dictionary<string, List<Dictionary<string, object>>> dic)
        {
            var ds = new DataSet();
            if (dic != null && dic.Count > 0)
            {
                dic.ForEach(c =>
                {
                    var table = c.Value.ToDataTable();
                    table.TableName = c.Key;
                    ds.Tables.Add(table);
                });
            }
            return ds;
        }
    }
}
