using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.LowLevel;

using QueueItem = System.ValueTuple<ConsoleColor, LogLevel, string>;

internal sealed class ConsoleLogger : BaseLowLevelLogger
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
    
    public ConsoleLogger(ILoggerConfiguration config, Variables variables) : base(config, variables) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ConsoleColor GetConsoleColor(LogLevel logLevel)
    {
        var config = (ConsoleConfiguration)Configuration;
        return logLevel switch
        {
            LogLevel.Debug => config.DebugColor,
            LogLevel.Information => config.InformationColor,
            LogLevel.Warning => config.WarningColor,
            LogLevel.Error => config.ErrorColor,
            LogLevel.Fatal => config.FatalColor,
            LogLevel.Notice => config.NoticeColor,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }

    protected override object? LockObject => null;
    
    protected override void InternalWrite(LogLevel logLevel, string message)
    {
#if !(__ANDROID__ || __MACOS__ || __IOS__ || __TVOS__ || __MACCATALYST__)
        Console.ForegroundColor = GetConsoleColor(logLevel);
#endif
        _fastCaller[(int)logLevel].Invoke(message);
    }
}