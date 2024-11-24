namespace Hazelnut.Log.Configurations;

[Serializable]
public class AndroidConfiguration : BaseConfiguration
{
    private AndroidConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice, bool keepAnsiEscapeCode)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice, keepAnsiEscapeCode)
    {

    }

    public class Builder : Builder<Builder>
    {
        public Builder()
        {
            MessageFormat = "${Message}";
        }
        
        public override ILoggerConfiguration Build()
        {
            return new AndroidConfiguration(MessageFormat, MinimumLevel, MaximumLevel, WriteNotice, KeepAnsiEscapeCode);
        }
    }
}