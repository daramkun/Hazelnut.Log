using Hazelnut.Log.Configurations;
using Hazelnut.Log.Backends;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log;

public class LoggerFactory(params ILoggerConfiguration[] configs) : ILoggerFactory
{
    public LoggerFactory(IEnumerable<ILoggerConfiguration> configs)
        : this(configs.ToArray())
    {
    }

    public LoggerFactory(LoggerConfigurations configs)
        : this(configs.Configurations)
    {
    }

    public LoggerFactory(ReadOnlySpan<ILoggerConfiguration> configs)
        : this(configs.ToArray())
    {
    }

    public LoggerFactory(Span<ILoggerConfiguration> configs)
        : this(configs.ToArray())
    {
    }

    public ILogger CreateLogger(string? name = null)
    {
        return new Logger(name, configs);
    }

    private sealed class Logger : ILogger
    {
        private readonly ILogBackend[] _loggers;
        private readonly bool _isSameMessageLayout;
        private readonly Variables _variables;
        
        public string Name { get; }
        public bool IsWriteDeferred { get; set; } = true;
        
        public Logger(string? name, IEnumerable<ILoggerConfiguration> configs)
        {
            Name = name ?? Guid.NewGuid().ToString();
            _variables = new Variables(Name);

            List<ILogBackend> loggers = new();
            foreach (var config in configs)
            {
                switch (config)
                {
                    case DebugConfiguration debugConfig: loggers.Add(new DebugLogger(debugConfig, _variables)); break;
                    case ConsoleConfiguration consoleConfig: loggers.Add(new ConsoleLogger(consoleConfig, _variables)); break;
                    case FileConfiguration fileConfig: loggers.Add(new FileLogger(fileConfig, _variables)); break;
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
            return _loggers.Any(logger => logger.Configuration.IsWritable(logLevel));
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
                
                if (!IsWriteDeferred)
                    logger.Write(logLevel, formattedMessage);
                else
                    DeferredLoggingQueue.Enqueue(logger, logLevel, formattedMessage);
            }
        }

        public void WaitForDeferredWritten()
        {
            if (IsWriteDeferred)
                DeferredLoggingQueue.WaitUntilNotEmpty();
        }
    }
}