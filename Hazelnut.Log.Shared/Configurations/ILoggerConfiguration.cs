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
        T SetMessageFormat(string messageFormat);

        T SetMinimumLevel(LogLevel minimumLevel);
        T SetMaximumLevel(LogLevel maximumLevel);
        T SetWriteNotice(bool writeNotice);
    }
}