using System.Runtime.Versioning;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

#if __ANDROID__
internal class AndroidLogger : BaseLogBackend
{
    private static readonly Action<string, string>[] _fastCaller =
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

    protected override object? LockObject => null;

    protected override void InternalWrite(LogLevel logLevel, string message)
    {
        _fastCaller[(int)logLevel](_name, message);
    }

    private static void LogDebug(string name, string message)
    {
        Android.Util.Log.Debug(name, message);
    }
    
    private static void LogInformation(string name, string message)
    {
        Android.Util.Log.Info(name, message);
    }
    
    private static void LogWarning(string name, string message)
    {
        Android.Util.Log.Warn(name, message);
    }
    
    private static void LogError(string name, string message)
    {
        Android.Util.Log.Error(name, message);
    }
    
    private static void LogFatal(string name, string message)
    {
        Android.Util.Log.Wtf(name, message);
    }
    
    private static void LogNotice(string name, string message)
    {
        Android.Util.Log.Verbose(name, message);
    }
}
#endif