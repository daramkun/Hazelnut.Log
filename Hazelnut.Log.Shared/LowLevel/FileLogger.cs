using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.LowLevel;

internal sealed class FileLogger : BaseLowLevelLogger
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

    protected override object? LockObject => _targetFileName;
    
    protected override void InternalWrite(LogLevel logLevel, string message)
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