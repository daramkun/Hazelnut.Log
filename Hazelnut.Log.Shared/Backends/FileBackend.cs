using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

internal sealed class FileLogger : BaseLogBackend
{
    private readonly string _targetFileName;
    private readonly FileInfo _targetFileInfo;

    public FileLogger(ILoggerConfiguration config, Variables variables)
        : base(config, variables)
    {
        var fileConf = (FileConfiguration)config; 
        
        _targetFileName = string.Intern(new FormatStringOrganizer(fileConf.FileName).Format(variables));
        _targetFileInfo = new FileInfo(_targetFileName);

        var path = Path.GetDirectoryName(_targetFileName)!;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    
    public override void Write(LogLevel logLevel, string message)
    {
        lock (_targetFileName)
        {
            File.AppendAllText(_targetFileName, message + Environment.NewLine);

            var config = (FileConfiguration)Configuration;
            if (config.ArchiveLength > 0 && !string.IsNullOrEmpty(config.ArchiveFileName) &&
                config.ArchiveLength <= _targetFileInfo.Length)
            {
                var movingFileName = new FormatStringOrganizer(config.ArchiveFileName).Format(Variables);
                File.Move(_targetFileName, movingFileName);
            }
        }
    }
}