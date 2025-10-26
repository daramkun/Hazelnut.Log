using System.Diagnostics;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

public sealed class DebugLogger : BaseLogBackend
{
    public DebugLogger(ILoggerConfiguration config, Variables variables) : base(config, variables) { }
    
    public override void Write(LogLevel logLevel, string message) =>
        Debug.WriteLine(message);
}