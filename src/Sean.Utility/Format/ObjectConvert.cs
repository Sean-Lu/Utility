using System;
using System.Collections.Generic;
using System.Linq;
using Sean.Utility.Extensions;

namespace Sean.Utility.Format
{
    /// <summary>
    /// 对象转换
    /// </summary>
    public class ObjectConvert
    {
        #region 对象映射
        /// <summary>
        /// 对象映射（属性和字段）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination, TSource>(TSource source) where TDestination : new()
        {
            var result = Activator.CreateInstance<TDestination>();
            Map(result, source);
            return result;
        }
        /// <summary>
        /// 对象映射（属性和字段）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void Map<TDestination, TSource>(TDestination destination, TSource source)
        {
            MapProperties<TDestination, TSource>(destination, source);
            MapFields<TDestination, TSource>(destination, source);
        }
        /// <summary>
        /// 对象映射（仅属性）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapProperties<TDestination, TSource>(TSource source) where TDestination : new()
        {
            var result = Activator.CreateInstance<TDestination>();
            MapProperties(result, source);
            return result;
        }
        /// <summary>
        /// 对象映射（仅属性）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void MapProperties<TDestination, TSource>(TDestination destination, TSource source)
        {
            if (source == null || destination == null)
            {
                return;
            }

            var typeSource = typeof(TSource);
            var typeDestination = typeof(TDestination);
            foreach (var destinationPropertyInfo in typeDestination.GetProperties())
            {
                var sourcePropertyInfo = typeSource.GetProperty(destinationPropertyInfo.Name);
                if (sourcePropertyInfo != null && destinationPropertyInfo.GetSetMethod() != null && sourcePropertyInfo.GetGetMethod() != null)
                {
                    destinationPropertyInfo.SetValue(destination, sourcePropertyInfo.GetValue(source, null), null);
                }
            }
        }
        /// <summary>
        /// 对象映射（仅字段）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapFields<TDestination, TSource>(TSource source) where TDestination : new()
        {
            var result = Activator.CreateInstance<TDestination>();
            MapFields(result, source);
            return result;
        }
        /// <summary>
        /// 对象映射（仅字段）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void MapFields<TDestination, TSource>(TDestination destination, TSource source)
        {
            if (source == null || destination == null)
            {
                return;
            }

            var typeSource = typeof(TSource);
            var typeDestination = typeof(TDestination);
            foreach (var destinationPropertyInfo in typeDestination.GetFields())
            {
                var sourcePropertyInfo = typeSource.GetField(destinationPropertyInfo.Name);
                if (sourcePropertyInfo != null)
                {
                    destinationPropertyInfo.SetValue(destination, sourcePropertyInfo.GetValue(source));
                }
            }
        }
        /// <summary>
        /// 对象映射（属性和字段）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapList<TDestination, TSource>(List<TSource> source) where TDestination : new()
        {
            if (source == null)
            {
                return null;
            }

            var result = new List<TDestination>();
            source.ForEach(c =>
            {
                var model = Activator.CreateInstance<TDestination>();
                Map(model, c);
                result.Add(model);
            });
            return result;
        }
        #endregion

        #region 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ChangeType<T>(object obj)
        {
            return (T)ChangeType(obj, typeof(T));
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object obj, Type type)
        {
            // √ 字符串类型 转 DateTime类型
            // √ 数值类型 转 bool类型
            // √ 数值类型 转 枚举类型

            if (obj == null || obj == DBNull.Value)
            {
                return type.GetDefaultValue();
            }

            var objType = obj.GetType();
            if (objType == type)
            {
                return obj;
            }

            if (type.IsEnum)
            {
                return Enum.ToObject(type, obj);
            }

            var changeType = Nullable.GetUnderlyingType(type) ?? type;
            if (Equals(obj, string.Empty) && changeType == typeof(DateTime))
            {
                return type.GetDefaultValue();
            }

            //var result = Convert.ChangeType(obj, type);// 可空类型会报错
            var result = Convert.ChangeType(obj, changeType);
            return result;
        }
        #endregion
    }
}
