using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sean.Utility.Extensions;

/// <summary>
/// Extensions for <see cref="Type"/>
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetDefaultValue(this Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    /// <summary>
    /// 是否是匿名类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsAnonymousType(this Type type)
    {
        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
               && type.IsGenericType && type.Name.Contains("AnonymousType")
               && (type.Name.StartsWith("<>"))
               && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
    }

    /// <summary>
    /// 是否是可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullableType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static string GetSimpleFullName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.FullName;
        }

        var genericTypeDefinition = type.GetGenericTypeDefinition();
        var genericArguments = type.GetGenericArguments();
        var listGenericArgumentFullName = new List<string>();
        foreach (var genericArgument in genericArguments)
        {
            listGenericArgumentFullName.Add(!genericArgument.IsGenericType
                ? genericArgument.FullName
                : genericArgument.GetSimpleFullName());
        }
        return $"{genericTypeDefinition.FullName}[{string.Join(", ", listGenericArgumentFullName)}]";
    }

    public static object CreateInstanceByConstructor(this Type type)
    {
        var emptyParamConstructor = type.GetConstructor(Type.EmptyTypes);
        if (emptyParamConstructor != null)
        {
            return Activator.CreateInstance(type);
        }

        var constructors = type.GetConstructors();
        var minParamConstructor = constructors.Length > 1
            ? constructors.OrderBy(c => c.GetParameters().Length).FirstOrDefault()
            : constructors.FirstOrDefault();
        if (minParamConstructor == null)
        {
            return default;
        }

        object[] parameterArgs = null;
        var parameters = minParamConstructor.GetParameters();
        if (parameters.Length > 0)
        {
            parameterArgs = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
#if NET40
                var paraDefaultValue = parameterInfo.DefaultValue;
                parameterArgs[i] = paraDefaultValue != DBNull.Value
                    ? parameterInfo.DefaultValue
                    : parameterInfo.ParameterType.GetDefaultValue();
#else
                parameterArgs[i] = parameterInfo.HasDefaultValue
                    ? parameterInfo.DefaultValue
                    : parameterInfo.ParameterType.GetDefaultValue();
#endif
            }
        }
        return Activator.CreateInstance(type, parameterArgs);
    }

    public static T CreateInstanceByConstructor<T>(this Type type) where T : class
    {
        return type.CreateInstanceByConstructor() as T;
    }
}