using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine.Profiling;

public static class ArrayPoolExtension
{

    #region "Pool Extension"

    /// <summary>
    /// 重新Rent一个Array，同时初始化需要的部分空间
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pool"></param>
    /// <param name="current"></param>
    /// <param name="minimumLength"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] ReRent<T>(this ArrayPool<T> pool, T[] current, int minimumLength)
    {
        Profiler.BeginSample("Rerent");
        if (current != null)
        {
            pool.Return(current);
        }
        current = pool.Rent(minimumLength);
        Array.Clear(current, 0, minimumLength);
        Profiler.EndSample();
        return current;
    }

    /// <summary>
    /// 如果为空，则重新申请，否则保持不变
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pool"></param>
    /// <param name="current"></param>
    /// <param name="minimumLength"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] RentIfNull<T>(this ArrayPool<T> pool, T[] current, int minimumLength)
    {
        if (current != null)
        {
            return current;
        }
        current = pool.Rent(minimumLength);
        Array.Clear(current, 0, minimumLength);
        return current;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] SafeReturn<T>(this ArrayPool<T> pool, T[] current, int minimumLength)
    {
        if (current != null)
        {
            Array.Clear(current, 0, minimumLength);
            pool.Return(current);
        }
        return null;
    }

    #endregion
}
