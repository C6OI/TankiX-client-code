using System.Collections;
using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Util;

namespace log4net.Repository {
    public interface ILoggerRepository {
        string Name { get; set; }

        RendererMap RendererMap { get; }

        PluginMap PluginMap { get; }

        LevelMap LevelMap { get; }

        Level Threshold { get; set; }

        bool Configured { get; set; }

        ICollection ConfigurationMessages { get; set; }

        PropertiesDictionary Properties { get; }

        event LoggerRepositoryShutdownEventHandler ShutdownEvent;

        event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;

        event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;

        ILogger Exists(string name);

        ILogger[] GetCurrentLoggers();

        ILogger GetLogger(string name);

        void Shutdown();

        void ResetConfiguration();

        void Log(LoggingEvent logEvent);

        IAppender[] GetAppenders();
    }
}