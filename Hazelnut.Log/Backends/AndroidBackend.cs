using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

#if __ANDROID__
public class AndroidLogger : BaseLogBackend
{
    private static readonly Action<string, string>[] FastCaller =
    {
        LogDebug,
        LogInformation,
        LogWarning,
        LogError,
        LogFatal,
        LogNotice,
    };
    
    private readonly string _name;

    public AndroidLogger(ILoggerConfiguration config, Variables variables, string name)
        : base(config, variables)
    {
        _name = name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override void Write(LogLevel logLevel, string message)
    {
        FastCaller[(int)logLevel](_name, message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogDebug(string name, string message)
    {
        Android.Util.Log.Debug(name, message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogInformation(string name, string message)
    {
        Android.Util.Log.Info(name, message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogWarning(string name, string message)
    {
        Android.Util.Log.Warn(name, message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogError(string name, string message)
    {
        Android.Util.Log.Error(name, message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogFatal(string name, string message)
    {
        Android.Util.Log.Wtf(name, message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static void LogNotice(string name, string message)
    {
        Android.Util.Log.Verbose(name, message);
    }
}
#endif