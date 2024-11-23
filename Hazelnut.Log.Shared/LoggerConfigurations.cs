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
        if (messageFormat != null)
            builder.WithMessageFormat(messageFormat);

        var minimumLevel = reader["MinimumLevel"];
        if (minimumLevel != null)
            builder.WithMinimumLevel(Enum.Parse<LogLevel>(minimumLevel, true));

        var maximumLevel = reader["MaximumLevel"];
        if (maximumLevel != null)
            builder.WithMaximumLevel(Enum.Parse<LogLevel>(maximumLevel, true));

        var writeNotice = reader["WriteNotice"];
        if (writeNotice != null)
            builder.WithWriteNotice(bool.Parse(writeNotice));
    }

    private static void LoadConsoleConfiguration(XmlReader reader, ConsoleConfiguration.Builder builder)
    {
        var debugColor = reader["DebugColor"];
        if (debugColor != null)
            builder.WithDebugColor(Enum.Parse<ConsoleColor>(debugColor, true));

        var informationColor = reader["InformationColor"];
        if (informationColor != null)
            builder.WithInformationColor(Enum.Parse<ConsoleColor>(informationColor, true));

        var warningColor = reader["WarningColor"];
        if (warningColor != null)
            builder.WithWarningColor(Enum.Parse<ConsoleColor>(warningColor, true));

        var errorColor = reader["ErrorColor"];
        if (errorColor != null)
            builder.WithErrorColor(Enum.Parse<ConsoleColor>(errorColor, true));

        var fatalColor = reader["FatalColor"];
        if (fatalColor != null)
            builder.WithFatalColor(Enum.Parse<ConsoleColor>(fatalColor, true));

        var noticeColor = reader["NoticeColor"];
        if (noticeColor != null)
            builder.WithNoticeColor(Enum.Parse<ConsoleColor>(noticeColor, true));
    }

    private static void LoadFileConfiguration(XmlReader reader, FileConfiguration.Builder builder)
    {
        var fileName = reader["FileName"];
        if (fileName != null)
            builder.WithFileName(fileName);

        var archiveLength = reader["ArchiveLength"];
        if (archiveLength != null)
            builder.WithArchiveLength(long.Parse(archiveLength));

        var archiveFileName = reader["ArchiveFileName"];
        if (archiveFileName != null)
            builder.WithArchiveFileName(archiveFileName);

        var encoding = reader["Encoding"];
        if (encoding != null)
            builder.WithEncoding(Encoding.GetEncoding(encoding));
    }

    private static void LoadUnityConfiguration(XmlReader reader, UnityConfiguration.Builder builder)
    {
        var debugColor = reader["DebugColor"];
        if (debugColor != null)
            builder.WithDebugColor(debugColor);
        
        var informationColor = reader["InformationColor"];
        if (informationColor != null)
            builder.WithInformationColor(informationColor);
        
        var warningColor = reader["WarningColor"];
        if (warningColor != null)
            builder.WithWarningColor(warningColor);
        
        var errorColor = reader["ErrorColor"];
        if (errorColor != null)
            builder.WithErrorColor(errorColor);
        
        var fatalColor = reader["FatalColor"];
        if (fatalColor != null)
            builder.WithFatalColor(fatalColor);
        
        var noticeColor = reader["NoticeColor"];
        if (noticeColor != null)
            builder.WithNoticeColor(noticeColor);
    }

    private static void LoadAppleConfiguration(XmlReader reader, AppleConfiguration.Builder builder)
    {
        var customBundleIdentifier = reader["CustomBundleIdentifier"];
        if (customBundleIdentifier != null)
            builder.SetCustomBundleIdentifier(customBundleIdentifier);
    }
}