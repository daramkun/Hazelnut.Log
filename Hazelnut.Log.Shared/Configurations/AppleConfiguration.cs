﻿namespace Hazelnut.Log.Configurations;

[Serializable]
public class AppleConfiguration : BaseConfiguration
{
    public string? CustomBundleIdentifier { get; }

    private AppleConfiguration(string messageFormat, LogLevel minimumLevel, LogLevel maximumLevel, bool writeNotice, bool keepAnsiEscapeCode,
        string? customBundleIdentifier)
        : base(messageFormat, minimumLevel, maximumLevel, writeNotice, keepAnsiEscapeCode)
    {
        CustomBundleIdentifier = customBundleIdentifier;
    }

    public class Builder : Builder<Builder>
    {
        private string? _customBundleIdentifier;

        public Builder SetCustomBundleIdentifier(string? customBundleIdentifier)
        {
            _customBundleIdentifier = customBundleIdentifier;
            return this;
        }
        
        public override ILoggerConfiguration Build()
        {
            return new AppleConfiguration(MessageFormat, MinimumLevel, MaximumLevel, WriteNotice, KeepAnsiEscapeCode,
                _customBundleIdentifier);
        }
    }
}