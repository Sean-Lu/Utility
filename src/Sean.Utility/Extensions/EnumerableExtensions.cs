using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sean.Utility.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Paging Execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="func"></param>
        /// <returns>Returns whether the execution was successful.</returns>
        public static bool PagingExecute<T>(this IEnumerable<T> list, int pageSize, Func<int, IEnumerable<T>, bool> func)
        {
            var pageNumber = 1;

            if (list == null || !list.Any()) return false;

            if (list.Count() <= pageSize)
            {
                return func(pageNumber, list);
            }

            do
            {
                var datas = list.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                if (!datas.Any()) break;
                if (!func(pageNumber, datas)) return false;

                pageNumber++;
            } while (true);

            return true;
        }

#if !NET40
        /// <summary>
        /// Asynchronous paging execution
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageSize">Page size</param>
        /// <param name="func"></param>
        /// <returns>Returns whether the execution was successful.</returns>
        public static async Task<bool> PagingExecuteAsync<T>(this IEnumerable<T> list, int pageSize, Func<int, IEnumerable<T>, Task<bool>> func)
        {
            var pageNumber = 1;

            if (list == null || !list.Any()) return false;

            if (list.Count() <= pageSize)
            {
                return await func(pageNumber, list);
            }

            do
            {
                var datas = list.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                if (!datas.Any()) break;
                if (!await func(pageNumber, datas)) return false;

                pageNumber++;
            } while (true);

            return true;
        }
#endif
    }
}
