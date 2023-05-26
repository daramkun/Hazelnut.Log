using Hazelnut.Log.Configurations;
using Hazelnut.Log.LowLevel;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log;

public class LoggerFactory : ILoggerFactory
{
    private readonly ILoggerConfiguration[] _configs;
    
    public LoggerFactory(params ILoggerConfiguration[] configs) => _configs = configs;
    public LoggerFactory(IEnumerable<ILoggerConfiguration> configs) => _configs = configs.ToArray();
    public LoggerFactory(LoggerConfigurations configs) => _configs = configs.Configurations;
    public LoggerFactory(ReadOnlySpan<ILoggerConfiguration> configs) => _configs = configs.ToArray();
    public LoggerFactory(Span<ILoggerConfiguration> configs) => _configs = configs.ToArray();

    public ILogger CreateLogger(string? name = null)
    {
        return new Logger(name, _configs);
    }

    private sealed class Logger : ILogger
    {
        private readonly ILowLevelLogger[] _loggers;
        private readonly bool _isSameMessageLayout;
        private readonly Variables _variables;
        
        public string Name { get; }
        
        public Logger(string? name, IEnumerable<ILoggerConfiguration> configs)
        {
            Name = name ?? Guid.NewGuid().ToString();
            _variables = new Variables(Name);

            List<ILowLevelLogger> loggers = new();
            foreach (var config in configs)
            {
                switch (config)
                {
                    case DebugConfiguration debugConfig: loggers.Add(new DebugLogger(debugConfig, _variables)); break;
                    case ConsoleConfiguration consoleConfig: loggers.Add(new ConsoleLogger(consoleConfig, _variables)); break;
                    case FileConfiguration fileConfig: loggers.Add(new FileLogger(fileConfig, _variables)); break;
#if NETSTANDARD2_1_OR_GREATER
                    case UnityConfiguration unityConfig:
                        if (UnityLogger.IsUnityEngine)
                            loggers.Add(new UnityLogger(unityConfig, _variables));
                        break;
#endif
#if __ANDROID__
                    case AndroidConfiguration androidConfig: loggers.Add(new AndroidLogger(androidConfig, _variables, name!)); break;
#endif
#if __MACOS__ || __IOS__ || __TVOS__ || __MACCATALYST__
                    case AppleConfiguration appleConfig: loggers.Add(new AppleLogger(appleConfig, _variables, name!)); break;
#endif
                    default: throw new NotSupportedException("Not supported configuration type.");
                }
            }

            _loggers = loggers.ToArray();
            _isSameMessageLayout = loggers.Select(logger => logger.Configuration.MessageFormat).Distinct().Count() == 1;
        }

        public void Dispose()
        {
            foreach (var logger in _loggers)
                logger.Dispose();
        }

        public bool IsWritable(LogLevel logLevel)
        {
            foreach (var logger in _loggers)
                if (logger.Configuration.IsWritable(logLevel))
                    return true;
            return false;
        }

        public void Write(LogLevel logLevel, string message)
        {
            string? formattedMessage = null;
            foreach (var logger in _loggers)
            {
                if (!logger.Configuration.IsWritable(logLevel))
                    continue;

                if (formattedMessage == null || !_isSameMessageLayout)
                    formattedMessage =
                        (logger.Configuration as BaseConfiguration)?.MessageFormatOrganizer.Format(_variables, logLevel, message)
                        ?? new FormatStringOrganizer(logger.Configuration.MessageFormat).Format(_variables, logLevel, message);
                
                logger.WriteSync(logLevel, formattedMessage);
            }
        }

        public void WriteAsync(LogLevel logLevel, string message)
        {
            string? formattedMessage = null;
            foreach (var logger in _loggers)
            {
                if (!logger.Configuration.IsWritable(logLevel))
                    continue;

                if (formattedMessage == null || !_isSameMessageLayout)
                    formattedMessage =
                        (logger.Configuration as BaseConfiguration)?.MessageFormatOrganizer.Format(_variables, logLevel, message)
                        ?? new FormatStringOrganizer(logger.Configuration.MessageFormat).Format(_variables, logLevel, message);
                
                logger.WriteAsync(logLevel, formattedMessage);
            }
        }

        public void FlushAsync()
        {
            foreach (var logger in _loggers)
                logger.Flush();
        }
    }
}