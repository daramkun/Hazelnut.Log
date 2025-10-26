using System.IO.Compression;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

public sealed class FileLogger : BaseLogBackend
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
                if (config.ArchiveCompressionMethod == ArchiveCompressionMethod.GZip)
                {
                    var recent = File.ReadAllBytes(_targetFileName);
                    File.Delete(_targetFileName);
                    
                    using var stream = File.OpenWrite(movingFileName);
                    using var gzip = new GZipStream(stream, CompressionMode.Compress, true);
                    gzip.Write(recent, 0, recent.Length);
                    gzip.Flush();
                }
#if NET7_0_OR_GREATER
                else if (config.ArchiveCompressionMethod == ArchiveCompressionMethod.Brotli)
                {
                    var recent = File.ReadAllBytes(_targetFileName);
                    File.Delete(_targetFileName);
                    
                    using var stream = File.OpenWrite(movingFileName);
                    using var br = new BrotliStream(stream, CompressionMode.Compress, true);
                    br.Write(recent, 0, recent.Length);
                    br.Flush();
                }
#endif
                else
                {
                    File.Move(_targetFileName, movingFileName);
                }
            }
        }
    }
}