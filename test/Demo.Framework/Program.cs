using System;
using System.Data;
using System.Dynamic;
using Demo.Framework.Impls;
using Newtonsoft.Json;
using Sean.Utility.Common;
using Sean.Utility.Extensions;
using Sean.Utility.Format;

namespace Demo.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 时间戳测试
            var timestamp = DateTimeHelper.GetTimestamp(DateTime.Now);
            Console.WriteLine($"当前时间戳：{timestamp}");
            var time = DateTimeHelper.GetDateTime(timestamp);
            //Console.WriteLine($"当前时间：{JsonConvert.SerializeObject(time)}");
            Console.WriteLine($"当前时间：{time.ToLongDateTimeWithTimezone()}");
            #endregion

            //JsonHelper.Serializer = NewJsonSerializer.Instance;

            #region json序列化测试
            //dynamic obj = new ExpandoObject();
            //obj.Id = 1001;
            //obj.Name = "Sean";
            //var json = JsonHelper.Serialize(obj);
            //var dynamicObj = JsonHelper.Deserialize<dynamic>(json);
            //var id = dynamicObj.Id;
            //var name = dynamicObj.Name;
            //Console.WriteLine(json);
            #endregion

            Console.WriteLine("--------------->Done.");
            Console.ReadLine();
        }

        private static DataSet GetTestDataSet()
        {
            var ds = new DataSet();
            var table = GetTestDataTable("TestTable1");
            ds.Tables.Add(table);
            var table2 = GetTestDataTable("TestTable2");
            ds.Tables.Add(table2);
            return ds;
        }
        private static DataTable GetTestDataTable(string tableName = null)
        {
            var table = new DataTable();
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                table.TableName = tableName;
            }

            table.Columns.Add("Id", typeof(long));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Age", typeof(int));

            var row = table.NewRow();
            row[0] = 1001;
            row[1] = "Sean";
            row[2] = 27;
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = 1002;
            row[1] = "qwe";
            row[2] = 30;
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = 1003;
            row[1] = null;
            row[2] = 32;
            table.Rows.Add(row);

            return table;
        }
    }
}
