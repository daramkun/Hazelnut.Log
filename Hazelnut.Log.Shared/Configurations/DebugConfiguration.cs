namespace Hazelnut.Log.Configurations;

[Serializable]
public class DebugConfiguration : BaseConfiguration
{
    private DebugConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice, bool keepAnsiEscapeCode)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice, keepAnsiEscapeCode)
    {

    }

    public class Builder : Builder<Builder>
    {
        public override ILoggerConfiguration Build() =>
            new DebugConfiguration(MessageFormat, MinimumLevel, MaximumLevel, WriteNotice, KeepAnsiEscapeCode);
    }
}