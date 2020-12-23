using System;

namespace Sean.Utility.Format
{
    /// <summary>
    /// 对象转换
    /// </summary>
    public class ObjectConvert
    {
        /// <summary>
        /// 将object对象转换为实体对象（反射）
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="source">object对象</param>
        /// <returns></returns>
        public static T ConvertObject<T>(object source) where T : class, new()
        {
            //创建实体对象实例
            var result = Activator.CreateInstance<T>();
            return ConvertObject(source, result);
        }
        /// <summary>
        /// 将object对象转换为实体对象（反射）
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="source">object对象</param>
        /// <param name="outputObject">object对象</param>
        /// <returns></returns>
        public static T ConvertObject<T>(object source, T outputObject) where T : class
        {
            if (outputObject == null)
            {
                return null;
            }

            if (source != null)
            {
                Type type = source.GetType();
                var properties = typeof(T).GetProperties();
                //遍历实体对象属性
                foreach (var info in properties)
                {
                    var propertyInfo = type.GetProperty(info.Name);
                    if (propertyInfo == null || !propertyInfo.CanWrite)
                    {
                        continue;
                    }

                    //取得object对象中此属性的值
                    var val = propertyInfo.GetValue(source, null);
                    if (val != null)
                    {
                        object obj;
                        if (!info.PropertyType.IsGenericType)
                        {
                            //非泛型
                            obj = Convert.ChangeType(val, info.PropertyType);
                        }
                        else
                        {
                            //泛型Nullable<>
                            Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                obj = Convert.ChangeType(val, info.PropertyType);
                            }
                        }
                        info.SetValue(outputObject, obj, null);
                    }
                }
            }
            return outputObject;
        }
        /// <summary>
        /// 将object对象转换为实体对象（json序列化）
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="source">object对象</param>
        /// <returns></returns>
        public static T ConvertObjectByJson<T>(object source)
        {
            //将object对象转换为json字符串
            var json = JsonHelper.Serialize(source);
            //将json字符串转换为实体对象
            return JsonHelper.Deserialize<T>(json);
        }
    }
}
