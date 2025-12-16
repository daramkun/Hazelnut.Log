using System.Runtime.InteropServices;
using Cysharp.Text;

namespace Hazelnut.Log.Configurations;

public enum ForegroundColor
{
    Default = 0,

    Black = 30,
    Red = 31,
    Green = 32,
    Yellow = 33,
    Blue = 34,
    Magenta = 35,
    Cyan = 36,
    White = 37,

    LightBlack = 90,
    LightRed = 91,
    LightGreen = 92,
    LightYellow = 93,
    LightBlue = 94,
    LightMagenta = 95,
    LightCyan = 96,
    LightWhite = 97,
}

public enum BackgroundColor
{
    Default = 0,

    Black = 40,
    Red = 41,
    Green = 42,
    Yellow = 43,
    Blue = 44,
    Magenta = 45,
    Cyan = 46,
    White = 47,

    LightBlack = 100,
    LightRed = 101,
    LightGreen = 102,
    LightYellow = 103,
    LightBlue = 104,
    LightMagenta = 105,
    LightCyan = 106,
    LightWhite = 107,
}

public enum BlinkKind
{
    None,
    Slow,
    Rapid,
}

[Serializable]
public class ConsoleDecoration
{
    public bool IsBold = false;
    public bool IsDim = false;
    public bool IsItalic = false;
    public bool IsUnderline = false;
    public BlinkKind BlinkKind = BlinkKind.None;
    public bool IsInvert = false;
    public bool IsStrikeThrough = false;

    public ForegroundColor Foreground;
    public BackgroundColor Background;

    public override string ToString()
    {
        if (!IsBold && !IsDim && !IsItalic && !IsUnderline && BlinkKind == BlinkKind.None && !IsInvert &&
            Foreground == ForegroundColor.Default && Background == BackgroundColor.Default)
            return "\e[0m";

        var builder = ZString.CreateStringBuilder(notNested: false);
        if (IsBold) builder.Append("\e[1m");
        if (IsDim) builder.Append("\e[2m");
        if (IsItalic) builder.Append("\e[3m");
        if (IsUnderline) builder.Append("\e[4m");
        if (BlinkKind == BlinkKind.Slow) builder.Append("\e[5m");
        if (BlinkKind == BlinkKind.Rapid) builder.Append("\e[6m");
        if (IsInvert) builder.Append("\e[7m");
        if (IsStrikeThrough) builder.Append("\e[9m");

        if (Foreground != ForegroundColor.Default) builder.Append($"\e[{(int)Foreground}m");
        if (Background != BackgroundColor.Default) builder.Append($"\e[{(int)Background}m");

        return builder.ToString();
    }
}

[Serializable]
public class ConsoleConfiguration : BaseConfiguration
{
    private static readonly string DefaultAnsiEscapeSequence = "\e[0m";

    public ConsoleDecoration? DebugDecoration { get; }
    public ConsoleDecoration? InformationDecoration { get; }
    public ConsoleDecoration? WarningDecoration { get; }
    public ConsoleDecoration? ErrorDecoration { get; }
    public ConsoleDecoration? FatalDecoration { get; }
    public ConsoleDecoration? NoticeDecoration { get; }

    public string DebugDecorationString { get; }
    public string InformationDecorationString { get; }
    public string WarningDecorationString { get; }
    public string ErrorDecorationString { get; }
    public string FatalDecorationString { get; }
    public string NoticeDecorationString { get; }

    public bool UseColors { get; }

    private ConsoleConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice,
        ConsoleDecoration? debugDecoration, ConsoleDecoration? informationDecoration, ConsoleDecoration? warningDecoration,
        ConsoleDecoration? errorDecoration, ConsoleDecoration? fatalDecoration, ConsoleDecoration? noticeDecoration,
        bool useColors)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice)
    {
        DebugDecoration = debugDecoration;
        InformationDecoration = informationDecoration;
        WarningDecoration = warningDecoration;
        ErrorDecoration = errorDecoration;
        FatalDecoration = fatalDecoration;
        NoticeDecoration = noticeDecoration;

        DebugDecorationString = debugDecoration?.ToString() ?? DefaultAnsiEscapeSequence;
        InformationDecorationString = informationDecoration?.ToString() ?? DefaultAnsiEscapeSequence;
        WarningDecorationString = warningDecoration?.ToString() ?? DefaultAnsiEscapeSequence;
        ErrorDecorationString = errorDecoration?.ToString() ?? DefaultAnsiEscapeSequence;
        FatalDecorationString = fatalDecoration?.ToString() ?? DefaultAnsiEscapeSequence;
        NoticeDecorationString = noticeDecoration?.ToString() ?? DefaultAnsiEscapeSequence;

        UseColors = useColors;

        if (Environment.OSVersion.Version.Major < 10)
            UseColors = false;
    }

    public class Builder : Builder<Builder>
    {
        private ConsoleDecoration? _debugSequence = new() { Foreground = ForegroundColor.Black };
        private ConsoleDecoration? _informationSequence = new() { Foreground = ForegroundColor.White };
        private ConsoleDecoration? _warningSequence = new() { Foreground = ForegroundColor.Yellow };
        private ConsoleDecoration? _errorSequence = new() { Foreground = ForegroundColor.LightRed };
        private ConsoleDecoration? _fatalSequence = new() { Foreground = ForegroundColor.Red };
        private ConsoleDecoration? _noticeSequence = new() { Foreground = ForegroundColor.Cyan };
        private bool _useColors = true;

        public Builder WithDebugSequence(ConsoleDecoration? decoration)
        {
            _debugSequence = decoration;
            return this;
        }

        public Builder WithInformationSequence(ConsoleDecoration? decoration)
        {
            _informationSequence = decoration;
            return this;
        }

        public Builder WithWarningSequence(ConsoleDecoration? decoration)
        {
            _warningSequence = decoration;
            return this;
        }

        public Builder WithErrorSequence(ConsoleDecoration? decoration)
        {
            _errorSequence = decoration;
            return this;
        }

        public Builder WithFatalSequence(ConsoleDecoration? decoration)
        {
            _fatalSequence = decoration;
            return this;
        }

        public Builder WithNoticeSequence(ConsoleDecoration? decoration)
        {
            _noticeSequence = decoration;
            return this;
        }

        public Builder WithUseColors(bool useColors)
        {
            _useColors = useColors;
            return this;
        }

        public override ILoggerConfiguration Build()
        {
            return new ConsoleConfiguration(MessageFormat, MinimumLevel, MaximumLevel, WriteNotice,
                _debugSequence, _informationSequence, _warningSequence,
                _errorSequence, _fatalSequence, _noticeSequence,
                _useColors);
        }
    }
}