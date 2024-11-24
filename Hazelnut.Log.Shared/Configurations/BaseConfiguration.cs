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
        private const string DefaultMessageFormat = @"[${Date:yyyy-MM-dd hh\:mm\:ss.fff}][${ShortLogType}][T:${ThreadId:0000}] ${Message}";

        protected string MessageFormat = DefaultMessageFormat;
        
        protected LogLevel MinimumLevel = LogLevel.Debug;
        protected LogLevel MaximumLevel = LogLevel.Fatal;
        protected bool WriteNotice = true;
        
        public T WithMessageFormat(string messageFormat = DefaultMessageFormat)
        {
            MessageFormat = messageFormat;
            return (T)this;
        }

        public T WithMinimumLevel(LogLevel minimumLevel)
        {
            MinimumLevel = minimumLevel;
            return (T)this;
        }

        public T WithMaximumLevel(LogLevel maximumLevel)
        {
            MaximumLevel = maximumLevel;
            return (T)this;
        }

        public T WithWriteNotice(bool writeNotice)
        {
            WriteNotice = writeNotice;
            return (T)this;
        }

        public abstract ILoggerConfiguration Build();
    }
}