﻿using System.Text;

namespace Hazelnut.Log.Configurations;

[Serializable]
public class FileConfiguration : BaseConfiguration
{
    public string FileName { get; }

    public string ArchiveFileName { get; }
    public long ArchiveLength { get; }
    
    public Encoding Encoding { get; }

    private FileConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice,
        string fileName, string archiveFileName, long archiveLength, Encoding encoding)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice)
    {
        FileName = fileName;

        ArchiveFileName = archiveFileName;
        ArchiveLength = archiveLength;

        Encoding = encoding;
    }

    public class Builder : Builder<Builder>
    {
        private string _fileName = "${BaseDir}/Logs/${Logger}.log";
        
        private string _archiveFileName = "${BaseDir}/Logs/${Logger}-${Date:yyyy-MM-dd}.log";
        private long _archiveLength = 12000000;

        private Encoding _encoding = Encoding.UTF8;

        public Builder SetFileName(string filename)
        {
            _fileName = filename;
            return this;
        }

        public Builder SetArchiveFileName(string archiveFileName)
        {
            _archiveFileName = archiveFileName;
            return this;
        }

        public Builder SetArchiveLength(long archiveLength)
        {
            _archiveLength = archiveLength;
            return this;
        }

        public Builder SetEncoding(Encoding encoding)
        {
            _encoding = encoding;
            return this;
        }
        
        public override ILoggerConfiguration Build()
        {
            return new FileConfiguration(_messageFormat, _minimumLevel, _maximumLevel, _writeNotice, _fileName,
                _archiveFileName, _archiveLength, _encoding);
        }
    }
}