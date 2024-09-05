using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sean.Utility.Extensions;

/// <summary>
/// Extensions for <see cref="IDictionary{TKey,TValue}"/>
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Add or update
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.ContainsKey(key))
        {
            dic[key] = value;
        }
        else
        {
            dic.Add(key, value);
        }
    }

    /// <summary>
    /// Add or update
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        dictionary.AddOrUpdate(key, value, (oldKey, oldValue) => value);
    }
}