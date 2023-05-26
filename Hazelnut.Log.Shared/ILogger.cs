using Cysharp.Text;
using Hazelnut.Log.Configurations;

namespace Hazelnut.Log;

public interface ILogger : IDisposable
{
    string Name { get; }

    bool IsWritable(LogLevel logLevel);

    void Write(LogLevel logLevel, string message);
    void WriteAsync(LogLevel logLevel, string message);

    void FlushAsync();
}