namespace Hazelnut.Log;

public interface ILoggerFactory
{
    ILogger CreateLogger(string? name = null);
}