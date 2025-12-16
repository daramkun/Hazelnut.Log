using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public abstract partial class BaseLogBackend(ILoggerConfiguration config, Variables variables)
    : ILogBackend
{
    public ILoggerConfiguration Configuration { get; } = config;
    protected Variables Variables { get; } = variables;

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
}