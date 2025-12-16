using System.Runtime.CompilerServices;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;
#if !NET7_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Hazelnut.Log.Backends;

public sealed class ConsoleLogger : BaseLogBackend
{
    private static readonly Action<string>[] FastCaller =
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
        if (OperatingSystem.IsWindows())
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
    
    public override void Write(LogLevel logLevel, string message)
    {
#if !(__ANDROID__ || __MACOS__ || __IOS__ || __TVOS__ || __MACCATALYST__)
        if (((ConsoleConfiguration)Configuration).UseColors)
        {
            var color = GetConsoleColor(logLevel);
            message = $"{color}{message}\x1b[0m";
        }
#endif
        FastCaller[(int)logLevel].Invoke(message);
    }
}