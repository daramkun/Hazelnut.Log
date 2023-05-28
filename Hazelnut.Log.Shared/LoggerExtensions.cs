﻿using System.Runtime.CompilerServices;
using Cysharp.Text;

namespace Hazelnut.Log;

public static class LoggerExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1>(this ILogger logger, LogLevel logLevel, string message, T1 arg1)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7, T8>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this ILogger logger, LogLevel logLevel, string message, params object[] args)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.Write(logLevel, ZString.Format(message, args));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1>(this ILogger logger, LogLevel logLevel, string message, T1 arg1)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this ILogger logger, LogLevel logLevel, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteAsync(this ILogger logger, LogLevel logLevel, string message, params object[] args)
    {
        if (!logger.IsWritable(logLevel))
            return;
        logger.WriteAsync(logLevel, ZString.Format(message, args));
    }
}