using Hazelnut.Log.Configurations;

namespace Hazelnut.Log.Backends;

public interface ILogBackend : IDisposable
{
    ILoggerConfiguration Configuration { get; }
    
    void Write(LogLevel logLevel, string message);
}