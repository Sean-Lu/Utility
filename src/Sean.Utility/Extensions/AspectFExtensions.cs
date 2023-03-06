using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sean.Utility.AOP;
using Sean.Utility.Contracts;
using Sean.Utility.Enums;

namespace Sean.Utility.Extensions
{
    public static class AspectFExtensions
    {
        [DebuggerStepThrough]
        public static AspectF Delay(this AspectF aspect, int milliseconds)
        {
            return aspect.Combine((work) =>
            {
                System.Threading.Thread.Sleep(milliseconds);
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustNotNull(this AspectF aspect, params object[] args)
        {
            return aspect.Combine((work) =>
            {
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == null)
                        {
                            throw new ArgumentNullException(nameof(args), $"Parameter at index {i} is null.");
                        }
                    }
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Until(this AspectF aspect, Func<bool> test, int? testDelayMilliseconds = null)
        {
            return aspect.Combine((work) =>
            {
                while (!test())
                {
                    if (testDelayMilliseconds.HasValue)
                    {
                        System.Threading.Thread.Sleep(testDelayMilliseconds.Value);
                    }
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF While(this AspectF aspect, Func<bool> test, int? testDelayMilliseconds = null)
        {
            return aspect.Combine((work) =>
            {
                while (test())
                {
                    work();

                    if (testDelayMilliseconds.HasValue)
                    {
                        System.Threading.Thread.Sleep(testDelayMilliseconds.Value);
                    }
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF WhenTrue(this AspectF aspect, params Func<bool>[] conditions)
        {
            return aspect.Combine((work) =>
            {
                foreach (Func<bool> condition in conditions)
                    if (!condition())
                        return;

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect, Action completeCallback)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncResult =>
            {
                work.EndInvoke(asyncResult);
                completeCallback();
            }, null));
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncResult =>
            {
                work.EndInvoke(asyncResult);
            }, null));
        }

        #region Retry
        [DebuggerStepThrough]
        public static AspectF RetryWhenError(this AspectF aspect, int retryDuration, int retryCount, Action<int, Exception> errorHandler)
        {
            var totalRetryCount = retryCount;
            return aspect.Combine((work) =>
            {
                do
                {
                    try
                    {
                        work();
                        return;
                    }
                    catch (Exception ex)
                    {
                        errorHandler?.Invoke(totalRetryCount - retryCount, ex);
                        System.Threading.Thread.Sleep(retryDuration);
                    }
                } while (retryCount-- > 0);
            });
        }
        #endregion

        #region Logger
        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, Action<string> before, Action<string> after, string beforeMessage, string afterMessage)
        {
            return aspect.Combine((work) =>
            {
                if (!string.IsNullOrEmpty(beforeMessage))
                {
                    before?.Invoke(beforeMessage);
                }

                work();

                if (!string.IsNullOrEmpty(afterMessage))
                {
                    after?.Invoke(afterMessage);
                }
            });
        }
        [DebuggerStepThrough]
        public static AspectF LogBegin(this AspectF aspect, Action<string> action, string message)
        {
            return aspect.Log(action, null, message, null);
        }
        [DebuggerStepThrough]
        public static AspectF LogEnd(this AspectF aspect, Action<string> action, string message)
        {
            return aspect.Log(null, action, null, message);
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, ILogger logger, LogLevel logLevel, string beforeMessage, string afterMessage)
        {
            return aspect.Log(msg => Log(logger, logLevel, msg), msg => Log(logger, logLevel, msg), beforeMessage, afterMessage);
        }
        [DebuggerStepThrough]
        public static AspectF LogBegin(this AspectF aspect, ILogger logger, LogLevel logLevel, string message)
        {
            return aspect.Log(logger, logLevel, message, null);
        }
        [DebuggerStepThrough]
        public static AspectF LogEnd(this AspectF aspect, ILogger logger, LogLevel logLevel, string message)
        {
            return aspect.Log(logger, logLevel, null, message);
        }

        [DebuggerStepThrough]
        public static AspectF LogDebug(this AspectF aspect, ILogger logger, string beforeMessage, string afterMessage)
        {
            return aspect.Log(logger, LogLevel.Debug, beforeMessage, afterMessage);
        }
        [DebuggerStepThrough]
        public static AspectF LogInfo(this AspectF aspect, ILogger logger, string beforeMessage, string afterMessage)
        {
            return aspect.Log(logger, LogLevel.Info, beforeMessage, afterMessage);
        }

        [DebuggerStepThrough]
        public static AspectF LogError(this AspectF aspect, ILogger logger, string errMsg = null, bool throwException = true)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception ex)
                {
                    logger?.LogError(errMsg ?? ex.Message, ex);
                    if (throwException)
                    {
                        throw;
                    }
                }
            });
        }
        [DebuggerStepThrough]
        public static AspectF IgnoreError(this AspectF aspect)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception)
                {
                    // ignored
                }
            });
        }
        #endregion

        #region Cache
        [DebuggerStepThrough]
        public static AspectF Cache<TReturnType>(this AspectF aspect, ICache cacheResolver, string key)
        {
            return aspect.Combine((work) =>
            {
                Cache<TReturnType>(aspect, cacheResolver, key, work, cached => cached);
            });
        }

        [DebuggerStepThrough]
        public static AspectF CacheList<TItemType, TListType>(this AspectF aspect, ICache cacheResolver, string listCacheKey, Func<TItemType, string> getItemKey)
            where TListType : IList<TItemType>, new()
        {
            return aspect.Combine((work) =>
            {
                Func<TListType> workDelegate = aspect.WorkDelegate as Func<TListType>;

                // Replace the actual work delegate with a new delegate so that
                // when the actual work delegate returns a collection, each item
                // in the collection is stored in cache individually.
                Func<TListType> newWorkDelegate = () =>
                {
                    TListType collection = workDelegate();
                    foreach (TItemType item in collection)
                    {
                        string key = getItemKey(item);
                        cacheResolver.Set(key, item);
                    }
                    return collection;
                };
                aspect.WorkDelegate = newWorkDelegate;

                // Get the collection from cache or real source. If collection is returned
                // from cache, resolve each item in the collection from cache
                Cache<TListType>(aspect, cacheResolver, listCacheKey, work,
                    cached =>
                    {
                        // Get each item from cache. If any of the item is not in cache
                        // then discard the whole collection from cache and reload the 
                        // collection from source.
                        TListType itemList = new TListType();
                        foreach (TItemType item in cached)
                        {
                            object cachedItem = cacheResolver.Get(getItemKey(item));
                            if (null != cachedItem)
                            {
                                itemList.Add((TItemType)cachedItem);
                            }
                            else
                            {
                                // One of the item is missing from cache. So, discard the 
                                // cached list.
                                return default;
                            }
                        }

                        return itemList;
                    });
            });
        }
        #endregion

        #region Private method
        private static void Cache<TReturnType>(AspectF aspect, ICache cacheResolver, string key, Action work, Func<TReturnType, TReturnType> foundInCache)
        {
            object cachedData = cacheResolver.Get(key);
            if (cachedData == null)
            {
                GetListFromSource<TReturnType>(aspect, cacheResolver, key);
            }
            else
            {
                // Give caller a chance to shape the cached item before it is returned
                TReturnType cachedType = foundInCache((TReturnType)cachedData);
                if (cachedType == null)
                {
                    GetListFromSource<TReturnType>(aspect, cacheResolver, key);
                }
                else
                {
                    aspect.WorkDelegate = new Func<TReturnType>(() => cachedType);
                }
            }

            work();
        }

        private static void GetListFromSource<TReturnType>(AspectF aspect, ICache cacheResolver, string key)
        {
            Func<TReturnType> workDelegate = aspect.WorkDelegate as Func<TReturnType>;
            TReturnType realObject = workDelegate();
            cacheResolver.Add(key, realObject);
            workDelegate = () => realObject;
            aspect.WorkDelegate = workDelegate;
        }

        private static void Log(ILogger logger, LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    logger?.LogDebug(message);
                    break;
                case LogLevel.Info:
                    logger?.LogInfo(message);
                    break;
                case LogLevel.Warn:
                    logger?.LogWarn(message);
                    break;
                case LogLevel.Error:
                    logger?.LogError(message);
                    break;
                case LogLevel.Fatal:
                    logger?.LogFatal(message);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}