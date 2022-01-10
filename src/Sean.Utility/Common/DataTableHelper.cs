using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Sean.Utility.Common
{
    public class DataTableHelper
    {
        public static DataTable ToDataTable<TEntity>(IEnumerable<TEntity> list) where TEntity : class
        {
            if (list == null)
            {
                return null;
            }

            var dt = CreateDataTable<TEntity>();
            var propertyInfos = typeof(TEntity).GetProperties();
            foreach (var model in list)
            {
                var dataRow = dt.NewRow();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (!dataRow.Table.Columns.Contains(propertyInfo.Name))
                    {
                        continue;
                    }

                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null) ?? DBNull.Value;
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateDataTable<TEntity>()
        {
            var dataTable = new DataTable(typeof(TEntity).Name);
            var properties = typeof(TEntity).GetProperties();
            foreach (var propertyInfo in properties)
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }
    }
}
