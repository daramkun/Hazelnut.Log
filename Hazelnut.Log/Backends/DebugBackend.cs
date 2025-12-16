using System.Diagnostics;
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.Backends;

public sealed class DebugLogger(ILoggerConfiguration config, Variables variables)
    : BaseLogBackend(config, variables)
{
    public override void Write(LogLevel logLevel, string message) =>
        Debug.WriteLine(message);
}