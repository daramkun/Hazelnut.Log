using System.Text;
using System.Xml;
using Hazelnut.Log.Configurations;

namespace Hazelnut.Log;

[Serializable]
public class LoggerConfigurations
{
    public ILoggerConfiguration[] Configurations { get; } = Array.Empty<ILoggerConfiguration>();

    public LoggerConfigurations() { }
    public LoggerConfigurations(params ILoggerConfiguration[] configs) => Configurations = configs;
    public LoggerConfigurations(IEnumerable<ILoggerConfiguration> configs) => Configurations = configs.ToArray();

    public static LoggerConfigurations FromFile(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open);
        return FromStream(stream);
    }

    public static LoggerConfigurations FromString(string text)
    {
        using var reader = new StringReader(text);
        return FromReader(reader);
    }

    public static LoggerConfigurations FromStream(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return FromReader(reader);
    }

    public static LoggerConfigurations FromReader(TextReader reader)
    {
        using var xmlReader = XmlReader.Create(reader, new XmlReaderSettings()
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
        });
        return FromXmlReader(xmlReader);
    }

    public static LoggerConfigurations FromXmlReader(XmlReader reader)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element &&
                reader.Name.Equals("LoggerConfigurations", StringComparison.OrdinalIgnoreCase))
                return new LoggerConfigurations(ReadSubTree(reader.ReadSubtree()));
        }

        throw new ArgumentOutOfRangeException(nameof(reader));
    }

    private static IEnumerable<ILoggerConfiguration> ReadSubTree(XmlReader reader)
    {
        var configs = new List<ILoggerConfiguration>();
        
        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element)
                continue;
            if (reader.Name == "LoggerConfigurations")
                continue;
            
            ILoggerConfiguration config;
            switch (reader.Name)
            {
                case "DebugConfiguration":
                {
                    var builder = new DebugConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                case "ConsoleConfiguration":
                {
                    var builder = new ConsoleConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    LoadConsoleConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                case "FileConfiguration":
                {
                    var builder = new FileConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    LoadFileConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                case "UnityConfiguration":
                {
                    var builder = new UnityConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    LoadUnityConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                case "AndroidConfiguration":
                {
                    var builder = new AndroidConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                case "AppleConfiguration":
                {
                    var builder = new AppleConfiguration.Builder();
                    LoadCommonConfiguration(reader, builder);
                    LoadAppleConfiguration(reader, builder);
                    config = builder.Build();
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(reader));
            }
            
            configs.Add(config);
        }

        return configs;
    }

    private static void LoadCommonConfiguration<T>(XmlReader reader, T builder) where T : ILoggerConfiguration.IBuilder<T>
    {
        var messageFormat = reader["MessageFormat"];
        var minimumLevel = reader["MinimumLevel"];
        var maximumLevel = reader["MaximumLevel"];
        var writeNotice = reader["WriteNotice"];

        if (messageFormat != null) builder.WithMessageFormat(messageFormat);
        if (minimumLevel != null) builder.WithMinimumLevel(Enum.Parse<LogLevel>(minimumLevel, true));
        if (maximumLevel != null) builder.WithMaximumLevel(Enum.Parse<LogLevel>(maximumLevel, true));
        if (writeNotice != null) builder.WithWriteNotice(bool.Parse(writeNotice));
    }

    private static void LoadConsoleConfiguration(XmlReader reader, ConsoleConfiguration.Builder builder)
    {
        var debugColor = reader["DebugColor"];
        var informationColor = reader["InformationColor"];
        var warningColor = reader["WarningColor"];
        var errorColor = reader["ErrorColor"];
        var fatalColor = reader["FatalColor"];
        var noticeColor = reader["NoticeColor"];

        if (debugColor != null) builder.WithDebugSequence(ConsoleColorToConsoleDecoration(debugColor));
        if (informationColor != null) builder.WithInformationSequence(ConsoleColorToConsoleDecoration(informationColor));
        if (warningColor != null) builder.WithWarningSequence(ConsoleColorToConsoleDecoration(warningColor));
        if (errorColor != null) builder.WithErrorSequence(ConsoleColorToConsoleDecoration(errorColor));
        if (fatalColor != null) builder.WithFatalSequence(ConsoleColorToConsoleDecoration(fatalColor));
        if (noticeColor != null) builder.WithNoticeSequence(ConsoleColorToConsoleDecoration(noticeColor));
    }

    private static ConsoleDecoration ReadConsoleDecoration(XmlReader reader)
    {
        var result = new ConsoleDecoration();

        if (!reader.Read()) return new();

        var foregroundColor = reader["Foreground"];
        var backgroundColor = reader["Background"];
        var bold = reader["Bold"];
        var dim = reader["Dim"];
        var italic = reader["Italic"];
        var underline = reader["Underline"];
        var strikeThrough = reader["StrikeThrough"];
        var invert = reader["Invert"];
        var blink = reader["Blink"];

        if (foregroundColor != null)
            result.Foreground = Enum.TryParse<ForegroundColor>(foregroundColor, out var resultForeground)
                ? resultForeground
                : ForegroundColor.Default;
        if (backgroundColor != null)
            result.Background = Enum.TryParse<BackgroundColor>(backgroundColor, out var resultBackground)
                ? resultBackground
                : BackgroundColor.Default;

        if (bold != null) result.IsBold = bool.TryParse(bold, out var boldValue) && boldValue;
        if (dim != null) result.IsDim = bool.TryParse(dim, out var dimValue) && dimValue;
        if (italic != null) result.IsItalic = bool.TryParse(italic, out var italicValue) && italicValue;
        if (underline != null) result.IsUnderline = bool.TryParse(underline, out var underlineValue) && underlineValue;
        if (strikeThrough != null) result.IsStrikeThrough = bool.TryParse(strikeThrough, out var strikeThroughValue) && strikeThroughValue;
        if (invert != null) result.IsInvert = bool.TryParse(invert, out var invertValue) && invertValue;

        if (blink != null) result.BlinkKind = Enum.TryParse<BlinkKind>(blink, out var resultBlinkKind)
            ? resultBlinkKind
            : BlinkKind.None;

        return result;
    }

    private static ConsoleDecoration ConsoleColorToConsoleDecoration(string colorString)
    {
        var consoleColor = Enum.Parse<ConsoleColor>(colorString, true);
        return consoleColor switch
        {
            ConsoleColor.Black => new ConsoleDecoration { Foreground = ForegroundColor.Black },
            ConsoleColor.Blue => new ConsoleDecoration { Foreground = ForegroundColor.LightBlue },
            ConsoleColor.Cyan => new ConsoleDecoration { Foreground = ForegroundColor.LightCyan },
            ConsoleColor.DarkBlue => new ConsoleDecoration { Foreground = ForegroundColor.Blue },
            ConsoleColor.DarkCyan => new ConsoleDecoration { Foreground = ForegroundColor.Cyan },
            ConsoleColor.DarkGray => new ConsoleDecoration { Foreground = ForegroundColor.White },
            ConsoleColor.DarkGreen => new ConsoleDecoration { Foreground = ForegroundColor.Green },
            ConsoleColor.DarkMagenta => new ConsoleDecoration { Foreground = ForegroundColor.Magenta },
            ConsoleColor.DarkRed => new ConsoleDecoration { Foreground = ForegroundColor.Red },
            ConsoleColor.DarkYellow => new ConsoleDecoration { Foreground = ForegroundColor.Yellow },
            ConsoleColor.Gray => new ConsoleDecoration { Foreground = ForegroundColor.LightBlack },
            ConsoleColor.Green => new ConsoleDecoration { Foreground = ForegroundColor.LightGreen },
            ConsoleColor.Magenta => new ConsoleDecoration { Foreground = ForegroundColor.LightMagenta },
            ConsoleColor.Red => new ConsoleDecoration { Foreground = ForegroundColor.LightRed },
            ConsoleColor.White => new ConsoleDecoration { Foreground = ForegroundColor.LightWhite },
            ConsoleColor.Yellow => new ConsoleDecoration { Foreground = ForegroundColor.LightYellow },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static void LoadFileConfiguration(XmlReader reader, FileConfiguration.Builder builder)
    {
        var fileName = reader["FileName"];
        var archiveLength = reader["ArchiveLength"];
        var archiveFileName = reader["ArchiveFileName"];
        var encoding = reader["Encoding"];

        if (fileName != null) builder.WithFileName(fileName);
        if (archiveLength != null) builder.WithArchiveLength(long.Parse(archiveLength));
        if (archiveFileName != null) builder.WithArchiveFileName(archiveFileName);
        if (encoding != null) builder.WithEncoding(Encoding.GetEncoding(encoding));
    }

    private static void LoadUnityConfiguration(XmlReader reader, UnityConfiguration.Builder builder)
    {
        var debugColor = reader["DebugColor"];
        var informationColor = reader["InformationColor"];
        var warningColor = reader["WarningColor"];
        var errorColor = reader["ErrorColor"];
        var fatalColor = reader["FatalColor"];
        var noticeColor = reader["NoticeColor"];

        if (debugColor != null) builder.WithDebugColor(debugColor);
        if (informationColor != null) builder.WithInformationColor(informationColor);
        if (warningColor != null) builder.WithWarningColor(warningColor);
        if (errorColor != null) builder.WithErrorColor(errorColor);
        if (fatalColor != null) builder.WithFatalColor(fatalColor);
        if (noticeColor != null) builder.WithNoticeColor(noticeColor);
    }

    private static void LoadAppleConfiguration(XmlReader reader, AppleConfiguration.Builder builder)
    {
        var customBundleIdentifier = reader["CustomBundleIdentifier"];
        if (customBundleIdentifier != null) builder.SetCustomBundleIdentifier(customBundleIdentifier);
    }
}