namespace Hazelnut.Log.Configurations;

public interface ILoggerConfiguration
{
    string MessageFormat { get; }
    
    LogLevel MinimumLevel { get; }
    LogLevel MaximumLevel { get; }
    bool WriteNotice { get; }

    public interface IBuilder
    {
        ILoggerConfiguration Build();
    }

    public interface IBuilder<out T> : IBuilder where T : IBuilder<T>
    {
        T WithMessageFormat(string messageFormat);

        T WithMinimumLevel(LogLevel minimumLevel);
        T WithMaximumLevel(LogLevel maximumLevel);
        T WithWriteNotice(bool writeNotice);
    }
}