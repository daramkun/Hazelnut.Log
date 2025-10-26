#if NET7_0_OR_GREATER
using System.Threading.Channels;
using Hazelnut.Log.Utils;
#else
using System.Collections.Concurrent;
#endif
using System.Diagnostics;
using Hazelnut.Log.Backends;

namespace Hazelnut.Log;

internal static class DeferredLoggingQueue
{
    private static readonly Thread QueueThread = new(QueueThreadBody)
    {
        Name = "Hazelnut.Log.Async",
        IsBackground = true,
        Priority = ThreadPriority.Lowest,
    };

#if NET7_0_OR_GREATER
    private static readonly Channel<(ILogBackend, LogLevel, string)> DeferredQueue =
        Channel.CreateUnbounded<(ILogBackend, LogLevel, string)>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });
#else
    private static readonly ConcurrentQueue<(ILogBackend, LogLevel, string)> DeferredQueue = new();
#endif

    static DeferredLoggingQueue()
    {
        QueueThread.Start();
    }

    public static void Enqueue(ILogBackend backend, LogLevel logLevel, string message)
    {
#if NET7_0_OR_GREATER
        DeferredQueue.Writer.TryWrite((backend, logLevel, message));
#else
        DeferredQueue.Enqueue((backend, logLevel, message));
#endif
    }

    public static void WaitUntilNotEmpty()
    {
#if NET7_0_OR_GREATER
        while (DeferredQueue.Reader.TryPeek(out _))
            Thread.Yield();
#else
        while (!DeferredQueue.IsEmpty)
            Thread.Yield();
#endif
    }
    
    private static
#if NET7_0_OR_GREATER
        async
#endif
        void QueueThreadBody()
    {
#if NET7_0_OR_GREATER
        try
        {
            while (await DeferredQueue.Reader.WaitToReadAsync())
            {
                while (DeferredQueue.Reader.TryRead(out var value))
                {
                    var (backend, logLevel, message) = value;
                    backend.Write(logLevel, message);
                }
            }
        }
        catch
        {
            // Ignore
        }
#else
        while (true)
        {
            while (DeferredQueue.TryDequeue(out var value))
            {
                var (backend, logLevel, message) = value;
                backend.Write(logLevel, message);
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
}