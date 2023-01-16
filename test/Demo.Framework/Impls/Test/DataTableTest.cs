using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Framework.Models;
using Sean.Utility.Common;
using Sean.Utility.Contracts;

namespace Demo.Framework.Impls.Test
{
    public class DataTableTest : ISimpleDo
    {
        public void Execute()
        {
            var model = new TestModel
            {
                Id = 1001,
                Age = 10,
                Name = "aaa",
                CreateTime = DateTime.Now
            };
            var table = DataTable<TestModel>.Create(model);
            DataTable<TestModel>.AddItem(table, new TestModel
            {
                Id = 1002,
                Age = 11,
                Name = "bbb",
                CreateTime = DateTime.Now
            });

            DataTableHelper.AddItem(table, new Dictionary<string, object>
            {
                {nameof(TestModel.Id),1010},
                {nameof(TestModel.Age),20},
                {nameof(TestModel.Name),"qqq"},
                {nameof(TestModel.CreateTime),DateTime.Now},
                {nameof(TestModel.UpdateTime),DateTime.Now},
                {"Test001",DateTime.Now},
            });
            DataTableHelper.AddItems(table, new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {nameof(TestModel.Id),1011},
                    {nameof(TestModel.Age),21},
                    {nameof(TestModel.Name),"www"},
                    {nameof(TestModel.CreateTime),DateTime.Now},
                }
            });
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
