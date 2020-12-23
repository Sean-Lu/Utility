using System;
using System.Collections.Generic;
using System.Linq;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Enumerable"/>
    /// </summary>
    public static class EnumerableTExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (action != null && items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }

            return items;
        }
    }
}
