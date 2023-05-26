namespace Hazelnut.Log.Configurations;

[Serializable]
public class ConsoleConfiguration : BaseConfiguration
{
    public ConsoleColor DebugColor { get; }
    public ConsoleColor InformationColor { get; }
    public ConsoleColor WarningColor { get; }
    public ConsoleColor ErrorColor { get; }
    public ConsoleColor FatalColor { get; }
    public ConsoleColor NoticeColor { get; }

    private ConsoleConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice,
        ConsoleColor debugColor, ConsoleColor informationColor, ConsoleColor warningColor, ConsoleColor errorColor,
        ConsoleColor fatalColor, ConsoleColor noticeColor)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice)
    {
        DebugColor = debugColor;
        InformationColor = informationColor;
        WarningColor = warningColor;
        ErrorColor = errorColor;
        FatalColor = fatalColor;
        NoticeColor = noticeColor;
    }

    public class Builder : Builder<Builder>
    {
        private ConsoleColor _debugColor = ConsoleColor.DarkGray;
        private ConsoleColor _informationColor = ConsoleColor.White;
        private ConsoleColor _warningColor = ConsoleColor.Yellow;
        private ConsoleColor _errorColor = ConsoleColor.Red;
        private ConsoleColor _fatalColor = ConsoleColor.DarkRed;
        private ConsoleColor _noticeColor = ConsoleColor.DarkCyan;

        public Builder SetDebugColor(ConsoleColor color)
        {
            _debugColor = color;
            return this;
        }

        public Builder SetInformationColor(ConsoleColor color)
        {
            _informationColor = color;
            return this;
        }

        public Builder SetWarningColor(ConsoleColor color)
        {
            _warningColor = color;
            return this;
        }

        public Builder SetErrorColor(ConsoleColor color)
        {
            _errorColor = color;
            return this;
        }

        public Builder SetFatalColor(ConsoleColor color)
        {
            _fatalColor = color;
            return this;
        }

        public Builder SetNoticeColor(ConsoleColor color)
        {
            _noticeColor = color;
            return this;
        }

        public override ILoggerConfiguration Build()
        {
            return new ConsoleConfiguration(_messageFormat, _minimumLevel, _maximumLevel, _writeNotice, _debugColor,
                _informationColor, _warningColor, _errorColor, _fatalColor, _noticeColor);
        }
    }
}