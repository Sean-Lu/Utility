using System;

namespace Sean.Utility;

public class ArrayHelper
{
    public static T[] Empty<T>()
    {
#if NET40 || NET45
        return new T[0];
#else
        return Array.Empty<T>();
#endif
    }
}