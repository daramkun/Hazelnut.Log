namespace Hazelnut.Log.Configurations;

public static class LoggerConfigurationExtensions
{
    public static bool IsWritable(this ILoggerConfiguration config, LogLevel logLevel) =>
        (logLevel == LogLevel.Notice && config.WriteNotice) ||
        (config.MinimumLevel <= logLevel && config.MaximumLevel >= logLevel);
}