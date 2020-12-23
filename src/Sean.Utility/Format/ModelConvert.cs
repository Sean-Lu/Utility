using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Sean.Utility.Extensions;

namespace Sean.Utility.Format
{
    /// <summary>
    /// 实体转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelConvert
    {
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> modelList)
        {
            if (modelList == null)
            {
                return null;
            }

            var dt = CreateTable<T>();

            foreach (var model in modelList)
            {
                var dataRow = dt.NewRow();
                var propertyInfos = typeof(T).GetProperties();
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
        public static DataTable CreateTable<T>()
        {
            var dataTable = new DataTable(typeof(T).Name);
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        /// <summary>
        /// 模型转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="showOnHtml"> 需要在前端页面显示数据，处理规则：
        /// <para>1. key将统一转换为小写。</para>
        /// <para>2. value为了防止精度丢失：</para>
        /// <para>- long类型将转换为string类型</para>
        /// </param>
        /// <param name="dicFunc">处理value返回值的func委托</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary<T>(T model, bool showOnHtml = false, Dictionary<string, Func<object, object>> dicFunc = null) where T : new()
        {
            if (model == null)
            {
                return null;
            }

            var result = new Dictionary<string, object>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                var methodInfo = propertyInfo.GetGetMethod();
                if (methodInfo != null && methodInfo.IsPublic)
                {
                    var key = propertyInfo.Name;
                    var value = methodInfo.Invoke(model, new object[] { });

                    var isValueProcessed = false;// value值是否已经做过处理
                    if (dicFunc != null && dicFunc.Count(c => c.Key.ToLower() == key.ToLower()) > 0)
                    {
                        var func = dicFunc.FirstOrDefault(c => c.Key.ToLower() == key.ToLower()).Value;
                        if (func != null)
                        {
                            value = func(value);
                            isValueProcessed = true;
                        }
                    }

                    if (showOnHtml && !isValueProcessed)
                    {
                        key = key.ToLower();
                        if (methodInfo.ReturnType == typeof(long) || methodInfo.ReturnType == typeof(long?))
                        {
                            //对于Long类型的数据，如果我们在Controller层将结果序列化为json，直接传给前端的话，在Long长度大于16位时会出现精度丢失的问题。如何避免精度丢失呢？最常用的办法就是将Long类型字段统一转成String类型。
                            //831829640523022334（后端：原始数据）
                            //831829640523022300（前端：js不支持long类型的数据，丢失精度）
                            value = value?.ToString();
                        }
                    }
                    result.Add(key, value);
                }
            }
            return result;
        }
    }
}
