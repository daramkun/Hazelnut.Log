using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

internal abstract partial class BaseLogBackend : ILogBackend
{
    public ILoggerConfiguration Configuration { get; }
    protected Variables Variables { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BaseLogBackend(ILoggerConfiguration config, Variables variables)
    {
        Configuration = config;
        Variables = variables;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ~BaseLogBackend()
    {
        Dispose(false);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void Dispose(bool disposing)
    {
        
    }
    
    public abstract void Write(LogLevel logLevel, string message);

    [return: NotNullIfNotNull(nameof(message))]
    private string? EscapeAnsiEscapeCode(string? message)
    {
        return message != null && !Configuration.KeepAnsiEscapeCode
            ? AnsiEscapeCodeRegex().Replace(message, string.Empty)
            : message;
    }

#if NETSTANDARD2_0_OR_GREATER
    private static readonly Regex InternalAnsiEscapeCodeRegex =
        new("\\\\e\\[([0-9]+)(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?m");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Regex AnsiEscapeCodeRegex() => InternalAnsiEscapeCodeRegex;
#else
    [GeneratedRegex(@"\\e\[([0-9]+)(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?m")]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static partial Regex AnsiEscapeCodeRegex();
#endif
}