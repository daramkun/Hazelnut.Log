using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

public abstract partial class BaseLogBackend : ILogBackend
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
}