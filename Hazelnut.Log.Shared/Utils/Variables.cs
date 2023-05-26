﻿using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Hazelnut.Log.Utils;

internal delegate string? DynamicVariableCallback(string? format, IFormatProvider? formatProvider);

internal class Variables
{
    private readonly ConcurrentDictionary<string, object?> _variables = new();

    public Variables(string name)
    {
        SetVariable("Logger", name);
        SetVariable("BaseDir", AppDomain.CurrentDomain.BaseDirectory);
        
        SetDynamicVariable("Date", (format, formatProvider) => DateTime.Now.ToString(format, formatProvider));
        SetDynamicVariable("UtcDate", (format, formatProvider) => DateTime.UtcNow.ToString(format, formatProvider));
        SetDynamicVariable("WorkingDir", (_, formatProvider) => Environment.CurrentDirectory.ToString(formatProvider));
        SetDynamicVariable("AppData", (_, formatProvider) => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(formatProvider));
        SetDynamicVariable("LocalAppData", (_, formatProvider) => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString(formatProvider));
        SetDynamicVariable("Documents", (_, formatProvider) => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(formatProvider));
        SetDynamicVariable("ThreadId", (format, formatProvider) => Environment.CurrentManagedThreadId.ToString(format, formatProvider));
        SetDynamicVariable("ThreadName", (_, formatProvider) => Thread.CurrentThread.Name?.ToString(formatProvider) ?? null);
        SetDynamicVariable("ProcessId", (format, formatProvider) => Process.GetCurrentProcess().Id.ToString(format, formatProvider));
        SetDynamicVariable("ProcessName", (_, formatProvider) => Process.GetCurrentProcess().ProcessName.ToString(formatProvider));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string? GetVariable(string name, string? format = null, IFormatProvider? formatProvider = null)
    {
        if (!_variables.TryGetValue(name, out var value))
            value = Environment.GetEnvironmentVariable(name);

        return value switch
        {
            null => null,
            string str => str.ToString(formatProvider),
            DynamicVariableCallback callback => callback(format, formatProvider),
            IFormattable formattable => formattable.ToString(format, formatProvider),
            _ => value.ToString()
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVariable(string name, object? value)
    {
        if (_variables.TryGetValue(name, out var originalValue))
            _variables.TryUpdate(name, value, originalValue);
        else
            _variables.TryAdd(name, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetDynamicVariable(string name, DynamicVariableCallback callback) => SetVariable(name, callback);
}