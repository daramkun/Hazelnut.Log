using System.Text;
using System.Xml;
using Hazelnut.Log.Configurations;

namespace Hazelnut.Log;

[Serializable]
public class LoggerConfigurations
{
    public ILoggerConfiguration[] Configurations { get; } = [];

    public LoggerConfigurations() { }
    public LoggerConfigurations(params ILoggerConfiguration[] configs) =>
        Configurations = configs;
    public LoggerConfigurations(IEnumerable<ILoggerConfiguration> configs) =>
        Configurations = configs.ToArray();
}