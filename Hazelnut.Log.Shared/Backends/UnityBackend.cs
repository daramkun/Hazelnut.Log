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
    
    private static readonly Assembly? UnityEngineAssembly = null;

    private static readonly DebugLogMethod? DebugLogFunc = null;
    private static readonly DebugLogMethod? DebugLogWarningFunc = null;
    private static readonly DebugLogMethod? DebugLogErrorFunc = null;

    public static bool IsUnityEngine => UnityEngineAssembly != null;

    static UnityLogger()
    {        
        try
        {
            UnityEngineAssembly = Assembly.Load("UnityEngine");
        }
        catch
        {
            UnityEngineAssembly = null;
            return;
        }

        var parameterTypes = new[] { typeof(object) };

        var debugClass = UnityEngineAssembly.GetType("UnityEngine.Debug");
        DebugLogFunc = debugClass!.GetMethod("Log", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
        DebugLogWarningFunc = debugClass!.GetMethod("LogWarning", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
        DebugLogErrorFunc = debugClass!.GetMethod("LogError", parameterTypes)!.CreateDelegate(typeof(DebugLogMethod), null) as DebugLogMethod;
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
        DebugLogFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogInformation(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.InformationColor))
            message = $"<color={config.InformationColor}>{message}</color>";
        DebugLogFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogWarning(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.WarningColor))
            message = $"<color={config.WarningColor}>{message}</color>";
        DebugLogWarningFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogError(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.ErrorColor))
            message = $"<color={config.ErrorColor}>{message}</color>";
        DebugLogErrorFunc?.Invoke(message);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogFatal(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.FatalColor))
            message = $"<color={config.FatalColor}>{message}</color>";
        DebugLogErrorFunc?.Invoke(message);
    }
    
    private static void LogNotice(UnityConfiguration config, string message)
    {
        if (!string.IsNullOrEmpty(config.NoticeColor))
            message = $"<color={config.NoticeColor}>{message}</color>";
        DebugLogFunc?.Invoke(message);
    }
}
#endif