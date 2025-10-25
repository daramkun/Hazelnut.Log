using Hazelnut.Log.Configurations;

namespace Hazelnut.Log.Backends;

internal interface ILogBackend : IDisposable
{
    ILoggerConfiguration Configuration { get; }
    
    void WriteSync(LogLevel logLevel, string message);
    void WriteAsync(LogLevel logLevel, string message);

    void Flush();
}