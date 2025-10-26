using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

#if __MACOS__ || __MACCATALYST__ || __IOS__ || __TVOS__
public sealed class AppleLogger : BaseLogBackend
{
    private readonly CoreFoundation.OSLog _logger;

    public AppleLogger(ILoggerConfiguration config, Variables variables, string name)
        : base (config, variables)
    {
        var appleConf = (AppleConfiguration)config;
        var bundleIdentifier = appleConf.CustomBundleIdentifier ?? NSBundle.MainBundle.BundleIdentifier;
        _logger = new CoreFoundation.OSLog(bundleIdentifier, name);
    }

    protected override void Dispose(bool disposing)
    {
        lock (_logger)
        {
            _logger.Dispose();
        }
    }

    public override void Write(LogLevel logLevel, string message)
    {
        lock (_logger)
        {
            _logger.Log(GetOSLogLevel(logLevel), message);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static CoreFoundation.OSLogLevel GetOSLogLevel(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Debug => CoreFoundation.OSLogLevel.Debug,
            LogLevel.Information => CoreFoundation.OSLogLevel.Info,
            LogLevel.Warning => CoreFoundation.OSLogLevel.Info,
            LogLevel.Error => CoreFoundation.OSLogLevel.Error,
            LogLevel.Fatal => CoreFoundation.OSLogLevel.Fault,
            LogLevel.Notice => CoreFoundation.OSLogLevel.Default,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
}
#endif