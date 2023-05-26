using Hazelnut.Log.Configurations;

namespace Hazelnut.Log.LowLevel;

internal interface ILowLevelLogger : IDisposable
{
    ILoggerConfiguration Configuration { get; }
    
    void WriteSync(LogLevel logLevel, string message);
    void WriteAsync(LogLevel logLevel, string message);

    void Flush();
}