using System.Runtime.CompilerServices;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;
#if NETSTANDARD2_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Hazelnut.Log.Backends;

internal sealed class ConsoleLogger : BaseLogBackend
{
    private static readonly Action<string>[] _fastCaller =
    {
        WriteLineToOut,         //< Debug
        WriteLineToOut,         //< Information
        WriteLineToOut,         //< Warning
        WriteLineToError,       //< Error
        WriteLineToError,       //< Fatal
        WriteLineToOut,         //< Notice
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteLineToError(string message) => Console.Error.WriteLine(message);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteLineToOut(string message) => Console.Out.WriteLine(message);

    public ConsoleLogger(ILoggerConfiguration config, Variables variables)
        : base(config, variables)
    {
#if NET7_0_OR_GREATER
        if (OperatingSystem.IsWindows())
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            NativeInterop.EnableConsoleToAnsiEscapeSequence();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string GetConsoleColor(LogLevel logLevel)
    {
        var config = (ConsoleConfiguration)Configuration;
        return logLevel switch
        {
            LogLevel.Debug => config.DebugDecorationString,
            LogLevel.Information => config.InformationDecorationString,
            LogLevel.Warning => config.WarningDecorationString,
            LogLevel.Error => config.ErrorDecorationString,
            LogLevel.Fatal => config.FatalDecorationString,
            LogLevel.Notice => config.NoticeDecorationString,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    protected override object? LockObject => null;
    
    protected override void InternalWrite(LogLevel logLevel, string message)
    {
#if !(__ANDROID__ || __MACOS__ || __IOS__ || __TVOS__ || __MACCATALYST__)
        if (((ConsoleConfiguration)Configuration).UseColors)
        {
            var color = GetConsoleColor(logLevel);
            message = $"{color}{message}\x1b[0m";
        }
#endif
        _fastCaller[(int)logLevel].Invoke(message);
    }
}