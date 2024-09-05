using System;
using System.Threading.Tasks;

namespace Sean.Utility.Extensions;

/// <summary>
/// Extensions for <see cref="TaskFactory"/>
/// </summary>
public static class TaskFactoryExtensions
{
    /// <summary>
    /// Creates and starts a task with catch exception.
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="tryToDo"></param>
    /// <param name="handleError"></param>
    /// <param name="finalToDo"></param>
    /// <returns></returns>
    public static Task StartNewCatchException(this TaskFactory factory, Action tryToDo, Action<Exception> handleError = null, Action finalToDo = null)
    {
        if (tryToDo == null)
        {
            return default;
        }

        return factory.StartNew(() => ExceptionHelper.TryCatchFinal(tryToDo, ex =>
        {
            handleError?.Invoke(ex);
            return false;
        }, finalToDo));
    }
    /// <summary>
    /// Creates and starts a task with catch exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <param name="tryToDo"></param>
    /// <param name="handleError"></param>
    /// <param name="finalToDo"></param>
    /// <returns></returns>
    public static Task<T> StartNewCatchException<T>(this TaskFactory factory, Func<T> tryToDo, Func<Exception, T> handleError = null, Action finalToDo = null)
    {
        if (tryToDo == null)
        {
            return default;
        }

        return factory.StartNew(() => ExceptionHelper.TryCatchFinal(tryToDo, handleError, finalToDo, false));
    }
}