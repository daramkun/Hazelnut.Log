namespace Hazelnut.Log.Configurations;

[Serializable]
public abstract class BaseConfiguration : ILoggerConfiguration
{
    public string MessageFormat { get; }

    public LogLevel MinimumLevel { get; }
    public LogLevel MaximumLevel { get; }
    public bool WriteNotice { get; }

    internal FormatStringOrganizer MessageFormatOrganizer { get; }

    protected BaseConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice)
    {
        MessageFormat = messageFormat;
        MessageFormatOrganizer = new FormatStringOrganizer(MessageFormat);

        MinimumLevel = minimumLevel;
        MaximumLevel = maximumLevel;
        WriteNotice = writeNotice;
    }

    public abstract class Builder<T> : ILoggerConfiguration.IBuilder<T> where T : Builder<T>
    {
        protected string _messageFormat = "[${Date:yyyy-MM-dd hh\\:mm\\:ss.fff}][${ShortLogType}][T:${ThreadId:0000}] ${Message}";
        
        protected LogLevel _minimumLevel = LogLevel.Debug;
        protected LogLevel _maximumLevel = LogLevel.Fatal;
        protected bool _writeNotice = true;
        
        public T SetMessageFormat(string messageFormat)
        {
            _messageFormat = messageFormat;
            return (T)this;
        }

        public T SetMinimumLevel(LogLevel minimumLevel)
        {
            _minimumLevel = minimumLevel;
            return (T)this;
        }

        public T SetMaximumLevel(LogLevel maximumLevel)
        {
            _maximumLevel = maximumLevel;
            return (T)this;
        }

        public T SetWriteNotice(bool writeNotice)
        {
            _writeNotice = writeNotice;
            return (T)this;
        }

        public abstract ILoggerConfiguration Build();
    }
}