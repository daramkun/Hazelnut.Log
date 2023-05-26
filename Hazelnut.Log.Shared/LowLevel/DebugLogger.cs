using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
#if NET7_0_OR_GREATER
using System.Threading.Channels;
#endif
using Hazelnut.Log.Configurations;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log.LowLevel;

internal sealed class DebugLogger : BaseLowLevelLogger
{
    public DebugLogger(ILoggerConfiguration config, Variables variables) : base(config, variables) { }

    protected override object? LockObject => null;
    
    protected override void InternalWrite(LogLevel logLevel, string message)
    {
        Debug.WriteLine(message);
    }
}