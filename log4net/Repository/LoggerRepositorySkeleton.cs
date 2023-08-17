using System;
using System.Collections;
using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Util;

namespace log4net.Repository {
    public abstract class LoggerRepositorySkeleton : ILoggerRepository {
        static readonly Type declaringType = typeof(LoggerRepositorySkeleton);

        readonly LevelMap m_levelMap;

        readonly RendererMap m_rendererMap;

        ICollection m_configurationMessages;

        bool m_configured;

        Level m_threshold;

        protected LoggerRepositorySkeleton()
            : this(new PropertiesDictionary()) { }

        protected LoggerRepositorySkeleton(PropertiesDictionary properties) {
            Properties = properties;
            m_rendererMap = new RendererMap();
            PluginMap = new PluginMap(this);
            m_levelMap = new LevelMap();
            m_configurationMessages = EmptyCollection.Instance;
            m_configured = false;
            AddBuiltinLevels();
            m_threshold = Level.All;
        }

        public virtual string Name { get; set; }

        public virtual Level Threshold {
            get => m_threshold;
            set {
                if (value != null) {
                    m_threshold = value;
                    return;
                }

                LogLog.Warn(declaringType, "LoggerRepositorySkeleton: Threshold cannot be set to null. Setting to ALL");
                m_threshold = Level.All;
            }
        }

        public virtual RendererMap RendererMap => m_rendererMap;

        public virtual PluginMap PluginMap { get; }

        public virtual LevelMap LevelMap => m_levelMap;

        public virtual bool Configured {
            get => m_configured;
            set => m_configured = value;
        }

        public virtual ICollection ConfigurationMessages {
            get => m_configurationMessages;
            set => m_configurationMessages = value;
        }

        public PropertiesDictionary Properties { get; }

        public event LoggerRepositoryShutdownEventHandler ShutdownEvent {
            add => m_shutdownEvent = (LoggerRepositoryShutdownEventHandler)Delegate.Combine(m_shutdownEvent, value);
            remove => m_shutdownEvent = (LoggerRepositoryShutdownEventHandler)Delegate.Remove(m_shutdownEvent, value);
        }

        public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset {
            add => m_configurationResetEvent =
                       (LoggerRepositoryConfigurationResetEventHandler)Delegate.Combine(m_configurationResetEvent, value);
            remove => m_configurationResetEvent =
                          (LoggerRepositoryConfigurationResetEventHandler)Delegate.Remove(m_configurationResetEvent, value);
        }

        public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged {
            add => m_configurationChangedEvent =
                       (LoggerRepositoryConfigurationChangedEventHandler)Delegate.Combine(m_configurationChangedEvent,
                           value);
            remove => m_configurationChangedEvent =
                          (LoggerRepositoryConfigurationChangedEventHandler)Delegate.Remove(m_configurationChangedEvent,
                              value);
        }

        public abstract ILogger Exists(string name);

        public abstract ILogger[] GetCurrentLoggers();

        public abstract ILogger GetLogger(string name);

        public virtual void Shutdown() {
            PluginCollection.IPluginCollectionEnumerator enumerator = PluginMap.AllPlugins.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    IPlugin current = enumerator.Current;
                    current.Shutdown();
                }
            } finally {
                IDisposable disposable = enumerator as IDisposable;

                if (disposable != null) {
                    disposable.Dispose();
                }
            }

            OnShutdown(null);
        }

        public virtual void ResetConfiguration() {
            m_rendererMap.Clear();
            m_levelMap.Clear();
            m_configurationMessages = EmptyCollection.Instance;
            AddBuiltinLevels();
            Configured = false;
            OnConfigurationReset(null);
        }

        public abstract void Log(LoggingEvent logEvent);

        public abstract IAppender[] GetAppenders();

        event LoggerRepositoryShutdownEventHandler m_shutdownEvent;

        event LoggerRepositoryConfigurationResetEventHandler m_configurationResetEvent;

        event LoggerRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;

        void AddBuiltinLevels() {
            m_levelMap.Add(Level.Off);
            m_levelMap.Add(Level.Emergency);
            m_levelMap.Add(Level.Fatal);
            m_levelMap.Add(Level.Alert);
            m_levelMap.Add(Level.Critical);
            m_levelMap.Add(Level.Severe);
            m_levelMap.Add(Level.Error);
            m_levelMap.Add(Level.Warn);
            m_levelMap.Add(Level.Notice);
            m_levelMap.Add(Level.Info);
            m_levelMap.Add(Level.Debug);
            m_levelMap.Add(Level.Fine);
            m_levelMap.Add(Level.Trace);
            m_levelMap.Add(Level.Finer);
            m_levelMap.Add(Level.Verbose);
            m_levelMap.Add(Level.Finest);
            m_levelMap.Add(Level.All);
        }

        public virtual void AddRenderer(Type typeToRender, IObjectRenderer rendererInstance) {
            if (typeToRender == null) {
                throw new ArgumentNullException("typeToRender");
            }

            if (rendererInstance == null) {
                throw new ArgumentNullException("rendererInstance");
            }

            m_rendererMap.Put(typeToRender, rendererInstance);
        }

        protected virtual void OnShutdown(EventArgs e) {
            if (e == null) {
                e = EventArgs.Empty;
            }

            LoggerRepositoryShutdownEventHandler shutdownEvent = m_shutdownEvent;

            if (shutdownEvent != null) {
                shutdownEvent(this, e);
            }
        }

        protected virtual void OnConfigurationReset(EventArgs e) {
            if (e == null) {
                e = EventArgs.Empty;
            }

            LoggerRepositoryConfigurationResetEventHandler configurationResetEvent = m_configurationResetEvent;

            if (configurationResetEvent != null) {
                configurationResetEvent(this, e);
            }
        }

        protected virtual void OnConfigurationChanged(EventArgs e) {
            if (e == null) {
                e = EventArgs.Empty;
            }

            LoggerRepositoryConfigurationChangedEventHandler configurationChangedEvent = m_configurationChangedEvent;

            if (configurationChangedEvent != null) {
                configurationChangedEvent(this, e);
            }
        }

        public void RaiseConfigurationChanged(EventArgs e) => OnConfigurationChanged(e);
    }
}