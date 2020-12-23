using System.Collections.Concurrent;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for ConcurrentDictionary
    /// </summary>
    public static class ConcurrentDictionaryExtensions
    {
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
}
