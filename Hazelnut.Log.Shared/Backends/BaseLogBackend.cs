using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

#if NET7_0_OR_GREATER
using System.Threading.Channels;
#else
using System.Collections.Concurrent;
#endif

namespace Hazelnut.Log.Backends;

internal abstract partial class BaseLogBackend : ILogBackend
{
    private static readonly Thread _asyncThread;
#if NET7_0_OR_GREATER
    private static readonly Channel<(BaseLogBackend, LogLevel, string)> _asyncQueue =
        Channel.CreateUnbounded<(BaseLogBackend, LogLevel, string)>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });
#else
    private static readonly ConcurrentQueue<(BaseLogBackend, LogLevel, string)> _asyncQueue = new();
#endif

    static BaseLogBackend()
    {
        _asyncThread = new Thread(AsyncBody)
        {
            Name = "Hazelnut.Log.Async",
            IsBackground = true,
            Priority = ThreadPriority.Lowest,
        };
        _asyncThread.Start();
    }

    private static
#if NET7_0_OR_GREATER
        async
#endif
        void AsyncBody()
    {
#if NET7_0_OR_GREATER
        while (await _asyncQueue.Reader.WaitToReadAsync())
        {
            while (_asyncQueue.Reader.TryRead(out var value))
            {
                var (self, logLevel, message) = value;
                var lockObject = self.LockObject;
                if (lockObject != null)
                    lock (lockObject)
                        self.InternalWrite(logLevel, message);
                else
                    self.InternalWrite(logLevel, message);
            }
        }
#else
        while (true)
        {
            while (_asyncQueue.TryDequeue(out var value))
            {
                var (self, logLevel, message) = value;
                var lockObject = self.LockObject;
                if (lockObject != null)
                    lock (lockObject)
                        self.InternalWrite(logLevel, message);
                else
                    self.InternalWrite(logLevel, message);
            }

            try
            {
                Thread.Sleep(int.MaxValue);
            }
            catch (ThreadInterruptedException)
            {
                // Ignore
            }
            catch
            {
                break;
            }
        }
#endif
        
        Debug.WriteLine("Async Logging Thread is stopped.");
    }
    
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSync(LogLevel logLevel, string message)
    {
        var lockObject = LockObject;
        if (lockObject != null)
            lock (lockObject)
                InternalWrite(logLevel, message);
        else
            InternalWrite(logLevel, message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAsync(LogLevel logLevel, string message)
    {
#if NET7_0_OR_GREATER
        _asyncQueue.Writer.WriteAsync((this, logLevel, message));
#else
        _asyncQueue.Enqueue((this, logLevel, message));
        _asyncThread.Interrupt();
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Flush()
    {
#if NET7_0_OR_GREATER
        while (_asyncQueue.Reader.TryPeek(out _))
            Thread.Yield();
#else
        while (!_asyncQueue.IsEmpty)
            Thread.Yield();
#endif
    }
    
    protected abstract object? LockObject { get; }
    protected abstract void InternalWrite(LogLevel logLevel, string message);



    [return: NotNullIfNotNull(nameof(message))]
    private string? EscapeAnsiEscapeCode(string? message)
    {
        return message != null && !Configuration.KeepAnsiEscapeCode
            ? AnsiEscapeCodeRegex().Replace(message, string.Empty)
            : message;
    }

#if NETSTANDARD2_0_OR_GREATER
    private static readonly Regex InternalAnsiEscapeCodeRegex =
        new("\\\\e\\[([0-9]+)(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?m");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Regex AnsiEscapeCodeRegex() => InternalAnsiEscapeCodeRegex;
#else
    [GeneratedRegex(@"\\e\[([0-9]+)(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?(;([0-9]+))?m")]
    private static partial Regex AnsiEscapeCodeRegex();
#endif
}