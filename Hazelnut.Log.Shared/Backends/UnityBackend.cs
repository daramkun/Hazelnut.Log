using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

#if NETSTANDARD2_0_OR_GREATER
internal sealed class UnityLogger : BaseLogBackend
{
    private delegate void DebugLogMethod(object message);
    
    private static readonly Assembly? _unityEngineAssembly = null;

    private static readonly DebugLogMethod? _debugLogFunc = null;
    private static readonly DebugLogMethod? _debugLogWarningFunc = null;
    private static readonly DebugLogMethod? _debugLogErrorFunc = null;

    public static bool IsUnityEngine => _unityEngineAssembly != null;

    static UnityLogger()
    {        
        try
        {
            _unityEngineAssembly = Assembly.Load("UnityEngine");
        }
        catch
        {
            _unityEngineAssembly = null;
            return;
        }

        var parameterTypes = new[] { typeof(object) };

        var debugClass = _unityEngineAssembly.GetType("UnityEngine.Debug");
        _debugLogFunc = debugClass!.GetMethod("Log", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
        _debugLogWarningFunc = debugClass!.GetMethod("LogWarning", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
        _debugLogErrorFunc = debugClass!.GetMethod("LogError", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
    }
    
    private static readonly Action<UnityConfiguration, string>[] FastCaller = 
    {
        LogDebug,
        LogInformation,
        LogWarning,
        LogError,
        LogFatal,
        LogNotice,
    };

    public UnityLogger(ILoggerConfiguration config, Variables variables) : base (config, variables) { }

    public override void Write(LogLevel logLevel, string message)
    {
        var config = (UnityConfiguration)Configuration;
        FastCaller[(int)logLevel](config, message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogDebug(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.DebugColor))
            message = $"<color={config.DebugColor}>{message}</color>";
        _debugLogFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogInformation(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.InformationColor))
            message = $"<color={config.InformationColor}>{message}</color>";
        _debugLogFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogWarning(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.WarningColor))
            message = $"<color={config.WarningColor}>{message}</color>";
        _debugLogWarningFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogError(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.ErrorColor))
            message = $"<color={config.ErrorColor}>{message}</color>";
        _debugLogErrorFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogFatal(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.FatalColor))
            message = $"<color={config.FatalColor}>{message}</color>";
        _debugLogErrorFunc?.Invoke(message);
    }
    
    private static void LogNotice(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.NoticeColor))
            message = $"<color={config.NoticeColor}>{message}</color>";
        _debugLogFunc?.Invoke(message);
    }
}
#endif