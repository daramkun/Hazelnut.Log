namespace Hazelnut.Log.Configurations;

[Serializable]
public class UnityConfiguration : BaseConfiguration
{
    public string? DebugColor { get; }
    public string? InformationColor { get; }
    public string? WarningColor { get; }
    public string? ErrorColor { get; }
    public string? FatalColor { get; }
    public string? NoticeColor { get; }

    private UnityConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice,
        string? debugColor, string? informationColor, string? warningColor, string? errorColor, string? fatalColor,
        string? noticeColor)
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
        private string? _debugColor;
        private string? _informationColor;
        private string? _warningColor;
        private string? _errorColor;
        private string? _fatalColor;
        private string? _noticeColor;

        public Builder WithDebugColor(string? debugColor)
        {
            _debugColor = debugColor;
            return this;
        }

        public Builder WithInformationColor(string? informationColor)
        {
            _informationColor = informationColor;
            return this;
        }

        public Builder WithWarningColor(string? warningColor)
        {
            _warningColor = warningColor;
            return this;
        }

        public Builder WithErrorColor(string? errorColor)
        {
            _errorColor = errorColor;
            return this;
        }

        public Builder WithFatalColor(string? fatalColor)
        {
            _fatalColor = fatalColor;
            return this;
        }

        public Builder WithNoticeColor(string? noticeColor)
        {
            _noticeColor = noticeColor;
            return this;
        }
        
        public override ILoggerConfiguration Build()
        {
            return new UnityConfiguration(MessageFormat, MinimumLevel, MaximumLevel, WriteNotice, _debugColor,
                _informationColor, _warningColor, _errorColor, _fatalColor, _noticeColor);
        }
    }
}