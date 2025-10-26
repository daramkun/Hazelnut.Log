namespace Hazelnut.Log;

public interface ILogger : IDisposable
{
    string Name { get; }
    bool IsWriteDeferred { get; set; }

    bool IsWritable(LogLevel logLevel);

    void Write(LogLevel logLevel, string message);

    void WaitForDeferredWritten();
}