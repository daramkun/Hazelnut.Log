using System.Diagnostics;
using Hazelnut.Log;

var loggerFactory = new LoggerFactory(LoggerConfigurations.FromFile("LogConfig.xml"));

var logger = loggerFactory.CreateLogger("Sample");

var stopwatch = new Stopwatch();

logger.IsWriteDeferred = false;
stopwatch.Restart();
Parallel.For(1, 100, i =>
{
    logger.Write(LogLevel.Debug, "Log Test - Debug - {0}", i);
    logger.Write(LogLevel.Information, "Log Test - Information - {0}", i);
    logger.Write(LogLevel.Warning, "Log Test - Warning - {0}", i);
    logger.Write(LogLevel.Error, "Log Test - Error - {0}", i);
    logger.Write(LogLevel.Fatal, "Log Test - Fatal - {0}", i);
    logger.Write(LogLevel.Notice, "Log Test - Notice - {0}", i);
});
stopwatch.Stop();

Console.WriteLine("=================================================");
Console.WriteLine("Sync Write: {0}", stopwatch.Elapsed);
Console.WriteLine("=================================================");

logger.IsWriteDeferred = true;
stopwatch.Restart();
Parallel.For(1, 100, i =>
{
    logger.Write(LogLevel.Debug, "Log Test - Debug - {0}", i);
    logger.Write(LogLevel.Information, "Log Test - Information - {0}", i);
    logger.Write(LogLevel.Warning, "Log Test - Warning - {0}", i);
    logger.Write(LogLevel.Error, "Log Test - Error - {0}", i);
    logger.Write(LogLevel.Fatal, "Log Test - Fatal - {0}", i);
    logger.Write(LogLevel.Notice, "Log Test - Notice - {0}", i);
});
var pooled = stopwatch.Elapsed;
logger.WaitForDeferredWritten();
stopwatch.Stop();

Console.WriteLine("=================================================");
Console.WriteLine("Async Write: {0} (Flushed: {1})", pooled, stopwatch.Elapsed);
Console.WriteLine("=================================================");

//Thread.Sleep(5000);